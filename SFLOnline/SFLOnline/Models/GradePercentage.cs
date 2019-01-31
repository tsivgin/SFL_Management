using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFLOnline.Models
{
    public class GradePercentage
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        [ForeignKey("Track")]
        public int TrackId { get; set; }
        public string name { get; set; }
        [Range(0, 100)]
        public int percentage { get; set; }
        [ForeignKey("Module")]
        public int ModuleId { get; set; }

        public virtual Module Module { get; set; }
        public virtual Course Course { get; set; }
        public virtual Track Track { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
    }
}