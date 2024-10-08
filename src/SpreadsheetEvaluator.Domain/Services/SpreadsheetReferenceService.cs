using System;
using SpreadsheetEvaluator.Domain.Extensions;
using SpreadsheetEvaluator.Domain.Interfaces;
using SpreadsheetEvaluator.Domain.Models.Enums;
using SpreadsheetEvaluator.Domain.Models.MathModels;
using SpreadsheetEvaluator.Domain.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetEvaluator.Domain.Services
{
    public class SpreadsheetReferenceService : ISpreadsheetReferenceService
    {
        public List<JobReferenced> ReplaceReferences(List<JobRaw> jobsList)
        {
            // Map our singleJobs to computed jobs,
            // to not change passed data directly and remove unused properties.

            var computedJobs = FormulaEvaluatorHelper.MapJobsRawToJobComputed(jobsList);

            foreach (var job in computedJobs)
            {

                foreach (var cellRow in job.Cells)
                {
                    // Reverse cellRow if the value is at the end and formulas are at the start.
                    var isCellRowInvertedNormalCellLastIndex = cellRow.FindLastIndex(x => x.Value.CellType != CellType.Formula);
                    var isCellRowInvertedFormulaCellFirstIndex = cellRow.FindIndex(x => x.Value.CellType == CellType.Formula);

                    if (isCellRowInvertedNormalCellLastIndex > isCellRowInvertedFormulaCellFirstIndex)
                    {
                        cellRow.Reverse();
                    }

                    foreach (var singleCell in cellRow)
                    {
                        var formula = singleCell.Value.Value as Formula;
                        if (formula == null)
                        {
                            continue;
                        }

                        // If the formula of a reference type, find a value that this reference points to.
                        var referencedCell = FormulaEvaluatorHelper.TryGetCellTypeAsReference(formula, job);
                        if (referencedCell != null)
                        {
                            singleCell.Value.UpdateCell(referencedCell.Value);
                            continue;
                        }


                        // Filter cells without formulas.
                        var cellsWithoutFormulas = FormulaEvaluatorHelper.GetCellsWithoutFormulas(cellRow);
                        if (FormulaEvaluatorHelper.IsTypesInFormulaAreMissmatched(cellsWithoutFormulas, formula))
                        {
                            singleCell.Value.SetCellAsErrorCell();
                            continue;
                        }

                        // Replace references in the formula.
                        // E.g "A1 + B1" is going to be "5 + 4" after this method execution.
                        var mathExpressionText = CalculationHelper.ReplaceFormulaReferencesWithValues(formula.Text, cellsWithoutFormulas);

                        formula.Text = mathExpressionText;

                        // Replace cell's value with the math expression, which is a string currently.
                        singleCell.Value.UpdateCell(new Formula(formula));
                    }
                }
            }
            return computedJobs;
        }
    }
}