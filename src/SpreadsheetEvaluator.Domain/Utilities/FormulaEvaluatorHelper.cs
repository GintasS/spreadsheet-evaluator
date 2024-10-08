using SpreadsheetEvaluator.Domain.Extensions;
using SpreadsheetEvaluator.Domain.Models.Enums;
using SpreadsheetEvaluator.Domain.Models.MathModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEvaluator.Domain.Utilities
{
    public static class FormulaEvaluatorHelper
    {
        public static void ReverseCellsIfFormulasAreAtTheEnd(List<Cell> cellRow)
        {
            // Reverse cellRow if the value is at the end and formulas are at the start.
            var isCellRowInvertedNormalCellLastIndex = cellRow.FindLastIndex(x => x.Value.CellType != CellType.Formula);
            var isCellRowInvertedFormulaCellFirstIndex = cellRow.FindIndex(x => x.Value.CellType == CellType.Formula);

            if (isCellRowInvertedNormalCellLastIndex > isCellRowInvertedFormulaCellFirstIndex)
            {
                cellRow.Reverse();
            }
        }

        public static bool IsTypesInFormulaAreMissmatched(List<Cell> cellsWithoutFormulas, Formula formula)
        {
            // Check if our values that are not yet replaced in the formula ("A1 + B1")
            // are of a mismatching type.
            var hasMismatchingElementTypes = cellsWithoutFormulas.Where(x => formula.Text.IndexOf(x.Key, StringComparison.Ordinal) >= 0)
                .Select(x => x.Value.CellType)
                .ToList()
                .HasMismatchingElementTypes();

            return hasMismatchingElementTypes;
        }

        public static List<Cell> GetCellsWithoutFormulas(List<Cell> cellRow)
        {
            // Filter cells without formulas.
            return cellRow.Where(x => x.Value.IsFormulaCell == false)
                .ToList();
        }

        public static Formula TryGetCellAsFormula(Cell individualCell)
        {
            var formula = individualCell.Value.Value as Formula;
            if (formula == null)
            {
                return null;
            }
            return formula;
        }

        public static Cell TryGetCellTypeAsReference(Formula formula, JobReferenced job)
        {
            return job.Cells.SelectMany(x => x)
                .FirstOrDefault(x => x.Key == formula.Text);
        }
        public static List<JobReferenced> MapJobsRawToJobComputed(List<JobRaw> jobsList)
        {
            return jobsList.Select(x => new JobReferenced
            {
                Id = x.Id,
                Cells = x.Cells
            }).ToList();
        }
    }
}
