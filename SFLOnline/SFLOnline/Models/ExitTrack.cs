using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFLOnline.Models
{
    public class ExitTrack
    {
        [Key]
        public int ExitTrackId { get; set; }

        [ForeignKey("Exit")]
        public int ExitId { get; set; }

        [ForeignKey("Track")]
        public int TrackId { get; set; }

        public int grade { get; set; }

        public virtual Track Track { get; set; }
        public virtual Exit Exit { get; set; }
    }
}