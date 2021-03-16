<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientTopUnitSubProductFormOfPaymentTypesVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client TopUnit - Client Detail SubProduct FormOfPaymentTypes</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="40%" class="row_header_left">SubProduct Name</th> 
			        <th width="53%">Form Of Payment Type</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                    foreach (var item in Model.SubProductFormOfPaymentTypes)
                    { 
                %>
                <tr>
                    <td><%= Html.Encode(item.SubProductName)%></td>
                    <td><%= Html.Encode(item.FormOfPaymentTypeDescription) %></td>
                    <td>
                        <%if (ViewData["Access"] == "WriteAccess"){ %>
                        <%=  Html.ActionLink("Delete", "Delete", new { id = item.ClientDetailId, sp = item.SubProductId, fpt = item.FormOfPaymentTypeId })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
                 <tr>
                <td colspan="3" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left">
                          <% if (Model.SubProductFormOfPaymentTypes.HasPreviousPage)
                             { %>
                        <%=  Html.RouteLink("<<Previous Page", "ListMain", new { controller = "ClientTopUnitSubProductFormOfPaymentTypeController", action = "List", id = Model.ClientDetail.ClientDetailId, page = (Model.SubProductFormOfPaymentTypes.PageIndex - 1) }, new { title = "Previous Page" })%>
                        <%}%>
                        </div>
                        <div class="paging_right">
                         <% if (Model.SubProductFormOfPaymentTypes.HasNextPage)
                            { %>
                        <%=  Html.RouteLink("Next Page>>", "ListMain", new { controller = "ClientTopUnitSubProductFormOfPaymentTypeController", action = "List", id = Model.ClientDetail.ClientDetailId, page = (Model.SubProductFormOfPaymentTypes.PageIndex + 1) }, new { title = "Next Page" })%>
                        <%}%> 
                        </div>
                        <div class="paging_centre"><%if (Model.SubProductFormOfPaymentTypes.TotalPages > 0)
                                                     { %>Page <%=Model.SubProductFormOfPaymentTypes.PageIndex%> of <%=Model.SubProductFormOfPaymentTypes.TotalPages%><%} %></div>
                    </div>
                </td>
            </tr>
		        <tr> 
                    <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="2"><%= Html.ActionLink("Create SubProduct FormOfPaymentType", "Create", new { id = Model.ClientDetail.ClientDetailId }, new { @class = "red", title = "Create SubProduct FormOfPaymentType" })%></td> 
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
