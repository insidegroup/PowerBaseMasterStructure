<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.SupplierVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Supplier</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete Supplier</th> 
		        </tr>  
                <tr>
					<td><label for="SupplierCode">Supplier Code</label></td>
					<td><%= Html.Encode(Model.Supplier.SupplierCode) %></td>
					<td></td>
				</tr>   
				<tr>
					<td><label for="SupplierName">Supplier Name</label></td>
					<td><%= Html.Encode(Model.Supplier.SupplierName) %></td>
					<td></td>
				</tr>
				<tr>
                    <td><label for="ProductName">Product</label></td>
                    <td><%= Html.Encode(Model.Supplier.ProductName)%></td>
                    <td></td>
                </tr>

				<% if(Model.AllowDelete == false) { %>
					
					<tr>
						<td colspan="3">You cannot delete this Supplier until all items referencing it are removed:</td>
					</tr>

					<% foreach(CWTDesktopDatabase.Models.SupplierReference supplierReference in Model.SupplierReferences) { %>
						<tr>
							<td colspan="3"><strong>Attached <%=supplierReference.TableName %></strong></td>
						</tr>
					<%}%>
								
				<%}%> 
				
				<tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;</td>
                    <td class="row_footer_blank_right" colspan="2">
						<% if(Model.AllowDelete) { %>
							<% using (Html.BeginForm()) { %>
								<%= Html.AntiForgeryToken() %>
								<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
								<%= Html.HiddenFor(model => model.Supplier.SupplierCode) %>
								<%= Html.HiddenFor(model => model.Supplier.ProductId) %>
								<%= Html.HiddenFor(model => model.Supplier.VersionNumber) %>
							<%}%>
						<%}%>
					</td>
				</tr>
			</table>
        </div>
    </div>
	<script src="<%=Url.Content("~/Scripts/ERD/Supplier.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Supplier", "Main", new { controller = "Supplier", action = "List", }, new { title = "Supplier" })%> &gt;
Delete Supplier &gt;
<%:Model.Supplier.SupplierName%>
</asp:Content>