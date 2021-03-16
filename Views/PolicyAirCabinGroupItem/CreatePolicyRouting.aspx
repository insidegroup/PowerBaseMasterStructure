<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CWTDesktopDatabase.ViewModels.PolicyAirCabinGroupItemViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DesktopDataAdmin - Policy Groups
</asp:Content>

<asp:Content ID="headerStuff" ContentPlaceHolderID="headerContent" runat="server">
<script src="<%=Url.Content("~/Scripts/ERD/PolicyAirCabinGroupItemPolicyRouting.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="contentarea">
    <div id="banner"><div id="banner_text">Policy Air Cabin Group Item</div></div>
        <div id="content">
        <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm()) {%>
        <%= Html.AntiForgeryToken() %>
        <%= Html.ValidationSummary(true) %>
            <table cellpadding="0" cellspacing="0" width="100%"> 
		        <tr> 
			        <th class="row_header" colspan="3">Create Routing</th> 
		        </tr> 
                <tr>
                    <td>PolicyGroup Name</td>
                    <td><strong><%= Html.Label(Model.PolicyAirCabinGroupItem.PolicyGroupName)%></strong></td>
                    <td></td>
                </tr>
                <tr>
                    <td><%= Html.Label("AirlineCabin Code")%></td>
                    <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.AirlineCabinDefaultDescription)%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Enabled?</td>
                    <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.EnabledFlag)%></td>
                    <td></td>
                </tr>  
                 <tr>
                <td>Enabled Date</td>
                <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.EnabledDate.HasValue ? Model.PolicyAirCabinGroupItem.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Expiry Date</td>
                <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.ExpiryDate.HasValue ? Model.PolicyAirCabinGroupItem.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Travel Date Valid From</td>
                <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.TravelDateValidFrom.HasValue ? Model.PolicyAirCabinGroupItem.TravelDateValidFrom.Value.ToString("MMM dd, yyyy") : "No Travel From Date")%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Travel Date Valid To</td>
                <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.TravelDateValidTo.HasValue ? Model.PolicyAirCabinGroupItem.TravelDateValidTo.Value.ToString("MMM dd, yyyy") : "No Travel To Date")%></td>
                <td></td>
            </tr>  
            <tr>
                 <td>Min Allowed Flight Duration</td>
                <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.FlightDurationAllowedMin)%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Min Allowed Flight Duration</td>
                <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.FlightDurationAllowedMin)%></td>
                <td></td>
            </tr> 
            <tr>
                <td>Max Allowed Flight Duration</td>
                <td> <%= Html.Encode(Model.PolicyAirCabinGroupItem.FlightDurationAllowedMax)%></td>
                <td></td>
            </tr>  
             <tr>
                <td>Mileage Minimum</td>
                <td> <%= Html.Encode(Model.PolicyAirCabinGroupItem.FlightMileageAllowedMin)%></td>
                <td></td>
            </tr>  
             <tr>
                <td>Mileage Maximum</td>
                <td> <%= Html.Encode(Model.PolicyAirCabinGroupItem.FlightMileageAllowedMax)%></td>
                <td></td>
            </tr>  

            <tr>
                <td>Policy Prohibited?</td>
                <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.PolicyProhibitedFlag)%></td>
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
                <td>Routing Name</td>
                <td><label id="lblAuto"></label></td>
                <td><%= Html.HiddenFor(model => model.PolicyRouting.Name)%><label id="lblPolicyRoutingNameMsg"/></td>
            </tr> 
            <tr>
                <td><label for="PolicyRouting_Name">From Global?</label></td>
                <td><%= Html.CheckBoxFor(model => model.PolicyRouting.FromGlobalFlag)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.FromGlobalFlag)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyRouting_FromCode">From</label></td>
                <td> <%= Html.TextBoxFor(model => model.PolicyRouting.FromCode)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.FromCode)%><label id="lblFrom"/></td>
            </tr> 
            <tr>
                <td><label for="PolicyRouting_ToGlobalFlag">To Global?</label></td>
                <td><%= Html.CheckBoxFor(model => model.PolicyRouting.ToGlobalFlag)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.ToGlobalFlag)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyRouting_ToCode">To</label></td>
                <td> <%= Html.TextBoxFor(model => model.PolicyRouting.ToCode)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.ToCode)%><label id="lblTo"/></td>
            </tr> 
            <tr>
                <td><label for="PolicyRouting_RoutingViceVersaFlag">Routing ViceVersa?</label></td>
                <td><%= Html.CheckBoxFor(model => model.PolicyRouting.RoutingViceVersaFlag)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.RoutingViceVersaFlag)%></td>
            </tr> 
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
            <tr>
                <td class="row_footer_blank_left"><a href="javascript:history.back();" class="red" title="Back">Back</a></td>
                <td class="row_footer_blank_right" colspan="2"><input type="submit" value="Save and Add another Routing" title="Save and Add another Routing" class="red" name="btnSubmit"/>&nbsp;<input type="submit" value="Save" title="Save" class="red" name="btnSubmit"/></td>
            </tr>
        </table>
           <%=Html.HiddenFor(model => model.PolicyRouting.FromCodeType)%>
           <%=Html.HiddenFor(model => model.PolicyRouting.ToCodeType)%>
           <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.PolicyGroupId)%>
           <%=Html.Hidden("PolicyAirCabinGroupItemId", Model.PolicyAirCabinGroupItem.PolicyAirCabinGroupItemId)%>
    <% } %>
   </div>
</div>
</asp:Content>
