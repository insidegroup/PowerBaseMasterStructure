<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.Models.CreditCard>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Credit Cards
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
    <div id="banner"><div id="banner_text">CWT Credit Cards</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		         <tr> 
			        <th class="row_header" colspan="3">Delete Credit Card</th> 
		        </tr> 
                                <tr>
                    <td>Client TopUnit</td>
                    <td><%= Html.Encode(Model.ClientTopUnitName)%></td>
                    <td></td>
                </tr>


                <tr>
                    <td>Credit Card Holder Name</td>
                    <td><%= Html.Encode(Model.CreditCardHolderName)%></td>
                    <td></td>
                </tr> 
                  <tr>
                    <td>Credit Card Number</td>
                    <td><%= Html.Encode(Model.MaskedCreditCardNumber)%></td>
                    <td></td>
                </tr> 
                  <tr>
                    <td>Credit Card Vendor</td>
                    <td><%= Html.Encode(Model.CreditCardVendorName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Credit Card Type</td>
                    <td><%= Html.Encode(Model.CreditCardTypeDescription)%></td>
                    <td></td>
                </tr> 
                  <tr>
                    <td>Credit Card Valid From</td>
                    <td><%= Html.Encode(Model.CreditCardValidFrom.HasValue ? Model.CreditCardValidFrom.Value.ToString("MMM yyyy") : "No ValidFrom Date")%></td>
                    <td></td>
                </tr> 
                  <tr>
                    <td>Credit Card Valid To</td>
                    <td><%= Html.Encode(Model.CreditCardValidTo.ToString("MMM yyyy"))%></td>
                    <td></td>
                </tr> 
                  <tr>
                    <td>Issue Number</td>
                    <td><%= Html.Encode(Model.CreditCardIssueNumber)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td><label for="HierarchyType">Hierarchy Type</label> </td>
                    <td><%= Html.Encode(Model.HierarchyType)%></td>
                    <td></td>
                </tr>
                <% if (Model.HierarchyType == "ClientSubUnitTravelerType"){ %>
                
                <tr>
                    <td><label for="ClientSubUnitName">Client Sub Unit Name</label> </td>
                    <td><%= Html.Encode(Model.ClientSubUnitName)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td><label for="TravelerTypeName">Traveler Type</label> </td>
                    <td><%= Html.Encode(Model.TravelerTypeName)%></td>
                    <td></td>
                </tr> <%}else{ %>
                <tr>
                    <td><%= Html.Label(Model.HierarchyType)%> </td>
                    <td><%= Html.Encode(Model.HierarchyItem)%></td>
                    <td></td>
                </tr> 
                <%}%>
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
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                        <%= Html.HiddenFor(model => model.VersionNumber) %>
                    <%}%>
                    </td>                
               </tr>
            </table>
        </div>
    </div>
<script type="text/javascript">
    $(document).ready(function () {
        //Navigation
        $('#menu_policies').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

