<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ApprovalGroup>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Approval Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Approval Group (Deleted)</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">UnDelete Approval Group</th> 
		        </tr> 
               <tr>
                    <td><label for="ApprovalGroupName">Group Name</label></td>
                    <td colspan="2"><%= Html.Encode(Model.ApprovalGroupName)%></td>
                </tr> 
                <tr>
                    <td><label for="EnabledFlag">Enabled?</label></td>
                    <td><%= Html.Encode(Model.EnabledFlag)%></td>
                    <td></td>
                </tr>  
                  <tr>
                    <td><label for="EnabledDate">Enabled Date</label></td>
                    <td><%= Html.Encode(Model.EnabledDate.HasValue ? Model.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="ExpiryDate">Expiry Date</label> </td>
                    <td><%= Html.Encode(Model.ExpiryDate.HasValue ? Model.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                    <td></td>
                </tr>
				<tr>
                    <td><label for="InheritFromParentFlag">Inherit From Parent?</label></td>
                    <td><%= Html.Encode(Model.InheritFromParentFlag == true ? "True" : "False")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="DeletedFlag">Deleted?</label></td>
                    <td><%= Html.Encode(Model.DeletedFlag == true ? "True" : "False")%></td>
                    <td></td>
                </tr>  
                <tr>
                    <td><label for="DeletedDateTime">Deleted Date/Time</label></td>
                    <td><%= Html.Encode(Model.DeletedDateTime.HasValue ? string.Format("{0} {1}", Model.DeletedDateTime.Value.ToShortDateString(), Model.DeletedDateTime.Value.ToLongTimeString()) : "No Deleted Date")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="HierarchyType">Hierarchy Type</label> </td>
                    <td><%= Html.Encode(Model.HierarchyType)%></td>
                    <td></td>
                </tr>
                <% if (Model.HierarchyType == "ClientSubUnitTravelerType"){ %>
                    <tr>
                        <td>Client Subunit Name</td>
                        <td><%= Html.Encode(Model.ClientSubUnitName)%>, <%= Html.Encode(Model.ClientTopUnitName) %></td>
                        <td></td>
                    </tr> 
                    <tr>
                        <td>Traveler Type Name</td>
                        <td><%= Html.Encode(Model.TravelerTypeName)%></td>
                        <td></td>
                    </tr> 
                <% } else if (Model.HierarchyType == "TravelerType"){ %>
                    <tr>
                        <td>Hierarchy Item</td>
                        <td colspan="2"><%= Html.Encode(Model.TravelerTypeName)%>, <%= Html.Encode(Model.ClientTopUnitName) %></td>
                    </tr> 
                <% } else if (Model.HierarchyType == "ClientSubUnit"){ %>
                    <tr>
                        <td>Hierarchy Item</td>
                        <td colspan="2"><%= Html.Encode(Model.HierarchyItem)%>, <%= Html.Encode(Model.ClientTopUnitName) %></td>
                    </tr> 
                <% } else if (Model.HierarchyType == "ClientAccount"){ %>
                    <tr>
                        <td>Hierarchy Item</td>
                        <td colspan="2"><%= Html.Encode(Model.HierarchyItem)%>, <%= Html.Encode(Model.HierarchyCode)%>, <%= Html.Encode(Model.SourceSystemCode)%></td>
                    </tr> 
                <%} else { %>
                    <tr>
                        <td>Hierarchy Item</td>
                        <td><%= Html.Encode(Model.HierarchyItem != null ? Model.HierarchyItem : "")%></td>
                        <td></td>
                    </tr> 
                <% } %>
                <tr>
                    <td>Approval Type(s)</td>
                    <td colspan="2">
                        <% if (Model.ApprovalGroupApprovalTypeItems != null && Model.ApprovalGroupApprovalTypeItems.Count > 0){ %>
                           <% =Html.Encode(String.Join(", ", Model.ApprovalGroupApprovalTypeItems.OrderBy(x => x.ApprovalGroupApprovalType.ApprovalGroupApprovalTypeDescription).ThenBy(x => x.ApprovalGroupApprovalTypeItemValue).Select(x => x.ApprovalGroupApprovalType.ApprovalGroupApprovalTypeDescription + "/" + x.ApprovalGroupApprovalTypeItemValue).ToList())) %>
                        <% } %>
                    </td>
                </tr>
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
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_approvalgroups').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Approval Groups", "Main", new { controller = "ApprovalGroup", action = "ListDeleted", }, new { title = "Approval Groups" })%> &gt;
<%=Model.ApprovalGroupName%>
</asp:Content>