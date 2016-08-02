<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site1.Master" CodeBehind="WebForm5.aspx.vb" Inherits="SQL_Interact.WebForm5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
        <asp:Login ID="Login1" runat="server" BackColor="#FFFFCC" BorderColor="Black" 
            BorderStyle="Ridge" CreateUserText="Not a member? Sign Up" 
            CreateUserUrl="~/Register1.aspx" DestinationPageUrl="~/Home.aspx" 
            Height="324px" TitleText="Log In To SQL Interact" Width="370px" 
            CssClass="center">
            <LabelStyle Font-Bold="True" />
            <TitleTextStyle Font-Names="Algerian" Font-Size="X-Large" 
                Font-Underline="True" />
        </asp:Login>
</asp:Content>
