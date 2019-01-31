using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFLOnline.Models
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Student")]
        public string StudentId { get; set; }
        [ForeignKey("GradePercentage")]
        public int GradePercentageId { get; set; }
        [ForeignKey("Module")]
        public int ModuleId { get; set; }        
        public int Grades { get; set; }


        public virtual Student Student { get; set; }
        public virtual GradePercentage GradePercentage { get; set; }
        public virtual Module Module { get; set; }

    }
}