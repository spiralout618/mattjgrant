$(document).ready(function () {
    Checklist();
});

function Checklist() {
    var classes = {
        Checklist: ".js-checklist",
        ChecklistItem: ".js-checklist-item-editable",
        Template: ".js-checklist-item-template"
    };
    var panel =$(".js-checklist-panel");

    var dynamicListSettings = {
        ParentElement: panel,
        CollectionName: "Items",
        ListWrapperClass: classes.Checklist,
        TemplateWrapperClass: classes.Template,
        ListItemClass: classes.ChecklistItem
    };

    var dynamicList = SDPMVC.DynamicList(dynamicListSettings);

    var updateOrdering = function () {
        var currentCount = 1;
        panel.find(classes.Checklist + " > " + classes.ChecklistItem).each(function () {
            $(this).find(".js-ordering").val(currentCount);
            currentCount++;
        });
    }

    var initialiseChecklistItem = function (item) {
        panel.find(classes.Checklist).sortable(
            {
                update: updateOrdering
            });
        item.find(".js-remove-checklist-item").click(function () {
            dynamicList.Remove(item);
        });
    };

    var initialiseChecklist = function () {
        panel.find(".js-add-checklist-item").click(function () {
            dynamicList.AddToEnd();
            updateOrdering();
            initialiseChecklistItem(panel.find(classes.Checklist + " > " + classes.ChecklistItem).last());
        });

        panel.find(".js-clear-checklist-items").click(function () {
            panel.find(".js-item-checkbox").removeAttr("checked");
        });

        var checklistItems = panel.find(classes.Checklist + " > " + classes.ChecklistItem);
        
        checklistItems.each(function () {
            initialiseChecklistItem($(this));
        });

       
    };

    initialiseChecklist();
}