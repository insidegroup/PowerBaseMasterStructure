<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.PolicyAirCabinGroupItemViewModel>" %>
<% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
<table cellpadding="0" cellspacing="0" width="100%"> 
    <tr> 
		<th class="row_header" colspan="3">Policy Air Cabin Group Item</th> 
	</tr> 
    <tr>
        <td>AirlineCabin Code</td>
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
        <td>FlightDuration Allowed Min</td>
        <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.FlightDurationAllowedMin)%></td>
        <td></td>
    </tr> 
    <tr>
        <td>FlightDuration Allowed Max</td>
        <td> <%= Html.Encode(Model.PolicyAirCabinGroupItem.FlightDurationAllowedMax)%></td>
        <td></td>
    </tr> 
    <tr>
        <td>Mileage Minimum</td>
        <td><%= Html.Encode(Model.PolicyAirCabinGroupItem.FlightMileageAllowedMin)%></td>
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
        <td><label for="PolicyRouting_FromGlobalFlag">From Global?</label> </td>
        <td><%= Html.CheckBoxFor(model => model.PolicyRouting.FromGlobalFlag)%></td>
        <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.FromGlobalFlag)%></td>
    </tr> 
    <tr>
        <td><label for="PolicyRouting_FromCode">From</label> </td>
        <td> <%= Html.TextBoxFor(model => model.PolicyRouting.FromCode)%><span class="error"> *</span></td>
        <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.FromCode)%><label id="lblFrom"/></td>
    </tr> 
    <tr>
        <td><label for="PolicyRouting_ToGlobalFlag">To Global?</label> </td>
        <td><%= Html.CheckBoxFor(model => model.PolicyRouting.ToGlobalFlag)%></td>
        <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.ToGlobalFlag)%></td>
    </tr> 
    <tr>
        <td><label for="PolicyRouting_ToCode">To</label> </td>
        <td> <%= Html.TextBoxFor(model => model.PolicyRouting.ToCode)%><span class="error"> *</span></td>
        <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.ToCode)%><label id="lblTo"/></td>
    </tr> 
    <tr>
        <td><label for="PolicyRouting_RoutingViceVersaFlag">Routing Vice Versa?</label> </td>
        <td><%=Html.CheckBox("PolicyRouting_RoutingViceVersaFlag", Model.PolicyRouting.RoutingViceVersaFlag,null)%></td>
        <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.RoutingViceVersaFlag)%></td>
    </tr> 
    <tr>
        <td width="30%" class="row_footer_left"></td>
        <td width="40%" class="row_footer_centre"></td>
        <td width="30%" class="row_footer_right"></td>
    </tr>
</table>
            <input type="hidden" id="MandatoryPolicyRouting" value="1" />
           <%=Html.HiddenFor(model => model.PolicyRouting.FromCodeType)%>
           <%=Html.HiddenFor(model => model.PolicyRouting.ToCodeType)%>
           <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.PolicyAirCabinGroupItemId)%>
           <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.PolicyGroupId)%>
           <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.AirlineCabinCode)%>
           <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.FlightDurationAllowedMin)%>
           <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.FlightDurationAllowedMax)%>
           <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.FlightMileageAllowedMin)%>
           <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.FlightMileageAllowedMax)%>
           <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.PolicyProhibitedFlag)%>
           <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.EnabledFlag)%>
           <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.EnabledDate)%>
           <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.ExpiryDate)%>
           <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.TravelDateValidFrom)%>
           <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.TravelDateValidTo)%>
    <% } %>
<script type="text/javascript">
$(document).ready(function () {
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    /////////////////////////////////////////////////////////
    // BEGIN POLICY ROUTING SETUP
    /////////////////////////////////////////////////////////

    //If FromCode is changed to blank, clear the Routing Name
    $("#PolicyRouting_FromCode").change(function () {
        if ($("#PolicyRouting_FromCode").val() == "") {
            $("#lblAuto").text("");
            $("#PolicyRouting_Name").val("");
            $("#lblPolicyRoutingNameMsg").text("");
        }
    });

    //If ToCode is changed to blank, clear the Routing Name
    $("#PolicyRouting_ToCode").change(function () {
        if ($("#PolicyRouting_ToCode").val() == "") {
            $("#lblAuto").text("");
            $("#PolicyRouting_Name").val("");
            $("#lblPolicyRoutingNameMsg").text("");
        }
    });

    /*OnClick of From Global Checkbox, if true Build Routing Name, if false, Clear Routing Name*/
    $("#PolicyRouting_FromGlobalFlag").click(function () {
        if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
            $("#PolicyRouting_FromCode").val("");
            $("#PolicyRouting_FromCode").attr('disabled', 'disabled');
            $("#PolicyRouting_FromCode_validationMessage").text("");
            $("#lblFrom").text("");

            var to = $("#PolicyRouting_ToCode").val();
            if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
                to = "Global";
            }
            BuildRoutingName("Global", to);
        } else {
            $("#PolicyRouting_FromCode").removeAttr('disabled');
            $("#lblAuto").text("");
            $("#PolicyRouting_Name").val("");
            $("#lblPolicyRoutingNameMsg").text("");
        }
    });

    /*OnClick of To Global Checkbox, if true Build Routing Name, if false, Clear Routing Name*/
    $("#PolicyRouting_ToGlobalFlag").click(function () {
        if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
            $("#PolicyRouting_ToCode").val("");
            $("#PolicyRouting_ToCode").attr('disabled', 'disabled');
            $("#PolicyRouting_ToCode_validationMessage").text("");
            $("#lblTo").text("");

            var from = $("#PolicyRouting_FromCode").val();
            if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
                from = "Global";
            }
            BuildRoutingName(from, "Global");
        } else {
            $("#PolicyRouting_ToCode").removeAttr('disabled');
            $("#lblAuto").text("");
            $("#PolicyRouting_Name").val("");
            $("#lblPolicyRoutingNameMsg").text("");
        }
    });

    /*AutoComplete*/
    $(function () {
        $("#PolicyRouting_ToCode").autocomplete({

            source: function (request, response) {
                $.ajax({
                    url: "/PolicyRouting.mvc/AutoCompletePolicyRoutingFromTo", type: "POST", dataType: "json",
                    data: { searchText: request.term, maxResults: 10 },
                    success: function (data) {

                        response($.map(data, function (item) {
                            return {
                                label: (item.CodeType == "TravelPortType") ? item.Name : item.Name + "," + item.Parent,
                                value: item.Code,
                                id: item.CodeType
                            }
                        }))
                    }
                })
            },
            select: function (event, ui) {
                if (ui.item.id == "TravelPortType") {
                    $("#lblTo").text("TravelPort Type");
                } else {
                    $("#lblTo").text(ui.item.label + ' (' + ui.item.value + ')');
                }
                $("#PolicyRouting_ToCodeType").val(ui.item.id);
                $("#PolicyRouting_ToGlobalFlag").attr('checked', false);


                var from = $("#PolicyRouting_FromCode").val();
                if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
                    from = "Global";
                }
                BuildRoutingName(from, ui.item.value);
            }
        });

    });

    /*AutoComplete*/
    $(function () {
        $("#PolicyRouting_FromCode").autocomplete({

            source: function (request, response) {
                $.ajax({
                    url: "/PolicyRouting.mvc/AutoCompletePolicyRoutingFromTo", type: "POST", dataType: "json",
                    data: { searchText: request.term, maxResults: 10 },
                    success: function (data) {

                        response($.map(data, function (item) {
                            return {
                                label: (item.CodeType == "TravelPortType") ? item.Name : item.Name + "," + item.Parent,
                                value: item.Code,
                                id: item.CodeType
                            }
                        }))
                    }
                })
            },
            select: function (event, ui) {
                if (ui.item.id == "TravelPortType") {
                    $("#lblFrom").text("TravelPort Type");
                } else {
                    $("#lblFrom").text(ui.item.label + ' (' + ui.item.value + ')');
                }
                $("#PolicyRouting_FromCodeType").val(ui.item.id);
                $("#PolicyRouting_FromGlobalFlag").attr('checked', false);

                var to = $("#PolicyRouting_ToCode").val();
                if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
                    to = "Global";
                }
                BuildRoutingName(ui.item.value, to);
            }
        });

    });

    function BuildRoutingName(from, to) {
        if (from == "" || to == "") {
            $("#lblAuto").text("");
            $("#PolicyRouting_Name").val("");
            $("#lblPolicyRoutingNameMsg").text("");
            return;
        }
        var autoName = from + "_to_" + to + "_AirCabin";
        autoName = autoName.replace(/ /g, "_");
        autoName = autoName.substring(0, 95);

        $.ajax({
            url: "/PolicyRouting.mvc/BuildRoutingName", type: "POST", dataType: "json",
            data: { routingName: autoName },
            success: function (data) {

                //list of unavailable names
                var arrUnAvailableNames = $("#UnAvailableRoutingNames").val().split(",");

                if (jQuery.inArray(data, arrUnAvailableNames) > -1) {
                    var data = GetAvailableAirCabinRoutingName(data);
                    $("#lblAuto").text(data);
                    $("#PolicyRouting_Name").val(data);
                    $("#lblPolicyRoutingNameMsg").text("");
                } else {
                    //does not exist, use for PolicyROutingName
                    $("#lblAuto").text(data);
                    $("#PolicyRouting_Name").val(data);
                    $("#lblPolicyRoutingNameMsg").text("");
                }
            }
        });
    }

    //this checks locally for Available Routing Names
    //In Wizard we can add multiple Policies at the same time
    //Checking against DB only does not work for this
    function GetAvailableAirCabinRoutingName(data) {
        var arrUnAvailableNames = $("#UnAvailableRoutingNames").val().split(",");
        if (jQuery.inArray(data, arrUnAvailableNames) > -1) {
            var autoName = data.substring(0, data.length - 5);
            var count = data.substring(data.length - 4, data.length);
            count++;
            var str = '' + count;
            while (str.length < 4) {
                str = '0' + str;
            }
            data = autoName + "_" + str;
            if (jQuery.inArray(data, arrUnAvailableNames) > -1) {
            	//data = GetAvailableName(data);
            	data = GetAvailableAirCabinRoutingName(data);
            }
            return data;
        }
    }
    /////////////////////////////////////////////////////////
    // END POLICY ROUTING SETUP
    /////////////////////////////////////////////////////////
});
</script>
