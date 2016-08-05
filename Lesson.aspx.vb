Imports System.Data.OleDb
Imports System.Configuration
Imports System.Data

Public Class WebForm4
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'create Hidden Field to capture lessonID value
        HFLessonID.Value = Request.QueryString("lessonID")

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

        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetResourceSql As String = "SELECT * FROM [Resources] WHERE lessonID=@lessonID"
        Dim ResourceCmd As OleDbCommand = New OleDbCommand(GetResourceSql, oleDbCon)
        'define parameters for SQL command
        ResourceCmd.Parameters.AddWithValue("@lessonID", HFLessonID.Value)

        'create Adapter that grabs data from DB
        Dim ResourceAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim ResourceData As New DataSet
        ResourceAdapter.SelectCommand = ResourceCmd
        ResourceAdapter.Fill(ResourceData)

        'create variables to store specific DataBase data
        Dim Resource As String = ResourceData.Tables(0).Rows(0).Item("resourceURL").ToString

        'close database connection
        oleDbCon.Close()

        If Not (IsPostBack) Then
            'open database connection
            oleDbCon.Open()

            'creates SQL statement to obtain records
            Dim GetActivitySql As String = "SELECT * FROM [LessonActivity] WHERE lessonID=@lessonID ORDER BY activityOrder ASC"
            Dim ActivityCmd As OleDbCommand = New OleDbCommand(GetActivitySql, oleDbCon)
            'define parameters for SQL command
            ActivityCmd.Parameters.AddWithValue("@lessonID", HFLessonID.Value)

            'create Adapter that grabs data from DB
            Dim ActivityAdapter As New OleDbDataAdapter
            'creates DataSet that stores captured DB data
            Dim ActivityData As New DataSet
            ActivityAdapter.SelectCommand = ActivityCmd
            ActivityAdapter.Fill(ActivityData)

            'create variables to store specific DataBase data
            Dim Instruction As String = ActivityData.Tables(0).Rows(IndexHF.Value).Item("activityText").ToString
            Dim Answer As String = ActivityData.Tables(0).Rows(IndexHF.Value).Item("activityAnswer").ToString

            ActivityInstructionlbl.Text = Instruction

            'close database connection
            oleDbCon.Close()
        End If

        'display lesson number and name
        LessonName.Text = "Lesson " & Order & ": " & Name
        'display lesson notes
        DisplayLesson.Text = Notes
        'display lesson video
        Videolbl.Text = "<video width=""400"" controls> " & "<source src=""" & Resource & """  type=""video/mp4""> " & "Your browser does not support HTML5 video. " & "</video>"

        'Hide Activity for Introduction Lesson
        If HFLessonID.Value = 1 Then
            ActivityPanel.Visible = False
            NextPagebtn.Visible = True
        Else
            ActivityPanel.Visible = True
            NextPagebtn.Visible = False
        End If

        ActivityTitlelbl.Text = LessonName.Text & " Activity"
        
        

    End Sub

    Protected Sub Submitbtn_Click(sender As Object, e As EventArgs) Handles Submitbtn.Click

        'create database connection
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)
        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetActivitySql As String = "SELECT * FROM [LessonActivity] WHERE lessonID=@lessonID ORDER BY activityOrder ASC"
        Dim ActivityCmd As OleDbCommand = New OleDbCommand(GetActivitySql, oleDbCon)
        'define parameters for SQL command
        ActivityCmd.Parameters.AddWithValue("@lessonID", HFLessonID.Value)

        'create Adapter that grabs data from DB
        Dim ActivityAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim ActivityData As New DataSet
        ActivityAdapter.SelectCommand = ActivityCmd
        ActivityAdapter.Fill(ActivityData)

        'create variables to store specific DataBase data
        Dim Answer As String = ActivityData.Tables(0).Rows(IndexHF.Value).Item("activityAnswer").ToString

        If String.Equals(UCase(UserInputtxt.Text), UCase(Answer)) Then
            If IndexHF.Value >= 2 Then
                UserInputtxt.ReadOnly = True
                IndexHF.Value = 0
                Feedbacklbl.Text = "Hooray you are correct!" & vbCrLf & "You are ready to take the quiz."
                NextQuestbtn.Visible = False
                TakeQuizbtn.Visible = True
                Submitbtn.Visible = False
            Else
                Feedbacklbl.Text = "Hooray you are correct!"
                NextQuestbtn.Visible = True
            End If
        Else
            Feedbacklbl.Text = "Sorry, please try again." & vbCrLf & "Remember to avoid unnecessary spaces in your answer and place each clause on a new line."
            ShowAnsbtn.Visible = True
        End If
    End Sub

    Protected Sub NextQuestbtn_Click(sender As Object, e As EventArgs) Handles NextQuestbtn.Click

        ShowAnsbtn.Visible = False
        ShowAnstxt.Visible = False

        If IndexHF.Value >= 2 Then
            UserInputtxt.ReadOnly = True
            IndexHF.Value = 0
            NextQuestbtn.Visible = False
            TakeQuizbtn.Visible = True
            Submitbtn.Visible = False
        End If

        If IndexHF.Value < 2 Then
            IndexHF.Value = IndexHF.Value + 1
        End If

        'create database connection
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)
        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetActivitySql As String = "SELECT * FROM [LessonActivity] WHERE lessonID=@lessonID ORDER BY activityOrder ASC"
        Dim ActivityCmd As OleDbCommand = New OleDbCommand(GetActivitySql, oleDbCon)
        'define parameters for SQL command
        ActivityCmd.Parameters.AddWithValue("@lessonID", HFLessonID.Value)

        'create Adapter that grabs data from DB
        Dim ActivityAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim ActivityData As New DataSet
        ActivityAdapter.SelectCommand = ActivityCmd
        ActivityAdapter.Fill(ActivityData)

        Dim Instruction As String = ActivityData.Tables(0).Rows(IndexHF.Value).Item("activityText").ToString


        Feedbacklbl.Text = ""

        ActivityInstructionlbl.Text = Instruction
        NextQuestbtn.Visible = False
    End Sub

    Protected Sub NextPagebtn_Click(sender As Object, e As EventArgs) Handles NextPagebtn.Click
        Response.Redirect("Lesson.aspx?lessonID=2")
    End Sub

    Protected Sub ShowAnsbtn_Click(sender As Object, e As EventArgs) Handles ShowAnsbtn.Click

        NextQuestbtn.Visible = True

        'create database connection
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)
        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetActivitySql As String = "SELECT * FROM [LessonActivity] WHERE lessonID=@lessonID ORDER BY activityOrder ASC"
        Dim ActivityCmd As OleDbCommand = New OleDbCommand(GetActivitySql, oleDbCon)
        'define parameters for SQL command
        ActivityCmd.Parameters.AddWithValue("@lessonID", HFLessonID.Value)

        'create Adapter that grabs data from DB
        Dim ActivityAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim ActivityData As New DataSet
        ActivityAdapter.SelectCommand = ActivityCmd
        ActivityAdapter.Fill(ActivityData)

        'create variables to store specific DataBase data
        Dim Answer As String = ActivityData.Tables(0).Rows(IndexHF.Value).Item("activityAnswer").ToString

        ShowAnstxt.Visible = True
        ShowAnstxt.Text = Answer

    End Sub
End Class