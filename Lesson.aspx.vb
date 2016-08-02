Imports System.Data.OleDb
Imports System.Configuration
Imports System.Data

Public Class WebForm4
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'create Hidden Field to capture lessonID value
        HFLessonID.Value = Request.Params("lessonID")

        'create database connection
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)
        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetNotesSql As String = "SELECT * FROM [Lessons] WHERE lessonID=@lessonID"
        Dim NotesCmd As OleDbCommand = New OleDbCommand(GetNotesSql, oleDbCon)
        'define parameters for SQL command
        NotesCmd.Parameters.AddWithValue("@lessonID", HFLessonID.Value)

        'create Adapter that grabs data from DB
        Dim NotesAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim NotesData As New DataSet
        NotesAdapter.SelectCommand = NotesCmd
        NotesAdapter.Fill(NotesData)

        'create variables to store specific DataBase data
        Dim Order As String = NotesData.Tables(0).Rows(0).Item("lessonOrder").ToString
        Dim Name As String = NotesData.Tables(0).Rows(0).Item("lessonName").ToString
        Dim Notes As String = NotesData.Tables(0).Rows(0).Item("lessonNotes").ToString

        'close database connection
        oleDbCon.Close()

        'display lesson number and name
        LessonName.Text = "Lesson " & Order & ": " & Name
        'display lesson notes
        DisplayLesson.Text = Notes
    End Sub

End Class