/*
OnReady
*/
$(document).ready(function() {
    
    //Navigation
    $('#menu_clients').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");

    $('#TravelerTypeSponsor_IsSponsorFlag').change(function () {
        if ($(this).is(":checked")) {
            $('#TravelerTypeSponsor_RequiresSponsorFlag').removeAttr('checked');
        }
    });
    $('#TravelerTypeSponsor_RequiresSponsorFlag').change(function () {
        if ($(this).is(":checked")) {
            $('#TravelerTypeSponsor_IsSponsorFlag').removeAttr('checked');
        }
    });

});
