<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.GDSAdditionalEntry>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - GDS Additional Entries
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">GDS Additional Entries</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Undelete GDS Additional Entry</th> 
		        </tr> 
                <tr>
                    <td><%= Html.LabelFor(model => model.GDSAdditionalEntryValue) %></td>
                    <td><%= Html.Encode(Model.GDSAdditionalEntryValue)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td> <%= Html.LabelFor(model => model.GDSCode) %></td>
                    <td><%= Html.Encode(Model.GDSName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td><%= Html.LabelFor(model => model.GDSAdditionalEntryEventId)%></td>
                    <td><%= Html.Encode(Model.GDSAdditionalEntryEventId)%></td>
                    <td></td>
                </tr>
                 <tr>
                    <td><%= Html.LabelFor(model => model.SubProductId)%></td>
                    <td><%= Html.Encode(Model.SubProductId)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><%= Html.LabelFor(model => model.EnabledFlag) %></td>
                    <td><%= Html.Encode(Model.EnabledFlag)%></td>
                    <td></td>
                </tr>  
                 <tr>
                    <td>Enabled Date</td>
                    <td><%= Html.Encode(Model.EnabledDate.HasValue ? Model.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Expiry Date</td>
                    <td><%= Html.Encode(Model.ExpiryDate.HasValue ? Model.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td> <%= Html.LabelFor(model => model.TripType)%></td>
                    <td><%= Html.Encode(Model.TripType)%></td>
                    <td></td>
                </tr> 
               <tr>
                    <td><%= Html.LabelFor(model => model.DeletedFlag)%></td>
                    <td><%= Html.Encode(Model.DeletedFlag)%></td>
                    <td></td>
                </tr>  
                <tr>
                    <td><%= Html.LabelFor(model => model.DeletedDateTime)%></td>
                    <td><%= Html.Encode(Model.DeletedDateTime)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td><%= Html.LabelFor(model => model.HierarchyType)%> </td>
                    <td><%= Html.Encode(Model.HierarchyType)%></td>
                    <td></td>
                </tr>
                 <% if (Model.HierarchyType == "ClientSubUnitTravelerType"){ %>
                
                <tr>
                    <td><%= Html.LabelFor(model => model.ClientSubUnitName)%> </td>
                    <td><%= Html.Encode(Model.ClientSubUnitName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><%= Html.LabelFor(model => model.TravelerTypeName)%> </td>
                    <td><%= Html.Encode(Model.TravelerTypeName)%></td>
                    <td></td>
                </tr> 
                 <%}else{ %>
                    <% if (Model.HierarchyType != null){ %>
                    <tr>
                        <td>Hierarchy Item</td>
                        <td><%= Html.Encode(Model.HierarchyItem)%></td>
                        <td></td>
                    </tr> 
                    <%}else{%>
                    <tr>
                        <td>Hierarchy Item</td>
                        <td></td>
                        <td></td>
                    </tr> 
                    <%}%>
                <%}%>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left" colspan="2"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                    <td class="row_footer_blank_right">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm UnDelete" title="Confirm UnDelete" class="red"/>
                        <%= Html.HiddenFor(model => model.VersionNumber) %>
                    <%}%>
                    </td>                
               </tr>
            </table>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_gdsadditionalentries').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    });
 </script>
</asp:Content>

