using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Coder.Models.Entity
{
    public class FileRequired
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        
        public int ProjectTaskId { get; set; }

        [ForeignKey("ProjectTaskId")]
        public virtual ProjectTask ProjectTask { get; set; }
    }
}