var coder = coder || {};

coder.taskTests = {
    counter: ($('.test', '#tests').length + 1),

    init: function () {
        $.each($('.editor'), function (key, value) {
            coder.taskTests.initEditor(value);
        });

        coder.taskTests.bind();
    },

    bind: function () {
        // Add button
        $("#newTest").click(function (e) {
            // Add new test
            var $tests = $('#tests');
            $tests.append(coder.taskTests.getNewTestHtml());

            // Init ace editor for the new test
            $.each($('.test', '#tests').last().find('.editor'), function (key, value) {
                coder.taskTests.initEditor(value);
            });

            // Rebind new remove button
            coder.taskTests.bindRemove();
            e.preventDefault();
            coder.taskTests.counter++;
        });

        $("#taskForm").submit(function (e) {
            var i = 1;

            // Get all tests
            var $allTests = $('.test', '#tests');
            $.each($allTests, function (key, value) {

                // Get all editors within each test
                var $testEditors = $(value).find('.editor');
                $.each($testEditors, function (key, value) {
                    var editor = ace.edit(value);
                    var type = $(value).hasClass('input') ? 'input' : 'output';
                    var input = '<input type="hidden" name="test_' + i + '_' + type + '" value="' + editor.getValue().replace(/"/g, "&quot;") + '"/>';
                    $("#taskForm").append(input);
                });
                i++;
            });

            return true;
        });

        coder.taskTests.bindRemove();
    },

    bindRemove: function () {
        // Remove button
        $('.remove-test', '#tests').click(function (e) {
            $(this).parent().remove();
            e.preventDefault();
        });
    },

    initEditor: function (value) {
        var editor = ace.edit(value);
        editor.setTheme("ace/theme/chrome");
        editor.setOptions({
            maxLines: Infinity,
            readOnly: $(value).hasClass('readonly')
        });

        // If editor has data-filename attribute, we autoselect the correct mode (syntax highlighting) for the editor.
        var attr = $(value).attr('data-filename');
        if (typeof attr !== typeof undefined && attr !== false) {
            var modelist = ace.require("ace/ext/modelist");
            var mode = modelist.getModeForPath(attr).mode;
            editor.session.setMode(mode);
        }
    },

    getNewTestHtml: function () {
        var $test = $('<div class="test clearfix whiteBox"></div>');
        $test.append('<h3>Test ' + coder.taskTests.counter + '</h3>');
        $test.append('<a class="btn btn-danger remove-test" href="#"><i class="fa fa-trash " aria-hidden="true"></i>Remove test</a>');

        var $testsWrap = $('<div class="tests-wrap row">');
        $testsWrap.append($('<div class="input-wrap col-sm-6"><p>Input</p><div class="editor input"></div></div>'));
        $testsWrap.append($('<div class="output-wrap col-sm-6"><p>Output</p><div class="editor output"></div></div>'));

        $test.append($testsWrap);

        return $test;
    }
}

$(document).ready(function () {
    coder.taskTests.init();

    tinymce.init({
        selector: 'textarea.codertiny',
        height: 500,
        plugins: [
          'advlist autolink lists link image charmap print preview hr anchor pagebreak',
          'searchreplace wordcount visualblocks visualchars code fullscreen',
          'insertdatetime media nonbreaking save table contextmenu directionality',
          'emoticons template paste textcolor colorpicker textpattern imagetools'
        ],
        toolbar1: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
        toolbar2: 'preview media | forecolor backcolor emoticons',
        image_advtab: true
    });


    $('.coder-table.coder-datatable').DataTable({
        //"aoColumnDefs": [{ 'bSortable': false, 'aTargets': [7] }], // TODO: Make action column not sortable
        "language": {
            "paginate": {
                "next": ">>",
                "previous": "<<"
            }
        }
    });
});