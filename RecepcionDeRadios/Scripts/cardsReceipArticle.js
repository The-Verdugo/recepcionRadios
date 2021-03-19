var receipArticleDetails= [];//arreglo de detalles de articlos
var i=0;//Contador para cargas individuales de elementos

//Funcion para agregar tarjetas al Div de los articulos
function addElemento(){
  if($('#IdArticulo').val().trim()=="" ){
    alert('El campo ID Articulo es obligatorio');
    document.getElementById("IdArticulo").focus();
  }else if($('#Falla').val().trim()==""){
    alert('El campo Falla es obligatorio');
    document.getElementById("Falla").focus();
  }else{
  var objeto = {
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

$(function() {
  //Funcion para guardar lista de articulos en la base de datos
  $('#saveData').click(function() {
  var receiparticle = {
      empleadoEntrega: $('#NoEmpleado').val().trim(),
      receipArticleDetails: receipArticleDetails
  }
    if(i==0) {
      alert('Primero debe agregar un articulo');
    }else{
    
    $(this).val('Espera un momento...');

    //Envio de datos POST mediante Ajax
    $.ajax({
        type: 'POST',
        url: '/receiparticle/create',
        
        data: JSON.stringify(receiparticle),
    
        contentType: 'application/json',
        success: function (data) { //Respuesta afirmativa desde el controlador
            if (data.estado) {
                alert('Articulo agregado correctamente');
                location.href ="/receiparticle/index";
            }
            else {
                alert('Ocurrio un error al intentar guardar los datos, compruebe todos los campos obligatorios');
                document.getElementById("Noempleado").focus();
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


