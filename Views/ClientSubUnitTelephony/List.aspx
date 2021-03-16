<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitTelephoniesVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnit - Telephony</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="30%" class="row_header_left">Dialled Number</th> 
			        <th width="30%">Identifier</th> 
			        <th width="29%"></th> 
                    <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 	
		        </tr> 
                <%
                    foreach (var item in Model.Telephonies)
                    { 
                %>
                <tr>
                    <td><%= Html.Encode(item.DialedNumber)%></td>
                    <td colspan="2"><%= Html.Encode(item.CallerEnteredDigitDefinitionTypeDescription) %></td>
                     <td>
                    <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.RouteLink("Edit", "Main", new { action = "Edit", dn = item.DialedNumber, id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "Edit" })%>
                    <%} %>
                </td>
                <td>
                    <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.RouteLink("Delete", "Main", new { action = "Delete", dn = item.DialedNumber, id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = "Delete" })%>
                    <%} %>
                </td>     
                </tr>        
                <% 
                } 
                %>
                 <tr>
                <td colspan="5" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                          <% if (Model.Telephonies.HasPreviousPage)
                             { %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ClientSubUnitTelephony", action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid, page = (Model.Telephonies.PageIndex - 1) }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                         <% if (Model.Telephonies.HasNextPage)
                            { %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ClientSubUnitTelephony", action = "List", id = Model.ClientSubUnit.ClientSubUnitGuid, page = (Model.Telephonies.PageIndex + 1) }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.Telephonies.TotalPages > 0)
                                                     { %>Page <%=Model.Telephonies.PageIndex%> of <%=Model.Telephonies.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		        <tr> 
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="3"><%if (ViewData["Access"] == "WriteAccess"){ %><%= Html.ActionLink("Create Telephony", "Create", new { id = Model.ClientSubUnit.ClientSubUnitGuid }, new { @class = "red", title = "Create Telephony" })%> <%}%> </td> 
		        </tr> 
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Top Units", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "Client Top Units" })%> &gt;

<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("Client Sub Units", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientSubUnit.ClientTopUnit.ClientTopUnitGuid }, new { title = "Client Sub Units" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName, "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid }, new { title = Model.ClientSubUnit.ClientSubUnitName })%> &gt;
Telephony
</asp:Content>
