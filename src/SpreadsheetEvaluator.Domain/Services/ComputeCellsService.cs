using SpreadsheetEvaluator.Domain.Extensions;
using SpreadsheetEvaluator.Domain.Interfaces;
using SpreadsheetEvaluator.Domain.Models.Enums;
using SpreadsheetEvaluator.Domain.Models.MathModels;
using SpreadsheetEvaluator.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEvaluator.Domain.Services
{
    public class ComputeCellService : IComputeCellServiceInterface
    {
        public List<JobComputed> ComputeCells(List<JobReferenced> jobs)
        {
            var computedJobs = jobs.Select(x => new JobComputed
            {
                Id = x.Id,
                Cells = x.Cells
            }).ToList();

            foreach (var jobReferenced in computedJobs)
            {
                foreach(var cellRow in jobReferenced.Cells)
                {
                    foreach (var singleCell in cellRow)
                    {
                        var formula = singleCell.Value.Value as Formula;
                        if (formula == null)
                        {
                            continue;
                        }

                        var computedValue = TryComputeSingleCell(formula);
                        if (computedValue == null)
                        {
                            singleCell.Value.SetCellAsErrorCell();
                            continue;
                        }
                        singleCell.Value.UpdateCell((dynamic)computedValue);
                    }
                }
            }
            return computedJobs;
        }

        public object TryComputeSingleCell(Formula formula)
        {
            // Compute math expression.
            // Do not compute value for formula with a string type,
            // because we will get an exception.
            if (formula.FormulaOperator.FormulaResultType != FormulaResultType.Text)
            {
                return formula.Text.CalculateMathExpression();
            }
            
            return formula.Text;
        }
    }
}
