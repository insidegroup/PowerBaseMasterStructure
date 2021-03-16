$(document).ready(function () {
    $('#menu_pnroutputs').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");


    $('#PNROutputPlaceHolder').dblclick(function () {

        //get the selected value in the textarea
        var range = $('#Value').getSelection();

        //variables for validation
        var beforeCursorSymbolCount = $('#Value').val().substring(0, range.start).split("%").length - 1;
        var afterCursorSymbolCount = $('#Value').val().substring(range.end).split("%").length - 1; ;
        var previousCharacter = $('#Value').val().substring(range.start - 1, range.start);
        var nextCharacter = $('#Value').val().substring(range.end, range.end + 1);

        //the selected item
        var placeholder = $("#PNROutputPlaceHolder").val();

        //we cannot have 2 placeholders beside eachother
        if (previousCharacter == "%") {
            placeholder = "-" + placeholder;
        }
        if (nextCharacter == "%") {
            placeholder = placeholder + "-";
        }

        if ((beforeCursorSymbolCount % 2 != 0) || (afterCursorSymbolCount % 2 != 0)) {
            alert("you cannot place item inside another item");
        } else {
            //update the textarea
            var newText = $('#Value').val().substring(0, range.start) + placeholder + $('#Value').val().substring(range.end)
            $('#Value').val(newText);
        }

    });
})
//Submit Form Validation
$('#form0').submit(function () {
    if ($('#Value').val().match(/ {2,}/g).length > 0) {
        alert("you cannot have two spaces in a row");
        return false;
    }
});