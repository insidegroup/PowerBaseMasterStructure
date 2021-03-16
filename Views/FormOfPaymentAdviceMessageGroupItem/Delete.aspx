<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.FormOfPaymentAdviceMessageGroupItem>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - FOP Advice Message Items
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">FOP Advice Message Item</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Delete FOP Advice Message Item</th> 
		        </tr> 
                 <tr>
                    <td>Product</td>
                    <td><%= Html.Encode(Model.ProductName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Supplier</td>
                    <td><%= Html.Encode(Model.SupplierName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Country</td>
                    <td class="wrap-text"><%= Html.Encode(Model.CountryName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Travel Indicator</td>
                    <td class="wrap-text"><%= Html.Encode(Model.TravelIndicator)%></td>
                    <td></td>
                </tr> 
				<tr>
                    <td>Language</td>
                    <td class="wrap-text"><%= Html.Encode(Model.LanguageName)%></td>
                    <td></td>
                </tr> 
				<tr>
                    <td>FOP Type</td>
                    <td class="wrap-text"><%= Html.Encode(Model.FormOfPaymentTypeDescription)%></td>
                    <td></td>
                </tr> 
				<tr>
                    <td>FOP Advice Message</td>
                    <td class="wrap-text"><%= Html.Encode(Model.FormOfPaymentAdviceMessage)%></td>
                    <td></td>
                </tr>             
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right" colspan="2">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                        <%= Html.HiddenFor(model => model.VersionNumber) %>
                    <%}%>
                    </td>                
               </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
    	$('#menu_fopadvicemessages').click();
	    $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
</script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("FOP Advice Message Groups", "Main", new { controller = "FormOfPaymentAdviceMessageGroup", action = "ListUnDeleted", }, new { title = "FOP Advice Message Groups" })%> &gt;
<%=Html.RouteLink(Model.FormOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupName, "Default", new { controller = "FormOfPaymentAdviceMessageGroup", action = "View", id = Model.FormOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupID }, new { title = Model.FormOfPaymentAdviceMessageGroup.FormOfPaymentAdviceMessageGroupName })%> &gt;
Items &gt;
Delete
</asp:Content>