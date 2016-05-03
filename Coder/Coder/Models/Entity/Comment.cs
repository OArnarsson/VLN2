﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Coder.Models.Entity
{
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public string UserId { get; set; }

        [ForeignKey("ProjectTask")]
        public int ProjectTaskId { get; set; }

        public ProjectTask ProjectTask { get; set; }
    }
}