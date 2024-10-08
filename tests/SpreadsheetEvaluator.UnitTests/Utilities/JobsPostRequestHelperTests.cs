using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using SpreadsheetEvaluator.Domain.Configuration;
using SpreadsheetEvaluator.Domain.Models.MathModels;
using SpreadsheetEvaluator.Domain.Utilities;
using SpreadsheetEvaluator.UnitTests.TestHelpers;
using Xunit;

namespace SpreadsheetEvaluator.UnitTests.Utilities
{
    public class JobsPostRequestHelperTests
    {
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
                        }
                    }
                }
            }
        };

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { JobsComputedExpected }
        };

        private readonly JobsPostRequestHelper _jobsPostRequestHelper;

        public JobsPostRequestHelperTests()
        {
            var applicationSettings = new ApplicationSettings
            {
                DeveloperEmailAddress = "testMail"
            };

            var applicationSettingsMock = new Mock<IOptionsMonitor<ApplicationSettings>>();
            applicationSettingsMock.Setup(c => c.CurrentValue).Returns(applicationSettings);

            _jobsPostRequestHelper = new JobsPostRequestHelper(applicationSettingsMock.Object);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Should_Replace_CellReferences_WithValues(object jobsComputed)
        {
            // Arrange
            var jobsComputedList = jobsComputed as List<JobComputed>;
            var expectedJobsPostRequestJson = ResourceFileReaderHelper.ReadFile("SpreadsheetEvaluator.UnitTests.Resources.JobsPostRequest.json");

            // Act
            var actualJobsPostRequest = _jobsPostRequestHelper.CreatePostRequest(jobsComputedList);
            
            var actualJobsPostRequestJson = JsonConvert.SerializeObject(
                actualJobsPostRequest,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }
            );

            // Assert
            actualJobsPostRequestJson.Should().Be(expectedJobsPostRequestJson);
            
        }
    }
}