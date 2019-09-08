/// <reference path="jquery-1.11.3.min.js" />


$(document).ready(function () {
    $('#name').keyup(function (e) {
        if (e.which == 13)
            $('form').submit();
        //$('form').load('/phones/search/?name='+$(this).val);
    })
});

//$(document).ready(function () {
//    $(".btnLink").click(function () {
//        $('#ModalPopUp').modal('show');
//    })
//});


$(function () {
    $(".btnLink").click(function () {
        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-id');
        var pasangers = $("#passangers").text();
        var options = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: '/Flights/ConfirmOrder',
            contentType: "application/json; charset=utf-8",
            data: { "Id": id, "passangers": pasangers },
            datatype: "json",
            success: function (data) {
                $('#myModalContent').html(data);
                $('#myModal').modal(options);
                $('#myModal').modal('show');

            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
    });
    //$("#closebtn").on('click',function(){  
    //    $('#myModal').modal('hide');    

    $("#closbtn").click(function () {
        $('#myModal').modal('hide');
    });
});  

$(function () {
    $("#SortFaster, #SortPrice").click(function () {
        var $buttonClicked = $(this);
        var sortBy = $buttonClicked.attr('data-id');
        var from = $("#from").attr('data-id');
        var to = $("#to").attr('data-id');
        var departure = $("#departure").text();
        var pasangers = $("#passangers").text();
        var options = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: '/Flights/SortList',
            contentType: "application/json; charset=utf-8",
            data: { "sortOrder": sortBy, "from": from, "to": to, "departure": departure, "passengers": pasangers },
            datatype: "json",
            success: function (data) {
                $('#flights').html(data);
            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
    });
    //$("#closebtn").on('click',function(){  
    //    $('#myModal').modal('hide');    

    $("#closbtn").click(function () {
        $('#myModal').modal('hide');
    });
});  