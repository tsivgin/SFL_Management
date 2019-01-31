using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFLOnline.Models
{
    public class Announcement
    {
        [Key]
        public int AnnouncementId { get; set; }
        [ForeignKey("Person")]
        public string WriterId { get; set; }
        [ForeignKey("Class")]
        public int ClassId { get; set; }

        public string AnnouncementName { get; set; }

        public string description { get; set; }

        public virtual Class Class { get; set; }
        public virtual Person Person { get; set; }
    }
}