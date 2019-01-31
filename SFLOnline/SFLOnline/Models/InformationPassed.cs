using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFLOnline.Models
{
    public class InformationPassed
    {
        [Key]
        public int InformationPassedId { get; set; }

        [ForeignKey("Exit")]
        public int ExitId { get; set; }     
        
        public int gradeAverage { get; set; }

        public int passed { get; set; }

        public int AttendanceLimit { get; set; }

        public virtual Exit Exit { get; set; }


    }
}