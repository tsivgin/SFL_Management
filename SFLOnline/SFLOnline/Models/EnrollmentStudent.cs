using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SFLOnline.Models
{
    public class EnrollmentStudent
    {

        [Key]
        public int Id { get; set; }
        [ForeignKey("Class")]
        public int ClassId { get; set; }
        [ForeignKey("Student")]
        public string StudentId { get; set; }


        public virtual Student Student { get; set; }
        public virtual Class Class { get; set; }

    }
}