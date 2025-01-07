//funcion para ejecutar la busqueda apretando enter
function Buscar(event) {
    var tecla = event.key;
    const boton = document.getElementById('btnBuscar');
    if (tecla === 'Enter') {
        boton.click();
    }
}
//funcion para cerrar la ventana modal Agregar cliente
document.getElementById("lblCerrar").addEventListener('click', function () {
    const modal = document.getElementById("VentanaInsert");
    const modalcont = document.getElementById("modal-container");
    modal.style.opacity = 0;
    modal.style.visibility = "hidden";
    modalcont.style.visibility = "hidden";
    modalcont.style.zIndex = 0;

});


//funcion para abrir la ventana modal Agregar cliente
function mostrarModal() {

    const modal = document.getElementById("VentanaInsert");
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


function mostrarModalPrecio() {
    const modal = document.getElementById("VentanaPrecio");
    const modalcont = document.getElementById("modal-containerPrecio");
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

// Función para cerrar el modal al hacer clic en "x"
document.getElementById("lblPrecioCerrar").onclick = function () {
    const modal = document.getElementById("VentanaPrecio");
    const modalcont = document.getElementById("modal-containerPrecio");
    modal.style.opacity = 0;
    modal.style.visibility = "hidden";
    modalcont.style.visibility = "hidden";
    modalcont.style.zIndex = 0;
};




function isNumberKeyPress(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false; // Solo permite números
    }
    return true; // Permite la entrada
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    var input = evt.target.value;

    // Permite números (0-9), la coma decimal (,) y la tecla de retroceso (8)
    if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode !== 44 && charCode !== 8) {
        return false; // Solo permite números y la coma decimal
    }

    // Permite solo una coma decimal
    if (charCode === 44 && input.indexOf(',') !== -1) {
        return false; // Solo permite una coma decimal
    }

    // Permite solo dos números después de la coma
    if (charCode === 44 || charCode === 8) { // Permitir la coma o retroceso
        return true;
    }

    var parts = input.split(',');
    if (parts.length > 1 && parts[1].length >= 2) {
        return false; // No permite más de dos dígitos después de la coma
    }

    return true; // Permite la entrada
}