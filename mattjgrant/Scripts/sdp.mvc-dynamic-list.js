var SDPMVC = {
    DynamicList: function(settings) {
        //Set up the initial state
        var errorMessage = " is not defined on the dynamic list settings";
        if (!settings.ParentElement)
            throw Error("ParentElement" + errorMessage);
        if (!settings.CollectionName)
            throw Error("CollectionName" + errorMessage);
        if (!settings.ListWrapperClass)
            throw Error("ListWrapperClass" + errorMessage);
        if (!settings.TemplateWrapperClass)
            throw Error("TemplateWrapperClass" + errorMessage);
        if (!settings.ListItemClass)
            throw Error("ListItemClass" + errorMessage);

        var parentElement = settings.ParentElement;
        var collectionName = settings.CollectionName;
        var listWrapperClass = settings.ListWrapperClass;
        var templateWrapperClass = settings.TemplateWrapperClass;
        var listItemClass = settings.ListItemClass;
        //This settings is used with nested dynamic lists and assumes that the lists will be applied in order of highest to lowest level
        var parentCollectionName = settings.ParentCollectionName;

        var template;

        //This can be called to reset the element that is being stored as the template
        //This is useful if you have a template that is updated using ajax for example
        var resetTemplate = function () {
            template = parentElement.find(templateWrapperClass + " > " + listItemClass).first().clone();
            //Disable the template's inputs so that it does not get submitted to the server
            parentElement.find(templateWrapperClass + " > " + listItemClass).find("input, select").attr("disabled", "disabled");
        };

        //the template to be cloned in future, update to the latest version of the template by calling resetTemplate()
        resetTemplate();

        //Methods
        var constructIdentifierForElement = function (initialIdentifier, collectionIdentifier, regexDelemiter) {
            if (parentCollectionName) {
                //If there is a parent collection specified then insert the collection details after it but before the property name
                var parentCollectionIdentifier = initialIdentifier.match(parentCollectionName + regexDelemiter);
                var initialCollectionIdentifier = initialIdentifier.match(collectionName + regexDelemiter);
                var splitIdentifier = initialIdentifier.split(parentCollectionIdentifier);
                var suffix = splitIdentifier.pop();
                if (initialCollectionIdentifier)
                    suffix = suffix.replace(initialCollectionIdentifier, "");
                var prefix = splitIdentifier.pop();//Get the elements before the parent collection
                if (prefix)
                    return prefix + parentCollectionIdentifier + collectionIdentifier + suffix;
                return parentCollectionIdentifier + collectionIdentifier + suffix;
            }
            else {
                var initialCollectionIdentifier = initialIdentifier.match(collectionName + regexDelemiter);
                if (initialCollectionIdentifier)
                    return initialIdentifier.replace(initialCollectionIdentifier, collectionIdentifier);
                else
                    return collectionIdentifier + initialIdentifier;
            }
        };

        var updateTemplate = function (index) {

            var namePrefix = collectionName + "[" + index + "].";
            var idPrefix = collectionName + "_" + index + "__";
            var forAttributePrefix = idPrefix;

            template.find("input, select, label").each(function () {
                var currentElement = $(this);
                var name = currentElement.attr("name");
                var id = currentElement.attr("id");
                var forAttribute = currentElement.attr("for");
                if (name)
                    currentElement.attr("name", constructIdentifierForElement(name, namePrefix, "\[[0-9]+\]."));
                if (id)
                    currentElement.attr("id", constructIdentifierForElement(id, idPrefix, "\_[0-9]+\_\_"));
                if (forAttribute)
                    currentElement.attr("for", constructIdentifierForElement(forAttribute, forAttributePrefix, "\[[0-9]+\]."));
            });
        };

        var updateListItem = function (item, index) {
            item.find("input, select, label").each(function () {
                var currentElement = $(this);
                var name = currentElement.attr("name");
                var id = currentElement.attr("id");
                var forAttribute = currentElement.attr("for");

                // Note: this relies on MVCs conventions
                if (name)
                    currentElement.attr("name", name.replace(name.match(collectionName + "\[[0-9]+\]"), collectionName + "[" + index + "]"));
                if (id)
                    currentElement.attr("id", id.replace(id.match(collectionName + "\_[0-9]+"), collectionName + "_" + index));
                if (forAttribute)
                    currentElement.attr("for", forAttribute.replace(forAttribute.match(collectionName + "\_[0-9]+"), collectionName + "_" + index));
            });
        };

        var addItemToEnd = function () {
            var items = parentElement.find(listWrapperClass + " > " + listItemClass);
            updateTemplate(items.length);
            var clone = template.clone();
            clone.show().removeClass("hide");
            var lastItem = items.last();
            if (lastItem.length > 0)
                lastItem.after(clone);
            else
                parentElement.find(listWrapperClass).prepend(clone);
            return false;
        };

        var addItemToStart = function() {
            var items = parentElement.find(listWrapperClass + " > " + listItemClass);
            updateTemplate(0);
            var clone = template.clone();
            clone.show().removeClass("hide");
            var firstItem = items.first();
            if (firstItem.length > 0)
                firstItem.before(clone);
            else
                parentElement.find(listWrapperClass).prepend(clone);

            var nextIndex = 1;//Clone has been placed at index 0
            items.each(function () {
                updateListItem($(this), nextIndex);
                nextIndex++;
            });
            return false;
        };

        var removeItem = function (item) {
            item.slideUp("fast");
            var editableItems = parentElement.find(listWrapperClass + ":not(" + templateWrapperClass + ") > " + listItemClass);
            //Delete specific item and shift the ids of the ones below it
            var nextIndex = 0;
            var elementSeen = false;
            editableItems.each(function () {
                var currentItem = $(this);
                if (elementSeen) {
                    updateListItem(currentItem, nextIndex);
                }
                if (!elementSeen && currentItem.html === item.html)
                    elementSeen = true;
                else
                    nextIndex++;
            });
            item.remove();
        };

        var softDeleteItem = function (item, onRemoveFunction) {
            item.slideUp("fast");
            onRemoveFunction(item);
            return false;
        };

        updateTemplate(0);//Initialise the template

        //Return the object to use with the public methods
        return {
            AddToEnd: addItemToEnd,
            AddToStart: addItemToStart,
            Remove: removeItem,
            SoftDelete: softDeleteItem,
            ResetTemplate: resetTemplate
        }
    }
}
