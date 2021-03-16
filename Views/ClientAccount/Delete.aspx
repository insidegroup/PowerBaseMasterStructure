<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.ClientAccount>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client TopUnits
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">Client Accounts</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete ClientAccount</th> 
		        </tr> 
                 <tr>
                    <td>ClientAccount Number</td>
                    <td><%= Html.Encode(Model.ClientAccountNumber)%></td>
                    <td></td>
                 </tr>
                 <tr>
                    <td>ClientAccount Name</td>
                    <td><%= Html.Encode(Model.ClientAccountName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Source System Code</td>
                    <td><%= Html.Encode(Model.SourceSystemCode)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Glory Account Name</td>
                    <td><%= Html.Encode(Model.GloryAccountName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Client Master Code</td>
                    <td><%= Html.Encode(Model.ClientMasterCode)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Effective Date</td>
                    <td><%= Html.Encode(Model.EffectiveDate.HasValue ? Model.EffectiveDate.Value.ToString("MMM dd, yyyy") : "No Effective Date")%></td>
                    <td></td>
                </tr>
                 <tr>
                    <td>Country</td>
                    <td><%= Html.Encode(Model.CountryName)%></td>
                    <td></td>
                </tr>
                <%if (!ViewData.ModelState.IsValid) { %>
                <%if (ViewData.ModelState["Exception"].Errors.Count > 0 ){ %>
                <tr>
                    <td></td>
                    <td colspan="2"><span class="error"><% =ViewData.ModelState["Exception"].Errors[0].ErrorMessage%></span></td>
                </tr> 
                <% } %>
                <% } %>
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
                        <%= Html.Hidden("ClientSubUnitGuid", ViewData["ClientSubUnitGuid"].ToString())%>
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

