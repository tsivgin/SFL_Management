using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace SFLOnline.Models
{
    public class Student : Person
    {

        public virtual ICollection<EnrollmentStudent>  EnrollmentStudent { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<StudentAttendance> StudentAttendances { get; set; }
        public virtual ICollection<StudentExitGrade> StudentExitGrades { get; set; }
        
    }
}