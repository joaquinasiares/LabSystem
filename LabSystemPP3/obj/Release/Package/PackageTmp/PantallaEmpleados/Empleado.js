//funcion para cerrar la ventana modal Agregar cliente
document.getElementById("lblCerrar").addEventListener('click', function () {
    const modal = document.getElementById("VentanaUPD");
    const modalcont = document.getElementById("modal-container");
    modal.style.opacity = 0;
    modal.style.visibility = "hidden";
    modalcont.style.visibility = "hidden";
    modalcont.style.zIndex = 0;

});


//funcion para abrir la ventana modal Agregar cliente
function mostrarModal() {

    const modal = document.getElementById("VentanaUPD");
    const modalcont = document.getElementById("modal-container");
    if (modal && modalcont) {
        modal.style.opacity = 1;
        modal.style.visibility = "visible";
        modalcont.style.visibility = "visible";
        modalcont.style.opacity = 1;
        modalcont.style.zIndex = 1000;
        modal.style.zIndex = 10001;
    } else {
        console.error("Modal or modal container not found");
    }
}

//funcion para ejecutar la busqueda apretando enter
function Buscar(event) {
    var tecla = event.key;
    const boton = document.getElementById('btnBuscar');
    if (tecla === 'Enter') {
        boton.click();
    }
}

window.onclick = function (event) {
    var modal = document.getElementById('myModal');
    if (event.target == modal) {
        modal.style.display = 'none';
    }
}