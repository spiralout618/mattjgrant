using mattjgrant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mattjgrant.ViewModels
{
    public class ChecklistViewModel
    {
        public string Name { get; set; }
        public List<ChecklistItemViewModel> Items { get; set; }

        public ChecklistViewModel(){}

        public ChecklistViewModel(Checklist checklist)
        {
            Name = checklist.Name ?? "";
            Items = checklist.ChecklistItems.Select(c => new ChecklistItemViewModel(c)).ToList();
        }
    }

    public class ChecklistItemViewModel
    {
        public int? ChecklistID { get; set; }//For the nested checklist
        public string Name { get; set; }//Name of the checklist item or the nested checklist
        public bool IsChecked { get; set; }

        public bool IsChecklist;

        public ChecklistItemViewModel() { }

        public ChecklistItemViewModel(ChecklistItem item)
        {

        }

        public void AddMetaData()
        {
            if (ChecklistID.HasValue)
                IsChecklist = true;
        }
    }
}