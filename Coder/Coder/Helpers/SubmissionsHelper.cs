using Coder.Models.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Coder.Helpers
{
    public class SubmissionsHelper
    {
        public bool createSubmission(ProjectTask task)
        {
            Debug.AutoFlush = true;

            var workingFolder = System.IO.Directory.GetCurrentDirectory();
            string projectRoot = System.AppDomain.CurrentDomain.BaseDirectory;

            return false;
        }
    }
}