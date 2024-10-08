using System.Collections.Generic;
using FluentAssertions;
using SpreadsheetEvaluator.Domain.Models.MathModels;
using SpreadsheetEvaluator.Domain.Utilities;
using Xunit;

namespace SpreadsheetEvaluator.UnitTests.Utilities
{
    public class CalculationHelperTests
    {
        private static readonly object[] FirstRow =
        {
            "A1 + B1",
            "5 + 10",
            new List<Cell>
            {
                new Cell
                {
                    Key = "A1",
                    Value = new CellValue(5)
                },
                new Cell
                {
                    Key = "B1",
                    Value = new CellValue(10)
                }
            }
        };

        private static readonly object[] SecondRow =
        {
            "IFF(A1>B1,A1,B1)",
            "IFF(5>10,5,10)",
            new List<Cell>
            {
                new Cell
                {
                    Key = "A1",
                    Value = new CellValue(5)
                },
                new Cell
                {
                    Key = "B1",
                    Value = new CellValue(10)
                }
            }
        };

        private static readonly object[] ThirdRow =
        {
            "NOT B1",
            "NOT 10",
            new List<Cell>
            {
                new Cell
                {
                    Key = "A1",
                    Value = new CellValue(5)
                },
                new Cell
                {
                    Key = "B1",
                    Value = new CellValue(10)
                }
            }
        };

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            FirstRow,
            SecondRow,
            ThirdRow
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void Should_Replace_CellReferences_WithValues(string formulaTextGiven, string formulaTexExpected, object cellListObject)
        {
            // Arrange
            var cellList = cellListObject as List<Cell>;

            // Act
            var actualReplacedFormulaText = CalculationHelper.ReplaceFormulaReferencesWithValues(formulaTextGiven, cellList);

            // Assert
            actualReplacedFormulaText.Should().Be(formulaTexExpected);
        }
    }
}