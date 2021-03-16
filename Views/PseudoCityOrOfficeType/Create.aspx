<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PseudoCityOrOfficeTypeVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Pseudo City/Office ID Type
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Pseudo City/Office ID Type</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Pseudo City/Office ID Type</th> 
		        </tr> 
				<tr>
                    <td><label for="PseudoCityOrOfficeType_PseudoCityOrOfficeTypeName">Pseudo City/Office ID Type</label></td>
                    <td><%= Html.TextBoxFor(model => model.PseudoCityOrOfficeType.PseudoCityOrOfficeTypeName, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.PseudoCityOrOfficeType.PseudoCityOrOfficeTypeName)%>
						<label id="lblPseudoCityOrOfficeTypeMsg"></label>
                    </td>
                </tr>
                <tr>
                    <td width="40%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="20%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create Pseudo City/Office ID Type" title="Create Pseudo City/Office ID Type" class="red"/></td>
                </tr>
            </table>
		<% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/PseudoCityOrOfficeType.js")%>" type="text/javascript"></script>
</asp:Content>