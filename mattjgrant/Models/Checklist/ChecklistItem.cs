using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mattjgrant.Models
{
    public enum ChecklistState{
        Checked, Unchecked, Hidden, Inactive
    }
    public class ChecklistItem
    {
        public int ChecklistItemID { get; set; }
        public int ChecklistID { get; set; }
        public int? NestedChecklistID { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public Checklist Checklist { get; set; }
        public ChecklistState State { get; set; }
    }
}