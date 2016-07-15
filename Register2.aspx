<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Register2.aspx.vb" Inherits="SQL_Interact.Register2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SQL INTERACT:REGISTER 1/2</title>
    <link href='./images/sql-icon.gif' rel='icon' type='image/x-icon'/>
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
            border: medium #C0C0C0 ridge;
            background-color: #FFFFCC;
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="center">
    
        <h1>
            REGISTRATION: Step 2/2
        </h1>
        <br />
        <p>
            First Name:
            <asp:TextBox ID="FNametxt" runat="server"></asp:TextBox>
        </p>
        <br />
        <p>
            Last Name:<asp:TextBox ID="LNametxt" runat="server"></asp:TextBox>
        </p>
        <br />
        <p>
            Gender:
            <asp:DropDownList ID="GenderDDL" runat="server">
                <asp:ListItem Value="M">Male</asp:ListItem>
                <asp:ListItem Value="F">Female</asp:ListItem>
            </asp:DropDownList>
        </p>
        <br />
        <p>
            Country:
            <asp:DropDownList ID="CountryDDL" runat="server" 
            DataSourceID="SQLInteractDB" DataTextField="countryName" 
            DataValueField="countryCode">
            </asp:DropDownList>
            <asp:AccessDataSource ID="SQLInteractDB" runat="server" 
            DataFile="~/App_Data/SQLInteractDB.mdb" 
            SelectCommand="SELECT [countryName], [countryCode] FROM [Country]">
            </asp:AccessDataSource>
        </p>
        <br />
        <p>
            <asp:Button ID="Registerbtn" runat="server" Text="Complete Registration" />
        </p>
    </div>
    </form>
</body>
</html>
