<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.GDSAdditionalEntry>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Additional Entries
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Additional Entries</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% Html.EnableUnobtrusiveJavaScript(); %>
        <%using (Html.BeginForm(null, null, FormMethod.Post, new { id = "form0" })){%>
        <%= Html.AntiForgeryToken() %>
        
            <table cellpadding="0" border="0" width="100%" cellspacing="0"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create GDS Additional Entry</th> 
		        </tr> 
                <tr>
                    <td><label for="GDSAdditionalEntryValue">Value</label></td>
                    <td><%= Html.TextBoxFor(model => model.GDSAdditionalEntryValue, new { maxlength="100"})%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSAdditionalEntryValue)%></td>
                </tr> 
                  <tr>
                    <td><label for="GDSCode">GDS</label></td>
                    <td><%= Html.DropDownList("GDSCode", ViewData["GDSs"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSCode)%></td>
                </tr> 
                  <tr>
                    <td><label for="GDSAdditionalEntryEventId">Event</label></td>
                    <td> <%= Html.TextBoxFor(model => model.GDSAdditionalEntryEventId, new { maxlength = "9" })%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.GDSAdditionalEntryEventId)%></td>
                </tr>
                 <tr>
                    <td><label for="ProductId">Product</label></td>
                    <td><%= Html.DropDownList("ProductId", ViewData["ProductList"] as SelectList, "Please Select...", new { onchange = "LoadSubProducts()"})%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.ProductId)%></td>
                </tr> 
                 <tr>
                    <td><label for="SubProductName">Sub Product</label></td>
                    <td><%= Html.DropDownList("SubProductName", new List<SelectListItem>())%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.SubProductName)%>
                        <%= Html.Hidden("SubProductId", Model.SubProductId)%>
                    </td>
                </tr>    
                <tr>
                    <td><label for="EnabledFlag">Enabled?</label></td>
                    <td><%= Html.CheckBox("EnabledFlag", true)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.EnabledFlag) %></td>
                </tr>  
                 <tr>
                    <td><label for="EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.EnabledDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.EnabledDate) %></td>
                </tr> 
                <tr>
                    <td><label for="ExpiryDate">Expiry Date</label> </td>
                    <td> <%= Html.EditorFor(model => model.ExpiryDate) %></td>
                    <td><%= Html.ValidationMessageFor(model => model.ExpiryDate) %></td>
                </tr> 
               
                <tr>
                    <td><label for="TripTypeId">TripType</label></td>
                    <td><%= Html.DropDownList("TripTypeId", ViewData["TripTypes"] as SelectList, "None")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.TripTypeId) %></td>
                </tr> 
                <tr>
                    <td><label for="HierarchyType">HierarchyType</label></td>
                    <td><%= Html.DropDownList("HierarchyType", ViewData["HierarchyTypes"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.HierarchyType)%></td>
                </tr>               
                <tr>
                    <td><label id="lblHierarchyItem"  for="HierarchyItem"/>HierarchyItem</td>
                    <td> <%= Html.TextBoxFor(model => model.HierarchyItem, new { disabled="disabled",  size = "30" })%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.HierarchyItem)%>
                        <%= Html.Hidden("HierarchyCode")%>
                        <label id="lblHierarchyItemMsg"/>
                    </td>
                </tr> 
                <tr style="display:none" id="TravelerType">
                    <td><label for="TravelerTypeName">TravelerType Name</label></td>
                    <td><%= Html.TextBoxFor(model => model.TravelerTypeName, new { size = "30" })%><span class="error"> *</span></td>
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
                    <td class="row_footer_blank_right"><input type="submit" value="Create GDS Additional Entry" class="red" title="Create GDS Additional Entry"/></td>
                </tr>
            </table>
            <%= Html.HiddenFor(model => model.GDSAdditionalEntryId) %>
            <%= Html.HiddenFor(model => model.SourceSystemCode)%>
    <% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/ERD/GDSAdditionalEntry.js")%>" type="text/javascript"></script>
</asp:Content>


