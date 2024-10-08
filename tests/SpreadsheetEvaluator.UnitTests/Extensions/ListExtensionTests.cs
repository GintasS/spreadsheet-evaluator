using System.Linq;
using FluentAssertions;
using SpreadsheetEvaluator.Domain.Extensions;
using Xunit;

namespace SpreadsheetEvaluator.UnitTests.Extensions
{
    public class ListExtensionTests
    {
        [Theory]
        [InlineData(false, new [] { 1, 1 })]
        [InlineData(true, new [] { 1, 2 })]
        [InlineData(false, new [] { 1 })]
        [InlineData(false, null)]
        public void Should_Check_List_ForMismatchingTypes(bool expected, int[] abcObjects)
        {
            // Arrange
            var list = abcObjects?.ToList();

            // Act
            var actual = list.HasMismatchingElementTypes();

            // Assert
            actual.Should().Be(expected);
        }
    }
}