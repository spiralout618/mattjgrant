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

    var initialiseChecklistItem = function (item) {
        item.find(".js-remove-checklist-item").click(function () {
            dynamicList.Remove(item);
        });
    };

    var initialiseChecklist = function () {
        panel.find(".js-add-checklist-item").click(function () {
            dynamicList.AddToEnd();
            initialiseChecklistItem(panel.find(classes.Checklist + " > " + classes.ChecklistItem).last());
        });

        panel.find(classes.Checklist + " > " + classes.ChecklistItem).each(function () {
            initialiseChecklistItem($(this));
        });
    };

    initialiseChecklist();
}