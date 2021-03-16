<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitContactsVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnit - Client Detail Contacts</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="36%" class="row_header_left">Last Name</th> 
			        <th width="49%">Job Title</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                    foreach (var item in Model.Contacts)
                    { 
                %>
                <tr>
                    <td><%= Html.Encode(item.LastName) %>, <%= Html.Encode(item.FirstName) %> <%= Html.Encode(item.MiddleName) %></td>
                    <td><%= Html.Encode(item.JobTitle) %></td>
                    <td align="center"><%= Html.ActionLink("View", "View", new { id = item.ContactId })%> </td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("Edit", "Edit", new { id = item.ContactId })%>
                        <%} %>
                    </td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("Delete", "Delete", new { id = item.ContactId })%>
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
                          <% if (Model.Contacts.HasPreviousPage){ %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ClientSubUnitContact", action = "List", id = Model.ClientDetail.ClientDetailId, page = (Model.Contacts.PageIndex - 1) }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                         <% if (Model.Contacts.HasNextPage){ %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ClientSubUnitContact", action = "List", id = Model.ClientDetail.ClientDetailId, page = (Model.Contacts.PageIndex + 1) }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.Contacts.TotalPages > 0){ %>Page <%=Model.Contacts.PageIndex%> of <%=Model.Contacts.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		        <tr> 
                    <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="4"><%= Html.ActionLink("Create Contact", "Create", new { id = Model.ClientDetail.ClientDetailId }, new { @class = "red", title = "Create Contact" })%></td> 
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
