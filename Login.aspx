<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="SQL_Interact.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <link href="images/sql-icon.gif" rel="icon" type="image/x-icon"/>
    <title>SQL INTERACT: LOGIN</title>
    <style type="text/css">
        html, body
        {
            width: 100%;
            padding: 0;
            margin: 0;
        }
        body
        {
            background: #598084 url(../images/pattern15.png);
            color: #474747;
        }
        .center {
            margin: auto;
            padding: 10px;
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
        }
    </style>
    
</head>

<body>
    <form id="form1" runat="server">
    <div class="center">
    
        <asp:Login ID="Login1" runat="server" BackColor="#FFFFCC" BorderColor="Black" 
            BorderStyle="Ridge" CreateUserText="Not a member? Sign Up" 
            CreateUserUrl="~/Register1.aspx" DestinationPageUrl="~/Home.aspx" 
            Height="324px" TitleText="Log In To SQL Interact" Width="370px" 
            CssClass="center">
            <LabelStyle Font-Bold="True" />
            <TitleTextStyle Font-Names="Algerian" Font-Size="X-Large" 
                Font-Underline="True" />
        </asp:Login>
    
    </div>
    </form>

</body>

</html>
