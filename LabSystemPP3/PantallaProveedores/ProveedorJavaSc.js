//funcion para cerrar la ventana modal Agregar Proveedor
document.getElementById("lblCerrar").addEventListener('click', function () {
    const modal = document.getElementById("VentanaUPD");
    const modalcont = document.getElementById("modal-container");
    modal.style.opacity = 0;
    modal.style.visibility = "hidden";
    modalcont.style.visibility = "hidden";
    modalcont.style.zIndex = 0;

});


//funcion para abrir la ventana modal Agregar Proveedor
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


//funcion para cerrar la ventana modal Agregar Proveedor
document.getElementById("lblDescCerrar").addEventListener('click', function () {
    const modal = document.getElementById("VentanaDesc");
    const modalcont = document.getElementById("modal-containerDesc");
    modal.style.opacity = 0;
    modal.style.visibility = "hidden";
    modalcont.style.visibility = "hidden";
    modalcont.style.zIndex = 0;

});

//funcion para ejecutar la busqueda apretando enter
function Buscar(event) {
    var tecla = event.key;
    const boton = document.getElementById('btnBuscar');
    if (tecla === 'Enter') {
        boton.click();
    }
}

function cerar() {
    const modal = document.getElementById("VentanaUPD");
    const modalcont = document.getElementById("modal-container");
    modal.style.opacity = 0;
    modal.style.visibility = "hidden";
    modalcont.style.visibility = "hidden";
    modalcont.style.zIndex = 0;
}