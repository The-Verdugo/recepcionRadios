var receipArticleDetails= [];//arreglo de detalles de articlos
var i=0;//Contador para cargas individuales de elementos


//Funcion para agregar tarjetas al Div de los articulos
function addElemento(){
  // Verifica los campos necesarios en el formulario
    if ($('#IdArticulo').val().trim() == "") {
        toastr["warning"]("El campo ID Articulo es obligatorio", "Advertencia");
        
        document.getElementById("IdArticulo").focus();
    } else if ($('#Marca').val().trim() == "") {
        toastr["warning"]("Por favor veifique que el Id del artículo sea correcto", "Advertencia");
        document.getElementById("IdArticulo").focus();
    }
    else if ($('#Falla').val().trim() == "") {
        toastr["warning"]("El campo Falla es obligatorio", "Advertencia");
    document.getElementById("Falla").focus();
  }else{
  var objeto = {
    Description: $('#Descripcion').val().trim(),
    ArticleID: $('#IdArticulo').val().trim(),
    ReportedFailure: $('#Falla').val().trim()
  }
  var Marca= $("#Marca").val();
  var nombre = $("#Descripcion").val();
  var Falla = $('#Falla').val();
  var id = $('#IdArticulo').val();
  var card = '<div class="article" id="article'+i+'"><div class="article-preview"> <h6>Articulo</h6><h4>'+nombre+'</h4><h6>ID</h6><h4>'+id+'</h4></div><div class="article-info"> <h6>Falla</h6><h4>'+Falla+'</h4><h6>Marca</h6><h4>'+Marca+'</h4><button id="'+i+'" class="btn-article" >Eliminar</button></div></div>'
  $('#articles').append(card);
  i++;
  //limpiar datos de los articulos en el form
  document.getElementById("Descripcion").value = "";
  document.getElementById("Falla").value = "";
  document.getElementById("Modelo").value = "";
  document.getElementById("Marca").value = "";
  document.getElementById("IdArticulo").value = "";
  document.getElementById("IdArticulo").focus();
  receipArticleDetails.push(objeto);
  }
}

var c=0;//Contador para cargas completas de elementos 

$(function () {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-left",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
  //Funcion para guardar lista de articulos en la base de datos
  $('#saveData').click(function() {
  var receiparticle = {
      empleadoEntrega: $('#NoEmpleado').val().trim(),
      receipArticleDetails: receipArticleDetails
  }
  if (i == 0) {
      toastr["error"]("Primero debe agregar un articulo", "Error");
  } else if ($('#Puesto').val().trim() == "") {
      toastr["warning"]("Por favor veifique que el número de colaborador sea correcto", "Advertencia");
      document.getElementById("#NoEmpleado").focus();
  }
  else {
    
    $(this).val('Espera un momento...');

    //Envio de datos POST mediante Ajax
    $.ajax({
        type: 'POST',
        url: '/receiparticle/create',
        
        data: JSON.stringify(receiparticle),
    
        contentType: 'application/json',
        success: function (Data) { //Respuesta afirmativa desde el controlador
            if (Data.estado) {//Comprobacion de que los datos fueron agregados
                toastr["success"]("Los datos fueron guardados correctamente", "Cambios Guardados");
                // alert('Articulo agregado correctamente');
                location.href = "/receiparticle/details/" + Data.ID;
            }
            else {//Error al agregar los datos
                toastr["error"]("El campo Número de colaborador es obligatorio", "Error");
                // alert('Ocurrio un error al intentar guardar los datos, ingrese un número de colaborador válido');
                document.getElementById("NoEmpleado").focus();
            }
            $('#saveData').val('Guardar Cambios');
        },
        error: function (error) { //En caso de error en el controlador
            console.log(error.responseText);
            $('#saveData').val('Guardar Cambios');
        }
    });

    c++;
  }
  })
  
  //Funcion para agregar articulos al arreglo
  $('#agregarArticulo').click(function(){
    addElemento();
  });
  //Funcion para remover elementos del Div de los articulos
  $(document).on('click', '.btn-article', function() {
    var button_id = $(this).attr("id");
    var Falla = $(this).before().attr("id");
    $('#article' + button_id + '').remove(); 
    receipArticleDetails.splice(button_id, 1 );
    i--;
  });
});
