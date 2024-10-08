using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SpreadsheetEvaluator.Domain.Configuration;
using SpreadsheetEvaluator.Domain.Interfaces;
using SpreadsheetEvaluator.Domain.Models.Enums;
using SpreadsheetEvaluator.Domain.Models.MathModels;
using SpreadsheetEvaluator.Domain.Services;
using Xunit;

namespace SpreadsheetEvaluator.UnitTests.Services
{
    public class FormulaEvaluatorServiceTests
    {
        private static readonly List<JobRaw> JobsRawGiven = new List<JobRaw>
        {
            new JobRaw
            {
                Id = "job-0",
                Cells = new List<List<Cell>>
                {
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
                        },
                        new Cell
                        {
                            Key = "C1",
                            Value = new CellValue(new Formula("A1 + B1", Constants.FormulaOperators[0]))
                        },
                        new Cell
                        {
                            Key = "D1",
                            Value = new CellValue(new Formula("Hello, World!", Constants.FormulaOperators[9]))
                        },
                        new Cell
                        {
                            Key = "E1",
                            Value = new CellValue(new Formula("5 + t", Constants.FormulaOperators[0]))
                        },
                        new Cell
                        {
                            Key = "F1",
                            Value = new CellValue(new Formula("IIF(A1>B1,A1,B1)", Constants.FormulaOperators[8]))
                        }
                    }
                }
            }
        };

        private static readonly List<JobComputed> JobsComputedExpected = new List<JobComputed>
        {
            new JobComputed
            {
                Id = "job-0",
                Cells = new List<List<Cell>>
                {
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
                        },
                        new Cell
                        {
                            Key = "C1",
                            Value = new CellValue(15)
                        },
                        new Cell
                        {
                            Key = "D1",
                            Value = new CellValue("Hello, World!")
                        },
                        new Cell
                        {
                            Key = "E1",
                            Value = new CellValue(Constants.Error.MismatchingTypes)
                        },
                        new Cell
                        {
                            Key = "F1",
                            Value = new CellValue(10)
                        }
                    }
                }
            }
        };

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { JobsComputedExpected, JobsRawGiven }
        };

        private readonly ISpreadsheetReferenceService _formulaEvaluatorService;
        private readonly IComputeCellServiceInterface _computeCellService;

        public FormulaEvaluatorServiceTests()
        {
            _formulaEvaluatorService = new SpreadsheetReferenceService();
            _computeCellService = new ComputeCellService();

            // Set 4th cell to be of the error type.
            JobsComputedExpected[0].Cells[0][4].Value.SetCellAsErrorCell();
        }

        [Theory]
        [MemberData(nameof(Data))]

        public void Should_Compute_Formulas_For_Jobs_Raw(object expectedValue, object givenValue)
        {
            // Arrange
            var expectedJobsComputed = expectedValue as List<JobComputed>;
            var givenJobsRaw = givenValue as List<JobRaw>;

            // Act
            var actual = _formulaEvaluatorService.ReplaceReferences(givenJobsRaw);

            var computedJobs = _computeCellService.ComputeCells(actual);
            
            var jobsComputedExpected = expectedJobsComputed[0].Cells[0]
                .Select(x => x)
                .ToList();

            var actualComputedJobs = computedJobs[0].Cells[0]
                .Select(x => x)
                .ToList();

            // Assert
            computedJobs[0].Id.Should().Be(givenJobsRaw[0].Id);

            for (var i = 0; i < actualComputedJobs.Count; i++)
            {
                actualComputedJobs[i].Should().NotBeSameAs(jobsComputedExpected[i]);
                actualComputedJobs[i].Value.Value.Should().Be(jobsComputedExpected[i].Value.Value);
                actualComputedJobs[i].Value.CellType.Should().Be(jobsComputedExpected[i].Value.CellType);
                actualComputedJobs[i].Key.Should().Be(jobsComputedExpected[i].Key);

                if (actualComputedJobs[i].Value.CellType.Equals(CellType.Error))
                {
                    actualComputedJobs[i].Value.IsErrorCell.Should().BeTrue();
                }
            }
        }
    }
}
