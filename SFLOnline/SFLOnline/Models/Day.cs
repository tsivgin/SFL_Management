using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFLOnline.Models
{
    public class Day
    {
        
        [Key]
        public int DayId { get; set; }


        public string DaysName { get; set; }

        public int DayNumber { get; set; }

        public virtual ICollection<ClassSchedule> ClassSchedules { get; set; }
    }
}