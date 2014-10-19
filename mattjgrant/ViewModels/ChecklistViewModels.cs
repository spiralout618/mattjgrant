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
        public int? ParentChecklistID { get; set; }
        public string Name { get; set; }
        public List<ChecklistItemViewModel> Items { get; set; }

        public ChecklistViewModel(){}

        public ChecklistViewModel(Checklist checklist)
        {
            ChecklistID = checklist.ChecklistID;
            Items = checklist.ChecklistItems.OrderBy(c => c.Order).Select(c => new ChecklistItemViewModel(c)).ToList();
        }

        public void CopyToModel(Checklist checklist, WebsiteContext context)
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
        public int? ChecklistID { get; set; }
        public string Name { get; set; }//Name of the checklist item or the nested checklist
        public bool IsChecked { get; set; }
        public int Order { get; set; }

        public bool IsChecklist;

        public ChecklistItemViewModel() { }

        public ChecklistItemViewModel(ChecklistItem item)
        {
            ChecklistID = item.ChecklistID;
            ChecklistItemID = item.ChecklistItemID;
            NestedChecklistID = item.NestedChecklistID;
            Name = item.Name;
            IsChecked = item.State == ChecklistState.Checked;
            Order = item.Order;
        }

        public void CopyToModel(ChecklistItem checklistItem, int checklistID)
        {
            checklistItem.ChecklistID = checklistID;
            checklistItem.NestedChecklistID = NestedChecklistID;
            
            checklistItem.State = IsChecked ? ChecklistState.Checked : ChecklistState.Unchecked;
            checklistItem.Order = Order;
            if (!IsChecklist)
            {
                checklistItem.Name = Name;
            }

        }

        public void AddMetaData()
        {
            if (NestedChecklistID.HasValue)
                IsChecklist = true;
        }
    }

    public class NestedChecklistViewModel
    {
        public int NestedChecklistID { get; set; }
        public int ChecklistID { get; set; }

        public List<NestedChecklistOption> Options { get; set; }

        public NestedChecklistViewModel() { }

        public NestedChecklistViewModel(int checklistID)
        {
            ChecklistID = checklistID;
        }

        public void AddMetaData(WebsiteContext context)
        {
            Options = context.Checklists.ToList().Select(c => new NestedChecklistOption(c)).ToList();
        }
    }

    public class NestedChecklistOption
    {
        public string Name;
        public int ChecklistID;

        public NestedChecklistOption(Checklist checklist)
        {
            Name = checklist.Name;
            ChecklistID = checklist.ChecklistID;
        }
    }

    public class CopyChecklistViewModel
    {
        public int OriginalChecklistID { get; set; }
        public string OldName { get; set; }
        public string NewName { get; set; }
    }
}