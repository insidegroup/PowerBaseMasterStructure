<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ClientAccount>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - ClientDetails
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Account Items</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="2" border="0" width="100%"> 
                <tr> 
	                <th class="row_header" width="100%"><strong>Client Items</strong></th> 
                </tr>  
                <tr> 
                    <td><%= Html.ActionLink("Addresses", "List", "ClientAccountAddress", new { can = Model.ClientAccountNumber, ssc = Model.SourceSystemCode }, null)%></td>
                </tr> 
                <tr> 
                    <td><%= Html.ActionLink("Contacts", "List", "ClientDetailContact", new { can = Model.ClientAccountNumber, ssc = Model.SourceSystemCode }, null)%></td>
                </tr> 	        
                <tr> 
                    <td><%= Html.ActionLink("ESCInformation", "List", "ClientDetailESCInformation", new { can = Model.ClientAccountNumber, ssc = Model.SourceSystemCode }, null)%></td>
                </tr> 
                <tr> 
                    <td><%= Html.ActionLink("SupplierProducts", "List", "ClientDetailSupplierProduct", new { can = Model.ClientAccountNumber, ssc = Model.SourceSystemCode }, null)%></td>
                </tr> 	        
                <tr> 
                    <td><%= Html.ActionLink("FormOfPaymentType", "List", "ClientDetailSubProductFormOfPaymentType", new { can = Model.ClientAccountNumber, ssc = Model.SourceSystemCode }, null)%></td>
                </tr>         
                <tr> 
	                <td class="row_footer"></td> 
                </tr> 
            </table> 
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_clientdetails').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Details", "Main", new { controller = "ClientDetail", action = "ListUnDeleted" }, new { title = "Client Details" })%> &gt;
<%=Html.RouteLink(Model.ClientAccountName, "Default", new { controller = "ClientDetail", action = "View", can = Model.ClientAccountNumber, ssc = Model.ClientAccountNumber }, new { title = Model.ClientAccountName })%> &gt;
Client Detail Items
</asp:Content>