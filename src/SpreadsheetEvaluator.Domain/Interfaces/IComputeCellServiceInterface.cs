using SpreadsheetEvaluator.Domain.Models.MathModels;
using System.Collections.Generic;

namespace SpreadsheetEvaluator.Domain.Interfaces
{
    public interface IComputeCellServiceInterface
    {
        List<JobComputed> ComputeCells(List<JobReferenced> jobs);
    }
}