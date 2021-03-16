<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientSubUnitESCInformationVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnit - Client Detail ESCInformation</div></div>
        <div id="content">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr> 
			        <th width="70%" class="row_header_left">ESCInformation</th> 
			        <th width="15%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="4%">&nbsp;</th> 
			        <th width="7%" class="row_header_right">&nbsp;</th> 
		        </tr> 
                <% if (Model.ClientDetailESCInformation != null)
                   { %>
                <tr>
                    <td><%= Html.Encode(Model.ClientDetailESCInformation.ESCInformation)%></td>
                    <td></td>
                    <td align="center"><%= Html.ActionLink("View", "View", new { id = Model.ClientDetail.ClientDetailId })%> </td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess")
                          { %>
                        <%=  Html.ActionLink("Edit", "Edit", new { id = Model.ClientDetail.ClientDetailId })%>
                        <%} %>
                    </td>
                    <td align="center">
                        <%if (ViewData["Access"] == "WriteAccess")
                          { %>
                        <%=  Html.ActionLink("Delete", "Delete", new { id = Model.ClientDetail.ClientDetailId })%>
                        <%} %>
                    </td>
                </tr>      
                <%} %>  
                <tr>
                <td colspan="8" class="row_footer">
                    <div class="paging_container">
                        <div class="paging_left"></div>
                        <div class="paging_right"></div>
                        <div class="paging_centre"></div>
                    </div>
                </td>
            </tr>
		        <tr> 
                    <td class="row_footer_blank_left"><a href="javascript:window.print();" class="red" title="Print">Print</a></td>
			        <td class="row_footer_blank_right" colspan="7">
                    <% if (Model.ClientDetailESCInformation == null)
                   { %>
                   <%= Html.ActionLink("Create ESCInformation", "Create", new { id = Model.ClientDetail.ClientDetailId }, new { @class = "red", title = "Create ESCInformation" })%>
                   <%} %>  </td> 
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
