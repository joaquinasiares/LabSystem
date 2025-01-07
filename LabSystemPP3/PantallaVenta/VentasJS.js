function mostrarModalDeudas() {

    const modal = document.getElementById("VentanaVerDeudas");
    const modalcont = document.getElementById("modal-containerVerDeudas");
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

function cerarDeudas() {
    const modal = document.getElementById("VentanaVerDeudas");
    const modalcont = document.getElementById("modal-containerVerDeudas");
    modal.style.opacity = 0;
    modal.style.visibility = "hidden";
    modalcont.style.visibility = "hidden";
    modalcont.style.zIndex = 0;
}