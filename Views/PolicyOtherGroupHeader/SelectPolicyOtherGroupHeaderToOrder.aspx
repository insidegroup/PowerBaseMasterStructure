<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyOtherGroupHeaderSequenceVM>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Other Group Header Order
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Other Group Header Order</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Edit Labels Order</th> 
		        </tr> 
		        <tr>
                    <td><label for="ServiceTypeId">Service Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.PolicyOtherGroupHeaderServiceTypeId, Model.PolicyOtherGroupHeaderServiceTypes, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyOtherGroupHeaderServiceTypeId)%></td>
                </tr>
				<tr>
                    <td><label for="ProductId">Product</label></td>
                    <td><%= Html.DropDownListFor(model => model.ProductId, Model.Products, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ProductId)%></td>
                </tr>
				<tr>
                    <td><label for="SubProductId">Sub Product</label></td>
                    <td><%= Html.DropDownListFor(model => model.SubProductId, Model.SubProducts, "Please Select...")%><span class="error" id="SubProductIdRequired"> *</span></td>
                    <td>
						<span id="SubProductIdError" class="error"></span>
                    </td>
                </tr>  
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="50%" class="row_footer_centre"></td>
                    <td width="20%" class="row_footer_right"></td>
                </tr>
               <tr>
                   <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td> 
                   <td class="row_footer_blank_right"><input type="submit" value="Continue" title="Continue" class="red"/></td>
                </tr>
            </table>
	    <% } %>
    </div>
</div>
    
	<script src="<%=Url.Content("~/Scripts/ERD/PolicyOtherGroupHeaderOrdering.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("Policy Other Group Headers", "Main", new { controller = "PolicyOtherGroupHeader", action = "List" }, new { title = "Policy Other Group Headers" })%> &gt;
Edit Order
</asp:Content>