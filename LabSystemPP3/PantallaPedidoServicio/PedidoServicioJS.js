//funcion para ejecutar la busqueda apretando enter
function Buscar(event) {
    var tecla = event.key;
    const boton = document.getElementById('btnBuscar');
    if (tecla === 'Enter') {
        boton.click();
    }
}

function Buscarid(event) {
    var tecla = event.key;
    const boton = document.getElementById('btnBuscarPedido');
    if (tecla === 'Enter') {
        boton.click();
    }
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