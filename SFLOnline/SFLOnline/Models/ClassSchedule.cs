using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFLOnline.Models
{
    public class ClassSchedule
    {
        [Key]
        public int ClassScheduleId { get; set; }
        [ForeignKey("EnrollmentInstructor")]
        public int EnrollmentInstructorId { get; set; }
        [ForeignKey("Slot")]
        public int SlotId { get; set; }
        [ForeignKey("Day")]
        public int DayId { get; set; }


        public virtual EnrollmentInstructor EnrollmentInstructor { get; set; }
        public virtual Slot Slot { get; set; }
        public virtual Day Day { get; set; }
    }
}