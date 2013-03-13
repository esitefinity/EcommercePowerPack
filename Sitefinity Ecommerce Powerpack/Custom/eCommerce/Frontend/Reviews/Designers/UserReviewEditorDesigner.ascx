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
    <label class="sfTxtLbl" for="selectedProductsPageLabel">Products Page</label>
    <span style="display: none;" class="sfSelectedItem" id="selectedProductsPageLabel">
        <asp:Literal runat="server" Text="" /></span>
    <span class="sfLinkBtn sfChange">
        <a href="javascript: void(0)" class="sfLinkBtnIn" id="pageSelectButtonProductsPage">
            <asp:Literal runat="server" Text="Select" />
        </a>
    </span>
    <div id="selectorTagProductsPage" runat="server" style="display: none;">
        <sf:PagesSelector runat="server" ID="pageSelectorProductsPage" 
            AllowExternalPagesSelection="false" AllowMultipleSelection="false" />
    </div>
    <div class="sfExample">Page where your product list widget resides</div>
    </li>
    
    <li class="sfFormCtrl">
    <asp:Label runat="server" AssociatedControlID="initialLoad" CssClass="sfTxtLbl">Show Count</asp:Label>
    <asp:TextBox ID="initialLoad" runat="server" CssClass="sfTxt" />
    <div class="sfExample">Initial show count for reviews</div>
    </li>
    
</ol>
</div>
