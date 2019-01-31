using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFLOnline.Models
{
    public class StudentExitGrade
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Student")]
        public string StudentId { get; set; }

        [ForeignKey("Exit")]
        public int ExitId { get; set; }

        public int grade { get; set; }

        public virtual Student Student { get; set; }
        public virtual Exit Exit { get; set; }

    }
}