<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Reviews.ascx.cs" Inherits="SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Controls.Reviews" %>
<%@ Register TagPrefix="sf" Assembly="Telerik.Sitefinity" Namespace="Telerik.Sitefinity.Web.UI.PublicControls" %>
<%@ Register TagPrefix="sf" Namespace="Telerik.Sitefinity.Web.UI" Assembly="Telerik.Sitefinity" %>
<link href="/Custom/eCommerce/Styles/Reviews/ReviewLight.css" rel="stylesheet" />
<sf:sitefinityupdatepanel id="SUP" runat="server">
    <ContentTemplate>
        
        <div class="sfReviewWrp">
            <div class="sfReviewHead">
                <asp:Label ID="lbl" runat="server" Text="Ratings and Reviews" CssClass="sfReviewHeadTitle" />
                <span class="sfReviewAvg" >
                    <telerik:RadRating ID="ratingOverview" runat="server" Orientation="Horizontal" Precision="Exact" ItemCount="5" SelectionMode="Continuous" ReadOnly="true"/>
                </span>
            </div>
            <div class="sfReviewInteract">
                <a onclick="showPanel()" id="btnShowReview" runat="server" ClientIdMode="Static" style="cursor: pointer;">Add a review</a>
                <asp:Label ID="lblReviewed" runat="server" Text="You have already reviewed this product" Visible="false" />
                <span style="float: right; display: block;">
                                <telerik:RadComboBox ID="comboFilter" runat="server" OnSelectedIndexChanged="comboFilter_SelectedIndexChanged" AutoPostBack="true" CausesValidation="false" AllowCustomText="false" Width="100">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="0" Text="Show all" />
                                        <telerik:RadComboBoxItem Value="1" Text="1 star" />
                                        <telerik:RadComboBoxItem Value="2" Text="2 stars" />
                                        <telerik:RadComboBoxItem Value="3" Text="3 stars" />
                                        <telerik:RadComboBoxItem Value="4" Text="4 stars" />
                                        <telerik:RadComboBoxItem Value="5" Text="5 stars" />
                                    </Items>
                                </telerik:RadComboBox>
                            </span>
                
            </div>
            <div style="width: 100%;">
                <asp:Panel ID="addReview" runat="server" ClientIDMode="Static" >
                    <div class="sfReviewInsertWrp">
                    <table>
                        <tr>
                            <td style="width: 104px; vertical-align: top;">
                                <telerik:RadRating ID="addRate" runat="server" Orientation="Horizontal" Precision="Item" ItemCount="5" SelectionMode="Continuous" Value="1" CssClass="rateInsert"/>
                                <telerik:RadBinaryImage ID="imgCurrentAvatar" runat="server" ResizeMode="Crop" Height="100" Width="100" ImageUrl="/SFRes/images/Telerik.Sitefinity.Resources/Images.DefaultPhoto.png" />
                            </td>
                            <td style="width: 100%; vertical-align: top;">
                                <div class="sfReviewAddTitleWrp">
                                    <asp:TextBox ID="txtTitle" CssClass="sfReviewTitleTxt" runat="server" placeholder="Your review title" />
                                    <asp:RequiredFieldValidator ID="validateTitle" runat="server" ControlToValidate="txtTitle" />
                                </div>
                                <div class="sfReviewAddThoughtsWrp">
                                    <asp:TextBox ID="txtComment" CssClass="sfReviewThoughtsTxt" runat="server" TextMode="MultiLine" placeholder="Your thoughts on this product"/>
                                    <asp:RequiredFieldValidator ID="validateThoughts" runat="server" ControlToValidate="txtComment" />
                                    
                                </div>
                                <div class="sfReviewSubmitWrp" style="text-align: right;">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit Review" OnClick="btnSubmit_Click" CssClass="sfReviewBtn"/>
                                </div>
                                
                                
                            </td>
                        </tr>
                    </table>
                    </div>
                </asp:Panel>
                <asp:Panel ID="response" runat="server" ClientIDMode="Static" Visible="false">
                    <div class="sfReviewResponse">
                        <asp:Label ID="lblErrorOrSuccess" runat="server" ClientIDMode="Static" />
                    </div>
                </asp:Panel>
            </div>
            <div class="sfReviewBody">
                <telerik:RadListView ID="listReviews" runat="server" ItemPlaceholderID="holder" OnItemDataBound="listReviews_ItemDataBound" ClientSettings-ClientEvents-OnListViewCreated='pageChecks'>
                    <LayoutTemplate>
                        <ul class="sfReviewList">
                            <asp:PlaceHolder ID="holder" runat="server" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li class="sfReviewItem">
                            <table style="width: 100% !important;">
                                <tr>
                                    <td style="max-width: 104px; vertical-align: top; text-align: center;">
                                        
                                        <telerik:RadRating ID="rating" runat="server"  ItemCount="5" Value='<%# Eval("Rating") %>' Precision="Item"  Orientation="Horizontal" ReadOnly="true"  />
                                        <telerik:RadBinaryImage ID="imgAvatar" runat="server" ResizeMode="Crop" Height="100" Width="100" ImageUrl="/SFRes/images/Telerik.Sitefinity.Resources/Images.DefaultPhoto.png" />
                                        <asp:Label ID="lblVerified" runat="server" Text="Verified Buyer" CssClass="sfReviewVerified" Visible='<%# Eval("VerifiedBuyer") %>' />
                                    </td>
                                    <td style="width: 100%; vertical-align: top; padding: 0 5px;">
                                        <div class="sfReviewItmTitle">
                                            <%# Eval("Title") %>
                                        </div>
                                        <div> by
                                            <asp:HyperLink ID="lnkReviewer" runat="server" Text='<%# Eval("Username") %>' CssClass="lnkProfile" />
                                            <span title='<%# Eval("Date") %>'><%# Eval("Date") %></span>
                                        </div>
                                        <span class="sfReviewItmThoughts">
                                            <%# Eval("Thoughts") %>
                                        </span>
                                        
                                    </td>
                                </tr>
                            </table>
                            
                        </li>
                    </ItemTemplate>
                </telerik:RadListView>
                <div style="text-align: center;">
                    <asp:Button ID="btnLoadMore" runat="server" Text="Load More Reviews" CausesValidation="false" CssClass="sfReviewBtn" style="width: 100%;"  />
                    <asp:HiddenField ID="hdCount" runat="server" Value="4" />
                </div>
            </div>
        </div>
<script>
    $(document).ready(function () {
        $('#addReview').hide();
    });

    function upDateTimes() {
        var elems = document.getElementsByTagName('span');
        for (i in elems) {
            if (elems[i].title != null && elems[i].title.length > 0) {
                elems[i].innerHTML = prettyDate(elems[i].title);
            }
        }
    }

    function showPanel() {
        if ($('#addReview').is(':visible')) {
            $('#addReview').hide('fast');
            $('#btnShowReview').html('Add a review');
        }
        else {
            $('#addReview').show('fast');
            $('#btnShowReview').html('Close');
        }
    }

    function pageChecks() {
        $('#addReview').hide();
        if ($('#response').is(':visible')) {
            setTimeout(function () {
                $('#response').fadeOut('fast');
            }, 4000);
        }
        upDateTimes();
    }


    /*
     * JavaScript Pretty Date
     * Copyright (c) 2011 John Resig (ejohn.org)
     * Licensed under the MIT and GPL licenses.
     */
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
