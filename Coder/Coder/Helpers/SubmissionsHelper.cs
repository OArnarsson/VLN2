using Coder.Models;
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

        public SubmissionsHelper(ApplicationDbContext context)
        {
            submissionRepository = new SubmissionsRepository(context);
        }

        public bool createCppSubmission(ProjectTask task, Submission submission)
        {
            TestResultStatus submissionStatus = TestResultStatus.Accepted;
            Debug.AutoFlush = true;

            var workingFolder = System.IO.Directory.GetCurrentDirectory();
            var projectRoot = System.AppDomain.CurrentDomain.BaseDirectory;
            var submissionFolder = System.IO.Path.Combine(projectRoot, "Uploads\\Submissions\\" + submission.Id);

            Process compiler = new Process();
            compiler.StartInfo.FileName = "cmd.exe";
            compiler.StartInfo.WorkingDirectory = submissionFolder;
            compiler.StartInfo.RedirectStandardInput = true;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.StartInfo.RedirectStandardError = true;
            compiler.StartInfo.UseShellExecute = false;

            // Combine filenames
            var filenames = "";
            foreach (var f in task.FilesRequired)
            {
                filenames += f.Name + " ";
            }
            filenames = filenames.TrimEnd(' ');

            // Compile
            compiler.Start();
            compiler.StandardInput.WriteLine("g++.exe -Wall -o program " + filenames);
            compiler.StandardInput.WriteLine("exit");
            string compilerOutput = compiler.StandardOutput.ReadToEnd();
            string compilerError = compiler.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(compilerError))
            {
                submissionStatus = TestResultStatus.CompileError;
            }

            compiler.WaitForExit();
            compiler.Close();

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
                        Status = (int)testStatus
                    });
                }
            }

            submission.Status = submissionStatus;
            submissionRepository.UpdateState(EntityState.Modified, submission);
            submissionRepository.SaveChanges();


            return true;
        }
    }
}