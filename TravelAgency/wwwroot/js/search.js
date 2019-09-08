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


//call the specific div (modal)

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
                $('#myModal').modal('open');

            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });
    });
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




      $("#slider").slider({
        range: true,
        min: 0,
        max: 1440,
        step: 15,
        values: [ 420, 1020 ],
        slide: function(e, ui) { 
           console.log(ui.values[0])            
           var initialcombo = toClock(ui.values[ 0 ])+ '<span> To </span>' + toClock(ui.values[ 1 ]);  
             $('#amount').html(initialcombo);   

         
          },
          stop: function (e, ui) {
              //change color here
              var from = $("#from").attr('data-id');
              var to = $("#to").attr('data-id');
              var departure = $("#departure").text();
            var pasangers = $("#passangers").text();
            var startTime = toClock(ui.values[0]);
            var endTime = toClock(ui.values[1]);
              var options = { "backdrop": "static", keyboard: true };
              $.ajax({
                  type: "GET",
                  url: '/Flights/FilterTimeList',
                  contentType: "application/json; charset=utf-8",
                  data: { "from": from, "to": to, "departure": departure, "passengers": pasangers, "startTime": startTime, "endTime": endTime },
                  datatype: "json",
                  success: function (data) {
                      $('#flights').html(data);
                  },
                  error: function () {
                      alert("Dynamic content load failed.");
                  }
              });
          }
    });
             
        function toClock(aa){
            var hours = Math.floor(aa / 60);
            var minutes = aa - (hours * 60);
            var ampm = "";  
            
            if(hours.length == 1) {hours = '12' + hours;}
            if(hours > 12) {ampm = "PM"; hours = hours-12;}
            else if(hours == 12){ampm = "PM";}
            else if(hours < 12){
                ampm = "AM"; 
                if(hours == 0)  hours =  12;            
                                       
            }      
            if(minutes == 0){minutes = '0' + minutes;}               
            var combo = hours+':'+minutes + ampm;
            return combo
        }
         
        function toUnits(bb){
           var hours = Math.floor(bb / 60);
           var minutes = bb - (hours * 60);       
        }  

        function toInitialize(a,b){
        console.log(a);
            if(a == ''){} 
            else{
                var new_start = a.split(':'); 
                console.log(new_start);
                $("#slider").slider("values", [0, 1440 ] );
            }     
          }        
                 
function updateLabels(){                  
    $('#amount').html(toClock($("#slider").slider("values", 0)) + '<span> To </span>' + toClock($("#slider").slider("values", 1)));
    
    var from = $("#from").attr('data-id');
    var to = $("#to").attr('data-id');
    var departure = $("#departure").text();
    var pasangers = $("#passangers").text();
    var startTime = toClock($("#slider").slider("values", 0));
    var endTime = toClock($("#slider").slider("values", 1));
    var options = { "backdrop": "static", keyboard: true };
    $.ajax({
        type: "GET",
        url: '/Flights/FilterTimeList',
        contentType: "application/json; charset=utf-8",
        data: { "from": from, "to": to, "departure": departure, "passengers": pasangers, "startTime": startTime, "endTime": endTime },
        datatype: "json",
        success: function (data) {
            $('#flights').html(data);
        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });
}
    
    
    
    // get numbers to start off with                  
toInitialize($('#start').val(), $('#end').val());
   // update the labels
updateLabels();    


