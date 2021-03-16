<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.Meeting>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Meeting
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Meeting Group (Deleted)</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">UnDelete Meeting Group</th> 
		        </tr> 
				<tr>
					<td><label for="MeetingName">Meeting Name</label></td>
					<td><%= Html.Encode(Model.MeetingName)%></td>
					<td></td>
				</tr> 
				<tr>
					<td><label for="MeetingReferenceNumber">Meeting Reference</label></td>
					<td><%= Html.Encode(Model.MeetingReferenceNumber)%></td>
					<td></td>
				</tr>  
				<tr>
					<td><label for="MeetingLocation">Meeting Site</label></td>
					<td><%= Html.Encode(Model.MeetingLocation)%></td>
					<td></td>
				</tr> 
				<tr>
					<td><label for="CityCode">Meeting City</label></td>
					<td><%= Html.Encode(Model.City.Name)%></td>
					<td></td>
				</tr> 
				<tr>
					<td><label for="MeetingStartDate">Meeting Start Date</label> </td>
					<td><%= Html.Encode(Model.MeetingStartDate.ToString("MMM dd, yyyy"))%></td>
					<td></td>
				</tr> 
				<tr>
					<td><label for="MeetingEndDate">Meeting End Date</label> </td>
					<td><%= Html.Encode(Model.MeetingEndDate.ToString("MMM dd, yyyy"))%></td>
					<td></td>
				</tr> 
				<tr>
					<td><label for="MeetingArriveByDateTime">Meeting Arrive By Date/Time</label> </td>
					<td>
						<%= Html.Encode(Model.MeetingArriveByDateTime.HasValue ? Model.MeetingArriveByDateTime.Value.ToString("MMM dd, yyyy HH:mm") : "")%> 
					</td>
					<td></td>
				</tr> 
				<tr>
					<td><label for="MeetingLeaveAfterDateTime">Meeting Leave After Date/Time</label> </td>
					<td><%= Html.Encode(Model.MeetingLeaveAfterDateTime.HasValue ? Model.MeetingLeaveAfterDateTime.Value.ToString("MMM dd, yyyy HH:mm") : "")%> </td>
					<td></td>
				</tr> 
				<tr>
                    <td><label for="EnabledFlag">Enabled?</label></td>
                    <td><%= Html.Encode(Model.EnabledFlag ? "True" : "False")%></td>
					<td></td>
                </tr>  
				<tr>
                    <td><label for="EnabledDate">Enabled Date</label></td>
                    <td><%= Html.Encode(Model.EnabledDate.HasValue ? Model.EnabledDate.Value.ToString("MMM dd, yyyy") : "")%></td>
					<td></td>
                </tr> 
				<tr>
                    <td><label for="ExpiryDate">Expiry Date</label></td>
                    <td><%= Html.Encode(Model.ExpiryDate.HasValue ? Model.ExpiryDate.Value.ToString("MMM dd, yyyy") : "")%></td>
                    <td></td>
                </tr> 
				<tr>
					<td>Deleted Flag</td>
					<td><%= Html.Encode(Model.DeletedFlag)%></td>
					<td></td>
				</tr>  
				<tr>
					<td><label for="DeletedDateTime">Deleted Date Time</label></td>
					<td><%= Html.Encode(Model.DeletedDateTime.HasValue ? Model.DeletedDateTime.Value.ToString("G") : "No Deleted Date")%></td>
					<td></td>
				</tr> 
					<tr>
					<td>Hierarchy Type</td>
					<td><%= Html.Encode(Model.HierarchyType)%></td>
					<td></td>
				</tr>
				<tr>
					<td>Hierarchy Item</td>
					<td><%= Html.Encode(Model.HierarchyItem)%></td>
					<td></td>
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
                        <input type="submit" value="Confirm UnDelete" class="red"/>
                        <%= Html.HiddenFor(model => model.VersionNumber) %>
                    <%}%>
                    </td>                
               </tr>
            </table>
            

    </div>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_servicingoptions').click();
	    $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
 </asp:Content>

 <asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("Meeting Groups", "Main", new { controller = "Meeting", action = "ListDeleted", }, new { title = "Meeting Groups" })%> &gt;
<%=Html.Encode(Model.ClientTopUnit.ClientTopUnitName) %> &gt;
<%=Model.MeetingName%> &gt;
UnDelete
</asp:Content>