﻿using Coder.Models;
using Coder.Models.Entity;
using Coder.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Coder.Helpers
{
    public class SubmissionsHelper
    {
        private SubmissionsRepository submissionRepository;

        /*
        * Initialization.
        */
        public SubmissionsHelper(ApplicationDbContext context)
        {
            submissionRepository = new SubmissionsRepository(context);
        }

        /*
        * Initialization, locates the submission folder.
        */
        public string GetSubmissionFolder(Submission submission)
        {
            var projectRoot = AppDomain.CurrentDomain.BaseDirectory;
            return System.IO.Path.Combine(projectRoot, "Uploads\\Submissions\\" + submission.Id);
        }

        /*
        * Handles the files handed in, see comments inside.
        */
        public bool CreateCppSubmission(ProjectTask task, Submission submission)
        {
            // Innocent until proven guilty
            var submissionStatus = TestResultStatus.Accepted;

            var submissionFolder = GetSubmissionFolder(submission);

            var compiler = new Process();
            compiler.StartInfo.FileName = "cmd.exe";
            compiler.StartInfo.WorkingDirectory = submissionFolder;
            compiler.StartInfo.RedirectStandardInput = true;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.StartInfo.RedirectStandardError = true;
            compiler.StartInfo.UseShellExecute = false;

            // Combine filenames
            var filenames = string.Join(" ", (from f in task.FilesRequired select f.Name).ToArray());

            // Compile
            compiler.Start();
            compiler.StandardInput.WriteLine("g++.exe -Wall -o program " + filenames);
            compiler.StandardInput.WriteLine("exit");
            compiler.WaitForExit();
            var compilerOutput = compiler.StandardOutput.ReadToEnd();
            var compilerError = compiler.StandardError.ReadToEnd();
            compiler.Close();

            // Check for compilation error
            if (!string.IsNullOrEmpty(compilerError))
            {
                submissionStatus = TestResultStatus.CompileError;
                submission.ErrorMessage = compilerError;
            }
            else
            {
                // Compilation successful so we run the program

                // Path to program
                var program = System.IO.Path.Combine(submissionFolder, "program.exe");

                // Run tests
                foreach (var test in task.TaskTests)
                {
                    var testStatus = TestResultStatus.Accepted;
                    var processInfoExe = new ProcessStartInfo(program, "");
                    processInfoExe.UseShellExecute = false;
                    processInfoExe.RedirectStandardOutput = true;
                    processInfoExe.RedirectStandardInput = true;
                    processInfoExe.CreateNoWindow = true;
                    using (var processExe = new Process())
                    {
                        processExe.StartInfo = processInfoExe;
                        processExe.Start();
                        processExe.StandardInput.WriteLine(test.Input);

                        // Wait for 5 seconds
                        if (!processExe.WaitForExit(5000))
                        {
                            // Program has not finished after 5 seconds. Kill it.
                            submissionStatus = TestResultStatus.TimeLimitExceeded;
                            testStatus = TestResultStatus.TimeLimitExceeded;
                            processExe.Kill();
                        }
                        else
                        {
                            var output = processExe.StandardOutput.ReadToEnd();

                            if (output != test.Output)
                            {
                                submissionStatus = TestResultStatus.WrongOutput;
                                testStatus = TestResultStatus.WrongOutput;
                            }

                            submissionRepository.AddSubmissionTestResult(new SubmissionTestResult
                            {
                                Input = test.Input,
                                Output = test.Output,
                                ObtainedOutput = output,
                                SubmissionId = submission.Id,
                                Status = testStatus
                            });
                        }
                    }
                }
            }

            submission.Status = submissionStatus;
            submissionRepository.UpdateState(EntityState.Modified, submission);
            submissionRepository.SaveChanges();

            // If accepted return true
            return submissionStatus == TestResultStatus.Accepted;
        }
    }
}