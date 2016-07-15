<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Register1.aspx.vb" Inherits="SQL_Interact.Register1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <link href='./images/sql-icon.gif' rel='icon' type='image/x-icon'/>
    <title>SQL INTERACT:REGISTER 1/2</title>
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
    <div>
    
        <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" BackColor="#FFFFCC" BorderColor="Black" 
            BorderStyle="Ridge" 
            CompleteSuccessText="Step 1 of registration complete." 
            ContinueDestinationPageUrl="~/Register2.aspx" 
            CreateUserButtonText="Save Account Details" Height="487px" Width="442px" 
            CssClass="center">
            <WizardSteps>
                <asp:CreateUserWizardStep runat="server" />
                <asp:CompleteWizardStep runat="server" />
            </WizardSteps>
        </asp:CreateUserWizard>
    
    </div>
    </form>
</body>
</html>
