using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFLOnline.Models
{
    public class Slot
    {
        
        [Key]
        public int SlotId { get; set; }
        
        public string SlotName { get; set; }

        public int SlotNumber { get; set; }

        public virtual ICollection<ClassSchedule> ClassSchedules { get; set; }
    }
}