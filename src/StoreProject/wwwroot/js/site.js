// Write your Javascript code.



//Cupon.click(function () {
//    $.getJSON("http://free.worldweatheronline.com/feed/weather.ashx?callback=?", 
//          { 
//              "q": "Toronto,Canada",
//              "num_of_days":5,
//              "format":"json",
//              "key":"your_api_key"
//          },
//                jsonParser
//    );  // end of getJSON
//});  // end of click
//}); // end of document.ready
      
    

(function () {
    var ele = $("#username");
    ele.text("chen luigi");

    var main = $("#main");
    main.on("mouseenter", function () {
        main.style = "background-color: #888;";
    }); main.on("mouseleave", function () {
        main.style = ""
    });

    var menuItems = $("ul.menu li a");
    menuItems.on("click", function () {
        var me = $(this);
        alert(me.text());
    });


    var $Vegtables = $("#sidebar,#wrapper");
    $("#sidebarToggle").on("click", function () {
        $sidebarAndWrapper.toggleClass("hide-sidebar");
        if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
            $(this).text("Show Sidebar");
        } else {
            $(this).text("Hide Sidebar");
        }
    });

})();

var mail = $('#submitMail');
mail.click(function () {
    var name = $('#inputEmail').val();
    var email = $('#textArea').val();
    var varData = 'name=' + name + '&email=' + email;
    $.ajax({
        type: "POST",
        url: '/Home/SendMail',
        data: varData,
        success: function () {
            alert("Mail Sent Successfully");
        }
    })
});
