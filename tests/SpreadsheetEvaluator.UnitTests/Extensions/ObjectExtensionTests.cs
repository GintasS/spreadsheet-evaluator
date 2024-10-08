using System.Collections.Generic;
using FluentAssertions;
using SpreadsheetEvaluator.Domain.Configuration;
using SpreadsheetEvaluator.Domain.Extensions;
using SpreadsheetEvaluator.Domain.Models.MathModels;
using Xunit;

namespace SpreadsheetEvaluator.UnitTests.Extensions
{
    public class ObjectExtensionTests
    {
        public static IEnumerable<object[]> DataIsNumber => new List<object[]>
        {
            new object[] { true, (sbyte)0 },
            new object[] { true, (byte)100 },
            new object[] { true, (short)100 },
            new object[] { true, (ushort)10000 },
            new object[] { true, -1 },
            new object[] { true, (uint)0 },
            new object[] { true, (long)-1000000 },
            new object[] { true, (ulong)1000000 },
            new object[] { true, 0.4f },
            new object[] { true, 0.454d },
            new object[] { true, "30.50", true },
            new object[] { false, "test" },
            new object[] { false, 't' },
            new object[] { false, true },
            new object[] { false, new Formula("test", Constants.FormulaOperators[0]) }
        };

        public static IEnumerable<object[]> DataIsValidCellValue => new List<object[]>
        {
            new object[] { true, "30.50", true },
            new object[] { true, "test" },
            new object[] { true, true },
            new object[] { true, new Formula("test", Constants.FormulaOperators[0]) },
            new object[] { true, new CellValue("a") },
            new object[] { false, new JobRaw() },
            new object[] { false, 't' }
        };

        public static IEnumerable<object[]> DataCalculateMathExpressions => new List<object[]>
        {
            new object[] { 10, "5 + 5" },
            new object[] { true, "True AND True" },
            new object[] { 6, "IIF(5>6,5,6)" },
            new object[] { null, "5 + t" },
            new object[] { null, new CellValue("a") },
            new object[] { null, new JobRaw() },
            new object[] { null, 't' },
            new object[] { null, "True AND 1" },
            new object[] { true, "NOT False" },
            new object[] { false, "5 > 6" },
            new object[] { 31.15376, "5.456 * 5.71" }
        };

        [Theory]
        [MemberData(nameof(DataIsNumber))]
        public void Should_Check_ForValidNumber_IsNumber(bool expected, object value, bool isDecimalValue = false)
        {
            // Arrange
            if (isDecimalValue)
            {
                decimal.TryParse(value.ToString(), out var decimalResult);
                value = decimalResult;
            }

            // Act
            var actual = value.IsNumber();

            // Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(DataIsValidCellValue))]
        public void Should_Check_ForValidNumber_IsValidCellValue(bool expected, object value, bool isDecimalValue = false)
        {
            // Arrange
            if (isDecimalValue)
            {
                decimal.TryParse(value.ToString(), out var decimalResult);
                value = decimalResult;
            }

            // Act
            var actual = value.IsValidCellValue();

            // Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(DataCalculateMathExpressions))]
        public void Should_Check_ForValidNumber_CalculateMathExpressions(object expectedValue, object valueToCompute)
        {
            // Act
            var actual = valueToCompute.CalculateMathExpression();

            // Assert
            actual.Should().Be(expectedValue);
        }
    }
}
