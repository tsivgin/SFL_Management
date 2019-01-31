using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFLOnline.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }

        [Range(0, 100)]
        public int Credits { get; set; }



        public virtual ICollection<EnrollmentStudent> Enrollments { get; set; }
        public virtual ICollection<TrackCourses> TrackCourses { get; set; }
        public virtual ICollection<GradePercentage> GradePercentages { get; set; }

    }
}