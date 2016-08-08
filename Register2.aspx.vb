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

        'create DB connection using connection string from web.config file
        Dim oleDbConn1 As New OleDb.OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)
        'create variable to store SQL statement for inserting record
        Dim AddInitialLessonSql As String = "Insert into CurrentLesson(userName,currentLessonID,numLessonsCompleted) Values(@userName,@currentLessonID,@numLessonsCompleted)"
        'command links SQL statement and database connection
        Dim addInitialLessoncmd As OleDbCommand = New OleDbCommand(AddInitialLessonSql, oleDbConn1)

        addInitialLessoncmd.CommandType = CommandType.Text
        'parameters point to actual values stored in front end controls
        addInitialLessoncmd.Parameters.AddWithValue("@userName", User.Identity.Name)
        addInitialLessoncmd.Parameters.AddWithValue("@currentLessonID", 1)
        addInitialLessoncmd.Parameters.AddWithValue("@numLessonsCompleted", 0)

        'open, run SQL statement and close database connection
        oleDbConn1.Open()
        addInitialLessoncmd.ExecuteNonQuery()
        oleDbConn1.Close()

        'redirect to Login Page
        Response.Redirect("Login.aspx")
    End Sub
End Class