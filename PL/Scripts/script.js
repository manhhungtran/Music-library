$(document)
    .ready(function() {
        var max_fields = 10; //maximum input boxes allowed
        var wrapper = $(".input_fields_wrap"); //Fields wrapper
        var add_button = $(".add_field_button"); //Add button ID

        var x = 1; //initlal text box count
        $(add_button)
            .click(function(e) { //on add input button click
                e.preventDefault();
                refreshImageValues();
                if (x < max_fields) { //max input box allowed
                    x++; //text box increment
                    $(wrapper)
                        .append('<div><input type="text" class="mytext" onkeyup="refreshImageValues()" name="mytext"/><a href="#" class="remove_field">Remove</a><br></div>'); //add input box
                }
            });

        $(wrapper)
            .on("click",
                ".remove_field",
                function (e) { //user click on remove text
                    refreshImageValues();
                    e.preventDefault();
                    $(this).parent('div').remove();
                    x--;
                });

        $(".genreFields")
            .on("click",
                ".remove_field",
                function (e) { //user click on remove text
                    refreshImageValues();
                    e.preventDefault();
                    $(this).parent('div').remove();
                    x--;
                });
    });

function refreshImageValues() {
    var inputs = $(".mytext");
    var result = "";

    for (var i = 0; i < inputs.length; i++) {
        var neco = $(inputs[i]).val();
        if (neco) {
            result += neco + ";";
        }
    }
    $("#Images").attr("value", result);

}

function addGenreField() {
    $(".genreFields")
        .append('<div><input type="text" id="Genres" name="Genres"><a href="#" class="remove_field">Remove</a></div>');
}
