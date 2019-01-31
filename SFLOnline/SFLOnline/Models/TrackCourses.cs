using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFLOnline.Models
{
    public class TrackCourses
    {

        [Key]
        public int Id { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        [ForeignKey("Track")]
        public int TrackId { get; set; }

        public virtual Track Track { get; set; }
        public virtual Course Course { get; set; }


    }
}