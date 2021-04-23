var arr = null;
var folioinputval = false;
$(document).ready(function () {
    //Agrega los checkbox seleccionados a un arreglo
    $('[name="checks[]"]').click(function () {

        arr = $('[name="checks[]"]:checked').map(function () {
            return this.value;
        }).get();

    });

    $('#GenerarPase').click(function () {
        if (folioinputval) {
            $('#FolioSalida').hide();
            folioinputval = false;
        } else {
            $('#FolioSalida').show();
            folioinputval = true;
        }
    });
    $('#btnSendID').click(function () {
        if ($('#PaseSalida').val().trim() == "") {
            addAlertsErrorIndex("El campo no puede quedar vacío");
            $('#PaseSalida').focus();
        } else {
            if (arr == null) {
                addAlertsErrorIndex("Seleccione por lo menos un articulo antes de continuar");
            } else {
                var articles = {
                    folio: $('#PaseSalida').val().trim(),
                    articlesid: arr
                }
                $.ajax({
                    type: 'POST',
                    url: '/receiparticledetail/pasesalida',

                    data: JSON.stringify(articles),

                    contentType: 'application/json',
                    success: function (Data) { //Respuesta afirmativa desde el controlador
                        if (Data.estado) {//Comprobacion de que los datos fueron agregados
                            alert("Pase de salida creado correctamente");
                            location.reload();
                        }
                        else {//Error al agregar los datos
                            alert("Ocurrio un error por favor verifique los datos");
                        }
                        $('#saveData').val('Guardar Cambios');
                    },
                    error: function (error) { //En caso de error en el controlador
                        console.log(error.responseText);
                        $('#saveData').val('Guardar Cambios');
                    }
                });
            }
        }
    });
});
//Funcion para agregar alerta de tipo Success en el DOM
function addAlertsSuccessIndex(text) {
    var cardAlert = '<div class="alert alert-success alert-dismissible" role="alert"><span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span><span class="sr-only"> Success:</span><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>' + text + '</strong></button></div>'
    $('#alertsDetails').append(cardAlert);
};
//Funcion para agregar alertas tipo error en el DOM
function addAlertsErrorIndex(text) {
    var cardAlert = '<div class="alert alert-danger alert-dismissible" role="alert"><span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span><span class="sr-only">Error:</span><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>' + text + '</strong></button></div>'
    $('#alertsDetails').append(cardAlert);
};