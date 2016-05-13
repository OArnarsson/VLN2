using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Coder.Models.Entity
{
    public class TaskTest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Input { get; set; }
        public string Output { get; set; }

        public int ProjectTaskId { get; set; }

        [ForeignKey("ProjectTaskId")]
        public virtual ProjectTask ProjectTask { get; set; }
    }
}