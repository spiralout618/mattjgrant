using mattjgrant.DAL;
using mattjgrant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mattjgrant.ViewModels
{
    public class ChecklistViewModel
    {
        public int ChecklistID { get; set; }
        public string Name { get; set; }
        public List<ChecklistItemViewModel> Items { get; set; }

        public ChecklistViewModel(){}

        public ChecklistViewModel(Checklist checklist)
        {
            Items = checklist.ChecklistItems.Select(c => new ChecklistItemViewModel(c)).ToList();
        }

        public void CopyToModel(Checklist checklist, ChecklistContext context)
        {
            
            var checklistItems = checklist.ChecklistItems.Where(ci => ci.ChecklistID == ChecklistID).ToList();

            foreach (var checklistItem in checklistItems)
            {
                if (Items.All(i => i.ChecklistItemID != checklistItem.ChecklistItemID))
                    context.ChecklistItems.Remove(checklistItem);
            }

            foreach (var checklistItemViewModel in Items)
            {
                var checklistItem = checklistItems.FirstOrDefault(i => i.ChecklistItemID == checklistItemViewModel.ChecklistItemID);
                if (checklistItem == null)
                {
                    checklistItem = new ChecklistItem();
                    checklistItemViewModel.CopyToModel(checklistItem, ChecklistID);
                    context.ChecklistItems.Add(checklistItem);
                }
                else
                {
                    checklistItemViewModel.CopyToModel(checklistItem, ChecklistID);
                }

            }
        }

        public void AddMetaData(Checklist checklist)
        {
            Name = checklist.Name;
            if (Items == null)
                Items = new List<ChecklistItemViewModel>();
            Items.ForEach(i => i.AddMetaData());
        }
    }

    public class ChecklistItemViewModel
    {
        public int? ChecklistItemID { get; set; }
        public int? NestedChecklistID { get; set; }//For the nested checklist
        public string Name { get; set; }//Name of the checklist item or the nested checklist
        public bool IsChecked { get; set; }

        public bool IsChecklist;

        public ChecklistItemViewModel() { }

        public ChecklistItemViewModel(ChecklistItem item)
        {
            ChecklistItemID = item.ChecklistItemID;
            NestedChecklistID = item.NestedChecklistID;
            Name = item.Name;
            IsChecked = item.State == ChecklistState.Checked;
        }

        public void CopyToModel(ChecklistItem checklistItem, int checklistID)
        {
            checklistItem.ChecklistID = checklistID;
            checklistItem.NestedChecklistID = NestedChecklistID;
            checklistItem.Name = Name;
            checklistItem.State = IsChecked ? ChecklistState.Checked : ChecklistState.Unchecked;
        }

        public void AddMetaData()
        {
            if (NestedChecklistID.HasValue)
                IsChecklist = true;
        }
    }
}