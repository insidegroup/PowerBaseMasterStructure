<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.Models.Team>" %>
<div id="divTeam">
    <% Html.EnableClientValidation(); %>
    <%//using (Ajax.BeginForm("TeamDetailsScreen", "TeamWizard", new AjaxOptions { UpdateTargetId = "tabs-2Content", })) %>
    <% using (Html.BeginForm("TeamDetailsScreen", "TeamWizard", FormMethod.Post, new { id = "frmTeamDetails", autocomplete = "off" }))
        {%>
        <%= Html.ValidationSummary(true)%>
        <div style="float:right; right:100px;">
        <span id="teamDetailsBackButton"><small><< Back</small></span>
            &nbsp;<span id="teamDetailsNextButton"><small>Next >></small></span>
              
        </div>
        <table cellpadding="0" cellspacing="0" width="100%" class="tablesorter_other" id='teamEditTable'> 
            <thead>
		    <tr> 
			    <th colspan="3"><%if (Model.TeamId == 0) {%>Create Team<%}else{ %>Edit Team<%} %></th> 
		    </tr> 
            </thead>
            <tbody>
            <tr>
                <td><label for="TeamName">Team Name</label></td>
                <td><%= Html.TextBoxFor(model => model.TeamName, new { maxlength = "50", autocomplete = "off" })%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.TeamName)%></td>
            </tr> 
            <tr>
                <td><label for="TeamEmail">Team Email</label></td>
                <td> <%= Html.TextBoxFor(model => model.TeamEmail, new { maxlength = "50", autocomplete = "off"  })%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.TeamEmail)%></td>
            </tr> 
            <tr>
                <td><label for="TeamPhoneNumber">Team Phone Number</label></td>
                <td><%= Html.TextBoxFor(model => model.TeamPhoneNumber, new { maxlength = "20", autocomplete = "off"  })%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.TeamPhoneNumber)%></td>
            </tr> 
                <tr>
                <td><label for="CityCode">Team City Code</label></td>
                <td><strong><%= Html.TextBoxFor(model => model.CityCode, new { maxlength="5", autocomplete = "off" })%></strong></td>
                <td><%= Html.ValidationMessageFor(model => model.CityCode)%>                   
                    <label id="lblCityCode"/>
                </td>
            </tr> 
            <tr>
                <td><label for="TeamQueue">Team Queue</label></td>
                <td><%= Html.TextBoxFor(model => model.TeamQueue, new { maxlength = "50", autocomplete = "off"  })%></td>
                <td><%= Html.ValidationMessageFor(model => model.TeamQueue)%></td>
            </tr>               
            <tr>
                <td><label for="TeamTypeCode">Team Type</label></td>
                <td><%= Html.DropDownList("TeamTypeCode", ViewData["TeamTypes"] as SelectList, "Please Select...")%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.TeamTypeCode)%></td>
            </tr>
            <tr>
                <td><label for="HierarchyType">Hierarchy</label></td>
                <td><%= Html.TextBoxFor(model => model.HierarchyType, new { disabled="disabled", Value = "Location", autocomplete = "off" })%></td>
                <td><%= Html.ValidationMessageFor(model => model.HierarchyType)%></td>
            </tr>               
            <tr><td><label for="lblHierarchyItem" id="lblHierarchyItem">HierarchyItem</label></td>
                <td> <%= Html.TextBoxFor(model => model.HierarchyItem, new {size = "30", autocomplete = "off"  })%><span class="error"> *</span></td>
                <td>
                    <%= Html.ValidationMessageFor(model => model.HierarchyItem)%>
                    <%= Html.Hidden("HierarchyCode")%>
                    <label id="lblHierarchyItemMsg"/>
                </td>
            </tr>
            </tbody>
        </table>
            
        <span id="deleteTeam" style="float:right"><small>Delete this team</small></span>
             
        <%= Html.HiddenFor(model => model.SourceSystemCode)%>
        <%= Html.HiddenFor(model => model.VersionNumber)%>
        <%= Html.HiddenFor(model => model.SystemUserCount)%>
        <%= Html.HiddenFor(model => model.ClientSubUnitCount)%>
    <% } %>

</div>