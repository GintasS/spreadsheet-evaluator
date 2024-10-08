using FluentAssertions;
using SpreadsheetEvaluator.Domain.Models.MathModels;
using SpreadsheetEvaluator.Domain.Configuration;
using Xunit;

namespace SpreadsheetEvaluator.UnitTests.Models.MathModels
{
    public class FormulaTests
    {
        [Fact]
        public void Should_Create_Formula_From_Normal_Values()
        {
            // Act
            var formula = new Formula("test", Constants.FormulaOperators[0]);

            // Assert
            formula.Text.Should().Be("test");
            formula.FormulaOperator.Should().NotBeSameAs(Constants.FormulaOperators[0]);
            formula.FormulaOperator.JsonName.Should().Be(Constants.FormulaOperators[0].JsonName);
            formula.FormulaOperator.MathSymbol.Should().Be(Constants.FormulaOperators[0].MathSymbol);
            formula.FormulaOperator.FormulaResultType.Should().Be(Constants.FormulaOperators[0].FormulaResultType);
        }

        [Fact]
        public void Should_Create_Formula_From_Another_Formula()
        {
            // Act
            var oldFormula = new Formula("test", Constants.FormulaOperators[0]);
            var formula = new Formula(oldFormula);

            // Assert
            formula.Text.Should().Be("test");
            formula.FormulaOperator.Should().NotBeSameAs(oldFormula.FormulaOperator);
            formula.FormulaOperator.JsonName.Should().Be(oldFormula.FormulaOperator.JsonName);
            formula.FormulaOperator.MathSymbol.Should().Be(oldFormula.FormulaOperator.MathSymbol);
            formula.FormulaOperator.FormulaResultType.Should().Be(oldFormula.FormulaOperator.FormulaResultType);
        }
    }
}