$(document).ready(function () {
    $('#menu_policies').click();
    $("tr:odd").addClass("row_odd");
    $("tr:even").addClass("row_even");
    //for pages with long breadcrumb and no search box
    $('#breadcrumb').css('width', '725px');
    $('#search').css('width', '5px');


    $(function () {
        $("textarea.mdd_editor").MarkdownDeep({
            help_location: "/Scripts/MarkdownDeep/mdd_help.htm",
            ExtraMode: true
        });
    })

    $("#PolicyMessageGroupItemLanguage_PolicyMessageGroupItemTranslationMarkdown").keypress(function () {
        if (this.value.length >= 4000) {
            return false;  
    }
});

  
})