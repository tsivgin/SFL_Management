using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFLOnline.Models
{
    public class Exit
    {
        [Key]
        public int ExitId { get; set; }

        public string ExitName { get; set; }        

        public Boolean ForEnrollment { get; set; }

        public virtual ICollection<ExitTrack> ExitTracks { get; set; }
        public virtual ICollection<InformationPassed> InformationPasseds { get; set; }
        public virtual ICollection<StudentExitGrade> StudentExitGrades { get; set; }
    }
}