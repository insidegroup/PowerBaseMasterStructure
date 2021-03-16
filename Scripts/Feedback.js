jQuery(function ($) {

    /* Feedbackify! */
    var fby = fby || [];
    fby.push(["customData", "SystemUserLoginIdentifier", $('#Tracking_SystemUserLoginIdentifier').val()]);
    fby.push(["customData", "TopUnitGUID", $('#Tracking_ClientTopUnitGUID').val()]);
    fby.push(["customData", "SubUnitGUID", $('#Tracking_ClientSubUnitGUID').val()]);
    fby.push(["customData", "Page", $('#Tracking_PageUrl').val()]);

    (function () {
        var f = document.createElement('script'); f.type = 'text/javascript'; f.async = true;
        f.src = '//cdn.feedbackify.com/f.js';
        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(f, s);
    })();
});