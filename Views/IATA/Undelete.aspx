<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.IATAVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - IATA
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">IATA</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">UnDelete IATA</th> 
		        </tr> 
                <tr>
                    <td><label for="IATA_IATABranchOrGLString">GDS</label></td>
                    <td colspan="2"><%= Html.Encode(Model.IATA.IATANumber)%></td>
                </tr> 
				<tr>
                    <td><label for="IATA_IATAName">Branch or GL String</label></td>
                    <td colspan="2"><%= Html.Encode(Model.IATA.IATABranchOrGLString)%></td>
                </tr>
				<tr>
                    <td><label for="IATA_AtatchedReferences">PCC/OIDs Attached</label></td>
                    <td colspan="2">
						<% foreach(CWTDesktopDatabase.Models.IATAReference IATAReference in Model.IATAReferences) { %>
							<%= Html.Encode(IATAReference.ReferenceName) %> 
						<%}%>
                    </td>
                </tr> 
				<tr>
                    <td><label for="IATA_DeletedFlag">Deleted?</label></td>
                    <td colspan="2"><%= Html.Encode(Model.IATA.DeletedFlag)%></td>
				</tr>
				<tr>
                    <td><label for="IATA_DeletedFlag">Deleted Date/Time</label></td>
                    <td colspan="2"><%= Html.Encode(Model.IATA.DeletedDateTime)%></td>
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
							<%= Html.HiddenFor(model => model.IATA.IATAId) %>
							<%= Html.HiddenFor(model => model.IATA.VersionNumber) %>
						<%}%>
                    </td>                
               </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
	$(document).ready(function () {
		$('#menu_gdsmanagement').click();
		$("tr:odd").addClass("row_odd");
		$("tr:even").addClass("row_even");
	})
</script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
GDS Management &gt;
<%=Html.RouteLink("IATA", "Main", new { controller = "IATA", action = "ListDeleted", }, new { title = "IATA" })%> &gt;
<%=Html.Encode(Model.IATA.IATANumber) %>
</asp:Content>