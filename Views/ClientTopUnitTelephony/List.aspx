<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientTopUnitTelephoniesVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client TopUnit - Telephony</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="30%" class="row_header_left">Dialed Number</th> 
			        <th width="70%" class="row_header_right">Identifier</th> 
		        </tr> 
                <%
                    foreach (var item in Model.Telephonies)
                    { 
                %>
                <tr>
                    <td><%= Html.Encode(item.DialedNumber)%></td>
                    <td><%= Html.Encode(item.CallerEnteredDigitDefinitionTypeDescription) %></td>
                </tr>        
                <% 
                } 
                %>
                 <tr>
                <td colspan="2" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                          <% if (Model.Telephonies.HasPreviousPage)
                             { %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ClientTopUnitTelephony", action = "List", id = Model.ClientTopUnit.ClientTopUnitGuid, page = (Model.Telephonies.PageIndex - 1) }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                         <% if (Model.Telephonies.HasNextPage)
                            { %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ClientTopUnitTelephony", action = "List", id = Model.ClientTopUnit.ClientTopUnitGuid, page = (Model.Telephonies.PageIndex + 1) }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.Telephonies.TotalPages > 0)
                                                     { %>Page <%=Model.Telephonies.PageIndex%> of <%=Model.Telephonies.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		        <tr> 
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right"></td> 
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
<%=Html.RouteLink(Model.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientTopUnit.ClientTopUnitName })%> &gt;
Telephony
</asp:Content>
