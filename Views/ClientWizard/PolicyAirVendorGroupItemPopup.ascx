<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.PolicyAirVendorGroupItemVM>" %>
<% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
           <table cellpadding="0" cellspacing="0" width="100%"> 
                <tr>
                    <td><label for="PolicyAirVendorGroupItem_PolicyAirStatusId">Air Status</label></td>
                    <td><%= Html.DropDownListFor(model => model.PolicyAirVendorGroupItem.PolicyAirStatusId, ViewData["PolicyAirStatusList"] as SelectList, "None")%> <span class="error"> *</span></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirVendorGroupItem.PolicyAirStatusId)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirVendorGroupItem_EnabledFlag">Enabled?</label></td>
                    <td><%= Html.CheckBoxFor(model => model.PolicyAirVendorGroupItem.EnabledFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirVendorGroupItem.EnabledFlag)%></td>
                </tr>  
                <tr>
                    <td><label for="PolicyAirVendorGroupItem_EnabledDate">Enabled Date</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyAirVendorGroupItem.EnabledDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirVendorGroupItem.EnabledDate)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirVendorGroupItem_ExpiryDate">Expiry Date</label> </td>
                    <td> <%= Html.EditorFor(model => model.PolicyAirVendorGroupItem.ExpiryDate)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirVendorGroupItem.ExpiryDate)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirVendorGroupItem_TravelDateValidFrom">Travel Date Valid From</label></td>
                    <td><%= Html.EditorFor(model => model.PolicyAirVendorGroupItem.TravelDateValidFrom)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirVendorGroupItem.TravelDateValidFrom)%></td>
                </tr> 
                <tr>
                    <td><label for="PolicyAirVendorGroupItem_TravelDateValidTo">Travel Date Valid To</label></td>
                    <td> <%= Html.EditorFor(model => model.PolicyAirVendorGroupItem.TravelDateValidTo)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirVendorGroupItem.TravelDateValidTo)%></td>
                </tr>  
                 <tr>
                    <td><label for="PolicyAirVendorGroupItem_SupplierName">Supplier</label></td>
                    <td> <%= Html.TextBoxFor(model => model.PolicyAirVendorGroupItem.SupplierName)%><span class="error"> *</span></td>
                    <td>
                        <%= Html.ValidationMessageFor(model => model.PolicyAirVendorGroupItem.SupplierName)%>
                        <%= Html.HiddenFor(model => model.PolicyAirVendorGroupItem.SupplierCode)%>
                        <label id="lblSupplierNameMsg"/>
                    </td>
                </tr>
                 <tr>
                    <td><label for="PolicyAirVendorGroupItem_AirVendorRanking">Ranking</label></td>
                    <td><%= Html.DropDownList("PolicyAirVendorGroupItem.AirVendorRanking", ViewData["AirVendorRankings"] as SelectList, "None")%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyAirVendorGroupItem.AirVendorRanking)%></td>
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
			        <th class="row_header" colspan="3">Policy Routing<span class="error"> *</span></th> 
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
                    <td><%= Html.CheckBoxFor(model => model.PolicyRouting.RoutingViceVersaFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.RoutingViceVersaFlag)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
            </table>
           <%=Html.HiddenFor(model => model.PolicyRouting.PolicyRoutingId)%>
           <%=Html.HiddenFor(model => model.PolicyRouting.Name)%>
           <%=Html.HiddenFor(model => model.PolicyRouting.FromCodeType)%>
           <%=Html.HiddenFor(model => model.PolicyRouting.ToCodeType)%>
           <%=Html.HiddenFor(model => model.PolicyAirVendorGroupItem.PolicyAirVendorGroupItemId)%>
           <%=Html.HiddenFor(model => model.PolicyAirVendorGroupItem.PolicyGroupId)%>
           <%=Html.HiddenFor(model => model.PolicyAirVendorGroupItem.VersionNumber)%>
           <%=Html.Hidden("PolicyAirVendorGroupItem_ProductId","1") %><!--Air-->
    <% } %>
<script type="text/javascript">
$(document).ready(function () {
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    /////////////////////////////////////////////////////////
    // BEGIN DATES SETUP
    /////////////////////////////////////////////////////////
	
	new JsDatePick({
			useMode:2,
			isStripped:false,
		    target:"PolicyAirVendorGroupItem_ExpiryDate",
			dateFormat:"%M %d %Y",
			cellColorScheme:"beige"
		});

new JsDatePick({
			useMode:2,
			isStripped:false,
		    target:"PolicyAirVendorGroupItem_EnabledDate",
			dateFormat:"%M %d %Y",
			cellColorScheme:"beige"
		});

   
new JsDatePick({
			useMode:2,
			isStripped:false,
		    target:"PolicyAirVendorGroupItem_TravelDateValidFrom",
			dateFormat:"%M %d %Y",
			cellColorScheme:"beige"
		});

new JsDatePick({
			useMode:2,
			isStripped:false,
		    target:"PolicyAirVendorGroupItem_TravelDateValidTo",
			dateFormat:"%M %d %Y",
			cellColorScheme:"beige"
		});



    if ($('#PolicyAirVendorGroupItem_EnabledDate').val() == "") {
        $('#PolicyAirVendorGroupItem_EnabledDate').val("No Enabled Date");
    }
    if ($('#PolicyAirVendorGroupItem_ExpiryDate').val() == "") {
        $('#PolicyAirVendorGroupItem_ExpiryDate').val("No Expiry Date");
    }
    if ($('#PolicyAirVendorGroupItem_TravelDateValidFrom').val() == "") {
        $('#PolicyAirVendorGroupItem_TravelDateValidFrom').val("No Travel Date Valid From");
    }
    if ($('#PolicyAirVendorGroupItem_TravelDateValidTo').val() == "") {
        $('#PolicyAirVendorGroupItem_TravelDateValidTo').val("No Travel Date Valid To");
    }
    /////////////////////////////////////////////////////////
    // END DATES SETUP
    /////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER SETUP - Version: No Product Select
    /////////////////////////////////////////////////////////

    $("#PolicyAirVendorGroupItem_SupplierName").change(function () {
        if ($("#PolicyAirVendorGroupItem_SupplierName").val() == "") {
            $("#PolicyAirVendorGroupItem_SupplierCode").val("");
            $("#SupplierCode").val("");
        }
    });
    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER SETUP
    /////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////
    // BEGIN AIRSTATUS SETUP
    /////////////////////////////////////////////////////////
    if ($("#PolicyAirVendorGroupItem_PolicyAirStatusId").val() != "1") {
        $("#PolicyAirVendorGroupItem_AirVendorRanking").attr("disabled", true);
    }
    $("#PolicyAirVendorGroupItem_PolicyAirStatusId").change(function () {
        if ($("#PolicyAirVendorGroupItem_PolicyAirStatusId").val() != "1") {
            $("#PolicyAirVendorGroupItem_AirVendorRanking").val("");
            $("#PolicyAirVendorGroupItem_AirVendorRanking").attr("disabled", true);
            $("#PolicyAirVendorGroupItem_AirVendorRanking_validationMessage").text("");
        } else {
            $("#PolicyAirVendorGroupItem_AirVendorRanking").removeAttr("disabled");
        }
    });
    /////////////////////////////////////////////////////////
    // END AIRSTATUS SETUP
    /////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////
    // BEGIN PRODUCT/SUPPLIER/POLICYROUTING AUTOCOMPLETE
    /////////////////////////////////////////////////////////
    $(function () {
        $("#PolicyAirVendorGroupItem_SupplierName").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/AutoComplete.mvc/ProductSuppliers", type: "POST", dataType: "json",
                    data: { searchText: request.term, productId: $("#PolicyAirVendorGroupItem_ProductId").val() },
                    success: function (data) {
                        
                        response($.map(data, function (item) {
                            return {
                                id: item.SupplierCode,
                                value: item.SupplierName
                            }
                        }))
                    }
                })
            },
            mustMatch: true,
            select: function (event, ui) {
                $("#PolicyAirVendorGroupItem_SupplierName").val(ui.item.value);
                $("#PolicyAirVendorGroupItem_SupplierCode").val(ui.item.id);
                $("#lblSupplierNameMsg").text("");

                var from = $("#PolicyRouting_FromCode").val();
                if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
                    from = "Global";
                }
                var to = $("#PolicyRouting_ToCode").val();
                if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
                    to = "Global";
                }
                BuildRoutingName(from, to);

            }
        });

    });
    /////////////////////////////////////////////////////////
    // END PRODUCT/SUPPLIER/POLICYROUTING AUTOCOMPLETE
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
            $("#PolicyRouting_FromCodeType").val("Global");
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
            $("#PolicyRouting_ToCodeType").val("Global");
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
        if ($("#PolicyAirVendorGroupItem_SupplierCode").val() == "" || from == "" || to == "") {
            $("#lblAuto").text("");
            $("#PolicyRouting_Name").val("");
            $("#lblPolicyRoutingNameMsg").text("");
            return;
        }
        var autoName = from + "_to_" + to + "_on_" + $("#PolicyAirVendorGroupItem_SupplierCode").val();
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
