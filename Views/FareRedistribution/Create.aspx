<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.FareRedistributionVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Fare Redistribution
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Fare Redistribution</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Fare Redistribution</th> 
		        </tr> 
				<tr>
                    <td><label for="FareRedistribution_GDSCode">GDS</label></td>
                    <td><%= Html.DropDownListFor(model => model.FareRedistribution.GDSCode, Model.GDSs as SelectList, "Please Select...")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.FareRedistribution.GDSCode)%></td>
                </tr>
				<tr>
                    <td><label for="FareRedistribution_FareRedistributionName">Fare Redistribution</label></td>
                    <td><%= Html.TextBoxFor(model => model.FareRedistribution.FareRedistributionName, new { maxlength = "50" })%><span class="error"> *</span></td>
                    <td>
						<%= Html.ValidationMessageFor(model => model.FareRedistribution.FareRedistributionName)%>
						<label id="lblFareRedistributionMsg"></label>
                    </td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create Fare Redistribution" title="Create Fare Redistribution" class="red"/></td>
                </tr>
            </table>
		<% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/FareRedistribution.js")%>" type="text/javascript"></script>
</asp:Content>