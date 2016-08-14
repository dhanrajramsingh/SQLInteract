<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WebForm3.aspx.vb" Inherits="SQL_Interact.WebForm3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="HiddenField1" runat="server" />

    <asp:Button ID="Button1" runat="server" Text="Submit" />
    <br />
    <br />
    <asp:Button ID="Button2" runat="server" Text="Next" />

    <br />
    <br />

    <asp:Label ID="Label1" runat="server"></asp:Label>
    <br />
    <asp:Label ID="Label2" runat="server"></asp:Label>
    <br />
    <asp:Label ID="Label3" runat="server"></asp:Label>
</asp:Content>
