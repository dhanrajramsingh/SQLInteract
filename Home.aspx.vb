Imports System.Data.OleDb
Imports System.Configuration
Imports System.Data

Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'create Variables to get and store DB values
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)
        oleDbCon.Open()
        'creates SQL statement to obtain records
        Dim GetNameSql As String = "SELECT * FROM [Users] WHERE userName=@userName"
        Dim NameCmd As OleDbCommand = New OleDbCommand(GetNameSql, oleDbCon)
        'define parameters for SQL command
        NameCmd.Parameters.AddWithValue("@userName", User.Identity.Name)
        'create Adapter that grabs data from DB
        Dim NameAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim NameData As New DataSet
        NameAdapter.SelectCommand = NameCmd
        NameAdapter.Fill(NameData)
        'create variables to refine DB data
        Dim FirstName = NameData.Tables(0).Rows(0).Item("firstName").ToString
        Dim LastName = NameData.Tables(0).Rows(0).Item("lastName").ToString
        oleDbCon.Close()

        Userlbl.Text = FirstName & " " & LastName

    End Sub

End Class