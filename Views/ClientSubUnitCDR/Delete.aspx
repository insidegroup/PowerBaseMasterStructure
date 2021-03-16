<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitCDRVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Clients
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client Sub Units - CDR Link</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		           <tr> 
			        <th class="row_header" colspan="3">Delete CDR Link</th> 
		        </tr> 
                 <tr>
                    <td>Client TopUnit Name</td>
                    <td><%= Html.Encode(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName)%></td>
                    <td></td>
                 </tr>
                  <tr>
                    <td>Client SubUnit Name</td>
                    <td><%= Html.Encode(Model.ClientSubUnit.ClientSubUnitName)%></td>
                    <td></td>
                 </tr>
                   <tr>
                    <td>Client SubUnit GUID</td>
                    <td><%= Html.Encode(Model.ClientSubUnit.ClientSubUnitGuid)%></td>
                    <td></td>
                 </tr>
                  <tr>
                    <td>CDR Display Name</td>
                    <td><%= Html.Encode(Model.ClientSubUnitClientDefinedReference.DisplayName)%></td>
                    <td></td>
                 </tr>
                   <tr>
                    <td>CDR Value</td>
                    <td><%= Html.Encode(Model.ClientSubUnitClientDefinedReference.Value)%></td>
                    <td></td>
                 </tr>
                 <tr>
                    <td>Account Name</td>
                    <td>
						<%if(Model.ClientSubUnitClientDefinedReference.ClientAccount.ClientAccountName != null){ %>
							<%= Html.Encode(Model.ClientSubUnitClientDefinedReference.ClientAccount.ClientAccountName)%>
						<%}else{%>
							None
						<%} %>
                    </td>
                    <td></td>
                 </tr>
                    <tr>
                    <td>Account</td>
                    <td>
						<%if(Model.ClientSubUnitClientDefinedReference.ClientAccount.ClientAccountName != null){ %>
							<%= Html.Encode(Model.ClientSubUnitClientDefinedReference.ClientAccount.ClientAccountNumber)%>
						<%}else{%>
							None
						<%} %>
                    </td>
                    <td></td>
                 </tr>
                  <tr>
                    <td>Credit Card Number</td>
                    <td><%if(Model.ClientSubUnitClientDefinedReference.CreditCard.CreditCardValidTo != default(System.DateTime)){ %>
                    <%= Html.Encode(Model.ClientSubUnitClientDefinedReference.CreditCard.MaskedCreditCardNumber)%>
                    <%}else{%>None<%} %></td>
                    <td></td>
                 </tr>
				<tr>
                    <td>Credit Card Holder Name</td>
                    <td><%if(Model.ClientSubUnitClientDefinedReference.CreditCard.CreditCardHolderName != null){ %>
							<%= Html.Encode(Model.ClientSubUnitClientDefinedReference.CreditCard.CreditCardHolderName)%>
						<%}else{%>
							None
						<%} %></td>
                    <td></td>
                 </tr>
                  <tr>
                    <td>Credit Card Valid To</td>
                    <td><%if(Model.ClientSubUnitClientDefinedReference.CreditCard.CreditCardValidTo != default(System.DateTime)){ %>
                    <%= Html.Encode(Model.ClientSubUnitClientDefinedReference.CreditCard.CreditCardValidTo.ToString("MMM yyyy"))%>
                    <%}else{%>None<%} %></td>
                    <td></td>
                 </tr>  
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right" colspan="2">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                        <%= Html.HiddenFor(model => model.ClientSubUnit.ClientSubUnitGuid)%>
                        <%= Html.HiddenFor(model => model.ClientSubUnitClientDefinedReference.ClientSubUnitClientDefinedReferenceId)%>
                        <%= Html.HiddenFor(model => model.ClientSubUnitClientDefinedReference.VersionNumber)%>
                    <%}%>
                    </td>                
               </tr>
            </table>
            
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#breadcrumb').css('width', '725px');
        $('#search').css('width', '5px');
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Top Units", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "Client Top Units" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("Client Sub Units", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = "Client Sub Units" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName, "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
<%=Html.RouteLink("CDR Links", "Main", new {action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "CDR Links" })%>
</asp:Content>