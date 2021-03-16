<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientFeeGroupsVM>" %>
<%@ Import Namespace="CWTDesktopDatabase.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text"><%=Html.Encode(Model.FeeTypeDisplayName)%>s </div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; table-layout: fixed;">
                <tr> 
			        <th width="12%" class="row_header_left"><%= Html.RouteLink("Group Name", "ListMain", new { page = 1, sortField = "ClientFeeGroupName", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], ft = Model.FeeTypeId })%></th> 
			        <th width="29%">&nbsp;</th> 
                    <th width="20%"><%= Html.RouteLink("Hierarchy", "ListMain", new { page = 1, sortField = "HierarchyType", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], ft = Model.FeeTypeId })%></th> 
			        <th width="11%"><%= Html.RouteLink("Multiple", "ListMain", new { page = 1, sortField = "LinkedItemCount", sortOrder = ViewData["NewSortOrder"].ToString(), filter = Request.QueryString["filter"], ft = Model.FeeTypeId })%></th> 
			        <th width="8%">&nbsp;</th> 
			        <th width="5%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <%
                foreach (var item in Model.ClientFeeGroups) { 
                %>
                <tr>
                    <td colspan="2"><%= Html.Encode(CWTStringHelpers.TrimString(item.ClientFeeGroupName, 45))%></td>
                    <td><%= Html.Encode(item.HierarchyType) %></td>
                    <td><%= Html.Encode(item.LinkedItemCount > 1 ? "Yes" : "No")%></td>       
                    <td><%= Html.RouteLink("Hierarchy", "Default", new { id = item.ClientFeeGroupId, action = "HierarchySearch", h = item.HierarchyType }, new { title = "Hierarchy" })%></td>   
                    <%if(Model.FeeTypeId == 1 || Model.FeeTypeId == 2){ %>      
                    <td><%= Html.RouteLink("Items", "Default", new { controller="ClientFeeItem", action="List",id = item.ClientFeeGroupId}, new { title = "View" })%> </td>
                    <%}else if(Model.FeeTypeId == 3){ %>
                    <td><%= Html.RouteLink("Items", "Default", new { controller="TransactionFeeClientFeeGroup", action="List",id = item.ClientFeeGroupId}, new { title = "View" })%> </td>
                    <%}else{ %>
                    <td><%= Html.RouteLink("Items", "Default", new { controller="MerchantFeeClientFeeGroup", action="List",id = item.ClientFeeGroupId}, new { title = "View" })%> </td>
                    <%} %>
                    <td><%= Html.RouteLink("View", "Default", new { action="View", id = item.ClientFeeGroupId }, new { title = "View" })%> </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Edit", "Default", new { action = "Edit", id = item.ClientFeeGroupId }, new { title = "Edit" })%>
                        <%} %>
                    </td>
                    <td>
                        <%if(item.HasWriteAccess.Value){%>
                        <%=  Html.RouteLink("Delete", "Default", new { action = "Delete", id = item.ClientFeeGroupId }, new { title = "Delete" })%>
                        <%} %>
                    </td>
                </tr>        
                <% 
                } 
                %>
               <tr>
                    <td colspan="9" class="row_footer">
                        <div class="paging_container">
                            <div class="paging_left"><% if (Model.ClientFeeGroups.HasPreviousPage)
                                                        { %><%= Html.RouteLink("<<Previous Page", "ListMain", new { page = (Model.ClientFeeGroups.PageIndex - 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], ft = Model.FeeTypeId }, new { title = "Previous Page" })%><%}%></div>
                            <div class="paging_right"><% if (Model.ClientFeeGroups.HasNextPage)
                                                         {  %><%= Html.RouteLink("Next Page>>>", "ListMain", new { page = (Model.ClientFeeGroups.PageIndex + 1), sortField = ViewData["CurrentSortField"], sortOrder = ViewData["CurrentSortOrder"].ToString(), filter = Request.QueryString["filter"], ft = Model.FeeTypeId }, new { title = "Next Page" })%><%}%> </div>
                            <div class="paging_centre"><%if (Model.ClientFeeGroups.TotalPages > 0)
                                                         { %>Page <%=Model.ClientFeeGroups.PageIndex%> of <%=Model.ClientFeeGroups.TotalPages%><%} %></div>
                        </div>
                    </td>
                </tr>
		        

                <%if (Model.FeeTypeId == 3){ %>
                <tr> 
                    <td colspan="9">
                        <table width="100%">
                            <tr>
                                <td class="row_footer_blank_left" style="width:30px"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			                    <td class="row_footer_blank_right">
                                    <%= Html.ActionLink("Orphaned " + Html.Encode(Model.FeeTypeDisplayNameShort) + "s", "ListOrphaned", new { ft = Model.FeeTypeId }, new { @class = "red", title = "Orphaned " + Html.Encode(Model.FeeTypeDisplayName) + "s" })%>&nbsp;
                                    <%= Html.ActionLink("Deleted " + Html.Encode(Model.FeeTypeDisplayNameShort) + "s", "ListDeleted", new { ft = Model.FeeTypeId }, new { @class = "red", title = "Deleted " + Html.Encode(Model.FeeTypeDisplayName) + "s" })%>&nbsp;
			                        <%if (Model.HasDomainWriteAccess)
                                        { %><%= Html.ActionLink("Create " + Html.Encode(Model.FeeTypeDisplayNameShort), "Create", new { ft = Model.FeeTypeId }, new { @class = "red", title = "Create  " + Html.Encode(Model.FeeTypeDisplayName) })%>
                                    <%}%>
                                     </td> 
                            </tr> 
                        </table>
                    </td>
                </tr>
		        <%}else{%> 
                 <tr> 
                    <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="8">
                        <%= Html.ActionLink("Orphaned " + Html.Encode(Model.FeeTypeDisplayNameShort) + "s", "ListOrphaned", new { ft = Model.FeeTypeId }, new { @class = "red", title = "Orphaned " + Html.Encode(Model.FeeTypeDisplayName) + "s" })%>&nbsp;
                        <%= Html.ActionLink("Deleted " + Html.Encode(Model.FeeTypeDisplayNameShort) + "s", "ListDeleted", new { ft = Model.FeeTypeId }, new { @class = "red", title = "Deleted " + Html.Encode(Model.FeeTypeDisplayName) + "s" })%>&nbsp;
			            <%if (Model.HasDomainWriteAccess)
                            { %><%= Html.ActionLink("Create " + Html.Encode(Model.FeeTypeDisplayNameShort), "Create", new { ft = Model.FeeTypeId }, new { @class = "red", title = "Create  " + Html.Encode(Model.FeeTypeDisplayName) })%>
                        <%}%>
                         </td> 
                </tr> 
                <%}%>
                   
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_clientfeegroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
        $('#search').show();
        $("#frmSearch input[name='ft']").val(<%=Model.FeeTypeId%>);

    })
    //Search
    $('#btnSearch').click(function() {
      if ($('#filter').val() == "") {
          alert("No Search Text Entered");
          return false;
      }
      $("#frmSearch").attr("action", "/ClientFeeGroup.mvc/ListUnDeleted");
      $("#frmSearch").submit();

  });
 </script>
</asp:Content>
