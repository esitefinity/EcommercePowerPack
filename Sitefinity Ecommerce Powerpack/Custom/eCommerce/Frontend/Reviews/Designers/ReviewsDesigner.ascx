<%@ Control %>
<%@ Register Assembly="Telerik.Sitefinity" TagPrefix="sf" Namespace="Telerik.Sitefinity.Web.UI" %>
<%@ Register Assembly="Telerik.Sitefinity" TagPrefix="sitefinity" Namespace="Telerik.Sitefinity.Web.UI" %>
<%@ Register Assembly="Telerik.Sitefinity" TagPrefix="sfFields" Namespace="Telerik.Sitefinity.Web.UI.Fields" %>

<sitefinity:ResourceLinks ID="resourcesLinks" runat="server">
    <sitefinity:ResourceFile Name="Styles/Ajax.css" />
    <sitefinity:ResourceFile Name="Styles/jQuery/jquery.ui.core.css" />
    <sitefinity:ResourceFile Name="Styles/jQuery/jquery.ui.dialog.css" />
    <sitefinity:ResourceFile Name="Styles/jQuery/jquery.ui.theme.sitefinity.css" />
</sitefinity:ResourceLinks>
<div class="sfContentViews sfSingleContentView" style="max-height: 400px; overflow: auto; ">
<ol>        
    <li class="sfFormCtrl">
    <label class="sfTxtLbl" for="selectedprofilePageLabel">User Profile Page</label>
    <span style="display: none;" class="sfSelectedItem" id="selectedprofilePageLabel">
        <asp:Literal runat="server" Text="" /></span>
    <span class="sfLinkBtn sfChange">
        <a href="javascript: void(0)" class="sfLinkBtnIn" id="pageSelectButtonprofilePage">
            <asp:Literal runat="server" Text="Select" />
        </a>
    </span>
    <div id="selectorTagprofilePage" runat="server" style="display: none;">
        <sf:PagesSelector runat="server" ID="pageSelectorprofilePage" 
            AllowExternalPagesSelection="false" AllowMultipleSelection="false" />
    </div>
    <div class="sfExample">Page where your user list widget resides</div>
    </li>
    
    <li class="sfFormCtrl">
    <asp:Label runat="server" AssociatedControlID="SuccessMessage" CssClass="sfTxtLbl">Success Message</asp:Label>
    <asp:TextBox ID="SuccessMessage" runat="server" CssClass="sfTxt" />
    <div class="sfExample">Message displayed to a user when a review is submitted</div>
    </li>
    
    <li class="sfFormCtrl">
    <asp:Label runat="server" AssociatedControlID="showCount" CssClass="sfTxtLbl">Show Count</asp:Label>
    <asp:TextBox ID="showCount" runat="server" CssClass="sfTxt" />
    <div class="sfExample">Initial show count of reviews on the page</div>
    </li>
    
    <li class="sfFormCtrl">
    <asp:CheckBox runat="server" ID="requireApproval" Text="Require Approval" CssClass="sfCheckBox"/>        
    <div class="sfExample">Reviews must be approved in the backend before being displayed</div>
    </li>
    
    <li class="sfFormCtrl">
    <asp:CheckBox runat="server" ID="requirePurchase" Text="Require Purchase" CssClass="sfCheckBox"/>        
    <div class="sfExample">Allow only verified buyers to review the product</div>
    </li>
    
    <li class="sfFormCtrl">
    <asp:CheckBox runat="server" ID="rateOnlyOnce" Text="Rate Only Once" CssClass="sfCheckBox"/>        
    <div class="sfExample">Prevent users from reviewing a product more than once</div>
    </li>
    
</ol>
</div>
