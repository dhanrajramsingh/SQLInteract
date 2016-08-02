Imports System.Data.OleDb
Imports System.Configuration
Imports System.Data

Public Class Register2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Registerbtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Registerbtn.Click

        'create DB connection using connection string from web.config file
        Dim oleDbConn As New OleDb.OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)
        'create variable to store SQL statement for inserting record
        Dim AddUserSql As String = "Insert into Users(userName,firstName,lastName,gender,country) Values(@userName,@firstName,@lastName,@gender,@country)"
        'command links SQL statement and database connection
        Dim addUsercmd As OleDbCommand = New OleDbCommand(AddUserSql, oleDbConn)

        addUsercmd.CommandType = CommandType.Text
        'parameters point to actual values stored in front end controls
        addUsercmd.Parameters.AddWithValue("@userName", User.Identity.Name)
        addUsercmd.Parameters.AddWithValue("@firstName", FNametxt.Text)
        addUsercmd.Parameters.AddWithValue("@lastName", LNametxt.Text)
        addUsercmd.Parameters.AddWithValue("@gender", GenderDDL.Text)
        addUsercmd.Parameters.AddWithValue("@country", CountryDDL.SelectedValue)

        'open, run SQL statement and close database connection
        oleDbConn.Open()
        addUsercmd.ExecuteNonQuery()
        oleDbConn.Close()

        'redirect to Login Page
        Response.Redirect("Login.aspx")
    End Sub
End Class