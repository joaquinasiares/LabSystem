<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DescripcionProveedor.aspx.cs" Inherits="LabSystemPP3.PantallaProveedores.DescripcionProveedor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="ProveedorEstilos.css"/>
</head>
<body>
    <form id="form1" runat="server">
          <div class="modal-container" id="modal-containerDesc" style="visibility:visible;opacity:1;">

            <div class="modal-body" id="VentanaDesc" style="visibility:visible;opacity:1;">
                <div class="modal-header">
                    <h3>Descripcion del proveedor</h3>
                </div>

                <div class="modal-content">
                    <asp:Label CssClass="modalitem" ID="lblDescNom" runat="server" Text="Cuit: "></asp:Label>
                    <br />
                    <asp:Label CssClass="modalitem" ID="lblDescCuit" runat="server" Text="Nombre: "></asp:Label>
                    <br />
                    <asp:Label CssClass="modalitem" ID="lblDescMostrar" runat="server" Text="Descrpcion: "></asp:Label>
                    <asp:Label ID="Label7" runat="server" Text=""></asp:Label>
                </div>
                <a href="GestorProveedores.aspx"><asp:Label CssClass="upd-cerrar" ID="lblDescCerrar" runat="server" Text="x"></asp:Label></a>
                

            </div>
        </div>
    </form>
    <script src="ProveedorJavaSc.js"></script>
</body>
</html>
