<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserRewiewEditor.ascx.cs" Inherits="SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Controls.UserRewiewEditor" %>
<%@ Register TagPrefix="sf" Assembly="Telerik.Sitefinity" Namespace="Telerik.Sitefinity.Web.UI.PublicControls" %>

<style>
    .sfReviewBtn
    {
        -moz-box-shadow: inset 0px 1px 0px 0px #ffffff;
        -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff;
        box-shadow: inset 0px 1px 0px 0px #ffffff;
        background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #ededed), color-stop(1, #dfdfdf) );
        background: -moz-linear-gradient( center top, #ededed 5%, #dfdfdf 100% );
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf');
        background-color: #ededed;
        -moz-border-radius: 6px;
        -webkit-border-radius: 6px;
        border-radius: 6px;
        border: 1px solid #dcdcdc;
        display: inline-block;
        color: #777777;
        font-family: arial;
        font-size: 15px;
        font-weight: bold;
        padding: 6px 24px;
        text-decoration: none;
        text-shadow: 1px 1px 0px #ffffff;
        cursor: pointer;
    }

        .sfReviewBtn:hover
        {
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #dfdfdf), color-stop(1, #ededed) );
            background: -moz-linear-gradient( center top, #dfdfdf 5%, #ededed 100% );
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#dfdfdf', endColorstr='#ededed');
            background-color: #dfdfdf;
        }

        .sfReviewBtn:active
        {
            position: relative;
            top: 1px;
        }
    .sfReviewItm
    {
        margin-bottom: 15px;
        border-bottom: 1px solid #f7f7f7;
        padding: 15px 0px;
    }

    .sfUsrReviewWrp h2
    {
        font-size: 20px;
    }

    .sfReviewDetailWrp
    {
        padding: 10px;
    }

    .sfReviewDeleteSuccess
    {
        background-color: #a6d175;
        color: #ffffff;
        font-size: 15px;
        padding: 10px;
        margin-bottom: 15px;
    }
    .sfBtnDeleteReview
    {
        background-color: transparent;
        border: none;
        outline: none;
        margin: 0 10px;
        padding: 0;
    }
    .sfReviewOptions
    {
        text-align: right;
    }
</style>
<sf:sitefinityupdatepanel id="SUP" runat="server">
    <ContentTemplate> 
<div class="sfUsrReviewWrp" onload="upDateTimes">
    <h2>Your product reviews</h2>
    <asp:Panel ID="pnlMessage" runat="server" Visible="false" ClientIDMode="Static" >
        <asp:Label ID="lblSuccessOrError" runat="server" Text="Your review has been deleted." />
    </asp:Panel>
    <telerik:RadListView ID="listReviews" runat="server" ItemPlaceholderID="holder" OnItemDataBound="listReviews_ItemDataBound1" OnItemCommand="listReviews_ItemCommand1" ClientSettings-ClientEvents-OnListViewCreated="upDateTimes">
        <LayoutTemplate>
            <ul>
                <asp:PlaceHolder ID="holder" runat="server" />
            </ul>
        </LayoutTemplate>
        <ItemTemplate>
            <li class="sfReviewItm">
            <div class="sfReviewHead">
                <asp:HyperLink ID="lnkProduct" runat="server" Text='<%# Eval("Title") %>' />
                
            </div>
            <div>
                <table>
                    <tr>
                        <td style="width: 104px;">
                            <telerik:RadRating ID="rating" runat="server" ReadOnly="true" Value='<%# Eval("Rating") %>' />
                        </td>

                        <td style="text-align: right; width: 100%; border-spacing: 0 10px; margin-left: 10px !important;">
                            <span title='<%# Eval("Date") %>'><%# Eval("Date") %></span>
                        </td>

                    </tr>
                </table>
            </div>
            <div class="sfReviewDetailWrp">
                <table>
                    <tr>
                        <td style="width: 100px; vertical-align: top; text-align: center;">
                            <telerik:RadBinaryImage ID="imgProduct" runat="server" Height="100" Width="100" AlternateText='<%# Eval("Product") %>' ImageUrl="/SFRes/images/Telerik.Sitefinity.Resources/Images.DefaultProductTmb.png" />
                            <asp:Label ID="lblProductName" runat="server" style="margin-top: 10px;" />
                        </td>
                        <td style="width: 100%; vertical-align: top;">
                            <div style="margin-left: 10px;">
                                <%# Eval("Thoughts") %>
                            </div>
                            
                        </td>
                    </tr>
                </table>
                
            </div>
            <div class="sfReviewOptions">
                <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("Id").ToString() %>' Text="Delete" CommandName="DeleteReview" CssClass="sfBtnDeleteReview"/>
            </div>
                </li>
        </ItemTemplate>
    </telerik:RadListView>
</div>
        <div style="text-align: center;">
            <asp:Button ID="btnLoadMoreReviews" runat="server" CssClass="sfReviewBtn" OnClick="btnLoadMoreReviews_Click" Text="Load more reviews" style="width: 100%;" />
            </div>
        
<asp:HiddenField ID="hdLoadCount" runat="server" />
<script>
    function upDateTimes() {

        if ($('#pnlMessage').is(':visible')) {
            setTimeout(function () {
                $('#pnlMessage').fadeOut('fast');
            }, 4000);
        }

        var elems = document.getElementsByTagName('span');
        for (i in elems) {
            if (elems[i].title != null && elems[i].title.length > 0) {
                elems[i].innerHTML = prettyDate(elems[i].title);
            }
        }
        
    }

    function prettyDate(date_str) {
        var time_formats = [
        [60, 'just now', ''], // 60
        [120, '1 minute ago', '1 minute from now'], // 60*2
        [3600, 'minutes', 60], // 60*60, 60
        [7200, '1 hour ago', '1 hour from now'], // 60*60*2
        [86400, 'hours', 3600], // 60*60*24, 60*60
        [172800, 'yesterday', 'tomorrow'], // 60*60*24*2
        [604800, 'days', 86400], // 60*60*24*7, 60*60*24
        [1209600, 'last week', 'next week'], // 60*60*24*7*4*2
        [2419200, 'weeks', 604800], // 60*60*24*7*4, 60*60*24*7
        [4838400, 'last month', 'next month'], // 60*60*24*7*4*2
        [29030400, 'months', 2419200], // 60*60*24*7*4*12, 60*60*24*7*4
        [58060800, 'last year', 'next year'], // 60*60*24*7*4*12*2
        [2903040000, 'years', 29030400], // 60*60*24*7*4*12*100, 60*60*24*7*4*12
        [5806080000, 'last century', 'next century'], // 60*60*24*7*4*12*100*2
        [58060800000, 'centuries', 2903040000] // 60*60*24*7*4*12*100*20, 60*60*24*7*4*12*100
        ];
        var time = ('' + date_str).replace(/-/g, "/").replace(/[TZ]/g, " ").replace(/^\s\s*/, '').replace(/\s\s*$/, '');
        if (time.substr(time.length - 4, 1) == ".") time = time.substr(0, time.length - 4);
        var seconds = (new Date - new Date(time)) / 1000;
        var token = 'ago', list_choice = 1;
        if (seconds < 0) {
            seconds = Math.abs(seconds);
            token = 'from now';
            list_choice = 2;
        }
        var i = 0, format;
        while (format = time_formats[i++])
            if (seconds < format[0]) {
                if (typeof format[2] == 'string')
                    return format[list_choice];
                else
                    return Math.floor(seconds / format[2]) + ' ' + format[1] + ' ' + token;
            }
        return time;
    };
</script>
    </ContentTemplate>
</sf:sitefinityupdatepanel>