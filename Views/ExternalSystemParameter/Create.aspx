<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ExternalSystemParameter>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - ExternalSystemParameters
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">External System Parameters</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0", autocomplete="off" })){%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create External System Parameter</th> 
		        </tr> 
                <tr>
                    <td><label for="ExternalSystemParameterValue">Value</label></td>
                    <td><%= Html.TextBoxFor(model => model.ExternalSystemParameterValue, new { maxlength = "50", autocomplete="off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ExternalSystemParameterValue)%></td>
                </tr> 
                 <tr>
                    <td><label for="ExternalSystemParameterTypeId">Type</label></td>
                    <td><%= Html.DropDownList("ExternalSystemParameterTypeId", ViewData["ExternalSystemParameterTypes"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ExternalSystemParameterTypeId)%></td>
                </tr>  
                <tr>
                    <td><label for="EnabledFlag">Enabled</label></td>
                    <td><%= Html.CheckBox("EnabledFlag", true, new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.EnabledFlag) %></td>
                </tr>  
                 <tr>
                    <td><label for="EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.EnabledDate, new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.EnabledDate) %></td>
                </tr> 
                <tr>
                    <td><label for="ExpiryDate">Expiry Date</label> </td>
                    <td> <%= Html.EditorFor(model => model.ExpiryDate, new { autocomplete="off" }) %></td>
                    <td><%= Html.ValidationMessageFor(model => model.ExpiryDate) %></td>
                </tr> 
                <tr>
                    <td><label for="TripTypeId">Trip Type</label></td>
                    <td><%= Html.DropDownList("TripTypeId", ViewData["TripTypes"] as SelectList, "None", new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TripTypeId) %></td>
                </tr> 
                 <tr>
                    <td><label for="HierarchyType">Hierarchy Type</label></td>
                    <td><%= Html.DropDownList("HierarchyType", ViewData["HierarchyTypes"] as SelectList, "Please Select...", new { autocomplete="off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.HierarchyType)%></td>
                </tr>               
                <tr>
                    <td><label for="HierarchyItem" id="lblHierarchyItem"/>Hierarchy Item</td>
                    <td> <%= Html.TextBoxFor(model => model.HierarchyItem, new { disabled="disabled", size = "30", autocomplete="off" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.HierarchyItem)%> 
                        <%= Html.Hidden("HierarchyCode")%>
                        <label id="lblHierarchyItemMsg"/>
                    </td>
                </tr> 
                <tr style="display:none" id="TravelerType">
                    <td><label for="TravelerTypeName">Traveler Type Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.TravelerTypeName, new { size = "30", autocomplete="off" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.TravelerTypeGuid)%>
                        <%= Html.Hidden("TravelerTypeGuid")%>
                        <label id="lblTravelerTypeMsg"/>
                    </td>
                </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>                    
                    <td class="row_footer_blank_right"><input type="submit" value="Create External System Parameter" title="Create ExternalSystemParameter" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.ExternalSystemParameterId) %>
            <%= Html.HiddenFor(model => model.SourceSystemCode)%>
            <% } %> 
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/ExternalSystemParameter.js")%>" type="text/javascript"></script>
</asp:Content>


