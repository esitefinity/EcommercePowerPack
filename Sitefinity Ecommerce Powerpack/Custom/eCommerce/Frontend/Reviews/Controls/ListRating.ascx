<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Register TagPrefix="sf" Namespace="Telerik.Sitefinity.Web.UI" Assembly="Telerik.Sitefinity" %>
<%@ Register TagPrefix="sf" Namespace="Telerik.Sitefinity.Web.UI.ContentUI" Assembly="Telerik.Sitefinity" %>
<style>
    .sfRatingLnk
    {
        font-size: 12px;
        
    }
</style>
<div class="sfProductRatingWrp">
    <table>
        <tr>
            <td style="vertical-align: middle; max-width: 104px;">
                <telerik:RadRating ID="rating" runat="server" ReadOnly="true" Value="0" />
            </td>
            <td style="vertical-align: middle;">
                <asp:Label ID="lblRating" runat="server" />
                <sf:DetailsViewHyperLink ID="lblGoToReviews" runat="server" CssClass="sfRatingLnk" />
            </td>
        </tr>
    </table>
    
</div>