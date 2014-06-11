using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mattjgrant.Models
{
    public class Checklist
    {
        public int ChecklistID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Test { get; set; }

        public virtual ICollection<ChecklistItem> ChecklistItems { get; set; }
    }
}