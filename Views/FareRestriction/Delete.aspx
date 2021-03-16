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
			        <th class="row_header" colspan="3">Delete FareRestriction</th> 
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
			        <th class="row_header" colspan="3">PolicyRouting</th> 
		        </tr> 
                <tr>
                    <td>Name</td>
                    <td><%= Html.Encode(Model.PolicyRouting.Name)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>FromGlobalFlag</td>
                     <td> <%if (Model.PolicyRouting.PolicyRoutingId > 0)
                           {  %><%= Html.Encode(Model.PolicyRouting.FromGlobalFlag)%><%} %>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>From</td>
                    <td><%= Html.Encode(Model.PolicyRouting.FromCode)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>ToGlobalFlag</td>
                    <td> <%if (Model.PolicyRouting.PolicyRoutingId > 0)
                           {  %><%= Html.Encode(Model.PolicyRouting.ToGlobalFlag)%><%} %>
                    </td>
                    <td></td>
                </tr> <tr>
                    <td>To</td>
                    <td><%= Html.Encode(Model.PolicyRouting.ToCode)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>RoutingViceVersaFlag</td>
                    <td> <%if (Model.PolicyRouting.PolicyRoutingId > 0)
                           {  %><%= Html.Encode(Model.PolicyRouting.RoutingViceVersaFlag)%><%} %>
                    </td>
                    <td></td>
                </tr>
                <%if (!ViewData.ModelState.IsValid) { %>
                <%if (ViewData.ModelState["Exception"].Errors.Count > 0 ){ %>
                <tr>
                    <td></td>
                    <td colspan="2"><span class="error"><% =ViewData.ModelState["Exception"].Errors[0].ErrorMessage %></span></td>
                </tr> 
                <% } %>
                <% } %>
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
                <tr>
                    <td class="row_footer_blank_left"><%= Html.ActionLink("Back to List", "List", null, new { @class = "red", title = "Back To List" })%></td>
                    <td class="row_footer_blank_right" colspan="2">
                    <% using (Html.BeginForm()) { %>
                        <%= Html.AntiForgeryToken() %>
                        <input type="submit" value="Confirm Delete" title="Confirm Delete" class="red"/>
                        <%= Html.Hidden("VersionNumber",Model.FareRestriction.VersionNumber)%>
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

