//funcion para ejecutar la busqueda apretando enter
function Buscar(event) {
    var tecla = event.key;
    const boton = document.getElementById('btnBuscar');
    if (tecla === 'Enter') {
        boton.click();
    }
}