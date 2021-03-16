<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.APISCountry>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">APIS Countries</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		       <tr> 
			        <th class="row_header" colspan="3">Create APIS Country</th> 
		        </tr> 
                <tr>
                    <td><label for="OriginCountryCode">Origin Country</label></td>
                    <td><%= Html.DropDownList("OriginCountryCode", ViewData["Countries"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.OriginCountryCode)%><label id="lblOriginCountryCode"/></td>
                </tr> 
                <tr>
                    <td><label for="DestinationCountryCode">Destination Country</label></td>
                    <td><%= Html.DropDownList("DestinationCountryCode", ViewData["Countries"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.DestinationCountryCode)%><label id="lblDestinationCountryCode"/></td>
                </tr> 
                <tr>
                    <td><label for="StartDate">Start Date</label></td>
                    <td><%= Html.TextBox("StartDate", "")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.StartDate)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create APISCountry" title="Create APIS Country" class="red"/></td>
                </tr>
            </table>
            <%= Html.Hidden("OriginalOCC", Model.OriginCountryCode) %>
            <%= Html.Hidden("OriginalDCC", Model.DestinationCountryCode) %>
    <% } %>
        </div>
    </div>
    <script src="<%=Url.Content("~/Scripts/ERD/APISCountry.js")%>" type="text/javascript"></script>
</asp:Content>



