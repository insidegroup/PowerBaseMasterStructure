<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.FareRestrictionViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Admin
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div id="contentarea">
     <div id="banner"><div id="banner_text">FareRestrictions</div></div>
        <div id="content">
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">View FareRestriction</th> 
		        </tr> 
                <tr>
                    <td>FareBasis</td>
                    <td><%= Html.Encode(Model.FareRestriction.FareBasis)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Changes</td>
                    <td><%= Html.Encode(Model.FareRestriction.Changes)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Cancellations</td>
                    <td><%= Html.Encode(Model.FareRestriction.Cancellations)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>ReRoute</td>
                    <td><%= Html.Encode(Model.FareRestriction.ReRoute)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>ValidOn</td>
                    <td><%= Html.Encode(Model.FareRestriction.ValidOn)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>MinimumStay</td>
                    <td><%= Html.Encode(Model.FareRestriction.MinimumStay)%></td>
                    <td></td>
                </tr>
                <tr>
                    <td>MaximumStay</td>
                    <td><%= Html.Encode(Model.FareRestriction.MaximumStay)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Supplier</td>
                    <td><%= Html.Encode(Model.FareRestriction.SupplierName)%></td>
                    <td></td>
                </tr>          
                <tr>
                    <td>Product</td>
                    <td><%= Html.Encode(Model.FareRestriction.ProductName)%></td>
                    <td></td>
                </tr>          
                <tr>
                    <td>LanguageCode</td>
                    <td><%= Html.Encode(Model.FareRestriction.LanguageName)%></td>
                    <td></td>
                </tr>          
               <tr>
                    <td>DefaultChecked?</td>
                    <td><%= Html.Encode(Model.FareRestriction.DefaultChecked)%></td>
                    <td></td>
                </tr>          
       
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
               
                  <tr>
                    <td class="row_footer_blank" colspan="3"></td>
                </tr>
                <tr> 
			        <th class="row_header" colspan="3">Policy Routing</th> 
		        </tr> 
                <tr>
                    <td>Policy Routing Name</td>
                    <td><%= Html.Encode(Model.PolicyRouting.Name)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>From Global?</td>
                     <td> <%if (Model.PolicyRouting.PolicyRoutingId > 0)
                           {  %><%= Html.Encode(Model.PolicyRouting.FromGlobalFlag)%><%} %>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>From></td>
                    <td><%= Html.Encode(Model.PolicyRouting.FromCode)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>To Global?</td>
                    <td> <%if (Model.PolicyRouting.PolicyRoutingId > 0){  %><%= Html.Encode(Model.PolicyRouting.ToGlobalFlag)%><%} %></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>To</td>
                    <td><%= Html.Encode(Model.PolicyRouting.ToCode)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Routing Vice Versa? </td>
                    <td> <%if (Model.PolicyRouting.PolicyRoutingId > 0){  %><%= Html.Encode(Model.PolicyRouting.RoutingViceVersaFlag)%><%} %></td>
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
    $(document).ready(function() {
        $('#menu_admin').click();
        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");
    })
 </script>
</asp:Content>

