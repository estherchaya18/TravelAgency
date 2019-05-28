

$(document).ready(function () {
    $('#name').keyup(function (e) {
        if (e.which == 13)
            $('form').submit();
        //$('form').load('/phones/search/?name='+$(this).val);
    })
});