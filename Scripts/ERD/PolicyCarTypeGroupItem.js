$(document).ready(function() {

     $('#menu_policies').click();
     $("tr:odd").addClass("row_odd");
     $("tr:even").addClass("row_even");

     //for pages with long breadcrumb and no search box
     $('#breadcrumb').css('width', '725px');
     $('#search').css('width', '5px');

     $('#TravelDateValidFrom').datepicker({
         constrainInput: true,
         buttonImageOnly: true,
         showOn: 'button',
         buttonImage: '/Images/Common/Calendar.png',
         dateFormat: 'M dd yy',
         duration: 0
     });
     $('#TravelDateValidTo').datepicker({
         constrainInput: true,
         buttonImageOnly: true,
         showOn: 'button',
         buttonImage: '/Images/Common/Calendar.png',
         dateFormat: 'M dd yy',
         duration: 0
     });
     $('#EnabledDate').datepicker({
         constrainInput: true,
         buttonImageOnly: true,
         showOn: 'button',
         buttonImage: '/Images/Common/Calendar.png',
         dateFormat: 'M dd yy',
         duration: 0
     });
     $('#ExpiryDate').datepicker({
         constrainInput: true,
         buttonImageOnly: true,
         showOn: 'button',
         buttonImage: '/Images/Common/Calendar.png',
         dateFormat: 'M dd yy',
         duration: 0
     });
 });

