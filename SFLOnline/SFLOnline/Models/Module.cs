using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFLOnline.Models
{
    public class Module
    {
        [Key]
        public int ModuleId { get; set; }

        public string ModuleName { get; set; }

        public bool active { get; set; }

        public virtual ICollection<GradePercentage> GradePercentages { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
    }
}