$(function () {
    var data = { id: 0, active: 0 };

    $('.btnDisabled').click(function () {
        var button_id = $(this).attr("id");
        $('#nameuser').append(button_id);
        data = { id: $(this).attr("name"), active: 2 }
    });

    $('.btnEnabled').click(function () {
        var button_id = $(this).attr("id");
        $('#nameuserAct').append(button_id);
        data = { id: $(this).attr("name"), active: 1 }
    });

    $('#cerrar').click(function () {
        $('#nameuser').empty();
    });
    $('.close').click(function () {
        $('#nameuser').empty();
    });

    $('#cerrarAct').click(function () {
        $('#nameuserAct').empty();
    });

    $('.Act').click(function () {
        $('#nameuserAct').empty();
    });

    $('.Confirm').click(function () {
        $.ajax({
            type: 'POST',
            url: '/User/ChangeActiveUser',

            data: JSON.stringify(data),

            contentType: 'application/json',
            success: function (Data) { //Respuesta afirmativa desde el controlador
                if (Data.estado) {//Comprobacion de que los datos fueron agregados
                    addAlertsSuccess("Ok");
                    location.reload();
                }
                else {//Error al agregar los datos
                    addAlertsError("No");
                }
            },
            error: function (error) { //En caso de error en el controlador
                console.log(error.responseText);
            }
        });
    });

});
