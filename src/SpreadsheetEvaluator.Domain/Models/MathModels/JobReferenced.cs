using System.Collections.Generic;

namespace SpreadsheetEvaluator.Domain.Models.MathModels
{
    public class JobReferenced
    {
        public string Id { get; set; }
        public List<List<Cell>> Cells { get; set; }
    }
}