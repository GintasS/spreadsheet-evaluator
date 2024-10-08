using SpreadsheetEvaluator.Domain.Models.Enums;

namespace SpreadsheetEvaluator.Domain.Models.MathModels
{
    public class Formula
    {
        public string Text { get; set; }
        public FormulaOperator FormulaOperator { get; }

        public Formula(string text, FormulaOperator formulaOperator)
        {
            Text = new string(text);
            FormulaOperator = new FormulaOperator(formulaOperator);
        }

        public Formula(Formula formula)
        {
            Text = new string(formula.Text);
            FormulaOperator = new FormulaOperator(formula.FormulaOperator);
        }
    }

    public class FormulaOperator
    {
        public string JsonName { get; set; }
        public string MathSymbol { get; set; }
        public FormulaResultType FormulaResultType { get; set; }

        public FormulaOperator()
        {

        }

        public FormulaOperator(FormulaOperator formulaOperator)
        {
            JsonName = new string(formulaOperator.JsonName);
            MathSymbol = new string(formulaOperator.MathSymbol);
            FormulaResultType = formulaOperator.FormulaResultType;
        }
    }
}