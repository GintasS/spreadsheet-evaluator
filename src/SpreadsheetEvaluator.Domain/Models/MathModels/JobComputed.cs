using System.Collections.Generic;

namespace SpreadsheetEvaluator.Domain.Models.MathModels
{
    public class JobComputed
    {
        public string Id { get; set; }
        public List<List<Cell>> Cells { get; set; } = new List<List<Cell>>();
    }
}