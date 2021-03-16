<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ClientSubUnitClientAccount>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnit - Client Accounts</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
           <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Edit Client Account of Client SubUnit</th> 
		        </tr> 
                <tr>
                    <td>Client SubUnit</td>
                    <td><strong><%= Model.ClientSubUnitName%></strong></td>
                    <td></td>
                </tr>   
                 <tr>
                    <td>Client Account</td>
                    <td><strong><%= Model.ClientAccountName%></strong></td>
                    <td></td>
                </tr>   
                  <tr>
                    <td><label for="ClientAccountLineOfBusinessId">Account Line Of Business</label></td>
                    <td><%= Html.DropDownList("ClientAccountLineOfBusinessId", ViewData["LineOfBusinessses"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientAccountLineOfBusinessId)%></td>
                </tr>  
                <tr>
                    <td><label for="DefaultFlag">Default?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.DefaultFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.DefaultFlag)%></td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Edit Client SubUnit Client Account" title="Edit Client SubUnit Client Account" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.ClientSubUnitGuid)%>
            <%= Html.HiddenFor(model => model.ClientAccountName)%>
            <%= Html.HiddenFor(model => model.ClientAccountNumber)%>
            <%= Html.HiddenFor(model => model.SourceSystemCode) %>

    <% } %>
        </div>
    </div>
    <script src="<%=Url.Content("~/Scripts/ERD/ClientSubUnitClientAccount.js")%>" type="text/javascript"></script>
</asp:Content>



