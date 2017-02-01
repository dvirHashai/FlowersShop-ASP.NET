    var Cupon = $("#Cupon");
    Cupon.click(function () {
        $.ajax({
            url: '/Cupons/Generate',
            type: 'POST',
            dataType: 'json',
            success: function (data) {

                switch (data.Status) {
                    case -1:
                        $("#result").text("Your Ip already register in the system");
                        break;

                    case 1:
                        $("#result").text("Your Cupons Key : " + data.Key);
                        break;

                    default: //0
                        $("#result").text("Error: please contact with us on the mail : abc@hmai.com");
                        break;
                }

            }, error: function (data) {

            }
        });
    });
        

    

