<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductosMasComprados.aspx.cs" Inherits="LabSystemPP3.Pantalla_Informes.ProductosMasComprados" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>

    <link href="informes2.css" rel="stylesheet" type="text/css" />
    <div class="container">


        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
            ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
            SelectCommandType="StoredProcedure" SelectCommand="SelectProductosMasComprados"></asp:SqlDataSource>

        <div class="contenido-central">
            <div id="encabezado">
                <h1>Cinco productos más vendidos</h1>
            </div>

            <div id="barchart_values" style="width: 900px; height: 300px;"></div>

        </div>

    </div>
</asp:Content>

