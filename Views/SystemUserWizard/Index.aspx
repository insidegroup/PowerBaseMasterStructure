<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Wizard.Master" Inherits="System.Web.Mvc.ViewPage" %>



<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDatabaseAdmin - System User Wizard
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/Wizard/AjaxAuthentication.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/Wizard/SystemUser.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/JSON/JSON2.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <div id="contentarea">
 

    <div id="banner"><div id="banner_text">System User Maintenance &nbsp;<span id="currentUser"></span><span id="menuSelection"><select id="wizardMenu" class="ui-state-default">
            <option value="0" selected="selected">Change to  ...</option>
            <option value="Home.mvc">Main Menu / Home</option>
            <option value="TeamWizard.mvc">Team Wizard</option>
            <option value="LocationWizard.mvc">Location Wizard</option>
            <option value="ClientWizard.mvc">Client Wizard</option>
            </select></span></div></div>
    
        <div id="content">
    
            <div id="tabs">
	            <ul>
		            <li><a href="#tabs-1" id="selectASystemUser">System User Selection</a></li>
		            <li><a href="#tabs-2" id="systemUserDetailsLink">System User Details</a></li>
		            <li><a href="#tabs-3" id="usersInTeamLink">System User Team</a></li>
		            <li><a href="#tabs-4" id="finishLink">Finish</a></li>
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
             
               
            </div>    

        </div>
        <div id="dialog-confirm" title="Delete this user?">
		</div>

  </div> 

<form id="frmSystemUserWizard" action="" method="post">
    <input type="hidden" id="SystemUserGuid" value=""/> 
    <input type="hidden" id="RemovedTeams"/>
    <input type="hidden" id="AddedTeams"/>
    <input type="hidden" id="SystemUserGDSs" value=""/>
    <input type="hidden" id="GDSChanged" value=""/>
    <input type="hidden" id="SystemUser" value=""/>  
    <input type="hidden" id="SystemUserLocation" value=""/> 
    <input type="hidden" id="ExternalSystemLogins" value=""/> 
</form>
<div style="visibility:hidden">

<table id="addedTeamMembers">

</table>

<table id="removedTeams">
</table>
<table id="addedTeams">
</table>
<table id="changedClientSubUnits">
</table>
		<input type="hidden" id="userGDSChanges" value="0" />
</div>
</asp:Content>
