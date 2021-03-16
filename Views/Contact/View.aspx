<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.ContactVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Client SubUnit - Contacts
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Client SubUnits - Contacts</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		          <tr> 
			        <th class="row_header" colspan="3">View Contact</th> 
		        </tr> 
                <tr>
                    <td>Contact Type</td>
                    <td><%= Html.Encode(Model.Contact.ContactType.ContactTypeName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Last Name</td>
                    <td><%= Html.Encode(Model.Contact.LastName)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>First Name</td>
                    <td><%= Html.Encode(Model.Contact.FirstName)%></td>
                    <td></td>
				</tr>
                <tr>
                    <td>Phone</td>
                    <td><%= Html.Encode(Model.Contact.PrimaryTelephoneNumber)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Emergency Phone</td>
                    <td><%= Html.Encode(Model.Contact.EmergencyTelephoneNumber)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Email</td>
                    <td><%= Html.Encode(Model.Contact.EmailAddress)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>Country</td>
                    <td><%= Html.Encode(Model.Contact.Country.CountryName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>First Address Line</td>
                    <td><%= Html.Encode(Model.Contact.FirstAddressLine)%></td>
                    <td></td>
                </tr> 
				<tr>
                    <td>Second Address Line</td>
                    <td><%= Html.Encode(Model.Contact.SecondAddressLine)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>City</td>
                    <td><%= Html.Encode(Model.Contact.CityName)%></td>
                    <td></td>
                </tr>
				<tr>
                    <td>State/Province</td>
                    <td><%= Html.Encode(Model.Contact.StateProvinceName)%></td>
                    <td></td>
                </tr> 
				<tr>
                    <td>Postal Code</td>
                    <td><%= Html.Encode(Model.Contact.PostalCode)%></td>
                    <td></td>
                </tr> 
				<tr>
                    <td>Responsibility</td>
                    <td><%= Html.Encode(Model.Contact.ResponsibilityDescription)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                  <tr>
                    <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;<a href="javascript:window.print();" class="red" title="Print">Print</a></td>
                    <td class="row_footer_blank_right" colspan="2"></td>
                </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        $('#menu_clients').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%=Html.RouteLink("ClientTopUnits", "Main", new { controller = "ClientTopUnit", action = "List", }, new { title = "ClientTopUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientTopUnitName.ToString(), "Main", new { controller = "ClientTopUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientTopUnitGuid.ToString() }, new { title = Model.ClientSubUnit.ClientTopUnitName.ToString() })%> &gt;
<%=Html.RouteLink("Client SubUnits", "Main", new { controller = "ClientSubUnit", action = "List", id = Model.ClientSubUnit.ClientTopUnitGuid.ToString() }, new { title = "Client SubUnits" })%> &gt;
<%=Html.RouteLink(Model.ClientSubUnit.ClientSubUnitName.ToString(), "Main", new { controller = "ClientSubUnit", action = "ViewItem", id = Model.ClientSubUnit.ClientSubUnitGuid.ToString() }, new { title = Model.ClientSubUnit.ClientSubUnitName.ToString() })%> &gt;
View Contact
</asp:Content>