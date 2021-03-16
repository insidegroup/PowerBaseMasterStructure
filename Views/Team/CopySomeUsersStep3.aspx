<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CWTDesktopDatabase.Models.spDesktopDataAdmin_SelectTeamSystemUserList_v1Result>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - LocalOperatingHoursGroups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Teams</div></div>
        <div id="content">
         <% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
            <table width="100%" cellpadding="0" cellspacing="0" id="Table1">
             <tr> 
		        <th class="row_header" colspan="3">Copy Team Members : Step 3</th> 
	        </tr>
             <tr> 
		        <td colspan="3" style="height:50px;">Please drag the users you wish to copy to the second list</td> 
	        </tr>
                <tr>
                    <td width="48%" valign="top" style="background-color:#ffffff;">
                        <table width="100%" cellpadding="0" cellspacing="0" id="sortable" border="0" class="droptrue">
                            <tbody class="content">  
                              <tr class="titlerow">
                                <th colspan="3" class="row_header">Team: <%=Html.Encode(ViewData["TeamName"]) %></th>
                             </tr>
                              <tr class="titlerow">
                                <th width="50%">User</th>
                                <th width="40%">Creation Time</th>
                                <th width="10%"></th>
                             </tr>
                            <%
                            var cssClass = "";
                            var iCounter = 1;
                            foreach (var item in Model) {
                                cssClass = "row_odd2";
                                if (iCounter%2 == 0)
                                {
                                    cssClass = "row_even";
                                }
                                iCounter++;
                            %>
                              <tr class="<%=cssClass%>" id="<%= Html.Encode(item.SystemUserGuid) %>">
                                <td width="50%"><%= Html.Encode(item.LastName)%><%//= Html.Encode(Model.MiddleName.ToString().Equals("") ? Model.MiddleName : " ")%>, <%= Html.Encode(item.FirstName) %></td>
                                <td width="40%"><%= Html.Encode(item.CreationTimestamp.HasValue ? item.CreationTimestamp.Value.ToString("MMM dd, yyyy - hh:mm") : "")%></td>
                                <td width="10%"><img src="../../images/arrow.png" /></td>
                             </tr>
                            <% 
                            } 
                            %>
                            </tbody>
                        </table>    
                    </td>
                    <td width="4%" style="background-color:#ffffff;"></td>
                     <td width="48%" valign="top" style="background-color:#ffffff;">
                        <table width="100%" cellpadding="0" cellspacing="0" id="sortable2" border="0" class="droptrue">
                            <tbody class="content">  
                             <tr class="titlerow">
                                <th colspan="3" class="row_header">Copy to Team:<%=Html.Encode(ViewData["NewTeamName"]) %></th>
                             </tr>
                              <tr class="titlerow">
                                <th width="50%">User</th>
                                <th width="40%">Creation Time</th>
                                <th width="10%"></th>
                             </tr>
                            </tbody>
                        </table>    
                    </td>
                </tr>
           
                <tr> 
		            <td colspan="3" style="height:50px;">When you click save, the users you have selected will be copied from <%=Html.Encode(ViewData["TeamName"]) %> to <%=Html.Encode(ViewData["NewTeamName"]) %>. <br />
		            They will be members of <%=Html.Encode(ViewData["TeamName"]) %> and of <%=Html.Encode(ViewData["NewTeamName"]) %></td> 
	            </tr>
		        <tr> 
                 <td class="row_footer_blank_left"></td>
                 <td class="row_footer_blank_right" colspan="2"><%= Html.ActionLink("Cancel", "EditSequence", new { }, new { @class = "red" })%>&nbsp;<input type="submit" value="Save" title="Save" class="red"/></td>
		        </tr> 
		        
            </table>
            <input type="hidden" name="SystemUsers" id="SystemUsers" />
             <% } %>
        </div>
    </div>
<script src="<%=Url.Content("~/Scripts/Sequencing/TeamSystemUsers.js")%>" type="text/javascript"></script>
</asp:Content>

