/*
Date: Apr 2014: 
GUI Version Added: v2.08.1 (QC 2328, 2329)
*/

// MMM dd yyyy eg APR 04 2014
function isValidDate(txtDate) {

    if (txtDate == '')
        return false;

    txtDate = txtDate.toUpperCase();
    var regEx = /^(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)(\s)(0[1-9]|[12][0-9]|3[01])(\s)([1-2]\d{3})$/;
    var dtArray = txtDate.match(regEx); // is format OK?
  
    if (dtArray == null)
        return false;

    dtDay = dtArray[3];
    dtMonth = "JANFEBMARAPRMAYJUNJULAUGSEPOCTNOVDEC".indexOf(dtArray[1]) / 3 + 1; //convert name to number, JAN=1,DEC=12
    dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }
    return true;
}