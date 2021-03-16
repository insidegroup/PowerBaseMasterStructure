<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ClientTopUnitMatrixDPCode>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Matrix DP Codes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Matrix DP Code</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginRouteForm("Default", new { controller = "MatrixDPCode", action = "Edit", hierarchyCode = Model.HierarchyCode, hierarchyItem = Model.HierarchyItem }, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Matrix DP Code</th> 
		        </tr> 
		        <tr>
                    <td>Matrix DP Code</td>
                    <td><%= Html.TextBoxFor(model => model.MatrixDPCode, new { maxlength="4" }) %><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.MatrixDPCode)%></td>
                </tr>
                <tr>
                    <td>Hierarchy Type</td>
                    <td><%= Html.Encode(Model.HierarchyType == "ClientSubUnit" ? "Client SubUnit" : "Traveler Type")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Hierarchy Item</td>
                    <td><%= Html.Encode(Model.HierarchyItem)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" title="Back" class="red">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Matrix DP Code" title="Edit Matrix DP Code" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.ClientTopUnit.ClientTopUnitGuid) %>
			<%= Html.HiddenFor(model => model.HierarchyCode) %>
    <% } %>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function() {
    	$('#menu_clients').click();
	    $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Client Top Units", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "Client Top Units" })%> &gt;
<%=Html.RouteLink(Model.ClientTopUnit.ClientTopUnitName, "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientTopUnit.ClientTopUnitGuid }, new { title = Model.ClientTopUnit.ClientTopUnitName })%> &gt;
<%=Html.RouteLink("Matrix DP Codes", "Main", new { controller = "ClientTopUnitMatrixDPCode", action = "List", id = Model.ClientTopUnit.ClientTopUnitGuid  }, new { title = "Matrix DP Codes" })%> &gt;
<%=Model.MatrixDPCode%>
</asp:Content>