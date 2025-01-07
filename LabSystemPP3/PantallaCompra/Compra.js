function Buscar(event) {
    var tecla = event.key;
    const boton = document.getElementById('btnBuscar');
    if (tecla === 'Enter') {
        boton.click();
    }
}

function mostrarModal() {

    const modal = document.getElementById("VentanaCompra");
    const modalcont = document.getElementById("modal-container");
    if (modal && modalcont) {
        modal.style.opacity = 1;
        modal.style.visibility = "visible";
        modalcont.style.visibility = "visible";
        modalcont.style.opacity = 1;
        modalcont.style.zIndex = 1000;
        modal.style.zIndex = 10001;
    } else {
        console.error("no se encontro el modal");
    }
}

function cerar() {
    const modal = document.getElementById("VentanaCompra");
    const modalcont = document.getElementById("modal-container");
    modal.style.opacity = 0;
    modal.style.visibility = "hidden";
    modalcont.style.visibility = "hidden";
    modalcont.style.zIndex = 0;
}