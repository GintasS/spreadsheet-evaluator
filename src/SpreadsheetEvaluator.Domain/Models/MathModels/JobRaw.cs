using System;
using System.Collections.Generic;

namespace SpreadsheetEvaluator.Domain.Models.MathModels
{
    public class JobRaw
    {
        public string Id { get; set; }
        public List<List<Cell>> Cells { get; set; } = new List<List<Cell>>();

        // Helper flags for recursion.
        public bool IteratingOverDataArray { get; set; }
        public bool IsMultiArray { get; set; }
        public bool FoundValue { get; set; }

        // Formulas.
        public bool IsInFormulaObject { get; set; }
        public bool FoundIfFormula { get; set; }

        // Indexes for cell key notation, e.g: "A1".
        private int _cellLetterIndex = 65;
        private int _cellNumberIndex = 1;

        public void SetCellValue(object value)
        {
            var y = _cellNumberIndex - 1;
            var x = _cellLetterIndex - 65;

            TryResizeAllAxis(y, x);

            Cells[y][x] = new Cell
            {
                Key = GetCellKeyFromIndexes(),
                Value = new CellValue(value)
            };
            IncrementLetterIndex();
        }

        private void TryResizeAllAxis(int y, int x)
        {
            if (y >= Cells.Count)
            {
                var diff = Math.Abs(Cells.Count - (y + 1));
                for (var i = 0; i < diff; i++)
                {
                    Cells.Add(new List<Cell>());
                }
            }

            if (x >= Cells[y].Count)
            {
                var diff = Math.Abs(Cells[y].Count - (x + 1));
                for (var i = 0; i < diff; i++)
                {
                    Cells[y].Add(new Cell());
                }
            }
        }

        private string GetCellKeyFromIndexes()
        {
            return new string((char)_cellLetterIndex + _cellNumberIndex.ToString());
        }

        public void IncrementCellIndex()
        {
            _cellNumberIndex++;
        }

        public void IncrementLetterIndex()
        {
            _cellLetterIndex++;
        }

        public void ResetLetterIndex()
        {
            _cellLetterIndex = 65;
        }
    } 
}