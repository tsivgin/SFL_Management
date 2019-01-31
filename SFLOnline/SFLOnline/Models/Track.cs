using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFLOnline.Models
{
    public class Track
    {
        [Key]
        public int TrackId { get; set; }

        public string TrackName { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<ExitTrack> ExitTracks { get; set; }
        public virtual ICollection<TrackCourses> TrackCourses { get; set; }
    }
}