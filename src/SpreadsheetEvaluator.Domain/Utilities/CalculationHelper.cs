using System.Collections.Generic;
using SpreadsheetEvaluator.Domain.Models.MathModels;

namespace SpreadsheetEvaluator.Domain.Utilities
{
    public static class CalculationHelper
    {
        public static string ReplaceFormulaReferencesWithValues(string formulaText, List<Cell> cellRow)
        {
            var replacedFormulaText = new string(formulaText);
            foreach (var cell in cellRow)
            {
                replacedFormulaText = replacedFormulaText.Replace(cell.Key, cell.Value.Value.ToString());
            }

            return replacedFormulaText;
        }
    }
}