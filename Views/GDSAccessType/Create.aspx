<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GDSAccessTypeVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Access Type
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Access Type</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create GDS Access Type</th> 
		        </tr> 
				<tr>
                    <td><label for="GDSAccessType_GDSCode">GDS</label></td>
                    <td><%= Html.DropDownListFor(model => model.GDSAccessType.GDSCode, Model.GDSs as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSAccessType.GDSCode)%></td>
                </tr>
				<tr>
                    <td><label for="GDSAccessType_GDSAccessTypeName">GDS Access Type</label></td>
                    <td><%= Html.TextBoxFor(model => model.GDSAccessType.GDSAccessTypeName, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.GDSAccessType.GDSAccessTypeName)%>
						<label id="lblGDSAccessTypeMsg"></label>
                    </td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create GDS Access Type" title="Create GDS Access Type" class="red"/></td>
                </tr>
            </table>
		<% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/GDSAccessType.js")%>" type="text/javascript"></script>
</asp:Content>