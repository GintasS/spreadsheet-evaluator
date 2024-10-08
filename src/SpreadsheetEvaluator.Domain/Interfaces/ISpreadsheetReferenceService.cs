using SpreadsheetEvaluator.Domain.Models.MathModels;
using System.Collections.Generic;

namespace SpreadsheetEvaluator.Domain.Interfaces
{
    public interface ISpreadsheetReferenceService
    {
        public List<JobReferenced> ReplaceReferences(List<JobRaw> jobsList);
    }
}