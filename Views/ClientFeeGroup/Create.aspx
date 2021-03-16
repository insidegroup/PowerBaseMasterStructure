<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ClientFeeGroupVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DesktopDataAdmin - Client Fee Groups</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
     <div id="banner"><div id="banner_text"><%= Html.Encode(Model.FeeTypeDisplayName)%>s</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0", autocomplete="off" })) {%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create <%= Html.Encode(Model.FeeTypeDisplayName)%></th> 
		        </tr> 
		         <tr>
                    <td><%if (Model.FeeTypeId == 1 || Model.FeeTypeId == 2)
                          { %> Client Fee <%} %>Group Name</td>
                    <td><label id="lblAuto"></label></td>
                    <td><%= Html.HiddenFor(model => model.ClientFeeGroup.ClientFeeGroupName)%><label id="lblClientFeeGroupNameMsg"/></td>
                </tr>  
                 <tr>
                    <td><label for="ClientFeeGroup_DisplayName">Display Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientFeeGroup.DisplayName, new { maxlength = "50", autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientFeeGroup.DisplayName)%></td>
                </tr> 
                <tr>
                    <td><label for="ClientFeeGroup_EnabledFlag">Enabled?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.ClientFeeGroup.EnabledFlag, new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientFeeGroup.EnabledFlag)%></td>
                </tr>  
                <tr>
                    <td><label for="ClientFeeGroup_EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.ClientFeeGroup.EnabledDate, new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientFeeGroup.EnabledDate)%></td>
                </tr> 
                <tr>
                    <td><label for="ClientFeeGroup_ExpiryDate">Expiry Date</label> </td>
                    <td> <%= Html.EditorFor(model => model.ClientFeeGroup.ExpiryDate, new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientFeeGroup.ExpiryDate)%></td>
                </tr>
                  <tr>
                    <td><label for="ClientFeeGroup_Mandatory">Mandatory?</label></td>
                    <td><%= Html.CheckBox("ClientFeeGroup.Mandatory", Model.ClientFeeGroup.Mandatory, new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientFeeGroup.Mandatory)%></td>
                </tr>
                <tr>
                    <td><label for="ClientFeeGroup_TripTypeID">Trip Type</label></td>
                    <td><%= Html.DropDownListFor(model => model.ClientFeeGroup.TripTypeId, Model.TripTypes, "None", new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.ClientFeeGroup.TripTypeId)%></td>
                </tr>  
                <tr>
                <td><label for="ClientFeeGroup_HierarchyType">Hierarchy Type</label></td>
                <td><%= Html.DropDownListFor(model => model.ClientFeeGroup.HierarchyType, Model.HierarchyTypes, "Please Select...", new { autocomplete="off" })%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.ClientFeeGroup.HierarchyType)%></td>
            </tr>           
                <tr>
                    <td><label id="lblHierarchyItem"/>HierarchyItem</td>
                    <td> <%= Html.TextBoxFor(model => model.ClientFeeGroup.HierarchyItem, new { disabled="disabled",  size = "30", autocomplete="off" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.ClientFeeGroup.HierarchyItem)%>
                        <%= Html.HiddenFor(model => model.ClientFeeGroup.HierarchyCode)%>
                        <label id="lblHierarchyItemMsg"/>
                    </td>
                </tr> 
                <tr style="display:none" id="TravelerType">
                    <td><label for="TravelerTypeName">Traveler Type</label></td>
                    <td><%= Html.TextBoxFor(model => model.ClientFeeGroup.TravelerTypeName, new { size = "30", autocomplete="off" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.ClientFeeGroup.TravelerTypeGuid)%>
                        <%= Html.Hidden("ClientFeeGroup.TravelerTypeGuid")%>
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
                    <td class="row_footer_blank_right"><input type="submit" value="Create <%= Html.Encode(Model.FeeTypeDisplayName)%>" class="red" title="Create <%= Html.Encode(Model.FeeTypeDisplayName)%>"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.ClientFeeGroup.FeeTypeId)%>
            <%= Html.HiddenFor(model => model.ClientFeeGroup.ClientFeeGroupId)%>
            <%= Html.HiddenFor(model => model.ClientFeeGroup.SourceSystemCode)%>
    <% } %>
        </div>
    </div>
    
<script src="<%=Url.Content("~/Scripts/ERD/ClientFeeGroup.js")%>" type="text/javascript"></script>
</asp:Content>


