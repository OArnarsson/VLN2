$(document).ready(function () {

    var i = $('.test', $('#tests')).size() + 1;

    $("#newTest").click(function (e) {
        var $tests = $('#tests');

        var $test = $('<div class="test clearfix"></div>');
        var $editors = $('<div class="editors clearfix"></div>');

        $('<div class="input"><div id="editor' + i + 'in" class="editor"></div></div>').appendTo($editors);
        $('<div class="output"><div id="editor' + i + 'out" class="editor"></div></div>').appendTo($editors);

        $test.append('<p>Test ' + i + '</p>');
        $test.append($editors);
        $test.append('<a href="#" class="btn btn-danger remove-test">Remove</div>');
        $test.appendTo($tests);

        var editor = ace.edit("editor" + i + "in");
        editor.setTheme("ace/theme/monokai");
        editor.setOptions({
            maxLines: Infinity
        });

        var editor = ace.edit("editor" + i + "out");
        editor.setTheme("ace/theme/monokai");
        editor.setOptions({
            maxLines: Infinity
        });

        $('.remove-test', '#tests').click(function (e) {
            $(this).parent().remove();
            e.preventDefault();
        });

        e.preventDefault();
        i++;
    });

    $("#TaskEditForm").submit(function (e) {
        $.each($(".editor"), function (i, val) {
            var editor = ace.edit(val);
            $("#TaskEditForm").append('<input type="hidden" name="test" value="' + editor.getValue() + '"/>');
        });
        return true;
    });
});