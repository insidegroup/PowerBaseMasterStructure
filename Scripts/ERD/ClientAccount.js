$(document).ready(function() {

    //Navigation
    $('#menu_clients').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    //Show DatePickers
    $('#EffectiveDate').datepicker({
        constrainInput: true,
        buttonImageOnly: true,
        showOn: 'button',
        buttonImage: '/Images/Common/Calendar.png',
        dateFormat: 'M dd yy',
        duration: 0
    });



    if ($('#Effective').val() == "") {
        $('#Effective').val("No Effective Date")
    }

});


$(function() {
    $("#CountryName").autocomplete({
        source: function(request, response) {
            $.ajax({
                url: "/Country.mvc/AutoCompleteCountries", type: "POST", dataType: "json",
                data: { searchText: request.term },
                success: function(data) {
                    response($.map(data, function(item) {
                        return {
                            label: item.HierarchyName,
                            id: item.HierarchyCode
                        }
                    }))
                }
            })
        },
        select: function(event, ui) {
            $("#CountryCode").val(ui.item.id);
        }
    });
});