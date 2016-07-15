Imports System.Data.OleDb
Imports System.Configuration
Imports System.Data

Public Class Register2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Registerbtn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Registerbtn.Click

        'create DB connection using web.config file
        Dim oleDbConn As New OleDb.OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)
        'write SQL statement for inserting record
        Dim AddUserSql As String = "Insert into Users(userName,firstName,lastName,gender,country) Values(@userName,@firstName,@lastName,@gender,@country)"
        'command links SQL statement and database connection
        Dim cmd As OleDbCommand = New OleDbCommand(AddUserSql, oleDbConn)

        cmd.CommandType = CommandType.Text
        'parameters point to actual values stored in front end tools
        cmd.Parameters.AddWithValue("@userName", User.Identity.Name)
        cmd.Parameters.AddWithValue("@firstName", FNametxt.Text)
        cmd.Parameters.AddWithValue("@lastName", LNametxt.Text)
        cmd.Parameters.AddWithValue("@gender", GenderDDL.Text)
        cmd.Parameters.AddWithValue("@country", CountryDDL.SelectedValue)

        'open, run and close database connection
        oleDbConn.Open()
        cmd.ExecuteNonQuery()
        oleDbConn.Close()

        'redirect to Home Page
        Response.Redirect("Login.aspx")
    End Sub
End Class