<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Wizard.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.LocationWizardVM>" %>



<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDatabaseAdmin - Location Wizard
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/Wizard/AjaxAuthentication.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/Wizard/Location.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/JSON/JSON2.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <div id="contentarea">
 

    <div id="banner"><div id="banner_text">Location Maintenance &nbsp;<span id="currentLocation"></span><span id="menuSelection"><select id="wizardMenu" class="ui-state-default">
            <option value="0" selected="selected">Change to  ...</option>
            <option value="Home.mvc">Main Menu / Home</option>
            <option value="TeamWizard.mvc">Team Wizard</option>
            <option value="SystemUserWizard.mvc">System User Wizard</option>
            <option value="ClientWizard.mvc">Client Wizard</option>
            </select></span></div></div>
    
        <div id="content">
    
            <div id="tabs">
	            <ul>
		            <li><a href="#tabs-1" id="selectALocationLink">Location Selection</a></li>
		            <li><a href="#tabs-2" id="locationDetailsLink">Location Details</a></li>
		            <li><a href="#tabs-3" id="usersInLocationLink">Users In This Location</a></li>
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
        <div id="dialog-confirm" title="Delete this Location?" style="width:600px; height:900px;">
		</div>

  </div> 

<form id="frmLocationWizard" action="" method="post">
    <input type="hidden" id="RemovedUsers"/>
    <input type="hidden" id="AddedUsers"/>
    <input type="hidden" id="Location" value=""/>
    <input type="hidden" id="Address" value=""/>
    <input type="hidden" id="LocationId" value=""/> 
</form>
<div style="visibility:hidden">
    <table id="removedLocationMembers"></table>
    <table id="addedLocationMembers"></table>
</div>
</asp:Content>
