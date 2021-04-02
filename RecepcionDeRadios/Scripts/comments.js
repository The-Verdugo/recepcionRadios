//Funcion para agregar alertas tipo success en el DOM
function addAlertsSuccessComment(text){
    var cardAlert = '<div class="alert alert-success alert-dismissible" role="alert"><span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span><span class="sr-only">Error:</span><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>'+text+'</strong></button></div>' 
    $('#FormComment').append(cardAlert);
};
//Funcion para agregar alertas tipo error en el DOM
function addAlertsErrorComment(text){
    var cardAlert = '<div class="alert alert-danger alert-dismissible" role="alert"><span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span><span class="sr-only">Error:</span><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>'+text+'</strong></button></div>' 
    $('#FormComment').append(cardAlert);
};
$(function(){
    var usuario = $('#NameHidden').val();
    $('#Username').val(usuario);
    var fecha = $('#FechaHidden').val();
    $('#fecha').val(fecha);
    var usuarioForm = $('#UserHidden').val();
    $('#commentFormUser').val(usuarioForm);
    var fecha = $('#FechaHidden').val();
    $('#commentFormDate').val(fecha);
    //Funcion para agregar alertas de error en el DOM
    //Oculta el form de agregar comentarios
    var formCommentActive = false;
    //funcion que detecta el clic para agregar comentarios
    $('#btnCreateComment').click(function(){
        if(formCommentActive){
            $('#FormComment').hide();
            formCommentActive=false;
        }else{
            $('#FormComment').show();
            formCommentActive=true;
        }
    });
    //funcion para enviar los datos al controlador usando Ajax
    $('#commentFormBtnSave').click(function(){

        if($('#commentFormSubject').val().trim()==""){
            addAlertsErrorComment("El campo Asunto es obligatorio");
            $('#commentFormSubject').focus();
        }else if($('#commentFormText').val().trim()==""){
            addAlertsErrorComment("El campo Comentario es obligatorio");
            $('#commentFormText').focus();
        }else{
        var comment = {
            ReceipArticleDetailID: $('#IdHidden').val().trim(),
            Username: $('#UserHidden').val().trim(),
            Subject: $('#commentFormSubject').val().trim(),
            Content: $('#commentFormText').val().trim(),
            fecha: $('#FechaHidden').val().trim()
        }
        
            //Envio de datos POST mediante Ajax
            $.ajax({
                type: 'POST',
                url: '/comments/create',
                
                data: JSON.stringify(comment),
            
                contentType: 'application/json',
                success: function (data) { //Respuesta afirmativa desde el controlador
                    if (data.estado) {//Comprobacion de que los datos fueron agregados
                        addAlertsSuccessComment("Comentario agregado correctamente");
                        location.reload();
                    }
                    else {//Error al agregar los datos
                        alert("Ocurrio un error al guardar los datos");
                    }
                },
                error: function (error) { //En caso de error en el controlador
                    console.log(error.responseText);
                }
            });
        }
    });
});    
