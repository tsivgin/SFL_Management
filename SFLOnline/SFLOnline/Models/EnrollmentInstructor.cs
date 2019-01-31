using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFLOnline.Models
{
    public class EnrollmentInstructor
    {

        [Key]
        public int Id { get; set; }
        [ForeignKey("Class")]
        public int ClassId { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        [ForeignKey("Instructor")]
        public string InstructorId { get; set; }


        public virtual ICollection<ClassSchedule> ClassSchedules { get; set; }
        public virtual Instructor Instructor { get; set; }
        
        public virtual Class Class { get; set; }
        
        public virtual Course Course { get; set; }
    }
}