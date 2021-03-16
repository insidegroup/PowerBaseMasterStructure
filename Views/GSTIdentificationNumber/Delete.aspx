<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.GSTIdentificationNumberVM>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">GST Identification Number</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Delete GST Identification Number</th> 
		        </tr>  
                <tr>
				    <td><label for="ClientTopUnitGuid">Client Top Name</label></td>
				    <td><%= Html.Encode(Model.GSTIdentificationNumber.ClientTopUnitName)%></td>
				    <td></td>
			    </tr>   
			    <tr>
                    <td><label for="ClientEntityName">Client Entity Name</label></td>
                    <td><%= Html.Encode(Model.GSTIdentificationNumber.ClientEntityName)%></td>
				    <td></td>
                </tr>
			    <tr>
				    <td><label for="GSTIdentificationNumber1">GST Identification Number</label></td>
                    <td><%= Html.Encode(Model.GSTIdentificationNumber.GSTIdentificationNumber1)%></td>
				    <td></td>
			    </tr>
                <tr>
				    <td><label for="BusinessPhoneNumber">Business Phone Number</label></td>
                    <td><%= Html.Encode(Model.GSTIdentificationNumber.BusinessPhoneNumber)%></td>
				    <td></td>
			    </tr>
                <tr>
				    <td><label for="BusinessEmailAddress">Business Email Address</label></td>
                    <td><%= Html.Encode(Model.GSTIdentificationNumber.BusinessEmailAddress)%></td>
				    <td></td>
			    </tr>
                <tr>
				    <td><label for="FirstAddressLine">First Address Line</label></td>
                    <td><%= Html.Encode(Model.GSTIdentificationNumber.FirstAddressLine)%></td>
				    <td></td>
			    </tr>
                <tr>
				    <td><label for="SecondAddressLine">Second Address Line</label></td>
                    <td><%= Html.Encode(Model.GSTIdentificationNumber.SecondAddressLine)%></td>
				    <td></td>
			    </tr>
			    <tr>
				    <td><label for="CityName">City</label></td>
                    <td><%= Html.Encode(Model.GSTIdentificationNumber.CityName)%></td>
				    <td></td>
			    </tr>
			    <tr>
                    <td><label for="CountryName">Country</label></td>
                    <td><%= Html.Encode(Model.GSTIdentificationNumber.CountryName)%></td>
				    <td></td>
                </tr>
			    <tr>
                    <td><label for="StateProvinceCode">State/Province</label></td>
                    <td><%= Html.Encode(Model.GSTIdentificationNumber.StateProvinceName)%></td>
				    <td></td>
                </tr>
			    <tr>
				    <td><label for="PostalCode">Postal Code</label></td>
                    <td><%= Html.Encode(Model.GSTIdentificationNumber.PostalCode)%></td>
				    <td></td>
			    </tr>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
               </tr>
               <tr>
                    <td class="row_footer_blank_left">
						<a href="javascript:history.back();" class="red" title="Back">Back</a>&nbsp;
                    </td>
                    <td class="row_footer_blank_right" colspan="2">
						<% using (Html.BeginForm()) { %>
							<%= Html.AntiForgeryToken() %>
							<input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
							<%= Html.HiddenFor(model => model.GSTIdentificationNumber.GSTIdentificationNumberId) %>
							<%= Html.HiddenFor(model => model.GSTIdentificationNumber.VersionNumber) %>
						<%}%>
					</td>
				</tr>
			</table>
        </div>
    </div>
    <script type="text/javascript">
    $(document).ready(function() {
        $('#menu_admin').click();
	    $("tr:odd").addClass("row_odd");
	    $("tr:even").addClass("row_even");
    })
    </script>
</asp:Content>
<asp:Content ID="BreadCrumbNav" ContentPlaceHolderID="BreadCrumbContent" runat="server">
Admin &gt;
<%=Html.RouteLink("GST Identification Numbers", "Main", new { controller = "GSTIdentificationNumber", action = "List", }, new { title = "GST Identification Numbers" })%> &gt;
<%= Html.Encode(Model.GSTIdentificationNumber.GSTIdentificationNumber1) %>
</asp:Content>