<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CWTDesktopDatabase.ViewModels.PolicyAirVendorGroupItemVM>" %>
<% Html.EnableClientValidation(); %>

        <% using (Html.BeginForm()) {%>
            <%= Html.AntiForgeryToken() %>
            <%= Html.ValidationSummary(true) %>
           <table cellpadding="0" cellspacing="0" width="100%"> 
                <tr>
                    <td>Air Status</td>
                    <td>
						<%= Html.Encode(Model.PolicyAirVendorGroupItem.PolicyAirStatus)%>
						<%= Html.HiddenFor(model => model.PolicyAirVendorGroupItem.PolicyAirStatus)%>
                    </td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Enabled?</td>
                    <td>
						<%= Html.Encode(Model.PolicyAirVendorGroupItem.EnabledFlag)%>
						<%= Html.HiddenFor(model => model.PolicyAirVendorGroupItem.EnabledFlag)%>
                    </td>
                    <td></td>
                </tr>  
                <tr>
                    <td>Enabled Date</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.EnabledDate.HasValue ? Model.PolicyAirVendorGroupItem.EnabledDate.Value.ToString("MMM dd, yyyy") : "No Enabled Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Expiry Date</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.ExpiryDate.HasValue ? Model.PolicyAirVendorGroupItem.ExpiryDate.Value.ToString("MMM dd, yyyy") : "No Expiry Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Travel Date Valid From</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.TravelDateValidFrom.HasValue ? Model.PolicyAirVendorGroupItem.TravelDateValidFrom.Value.ToString("MMM dd, yyyy") : "No Travel From Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Travel Date Valid To</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.TravelDateValidTo.HasValue ? Model.PolicyAirVendorGroupItem.TravelDateValidTo.Value.ToString("MMM dd, yyyy") : "No Travel To Date")%></td>
                    <td></td>
                </tr> 
                <tr>
                    <td>Product</td>
                    <td> <%= Html.Encode(Model.PolicyAirVendorGroupItem.ProductName)%></td>
                    <td></td>
                </tr> 
                 <tr>
                    <td>Supplier</td>
                    <td> <%= Html.Encode(Model.PolicyAirVendorGroupItem.SupplierName)%></td>
                    <td></td>
                </tr>
                 <tr>
                    <td>Ranking</td>
                    <td><%= Html.Encode(Model.PolicyAirVendorGroupItem.AirVendorRanking)%></td>
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
			        <th class="row_header" colspan="3">Policy Routing<span class="error"> *</span></th> 
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
                    <td><%= Html.CheckBoxFor(model => model.PolicyRouting.RoutingViceVersaFlag)%></td>
                    <td><%= Html.ValidationMessageFor(model => model.PolicyRouting.RoutingViceVersaFlag)%></td>
                </tr> 
                <tr>
                    <td width="30%" class="row_footer_left"></td>
                    <td width="40%" class="row_footer_centre"></td>
                    <td width="30%" class="row_footer_right"></td>
                </tr>
            </table>
            </table>
            <%=Html.HiddenFor(model => model.PolicyRouting.PolicyRoutingId)%>
           <%=Html.HiddenFor(model => model.PolicyRouting.FromCodeType)%>
           <%=Html.HiddenFor(model => model.PolicyRouting.ToCodeType)%>
           <%=Html.HiddenFor(model => model.PolicyAirVendorGroupItem.PolicyAirVendorGroupItemId)%>
           <%=Html.HiddenFor(model => model.PolicyAirVendorGroupItem.PolicyGroupId)%>
           <%= Html.HiddenFor(model => model.PolicyAirVendorGroupItem.PolicyAirStatusId)%>
           <%= Html.HiddenFor(model => model.PolicyAirVendorGroupItem.EnabledFlag)%>
           <%= Html.HiddenFor(model => model.PolicyAirVendorGroupItem.SupplierName)%>
           <%= Html.HiddenFor(model => model.PolicyAirVendorGroupItem.AirVendorRanking)%>
           <%= Html.HiddenFor(model => model.PolicyAirVendorGroupItem.SequenceNumber)%>
           <%= Html.HiddenFor(model => model.PolicyAirVendorGroupItem.EnabledDate)%>
           <%= Html.HiddenFor(model => model.PolicyAirVendorGroupItem.ExpiryDate)%>
           <%= Html.HiddenFor(model => model.PolicyAirVendorGroupItem.TravelDateValidFrom)%>
           <%= Html.HiddenFor(model => model.PolicyAirVendorGroupItem.TravelDateValidTo)%>
           <%= Html.HiddenFor(model => model.PolicyAirVendorGroupItem.SupplierCode)%>
           <%= Html.HiddenFor(model => model.PolicyAirVendorGroupItem.ProductId)%>
           <%=Html.HiddenFor(model => model.PolicyAirVendorGroupItem.VersionNumber)%>
    <% } %>
<script type="text/javascript">
    $(document).ready(function () {

        $("tr:odd").addClass("row_odd");
        $("tr:even").addClass("row_even");


        $("#PolicyAirVendorGroupItem_ProductId").change(function () {
            $("#PolicyAirVendorGroupItem_SupplierName").val("");
            $("#lblAuto").text("");
            $("#PolicyRouting_Name").val("");
            $("#lblPolicyRoutingNameMsg").text("");

            if ($("#PolicyAirVendorGroupItem_ProductId").val() == "") {
                $("#PolicyAirVendorGroupItem_SupplierName").attr("disabled", true);
            } else {
                $("#PolicyAirVendorGroupItem_SupplierName").removeAttr("disabled");
            }
        });

        $("#PolicyAirVendorGroupItem_SupplierName").change(function () {
            if ($("#PolicyAirVendorGroupItem_SupplierName").val() == "") {
                $("#PolicyAirVendorGroupItem_SupplierCode").val("");
                $("#SupplierCode").val("");
            }
        });



        $(function () {
            $("#PolicyAirVendorGroupItem_SupplierName").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "/Product.mvc/AutoCompleteProductSuppliers", type: "POST", dataType: "json",
                        data: { searchText: request.term, productId: $("#PolicyAirVendorGroupItem_ProductId").val(), maxResults: "10" },
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
                minLength: 1,
                mustMatch: true,
                select: function (event, ui) {
                    $("#PolicyAirVendorGroupItem_SupplierName").val(ui.item.value);
                    $("#PolicyAirVendorGroupItem_SupplierCode").val(ui.item.id);
                    $("#SupplierCode").val(ui.item.id);
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

        if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
            $("#PolicyRouting_FromCode").attr('disabled', 'disabled');
        }
        if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
            $("#PolicyRouting_ToCode").attr('disabled', 'disabled');
        }

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


        $('#form0').submit(function () {
            if ($("#PolicyRouting_Name").val() != "") {
                return true;
            } else {

                var validFrom = false;
                var validTo = false;
                $("#lblFrom").text("");
                $("#lblTo").text("");

                if ($("#PolicyRouting_FromGlobalFlag").is(':checked')) {
                    validFrom = true;
                }
                if ($("#PolicyRouting_ToGlobalFlag").is(':checked')) {
                    validTo = true;
                }
                if (!$("#PolicyRouting_FromGlobalFlag").is(':checked') && $("#PolicyRouting_FromCode").val() == "") {
                    $("#lblFrom").removeClass('field-validation-valid');
                    $("#lblFrom").addClass('field-validation-error');
                    $("#lblFrom").text("Please enter a value or choose Global");
                }
                if ($("#PolicyRouting_FromCode").val() != "") {
                    jQuery.ajax({
                        type: "POST",
                        url: "/Validation.mvc/IsValidPolicyRoutingFromTo",
                        data: { fromto: $("#PolicyRouting_FromCode").val(), codetype: $("#PolicyRouting_FromCodeType").val() },
                        success: function (data) {
                            if (!jQuery.isEmptyObject(data)) {
                                validFrom = true;
                            }
                        },
                        dataType: "json",
                        async: false
                    });

                    if (!validFrom) {
                        $("#lblFrom").removeClass('field-validation-valid');
                        $("#lblFrom").addClass('field-validation-error');
                        $("#lblFrom").text("This is not a valid entry");
                    }
                };

                if (!$("#PolicyRouting_ToGlobalFlag").is(':checked') && $("#PolicyRouting_ToCode").val() == "") {
                    $("#lblTo").removeClass('field-validation-valid');
                    $("#lblTo").addClass('field-validation-error');
                    $("#lblTo").text("Please enter a value or choose Global");
                }



                if ($("#PolicyRouting_ToCode").val() != "") {
                    jQuery.ajax({
                        type: "POST",
                        url: "/Validation.mvc/IsValidPolicyRoutingFromTo",
                        data: { fromto: $("#PolicyRouting_ToCode").val(), codetype: $("#PolicyRouting_ToCodeType").val() },
                        success: function (data) {
                            if (!jQuery.isEmptyObject(data)) {
                                validTo = true;
                            }

                        },
                        dataType: "json",
                        async: false
                    });

                    if (!validTo) {
                        $("#lblTo").removeClass('field-validation-valid');
                        $("#lblTo").addClass('field-validation-error');
                        $("#lblTo").text("This is not a valid entry");
                    }
                };
                if (validFrom && validTo) {
                    return true;
                } else {
                    return false
                };
            };

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
                $("#lblAuto").text(data);
                $("#PolicyRouting_Name").val(data);
                $("#lblPolicyRoutingNameMsg").text("");
            }
        })
    }
</script>