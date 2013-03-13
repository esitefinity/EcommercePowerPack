Type.registerNamespace("SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers");

SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers.ReviewsDesigner = function (element) {
 
    this._pageSelectorprofilePage = null;
    this._selectorTagprofilePage = null;
    this._profilePageDialog = null;
 
    this._showPageSelectorprofilePageDelegate = null;
    this._pageSelectedprofilePageDelegate = null;
    this._successMessage = null;
    this._showCount = null;
    this._requireApproval = null;
    this._requirePurchase = null;
    this._rateOnlyOnce = null;

    SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers.ReviewsDesigner.initializeBase(this, [element]);
}

SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers.ReviewsDesigner.prototype = {
    /* --------------------------------- set up and tear down --------------------------------- */
    initialize: function () {
        /* Here you can attach to events or do other initialization */
        SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers.ReviewsDesigner.callBaseMethod(this, 'initialize');

        /* Initialize profilePage */
        this._showPageSelectorprofilePageDelegate = Function.createDelegate(this, this._showPageSelectorprofilePageHandler);
        $addHandler(this.get_pageSelectButtonprofilePage(), "click", this._showPageSelectorprofilePageDelegate);

        this._pageSelectedprofilePageDelegate = Function.createDelegate(this, this._pageSelectedprofilePageHandler);
        this.get_pageSelectorprofilePage().add_doneClientSelection(this._pageSelectedprofilePageDelegate);

        if (this._selectorTagprofilePage) {
            this._profilePageDialog = jQuery(this._selectorTagprofilePage).dialog({
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
        SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers.ReviewsDesigner.callBaseMethod(this, 'dispose');

        /* Dispose profilePage */
        if (this._showPageSelectorprofilePageDelegate) {
            $removeHandler(this.get_pageSelectButtonprofilePage(), "click", this._showPageSelectorprofilePageDelegate);
            delete this._showPageSelectorprofilePageDelegate;
        }

        if (this._pageSelectedprofilePageDelegate) {
            this.get_pageSelectorprofilePage().remove_doneClientSelection(this._pageSelectedprofilePageDelegate);
            delete this._pageSelectedprofilePageDelegate;
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

        /* RefreshUI profilePage */
        if (controlData.profilePage && controlData.profilePage !== "00000000-0000-0000-0000-000000000000") {
            var pagesSelectorprofilePage = this.get_pageSelectorprofilePage().get_pageSelector();
            var selectedPageLabelprofilePage = this.get_selectedprofilePageLabel();
            var selectedPageButtonprofilePage = this.get_pageSelectButtonprofilePage();
            pagesSelectorprofilePage.add_selectionApplied(function (o, args) {
                var selectedPage = pagesSelectorprofilePage.get_selectedItem();
                if (selectedPage) {
                    selectedPageLabelprofilePage.innerHTML = selectedPage.Title.Value;
                    jQuery(selectedPageLabelprofilePage).show();
                    selectedPageButtonprofilePage.innerHTML = '<span>Change page</span>';
                }
            });
            pagesSelectorprofilePage.set_selectedItems([{ Id: controlData.profilePage}]);
        }        
        /* RefreshUI SuccessMessage */
        jQuery(this.get_successMessage()).val(controlData.SuccessMessage);
        /* RefreshUI showCount */
        jQuery(this.get_showCount()).val(controlData.showCount);
        /* RefreshUI requireApproval */
        jQuery(this.get_requireApproval()).attr("checked", controlData.requireApproval);
        /* RefreshUI requirePurchase */
        jQuery(this.get_requirePurchase()).attr("checked", controlData.requirePurchase);
        /* RefreshUI rateOnlyOnce */
        jQuery(this.get_rateOnlyOnce()).attr("checked", controlData.rateOnlyOnce);
    },

    /* Called when the "Save" button is clicked. Here you can transfer the settings from the designer to the control */
    applyChanges: function () {
        var controlData = this._propertyEditor.get_control();

        controlData.SuccessMessage = jQuery(this.get_successMessage()).val();
        controlData.showCount = jQuery(this.get_showCount()).val();
        controlData.requireApproval = jQuery(this.get_requireApproval()).is(":checked");
        controlData.requirePurchase = jQuery(this.get_requirePurchase()).is(":checked");
        controlData.rateOnlyOnce = jQuery(this.get_rateOnlyOnce()).is(":checked");
    },


    _showPageSelectorprofilePageHandler: function (selectedItem) {
        var controlData = this._propertyEditor.get_control();
        var pagesSelector = this.get_pageSelectorprofilePage().get_pageSelector();
        if (controlData.profilePage) {
            pagesSelector.set_selectedItems([{ Id: controlData.profilePage }]);
        }
        this._profilePageDialog.dialog("open");
        jQuery("body > form").hide();
        this._profilePageDialog.dialog().parent().css("min-width", "355px");
        dialogBase.resizeToContent();
    },

    _pageSelectedprofilePageHandler: function (items) {
        var controlData = this._propertyEditor.get_control();
        var pagesSelector = this.get_pageSelectorprofilePage().get_pageSelector();
        this._profilePageDialog.dialog("close");
        jQuery("body > form").show();
        dialogBase.resizeToContent();
        if (items == null) {
            return;
        }
        var selectedPage = pagesSelector.get_selectedItem();
        if (selectedPage) {
            this.get_selectedprofilePageLabel().innerHTML = selectedPage.Title.Value;
            jQuery(this.get_selectedprofilePageLabel()).show();
            this.get_pageSelectButtonprofilePage().innerHTML = '<span>Change page</span>';
            controlData.profilePage = selectedPage.Id;
        }
        else {
            jQuery(this.get_selectedprofilePageLabel()).hide();
            this.get_pageSelectButtonprofilePage().innerHTML = '<span>Select</span>';
            controlData.profilePage = "00000000-0000-0000-0000-000000000000";
        }
    },


    /* --------------------------------- properties -------------------------------------- */

    /* profilePage properties */
    get_pageSelectButtonprofilePage: function () {
        if (this._pageSelectButtonprofilePage == null) {
            this._pageSelectButtonprofilePage = this.findElement("pageSelectButtonprofilePage");
        }
        return this._pageSelectButtonprofilePage;
    },
    get_selectedprofilePageLabel: function () {
        if (this._selectedprofilePageLabel == null) {
            this._selectedprofilePageLabel = this.findElement("selectedprofilePageLabel");
        }
        return this._selectedprofilePageLabel;
    },
    get_pageSelectorprofilePage: function () {
        return this._pageSelectorprofilePage;
    },
    set_pageSelectorprofilePage: function (val) {
        this._pageSelectorprofilePage = val;
    },
    get_selectorTagprofilePage: function () {
        return this._selectorTagprofilePage;
    },
    set_selectorTagprofilePage: function (value) {
        this._selectorTagprofilePage = value;
    },

    /* SuccessMessage properties */
    get_successMessage: function () { return this._successMessage; }, 
    set_successMessage: function (value) { this._successMessage = value; },

    /* showCount properties */
    get_showCount: function () { return this._showCount; }, 
    set_showCount: function (value) { this._showCount = value; },

    /* requireApproval properties */
    get_requireApproval: function () { return this._requireApproval; }, 
    set_requireApproval: function (value) { this._requireApproval = value; },

    /* requirePurchase properties */
    get_requirePurchase: function () { return this._requirePurchase; }, 
    set_requirePurchase: function (value) { this._requirePurchase = value; },

    /* rateOnlyOnce properties */
    get_rateOnlyOnce: function () { return this._rateOnlyOnce; }, 
    set_rateOnlyOnce: function (value) { this._rateOnlyOnce = value; }
}

SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers.ReviewsDesigner.registerClass('SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers.ReviewsDesigner', Telerik.Sitefinity.Web.UI.ControlDesign.ControlDesignerBase);

