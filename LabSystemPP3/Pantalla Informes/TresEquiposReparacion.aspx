<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TresEquiposReparacion.aspx.cs" Inherits="LabSystemPP3.Pantalla_Informes.TresEquiposReparacion" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">
        google.charts.load("current", { packages: ["corechart"] });
        google.charts.setOnLoadCallback(drawChart);

        function drawChart() {
            // La función cargarInforme() genera los datos en formato JSON válido
            var data = google.visualization.arrayToDataTable(<%= cargarInforme() %>);

            // Opciones para personalizar el gráfico
            var options = {
                title: "Top 3 Productos que más requieren servicio",
                width: 900,
                height: 500,
                bar: { groupWidth: "95%" },
                legend: { position: "none" }
            };

            // Crear y renderizar el gráfico de barras
            var chart = new google.visualization.BarChart(document.getElementById("barchart_values"));
            chart.draw(data, options);
        }
</script>

    <link href="informes2.css" rel="stylesheet" type="text/css" />
    <div class="container">

        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
            ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
            SelectCommandType="StoredProcedure" SelectCommand="TresProductosQueMasRequierenServicio">
            <SelectParameters>
                <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlDataSource2" runat="server"
            ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
            SelectCommandType="StoredProcedure" SelectCommand="TresProductosQueMasRequierenServicioFecha">
            <SelectParameters>
                <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="mes" Type="Int32"></asp:Parameter>
                <asp:Parameter Name="anio" Type="Int32"></asp:Parameter>
            </SelectParameters>
        </asp:SqlDataSource>

        <div class="contenido-central">
            <div id="encabezado">
                <h1>Tres equipos que mas se dañan</h1>
            </div>

            <div id="barchart_values" style="width: 900px; height: 300px;"></div>


        </div>
    </div>
</asp:Content>
