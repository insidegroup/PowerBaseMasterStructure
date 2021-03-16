<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.SystemUserGDS>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - System Users
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="contentarea">
    <div id="banner"><div id="banner_text">System User GDS</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			    <th class="row_header" colspan="3">Delete System User GDS</th> 
		    </tr> 

            <tr>
                <td>System User Name</td>
                <td><%= Html.Encode(Model.SystemUserName)%></td>
                <td></td>
            </tr>  
            <tr>
                <td>GDS</td>
                <td><%= Html.Encode(Model.GDSName)%></td>
                <td></td>
            </tr>
            <%if(Model.GDSCode !="ALL"){ %>
                <tr>
                <td>Home Pseudo City or Office ID</td>
                <td><%= Html.Encode(Model.PseudoCityOrOfficeId)%></td>
                <td></td>
            </tr>     
            <tr>
                <td>GDS Sign On ID</td>
                <td><%= Html.Encode(Model.GDSSignOn)%></td>
                <td></td>
            </tr>     
            <%}%>
            <tr>
                <td>Default GDS?</td>
                <td><%= Html.Encode(Model.DefaultGDS)%></td>
                <td></td>
            </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td> 
                    <td class="row_footer_blank_right" colspan="2">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                        <%= Html.HiddenFor(model => model.VersionNumber) %>
                        <%= Html.HiddenFor(model => model.SystemUserGuid) %>
                        <%= Html.HiddenFor(model => model.GDSCode) %>
                    <%}%>
                    </td>                
               </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_teams').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

