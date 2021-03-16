<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PartnerVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Partners
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Partners</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Partner</th> 
		        </tr> 
				<tr>
                    <td><label for="Partner_PartnerName">Partner Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.Partner.PartnerName, new { maxlength = "100" })%><span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.Partner.PartnerName)%>
						<label id="lblPartnerMsg"></label>
                    </td>
                </tr>
				<tr>
					<td><label for="Partner_CountryCode">Country</label></td>
					<td><%= Html.DropDownListFor(model => model.Partner.CountryCode, Model.Countries, "Please Select...")%><span class="error"> *</span></td>
					<td><%= Html.ValidationMessageFor(model => model.Partner.CountryCode)%></td>
				</tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
				<tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create Partner" title="Create Partner" class="red"/></td>
                </tr>
            </table>
		<% } %>
    </div>
</div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_admin, #menu_admin_gdsmanagement').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
		$('#search').hide();
	});
</script>
<script src="<%=Url.Content("~/Scripts/ERD/Partner.js")%>" type="text/javascript"></script>
</asp:Content>