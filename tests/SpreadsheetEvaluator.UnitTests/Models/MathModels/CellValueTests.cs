using FluentAssertions;
using SpreadsheetEvaluator.Domain.Configuration;
using SpreadsheetEvaluator.Domain.Models.MathModels;
using System.Collections.Generic;
using SpreadsheetEvaluator.Domain.Models.Enums;
using Xunit;
using System;
using Microsoft.CSharp.RuntimeBinder;

namespace SpreadsheetEvaluator.UnitTests.Models.MathModels
{
    public class CellValueTests
    {
        public static IEnumerable<object[]> DataCreateCell => new List<object[]>
        {
            new object[] { 0, CellType.Number, 0 },
            new object[] { true, CellType.Boolean, true },
            new object[] { 3.456, CellType.Number, 3.456 }
        };

        public static IEnumerable<object[]> DataCreateCellString= new List<object[]>
        {
            new object[] { "test", CellType.Text, "test"}
        };

        public static IEnumerable<object[]> DataCreateCellDecimal = new List<object[]>
        {
            new object[] { "30.50", CellType.Number, "30.50"}
        };

        [Theory]
        [MemberData(nameof(DataCreateCell))]
        public void Should_Create_Cell_Value(object expectedValue, CellType expectedCellType, object valueToSet)
        {
            // Act
            var cellValue = new CellValue(valueToSet);

            // Assert
            cellValue.Value.Should().Be(expectedValue);
            cellValue.CellType.Should().Be(expectedCellType);
        }

        [Theory]
        [MemberData(nameof(DataCreateCellString))]
        public void Should_Create_Cell_Value_For_String_Value(object expectedValue, CellType expectedCellType, object valueToSet)
        {
            // Act
            var cellValue = new CellValue(valueToSet);

            // Assert
            cellValue.Value.Should().Be(expectedValue);
            cellValue.CellType.Should().Be(expectedCellType);
            cellValue.Value.Should().NotBeSameAs(expectedValue);
        }

        [Theory]
        [MemberData(nameof(DataCreateCellDecimal))]
        public void Should_Create_Cell_Value_For_Decimal_Value(object expectedValue, CellType expectedCellType, object valueToSet)
        {
            // Arrange
            decimal.TryParse(expectedValue.ToString(), out var expectedDecimalResult);
            decimal.TryParse(valueToSet.ToString(), out var givenDecimalResult);
            expectedValue = expectedDecimalResult;
            valueToSet = givenDecimalResult;

            // Act
            var cellValue = new CellValue(valueToSet);

            // Assert
            cellValue.Value.Should().Be(expectedValue);
            cellValue.CellType.Should().Be(expectedCellType);
        }

        [Fact]
        public void Should_Create_Cell_Value_For_Formula_Object_Value()
        {
            // Arrange
            var formula = new Formula("A1", Constants.FormulaOperators[0]);

            // Act
            var cellValue = new CellValue(formula);

            // Assert
            var formulaValueFromCell = cellValue.Value as Formula;

            formulaValueFromCell?.Should().NotBe(null);
            formulaValueFromCell?.Should().NotBeSameAs(formula);
            formulaValueFromCell?.FormulaOperator.Should().NotBeSameAs(formula.FormulaOperator);
            formulaValueFromCell?.FormulaOperator.JsonName.Should().Be(formula.FormulaOperator.JsonName);
            formulaValueFromCell?.FormulaOperator.MathSymbol.Should().Be(formula.FormulaOperator.MathSymbol);
            formulaValueFromCell?.FormulaOperator.FormulaResultType.Should().Be(formula.FormulaOperator.FormulaResultType);
            formulaValueFromCell?.Text.Should().Be(formula.Text);
            cellValue.Value.Should().BeOfType<Formula>();
            cellValue.IsFormulaCell.Should().BeTrue();
        }

        [Fact]
        public void Should_Create_Cell_Value_For_Cell_Value_Object()
        {
            // Arrange
            var originalCellValue = new CellValue("test");

            // Act
            var cellValueToBeCreated = new CellValue(originalCellValue);

            // Assert
            cellValueToBeCreated.Should().NotBe(null);
            cellValueToBeCreated.Should().NotBe(originalCellValue);
            cellValueToBeCreated.Value.Should().Be("test");
            cellValueToBeCreated.CellType.Should().Be(CellType.Text);
            cellValueToBeCreated.Value.Should().NotBeSameAs(originalCellValue.Value);
        }

        [Fact]
        public void Should_Set_Cell_Value_To_Error()
        {
            // Arrange
            var cellValue = new CellValue("test");

            // Act
            cellValue.SetCellAsErrorCell();

            // Assert
            cellValue.Value.Should().Be(Constants.Error.MismatchingTypes);
            cellValue.IsErrorCell.Should().BeTrue();
        }

        [Fact]
        public void Should_Get_Exception_When_InvalidCellType()
        {
            // Act
            Action action = () => new CellValue(new JobRaw());

            // Assert
            action.Should().Throw<RuntimeBinderException>();
        }
    }
}