<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PromedioServiciosMensuales.aspx.cs" Inherits="LabSystemPP3.Pantalla_Informes.PromedioServiciosMensuales" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:SqlDataSource ID="SqlCargarInforme" runat="server"
        ConnectionString="<%$ ConnectionStrings:conexionBD %>" ProviderName="<%$ ConnectionStrings:conexionBD.ProviderName %>"
        SelectCommandType="StoredProcedure" SelectCommand="selectPromedioServiciosMensualesCliente">
     <SelectParameters>
         <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32"></asp:Parameter>
         <asp:Parameter Name="mes" Type="Int32"></asp:Parameter>
         <asp:Parameter Name="anio" Type="Int32"></asp:Parameter>
     </SelectParameters>
 </asp:SqlDataSource>

 <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
<script type="text/javascript">
    google.charts.load('current', { 'packages': ['corechart'] });
    google.charts.setOnLoadCallback(drawChart);

    function drawChart() {

        var data = google.visualization.arrayToDataTable(<%= cargarInforme() %>);

        var options = {
            title: 'Promedio de servicios mensuales por cliente'
        };

        var chart = new google.visualization.PieChart(document.getElementById('piechart'));

        chart.draw(data, options);
    }
</script>

 <div>
     <div>
         <asp:Label ID="Label1" runat="server" Text="Mes"></asp:Label>
         <asp:DropDownList ID="ddlMeses" runat="server">
             <asp:ListItem Text="Enero" Value="1" />
             <asp:ListItem Text="Febrero" Value="2" />
             <asp:ListItem Text="Marzo" Value="3" />
             <asp:ListItem Text="Abril" Value="4" />
             <asp:ListItem Text="Mayo" Value="5" />
             <asp:ListItem Text="Junio" Value="6" />
             <asp:ListItem Text="Julio" Value="7" />
             <asp:ListItem Text="Agosto" Value="8" />
             <asp:ListItem Text="Septiembre" Value="9" />
             <asp:ListItem Text="Octubre" Value="10" />
             <asp:ListItem Text="Noviembre" Value="11" />
             <asp:ListItem Text="Diciembre" Value="12" />
         </asp:DropDownList>
     </div>

     <div>
         <asp:Label ID="Label2" runat="server" Text="Año"></asp:Label>
         <asp:TextBox ID="TbAño" TextMode="Number" runat="server" placeholder="Formato AAAA"></asp:TextBox>
     </div>

     <div>
         <asp:Label ID="Label3" runat="server" Text="Realizar consulta"></asp:Label>
         <asp:Button ID="Button1" runat="server" Text="Consultar" OnClientClick="generarGrafico(); return false;" />
     </div>
 </div>

 <div id="piechart" style="width: 900px; height: 500px;"></div>

</asp:Content>

