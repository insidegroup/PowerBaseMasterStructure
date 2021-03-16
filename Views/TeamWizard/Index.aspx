<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Wizard.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.TeamWizardVM>" %>



<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDatabaseAdmin - Team Wizard
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/Wizard/AjaxAuthentication.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/Wizard/Team.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/JSON/JSON2.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <div id="contentarea">
 

    <div id="banner"><div id="banner_text">Team Maintenance &nbsp;<span id="currentTeam"></span><span id="menuSelection"><select id="wizardMenu" class="ui-state-default">
            <option value="0" selected="selected">Change to  ...</option>
            <option value="Home.mvc">Main Menu / Home</option>
            <option value="LocationWizard.mvc">Location Wizard</option>
            <option value="SystemUserWizard.mvc">System User Wizard</option>
            <option value="ClientWizard.mvc">Client Wizard</option>
            </select></span>
    </div></div>
    
        <div id="content">
    
            <div id="tabs">
	            <ul>
		            <li><a href="#tabs-1" id="selectATeamLink">Team Selection</a></li>
		            <li><a href="#tabs-2" id="teamDetailsLink">Team Details</a></li>
		            <li><a href="#tabs-3" id="usersInTeamLink">Users In This Team</a></li>
		            <li><a href="#tabs-4" id="clientsInTeamLink">Clients In This Team</a></li>
		            <li><a href="#tabs-5" id="finishLink">Finish</a></li>
	            </ul>
	            <div id="tabs-1">
                	<div id="tabs-1Content"></div>
	            </div>
	            <div id="tabs-2">
                   	<div id="tabs-2Content"></div>
	            </div>
	            <div id="tabs-3">
                	<div id="tabs-3Content"></div>
                </div>
                <div id="tabs-4">
                	<div id="tabs-4Content"></div>	        
	            </div>
                <div id="tabs-5">
                	<div id="tabs-5Content"></div>	        
	            </div>
             
               
            </div>    

        </div>
        <div id="dialog-confirm" title="Delete this team?" style="width:600px; height:900px;">
		</div>

  </div> 

<form id="frmTeamWizard" action="" method="post">
    <input type="hidden" id="RemovedUsers" name="RemovedUsers"/>
    <input type="hidden" id="AddedUsers"/>
    <input type="hidden" id="RemovedClientSubUnits"/>
    <input type="hidden" id="AddedClientSubUnits"/>
    <input type="hidden" id="AlteredClientSubUnits" />
    <input type="hidden" id="Team" value=""/>
    <input type="hidden" id="TeamId" value=""/> 
</form>
<div style="visibility:hidden">

<table id="removedTeamMembers">


</table>
<table id="addedTeamMembers">

</table>

<table id="removedClientSubUnits">
</table>
<table id="addedClientSubUnits">
</table>
<table id="changedClientSubUnits">
</table>

</div>
</asp:Content>
