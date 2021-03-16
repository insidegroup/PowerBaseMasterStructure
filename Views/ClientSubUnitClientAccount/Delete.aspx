<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ClientSubUnitClientAccount>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnit - Client Accounts</div></div>
        <div id="content">
           <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Remove Client Account from Client SubUnit</th> 
		        </tr> 
                <tr>
                    <td>Client SubUnit</td>
                    <td><%= Model.ClientSubUnitName%></td>
                    <td><%= Html.HiddenFor(model => model.ClientSubUnitGuid)%></td>
                </tr>
                <tr>
                    <td>Client Account</td>
                    <td><%= Model.ClientAccountName%></td>
                    <td><%= Html.HiddenFor(model => model.ClientAccountName)%></td>
                </tr>
                <tr>
                    <td>Account Line Of Business</td>
                    <td><%= Html.Encode(Model.LineOfBusiness != null ? Model.LineOfBusiness.LineofBusinessDescription : "")%></td>
                    <td></td>
                </tr>  
                <tr>
                    <td>Default?</td>
                    <td><%= Html.Encode(Model.DefaultFlag ? "True" : "False")%></td>
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
                        <input type="submit" value="Confirm Removal" title="Confirm Removal" class="red"/>
                        <%= Html.HiddenFor(model => model.VersionNumber) %>
                    <%}%>
                    </td>                
               </tr>
            </table>
        </div>
    </div>

<script type="text/javascript">
    $(document).ready(function() {
        $('#menu_clients').click();
		$("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>