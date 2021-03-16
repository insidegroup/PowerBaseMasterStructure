//Needs to be included in pages that use AJAX to populate divs
//Stops Logon page from loading into DIV if user timesout
$().ready(function () {
    $(document).ajaxSuccess(function (e, xhr, settings) {
        var header = xhr.getResponseHeader("X_User_Logged_In");
        if (header !== "true") {
            window.location = "/Account.mvc/Logon";
        }
    });
});

