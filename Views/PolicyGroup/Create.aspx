<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.PolicyGroup>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Create Policy Group</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0", autocomplete="off" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Policy Group</th> 
		        </tr> 
		        <tr>
                    <td>Policy Group Name</td>
                    <td><label id="lblAuto"></label></td>
                    <td><%= Html.HiddenFor(model => model.PolicyGroupName) %><label id="lblPolicyGroupNameMsg"/></td>
                </tr>                
                <tr>
                    <td><label for="EnabledFlag">Enabled?</label></td>
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
                    <td><label for="InheritFromParentFlag">Inherit From Parent?</label></td>
                   <td><%= Html.CheckBox("InheritFromParentFlag", true, new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.InheritFromParentFlag)%></td>
                </tr>
                <tr>
                    <td> <%= Html.LabelFor(model => model.TripTypeId) %></td>
                    <td><%= Html.DropDownList("TripTypeId", ViewData["TripTypes"] as SelectList, "None", new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TripTypeId) %></td>
                </tr> 
                
                <tr>
                    <td><label for="HierarchyType">Hierarchy Type</label></td>
                    <td><%= Html.DropDownList("HierarchyType", ViewData["HierarchyTypes"] as SelectList, "Please Select...", new { autocomplete="off" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.HierarchyType)%></td>
                </tr>               
                <tr>
                    <td><label id="lblHierarchyItem"/>HierarchyItem</td>
                    <td> <%= Html.TextBoxFor(model => model.HierarchyItem, new { disabled="disabled",  size = "30", autocomplete="off" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.HierarchyItem)%>
                        <%= Html.Hidden("HierarchyCode")%>
						<%= Html.Hidden("TT_CSU")%>
                        <label id="lblHierarchyItemMsg"/>
                    </td>
                </tr> 
                <tr style="display:none" id="TravelerType">
                    <td><label for="TravelerTypeName">Traveler Type</label></td>
                    <td><%= Html.TextBoxFor(model => model.TravelerTypeName, new { size = "30", autocomplete="off" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.TravelerTypeGuid)%>
                        <%= Html.Hidden("TravelerTypeGuid")%>
                        <label id="lblTravelerTypeMsg"/>
                    </td>
                </tr>
				<tr id="MeetingIDRow">
                    <td><label for="MeetingID">Meeting</label></td>
                    <td><%= Html.DropDownList("MeetingID", ViewData["Meetings"] as SelectList, "Please Select...", new { autocomplete="off" })%></td>
                    <td><%= Html.ValidationMessageFor(model => model.MeetingID) %></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right"><input type="submit" value="Create Policy Group" title="Create Policy Group" class="red"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.PolicyGroupId) %>
            <%= Html.HiddenFor(model => model.SourceSystemCode)%>
    <% } %>


        </div>
    </div>
    
    <script src="<%=Url.Content("~/Scripts/ERD/policygroup.js")%>" type="text/javascript"></script>
</asp:Content>


<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Policy Groups", "Main", new { controller = "PolicyGroup", action = "ListUnDeleted", }, new { title = "Policy Groups" })%>
</asp:Content>