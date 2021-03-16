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
                <td><label for="PolicyAirCabinGroupItem_AirlineCabinCode">Airline Cabin Code</label></td>
                <td><%= Html.DropDownList("PolicyAirCabinGroupItem.AirlineCabinCode", ViewData["AirlineCabinCodeList"] as SelectList, "None")%><span class="error"> *</span></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.AirlineCabinCode)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyAirCabinGroupItem_EnabledFlag">Enabled?</label></td>
                <td><%= Html.CheckBoxFor(model => model.PolicyAirCabinGroupItem.EnabledFlag)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.EnabledFlag)%></td>
            </tr>  
            <tr>
                <td><label for="PolicyAirCabinGroupItem_EnabledDate">Enabled Date</label></td>
                <td><%= Html.EditorFor(model => model.PolicyAirCabinGroupItem.EnabledDate)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.EnabledDate)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyAirCabinGroupItem_ExpiryDate">Expiry Date</label></td>
                <td> <%= Html.EditorFor(model => model.PolicyAirCabinGroupItem.ExpiryDate)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.ExpiryDate)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyAirCabinGroupItem_TravelDateValidFrom">Travel Date Valid From</label></td>
                <td><%= Html.EditorFor(model => model.PolicyAirCabinGroupItem.TravelDateValidFrom)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.TravelDateValidFrom)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyAirCabinGroupItem_TravelDateValidTo">Travel Date Valid To</label></td>
                <td> <%= Html.EditorFor(model => model.PolicyAirCabinGroupItem.TravelDateValidTo)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.TravelDateValidTo)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyAirCabinGroupItem_FlightDurationAllowedMin">Min Allowed Flight Duration</label></td>
                <td><%= Html.TextBoxFor(model => model.PolicyAirCabinGroupItem.FlightDurationAllowedMin, new { maxlength = "9" })%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.FlightDurationAllowedMin)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyAirCabinGroupItem_FlightDurationAllowedMax">Max Allowed Flight Duration</label></td>
                <td> <%= Html.TextBoxFor(model => model.PolicyAirCabinGroupItem.FlightDurationAllowedMax, new { maxlength="9" })%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.FlightDurationAllowedMax)%></td>
            </tr>  
             <tr>
                <td><label for="PolicyAirCabinGroupItem_FlightMileageAllowedMin">Mileage Minimum</label></td>
                <td><%= Html.TextBoxFor(model => model.PolicyAirCabinGroupItem.FlightMileageAllowedMin, new { maxlength = "6" })%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.FlightMileageAllowedMin)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyAirCabinGroupItem_FlightMileageAllowedMax">Mileage Maximum</label></td>
                <td> <%= Html.TextBoxFor(model => model.PolicyAirCabinGroupItem.FlightMileageAllowedMax, new { maxlength = "6" })%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.FlightMileageAllowedMax)%></td>
            </tr>  
            <tr>
                <td><label for="PolicyAirCabinGroupItem_PolicyProhibitedFlag">Policy Prohibited?</label></td>
                <td><%= Html.CheckBoxFor(model => model.PolicyAirCabinGroupItem.PolicyProhibitedFlag)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyAirCabinGroupItem.PolicyProhibitedFlag)%></td>
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
                <td><label id="lblAuto"><%=Html.Encode(Model.PolicyRouting.Name) %></label></td>
                <td><%= Html.HiddenFor(model => model.PolicyRouting.Name)%><label id="lblPolicyRoutingNameMsg"/></td>
            </tr>    
            <tr>
                <td><label for="PolicyRouting_FromGlobalFlag">From Global?</label> </td>
                <td><%= Html.CheckBoxFor(model => model.PolicyRouting.FromGlobalFlag)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.FromGlobalFlag)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyRouting_FromCode">From</label> </td>
                <td> <%= Html.TextBoxFor(model => model.PolicyRouting.FromCode)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.FromCode)%><label id="lblFrom"/></td>
            </tr> 
            <tr>
                <td><label for="PolicyRouting_ToGlobalFlag">To Global?</label> </td>
                <td><%= Html.CheckBoxFor(model => model.PolicyRouting.ToGlobalFlag)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.ToGlobalFlag)%></td>
            </tr> 
            <tr>
                <td><label for="PolicyRouting_ToCode">To</label> </td>
                <td> <%= Html.TextBoxFor(model => model.PolicyRouting.ToCode)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.ToCode)%><label id="lblTo"/></td>
            </tr> 
            <tr>
                <td><label for="PolicyRouting_RoutingViceVersaFlag">Routing Vice Versa?</label> </td>
                <td><%= Html.CheckBoxFor(model => model.PolicyRouting.RoutingViceVersaFlag)%></td>
                <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.RoutingViceVersaFlag)%></td>
            </tr> 
            <tr>
                <td width="30%" class="row_footer_left"></td>
                <td width="40%" class="row_footer_centre"></td>
                <td width="30%" class="row_footer_right"></td>
            </tr>
        </table>
        <input type="hidden" id="MandatoryPolicyRouting" value="0" />
        <%=Html.HiddenFor(model => model.PolicyRouting.PolicyRoutingId)%>
        <%=Html.HiddenFor(model => model.PolicyRouting.FromCodeType)%>
        <%=Html.HiddenFor(model => model.PolicyRouting.ToCodeType)%>
        <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.PolicyAirCabinGroupItemId)%>
        <%=Html.HiddenFor(model => model.PolicyAirCabinGroupItem.VersionNumber)%>
       
        
<% } %>
<script type="text/javascript">


    $(document).ready(function () {


        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");


        /////////////////////////////////////////////////////////
        // BEGIN DATES SETUP
        /////////////////////////////////////////////////////////

        new JsDatePick({
            useMode: 2,
            isStripped: false,
            target: "PolicyAirCabinGroupItem_EnabledDate",
            dateFormat: "%M %d %Y",
            cellColorScheme: "beige"
        });


        new JsDatePick({
            useMode: 2,
            isStripped: false,
            target: "PolicyAirCabinGroupItem_ExpiryDate",
            dateFormat: "%M %d %Y",
            cellColorScheme: "beige"
        });


        new JsDatePick({
            useMode: 2,
            isStripped: false,
            target: "PolicyAirCabinGroupItem_TravelDateValidTo",
            dateFormat: "%M %d %Y",
            cellColorScheme: "beige"
        });

        new JsDatePick({
            useMode: 2,
            isStripped: false,
            target: "PolicyAirCabinGroupItem_TravelDateValidFrom",
            dateFormat: "%M %d %Y",
            cellColorScheme: "beige"
        });



        if ($('#PolicyAirCabinGroupItem_EnabledDate').val() == "") {
            $('#PolicyAirCabinGroupItem_EnabledDate').val("No Enabled Date");
        }
        if ($('#PolicyAirCabinGroupItem_ExpiryDate').val() == "") {
            $('#PolicyAirCabinGroupItem_ExpiryDate').val("No Expiry Date");
        }
        if ($('#PolicyAirCabinGroupItem_TravelDateValidFrom').val() == "") {
            $('#PolicyAirCabinGroupItem_TravelDateValidFrom').val("No Travel Date Valid From");
        }
        if ($('#PolicyAirCabinGroupItem_TravelDateValidTo').val() == "") {
            $('#PolicyAirCabinGroupItem_TravelDateValidTo').val("No Travel Date Valid To");
        }
        /////////////////////////////////////////////////////////
        // END DATES SETUP
        /////////////////////////////////////////////////////////

        // POLICY ROUTING SETUP - ONLY NEEDED FOR EDIT SCREENS
        if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
            $("#PolicyRouting_FromCode").attr('disabled', 'disabled');
        }
        if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
            $("#PolicyRouting_ToCode").attr('disabled', 'disabled');
        }

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
