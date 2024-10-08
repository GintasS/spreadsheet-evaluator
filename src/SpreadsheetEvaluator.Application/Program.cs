using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using SpreadsheetEvaluator.Application.Enums;
using SpreadsheetEvaluator.Domain;
using SpreadsheetEvaluator.Domain.Interfaces;
using SpreadsheetEvaluator.Domain.Models.Responses;
using SpreadsheetEvaluator.Domain.Utilities;
using System;
using System.Collections.Generic;

namespace SpreadsheetEvaluator.Application
{
    internal class Program
    {
        private static ISpreadsheetReferenceService _spreadsheetReferenceService;
        private static ISpreadsheetCreationService _spreadsheetCreationService;
        private static IComputeCellServiceInterface _computeCellServiceInterface;
        private static JobsPostRequestHelper _jobsPostRequestHelper;

        static Program()
        {
            var serviceProvider = Startup.InitServiceProvider();
            _spreadsheetReferenceService = serviceProvider.GetService<ISpreadsheetReferenceService>();
            _spreadsheetCreationService = serviceProvider.GetService<ISpreadsheetCreationService>();
            _computeCellServiceInterface = serviceProvider.GetService<IComputeCellServiceInterface>();
            _jobsPostRequestHelper = serviceProvider.GetService<JobsPostRequestHelper>();
        }

        class Options
        {
            [Option('i', "input", Required = true, HelpText = "Input file to be processed.")]
            public string InputFile { get; set; }

            [Option('o', "output", Required = true, HelpText = "Output file to save results.")]
            public string OutputFile { get; set; }

            // Omitting long name, defaults to name of property, ie "--verbose"
            [Option(
              Default = false,
              HelpText = "Prints all messages to standard output.")]
            public bool Verbose { get; set; }
        }

        private static int Main(string[] args)
        {
            return CommandLine.Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    (Options opts) => RunOptions(opts), // If parsing is successful, return 0
                    errs => HandleParseError(errs)      // If parsing fails, return 1
                );
        }

        static int RunOptions(Options opts)
        {
            // 0. Handle CLI arguments.
            var inputFilePath = opts.InputFile;
            var outputFilePath = opts.OutputFile;

            // 1. Get Jobs from the file.
            var jobsGetRawResponse = new JobsGetRawResponse();
            try
            {
                jobsGetRawResponse = FileHelper.ReadSpreadsheetFromJsonAsClass(inputFilePath);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (int)ExitCode.ErrorReadingInputFile;
            }

            // 2. Parse Json object to actual jobs list that we can use.
            var createdJobs = _spreadsheetCreationService.Create(jobsGetRawResponse);

            // 3. Compute formulas for cells.
            var referencedJobs = _spreadsheetReferenceService.ReplaceReferences(createdJobs);

            // 4. Computed Formulas. Before this, they are in "5 + 6" string state.
            var computedJobs = _computeCellServiceInterface.ComputeCells(referencedJobs);

            // 5. Create a post request to help us with the output file's json structure.
            var jobsPostRequest = _jobsPostRequestHelper.CreatePostRequest(computedJobs);

            // 6. Serialize the computed jobs and save them to a file.
            try
            {
                FileHelper.SaveJsonSpreadsheetToFile(outputFilePath, _jobsPostRequestHelper.SerializePostRequest(jobsPostRequest));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (int)ExitCode.ErrorWritingOutputFile;
            }

            return (int)ExitCode.Success;
        }

        static int HandleParseError(IEnumerable<Error> errs)
        {
            return (int)ExitCode.ErrorParsingArguments;
        }
    }
}
