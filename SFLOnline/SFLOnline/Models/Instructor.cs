using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFLOnline.Models
{
    public class Instructor : Person
    {


        public virtual ICollection<EnrollmentInstructor> Enrollments { get; set; }
        
    }
}