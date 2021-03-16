/* 
   Simple JQuery Accordion menu.
   HTML structure to use:

   <ul id="menu">
     <li><a href="#">Sub menu heading</a>
     <ul>
       <li><a href="http://site.com/">Link</a></li>
       <li><a href="http://site.com/">Link</a></li>
       <li><a href="http://site.com/">Link</a></li>
       ...
       ...
     </ul>
     <li><a href="#">Sub menu heading</a>
     <ul>
       <li><a href="http://site.com/">Link</a></li>
       <li><a href="http://site.com/">Link</a></li>
       <li><a href="http://site.com/">Link</a></li>
       ...
       ...
     </ul>
     ...
     ...
   </ul>

Copyright 2007 by Marco van Hylckama Vlieg

web: http://www.i-marco.nl/weblog/
email: marco@i-marco.nl

Free for non-commercial use
*/


$(document).ready(function () {
    $('#menu ul').hide();
    $('#menu li a').click(function () {
        var checkElement = $(this).next();
        if ((checkElement.is('ul')) && (checkElement.is(':visible'))) {
            if (!checkElement.hasClass("sub-menu")) {
                $('#menu ul:visible').slideDown('normal');
            }
            checkElement.slideUp('normal');
            return false;
        }
        if ((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
            if (!checkElement.hasClass("sub-menu")) {
                $('#menu ul:visible').slideUp('normal');
            }
            checkElement.slideDown('normal');
            return false;
        }
    });

    //need to open a menu for the section that we are in  
    //$('#menu_home').click();

});