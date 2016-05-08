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

        $("#taskEditForm").submit(function (e) {
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
                    $("#taskEditForm").append(input);
                });
                i++;
            });

            return true;
        });

        coder.taskTests.bindRemove();
    },

    bindRemove: function() {
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
    },

    getNewTestHtml: function () {
        var $test = $('<div class="test clearfix"></div>');
        $test.append('<h3>Test ' + coder.taskTests.counter + '</h3>');
        $test.append('<a href="#" class="remove-test"><i class="fa fa-trash " aria-hidden="true"></i></a>');

        var $testsWrap = $('<div class="tests-wrap row">');
        $testsWrap.append($('<div class="input-wrap col-sm-6"><div class="editor input"></div></div>'));
        $testsWrap.append($('<div class="output-wrap col-sm-6"><div class="editor output"></div></div>'));

        $test.append($testsWrap);
        
        return $test;
    }
}

$(document).ready(function () {
    coder.taskTests.init();

    tinymce.init({
        selector: 'textarea.codertiny'
    });

    /*Dropzone.options.dropzoneForm = {
        init: function () {
            //maxFilesize: 20,
            dictDefaultMessage: "Drop files or click here to upload",
            this.on("complete", function (data) {
                //var res = eval('(' + data.xhr.responseText + ')');
                var res = JSON.parse(data.xhr.responseText);
            });

            this.on("error", function (data, errorMessage, xhr) {
                $(".alertError").hide();
                $(".alertSuccess").hide();
                $(".dz-error-message").show();
                $(".dz-error-message").html("<p>" + errorMessage.Message + "</p>");
            });

            this.on("processing", function (data) {
                $(".alertError").hide();
                $(".alertSuccess").hide();
            });

            this.on("success", function (data) {
                $(".alertError").hide();
                $(".alertSuccess").show();
            });
        }
    };*/
});