<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestorInformes.aspx.cs" Inherits="LabSystemPP3.Pantalla_Informes.Gestor_Informes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <link href="informes.css" rel="stylesheet" type="text/css" />
  
        <div class="container"> 

                    <div class="contenido-central">
                        <h1>Informes disponibles</h1>
            <div>
                <asp:Label ID="Label1" runat="server" Text="Cinco productos mas vendidos" Font-Bold="True" Font-Size="Larger"></asp:Label>
                <asp:ImageButton ID="imgBtn1" runat="server" ToolTip="Ver informe" ImageUrl="~/iconos/Ver.png" OnClick="imgBtn1_Click" />
                <br />
            </div>
            <div>
                <asp:Label ID="Label2" runat="server" Text="Tres equipos que mas requieren reparación" Font-Bold="True" Font-Size="Larger"></asp:Label>
                <asp:ImageButton ID="btnTop3Servicio" runat="server" ToolTip="Ver informe" ImageUrl="~/iconos/Ver.png" OnClick="btnTop3Servicio_Click" />
                <br />
            </div>
            <div>
                <asp:Label ID="Label3" runat="server" Text="Promedio de ventas mensuales" Font-Bold="True" Font-Size="Larger"></asp:Label>
                <asp:ImageButton ID="BtnPromedoVentasCli" runat="server" ToolTip="Ver informe" ImageUrl="~/iconos/Ver.png" OnClick="BtnPromedoVentasCli_Click" />
                <br />
            </div>
            <div>
                <asp:Label ID="Label4" runat="server" Text="Promedio de servicios mensuales" Font-Bold="True" Font-Size="Larger"></asp:Label>
                <asp:ImageButton ID="btnServiciosMens" runat="server" ToolTip="Ver informe" ImageUrl="~/iconos/Ver.png" OnClick="btnServiciosMens_Click" />
                <br />
            </div>
            <div>
                <asp:Label ID="Label5" runat="server" Text="Cinco productos mas comprados" Font-Bold="True" Font-Size="Larger"></asp:Label>
                <asp:ImageButton ID="BtnMasComprados" runat="server" ToolTip="Ver informe" ImageUrl="~/iconos/Ver.png" OnClick="BtnMasComprados_Click" />
                <br />
            </div>
        </div>

        </div>

    </asp:Content>
