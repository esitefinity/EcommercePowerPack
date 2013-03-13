Type.registerNamespace("SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers");

SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers.UserReviewEditorDesigner = function (element) {
 
    this._pageSelectorProductsPage = null;
    this._selectorTagProductsPage = null;
    this._ProductsPageDialog = null;
 
    this._showPageSelectorProductsPageDelegate = null;
    this._pageSelectedProductsPageDelegate = null;
    this._initialLoad = null;

    SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers.UserReviewEditorDesigner.initializeBase(this, [element]);
}

SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers.UserReviewEditorDesigner.prototype = {
    /* --------------------------------- set up and tear down --------------------------------- */
    initialize: function () {
        /* Here you can attach to events or do other initialization */
        SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers.UserReviewEditorDesigner.callBaseMethod(this, 'initialize');

        /* Initialize ProductsPage */
        this._showPageSelectorProductsPageDelegate = Function.createDelegate(this, this._showPageSelectorProductsPageHandler);
        $addHandler(this.get_pageSelectButtonProductsPage(), "click", this._showPageSelectorProductsPageDelegate);

        this._pageSelectedProductsPageDelegate = Function.createDelegate(this, this._pageSelectedProductsPageHandler);
        this.get_pageSelectorProductsPage().add_doneClientSelection(this._pageSelectedProductsPageDelegate);

        if (this._selectorTagProductsPage) {
            this._ProductsPageDialog = jQuery(this._selectorTagProductsPage).dialog({
                autoOpen: false,
                modal: false,
                width: 395,
                closeOnEscape: true,
                resizable: false,
                draggable: false,
                zIndex: 5000
            });
        }
    },
    dispose: function () {
        /* this is the place to unbind/dispose the event handlers created in the initialize method */
        SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers.UserReviewEditorDesigner.callBaseMethod(this, 'dispose');

        /* Dispose ProductsPage */
        if (this._showPageSelectorProductsPageDelegate) {
            $removeHandler(this.get_pageSelectButtonProductsPage(), "click", this._showPageSelectorProductsPageDelegate);
            delete this._showPageSelectorProductsPageDelegate;
        }

        if (this._pageSelectedProductsPageDelegate) {
            this.get_pageSelectorProductsPage().remove_doneClientSelection(this._pageSelectedProductsPageDelegate);
            delete this._pageSelectedProductsPageDelegate;
        }
    },

    /* --------------------------------- public methods ---------------------------------- */

    findElement: function (id) {
        var result = jQuery(this.get_element()).find("#" + id).get(0);
        return result;
    },

    /* Called when the designer window gets opened and here is place to "bind" your designer to the control properties */
    refreshUI: function () {
        var controlData = this._propertyEditor.get_control(); /* JavaScript clone of your control - all the control properties will be properties of the controlData too */

        /* RefreshUI ProductsPage */
        if (controlData.ProductsPage && controlData.ProductsPage !== "00000000-0000-0000-0000-000000000000") {
            var pagesSelectorProductsPage = this.get_pageSelectorProductsPage().get_pageSelector();
            var selectedPageLabelProductsPage = this.get_selectedProductsPageLabel();
            var selectedPageButtonProductsPage = this.get_pageSelectButtonProductsPage();
            pagesSelectorProductsPage.add_selectionApplied(function (o, args) {
                var selectedPage = pagesSelectorProductsPage.get_selectedItem();
                if (selectedPage) {
                    selectedPageLabelProductsPage.innerHTML = selectedPage.Title.Value;
                    jQuery(selectedPageLabelProductsPage).show();
                    selectedPageButtonProductsPage.innerHTML = '<span>Change page</span>';
                }
            });
            pagesSelectorProductsPage.set_selectedItems([{ Id: controlData.ProductsPage}]);
        }        
        /* RefreshUI initialLoad */
        jQuery(this.get_initialLoad()).val(controlData.initialLoad);
    },

    /* Called when the "Save" button is clicked. Here you can transfer the settings from the designer to the control */
    applyChanges: function () {
        var controlData = this._propertyEditor.get_control();

        controlData.initialLoad = jQuery(this.get_initialLoad()).val();
    },


    _showPageSelectorProductsPageHandler: function (selectedItem) {
        var controlData = this._propertyEditor.get_control();
        var pagesSelector = this.get_pageSelectorProductsPage().get_pageSelector();
        if (controlData.ProductsPage) {
            pagesSelector.set_selectedItems([{ Id: controlData.ProductsPage }]);
        }
        this._ProductsPageDialog.dialog("open");
        jQuery("body > form").hide();
        this._ProductsPageDialog.dialog().parent().css("min-width", "355px");
        dialogBase.resizeToContent();
    },

    _pageSelectedProductsPageHandler: function (items) {
        var controlData = this._propertyEditor.get_control();
        var pagesSelector = this.get_pageSelectorProductsPage().get_pageSelector();
        this._ProductsPageDialog.dialog("close");
        jQuery("body > form").show();
        dialogBase.resizeToContent();
        if (items == null) {
            return;
        }
        var selectedPage = pagesSelector.get_selectedItem();
        if (selectedPage) {
            this.get_selectedProductsPageLabel().innerHTML = selectedPage.Title.Value;
            jQuery(this.get_selectedProductsPageLabel()).show();
            this.get_pageSelectButtonProductsPage().innerHTML = '<span>Change page</span>';
            controlData.ProductsPage = selectedPage.Id;
        }
        else {
            jQuery(this.get_selectedProductsPageLabel()).hide();
            this.get_pageSelectButtonProductsPage().innerHTML = '<span>Select</span>';
            controlData.ProductsPage = "00000000-0000-0000-0000-000000000000";
        }
    },


    /* --------------------------------- properties -------------------------------------- */

    /* ProductsPage properties */
    get_pageSelectButtonProductsPage: function () {
        if (this._pageSelectButtonProductsPage == null) {
            this._pageSelectButtonProductsPage = this.findElement("pageSelectButtonProductsPage");
        }
        return this._pageSelectButtonProductsPage;
    },
    get_selectedProductsPageLabel: function () {
        if (this._selectedProductsPageLabel == null) {
            this._selectedProductsPageLabel = this.findElement("selectedProductsPageLabel");
        }
        return this._selectedProductsPageLabel;
    },
    get_pageSelectorProductsPage: function () {
        return this._pageSelectorProductsPage;
    },
    set_pageSelectorProductsPage: function (val) {
        this._pageSelectorProductsPage = val;
    },
    get_selectorTagProductsPage: function () {
        return this._selectorTagProductsPage;
    },
    set_selectorTagProductsPage: function (value) {
        this._selectorTagProductsPage = value;
    },

    /* initialLoad properties */
    get_initialLoad: function () { return this._initialLoad; }, 
    set_initialLoad: function (value) { this._initialLoad = value; }
}

SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers.UserReviewEditorDesigner.registerClass('SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers.UserReviewEditorDesigner', Telerik.Sitefinity.Web.UI.ControlDesign.ControlDesignerBase);

