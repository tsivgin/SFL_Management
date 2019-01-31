using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFLOnline.Models
{
    public class Class
    {
        [Key]
        public int ClassId { get; set; }

        public string ClassName { get; set; }

        public int Quota { get; set; }

        public int RequiredQuota { get; set; }
        [ForeignKey("Track")]
        public int TrackId { get; set; }


        public virtual Track Track { get; set; }
        public virtual ICollection<Announcement> Announcements { get; set; }
        public virtual ICollection<EnrollmentStudent> Enrollment { get; set; }
        public virtual ICollection<EnrollmentInstructor> EnrollmentsInstructor { get; set; }

    }
}