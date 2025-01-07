<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowPDF.aspx.cs" Inherits="LabSystemPP3.PantallaVenta.ShowPDF" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <iframe id="pdfViewer" runat="server" style="width: 100%; height: 100vh;" src=""></iframe>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </div>
    </form>
</body>
</html>
