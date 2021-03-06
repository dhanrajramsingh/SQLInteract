﻿Imports System.Data.OleDb
Imports System.Configuration
Imports System.Data

Public Class WebForm4
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Hidden Field used to capture lessonID value from query string
        HFLessonID.Value = Request.QueryString("lessonID")

        If Not (IsPostBack) Then
            IndexHF.Value = 0
            TaskNumberHF.Value = 1
            NumTaskAttemptsHF.Value = 0
            ActivityNumberlbl.Text = TaskNumberHF.Value
            Dim Progress As Integer = CInt((IndexHF.Value / 3) * 100)
            Progresslbl.Text = "<div class=""w3-progressbar w3-blue w3-round-xlarge"" style=""width:" & Progress & "%"">" & " <div class=""w3-center w3-text-white"">" & Progress & "%</div> </div>"
        End If

        'create database connection
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)

        '----------------------------------------------------------------------------------------------------------------------------
        'Get the Lesson Name and Notes from the database
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

        Dim Order As String = ""
        Dim Name As String = ""
        Dim Explanation As String = ""
        Dim Syntax As String = ""
        Dim Example As String = ""
        'create variables to store specific DataBase data
        If NotesData.Tables(0).Rows.Count <> 0 Then
            Order = NotesData.Tables(0).Rows(0).Item("lessonOrder").ToString
            Name = NotesData.Tables(0).Rows(0).Item("lessonName").ToString
            Explanation = NotesData.Tables(0).Rows(0).Item("lessonExplanation").ToString
            Syntax = NotesData.Tables(0).Rows(0).Item("lessonSyntax").ToString
            Example = NotesData.Tables(0).Rows(0).Item("lessonExample").ToString
        End If

        'close database connection
        oleDbCon.Close()

        'display lesson number and name
        LessonName.Text = "Lesson " & Order & ": " & Name

        'display lesson notes
        If String.Equals(HFLessonID.Value, "1") Then
            IntroductionPanel.Visible = True
            SQLLessonPanel.Visible = False
            Introductionlbl.Text = Explanation
        Else
            IntroductionPanel.Visible = False
            SQLLessonPanel.Visible = True
            DisplayExplanationlbl.Text = Explanation
            DisplaySyntaxlbl.Text = Syntax
            DisplayExamplelbl.Text = Example
            ExplanationPanel.Visible = True
            SyntaxPanel.Visible = False
            ExamplePanel.Visible = False
            VideoPanel.Visible = False
            ExplanationLink.BackColor = Drawing.Color.Red
            SyntaxLink.BackColor = Drawing.Color.Gray
            ExampleLink.BackColor = Drawing.Color.Gray
            VideoLink.BackColor = Drawing.Color.Gray
        End If



        '----------------------------------------------------------------------------------------------------------------------------
        'Get the video for the lesson from the database
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

        Dim Resource As String = ""
        'create variables to store specific DataBase data
        If ResourceData.Tables(0).Rows.Count <> 0 Then
            Resource = ResourceData.Tables(0).Rows(0).Item("resourceURL").ToString
        End If

        'close database connection
        oleDbCon.Close()

        'display lesson video
        Videolbl.Text = "<video  controls> " & "<source src=""" & Resource & """  type=""video/mp4""> " & "Your browser does not support HTML5 video. " & "</video>"

        '----------------------------------------------------------------------------------------------------------------------------
        'Get the 1st activity instruction and answer from the database. This is only done when the page initially loads and not on postbacks
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

            Dim Instruction As String = ""
            Dim Answer As String = ""
            If ActivityData.Tables(0).Rows.Count <> 0 Then
                'create variables to store specific DataBase data
                Instruction = ActivityData.Tables(0).Rows(IndexHF.Value).Item("activityText").ToString
                Answer = ActivityData.Tables(0).Rows(IndexHF.Value).Item("activityAnswer").ToString
            End If

            ActivityInstructionlbl.Text = Instruction

            'close database connection
            oleDbCon.Close()
        End If

        '---------------------------------------------------------------------------------------------------------------------------
        'Hide Activity for Introduction Lesson
        If String.Equals(HFLessonID.Value, "1") Then
            ActivityPanel.Visible = False
            ResultPanel.Visible = False
            NextPagebtn.Visible = True
        Else
            ActivityPanel.Visible = True
            NextPagebtn.Visible = False
        End If

        ActivityTitlelbl.Text = Name & " Activity"
        '----------------------------------------------------------------------------------------------------------------------------
        'Check User's current lesson and ensure the lesson clicked on can be viewed. If it cannot be viewed redirect to homepage.

        If IsPostBack = False Then
            'opens DB connection
            oleDbCon.Open()
            'create SQL command
            Dim CurrentLessonSql As String = "SELECT * FROM CurrentLesson WHERE userName=@UserName"
            'create DB command
            Dim CurrentLessonCmd As OleDbCommand = New OleDbCommand(CurrentLessonSql, oleDbCon)
            'define parameters
            CurrentLessonCmd.Parameters.AddWithValue("@UserName", User.Identity.Name)
            'create Data Adapter
            Dim CurrentLessonAdapter As New OleDbDataAdapter
            'create Data Set
            Dim CurrentLessonData As New DataSet
            'use Data Adapter to run the command
            CurrentLessonAdapter.SelectCommand = CurrentLessonCmd
            'Data Adapter fills the Data Set with the resulting data
            CurrentLessonAdapter.Fill(CurrentLessonData)
            'create variables from data values in data set results
            Dim currentLessonID = CurrentLessonData.Tables(0).Rows(0).Item("currentLessonID").ToString
            oleDbCon.Close()

            Dim currentID As Integer = Convert.ToInt32(currentLessonID)
            Dim chosenID As Integer = Convert.ToInt32(HFLessonID.Value)

            If chosenID > currentID Then
                Response.Redirect("Home.aspx?LessonCheck=1&ScrollPos=1")
            End If
        End If
        '-------------------------------------------------------------------------------------------------------------------------
        'Get Quiz ID for the lessonID
        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetQuizIDSql As String = "SELECT * FROM [Quiz] WHERE lessonID=@lessonID"
        Dim GetQuizIDCmd As OleDbCommand = New OleDbCommand(GetQuizIDSql, oleDbCon)
        'define parameters for SQL command
        GetQuizIDCmd.Parameters.AddWithValue("@lessonID", HFLessonID.Value)

        'create Adapter that grabs data from DB
        Dim GetQuizIDAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim GetQuizIDData As New DataSet
        GetQuizIDAdapter.SelectCommand = GetQuizIDCmd
        GetQuizIDAdapter.Fill(GetQuizIDData)

        Dim QuizID As String = GetQuizIDData.Tables(0).Rows(0).Item("quizID")
        'Close database connection
        oleDbCon.Close()

        'Get number of attempts for Quiz ID just retrieved
        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetNumAttemptsSql As String = "SELECT * FROM [CountAttempts] WHERE userName=@userName AND quizID=@quizID"
        Dim GetNumAttemptsCmd As OleDbCommand = New OleDbCommand(GetNumAttemptsSql, oleDbCon)
        'define parameters for SQL command
        GetNumAttemptsCmd.Parameters.AddWithValue("@userName", User.Identity.Name)
        GetNumAttemptsCmd.Parameters.AddWithValue("@quizID", QuizID)

        'create Adapter that grabs data from DB
        Dim GetNumAttemptsAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim GetNumAttemptsData As New DataSet
        GetNumAttemptsAdapter.SelectCommand = GetNumAttemptsCmd
        GetNumAttemptsAdapter.Fill(GetNumAttemptsData)

        Dim NumAttempts As String = ""
        If GetNumAttemptsData.Tables(0).Rows.Count = 0 Then
            NumAttempts = "0"
        Else
            NumAttempts = GetNumAttemptsData.Tables(0).Rows(0).Item("numAttempts")
        End If

        'Close database connection
        oleDbCon.Close()

        Dim numberAttempts As Integer = Convert.ToInt32(NumAttempts)

        If numberAttempts >= 1 Then
            TakeQuizbtn.Visible = True
        Else
            TakeQuizbtn.Visible = False
        End If

    End Sub

    Protected Sub Submitbtn_Click(sender As Object, e As EventArgs) Handles Submitbtn.Click
        InfoPanel.Visible = False

        'create database connection
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)

        '---------------------------------------------------------------------------------------------------------------------------
        'Get answer from database
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
        Dim Response As String = UserInputtxt.Text

        '---------------------------------------------------------------------------------------------------------------------------
        'Take User's response and break into an array of lines
        Dim Responselist As New ArrayList
        Dim newLine As String = vbCrLf
        Dim Continueloop As Boolean = True
        Dim StartPos As Integer = 1
        Dim StartCopy As Integer = 1
        Dim CharPos As Integer
        Dim Size As Integer = Len(Response)
        While Continueloop
            CharPos = InStr(StartPos, Response, newLine, CompareMethod.Binary)
            If CharPos = 0 Then
                Responselist.Add(Mid(Response, StartCopy, Size - StartPos))
                Continueloop = False
            Else
                Responselist.Add(Mid(Response, StartCopy, CharPos - StartPos))
                StartPos = CharPos + 1
                StartCopy = CharPos + 1
            End If
        End While
        '-----------------------------------------------------------------------
        'Take Answer and break into an array of lines
        Dim Answerlist As New ArrayList
        Continueloop = True
        StartPos = 1
        StartCopy = 1
        Size = Len(Answer)
        While Continueloop
            CharPos = InStr(StartPos, Answer, newLine, CompareMethod.Binary)
            If CharPos = 0 Then
                Answerlist.Add(Mid(Answer, StartCopy, Size - StartPos))
                Continueloop = False
            Else
                Answerlist.Add(Mid(Answer, StartCopy, CharPos - StartPos))
                StartPos = CharPos + 1
                StartCopy = CharPos + 1
            End If
        End While
        '-----------------------------------------------------------------------
        'Prepare hints list
        ShowAnstxt.Text = "Hints:" & vbCrLf
        If InStr(1, Response, Chr(59), CompareMethod.Binary) = 0 Then
            ShowAnstxt.Text &= "Please end SQL command with a semicolon." & vbCrLf
        End If
        If (Responselist.Count = 1) Or (Answerlist.Count <> Responselist.Count) Then
            ShowAnstxt.Text &= "Please fragment SQL command and put each clause on a new line. Refer to notes." & vbCrLf
        End If
        If Answerlist.Count = Responselist.Count Then
            For loopCounter As Integer = 0 To Answerlist.Count - 1
                If Not String.Equals(UCase(Answerlist(loopCounter)), UCase(Responselist(loopCounter))) Then
                    ShowAnstxt.Text &= "Line " & loopCounter + 1 & " does not match the answer." & vbCrLf
                Else
                    ShowAnstxt.Text &= "Line " & loopCounter + 1 & " is correct." & vbCrLf
                End If
            Next
        End If
        ShowAnstxt.Visible = True
        '-----------------------------------------------------------------------
        'Test if answer is correct. If it is, hide answer elements and display results of the query. If it is correct and the last
        'task of the activity, inform user of the quiz and make its button visible. If the answer is incorrect ask user to re-try
        If String.Equals(UCase(UserInputtxt.Text), UCase(Answer)) Then
            NumTaskAttemptsHF.Value = 0
            'Calculate activity percentage complete for progress bar
            Dim Progress As Integer = CInt((TaskNumberHF.Value / 3) * 100)
            Progresslbl.Text = "<div class=""w3-progressbar w3-blue w3-round-xlarge"" style=""width:" & Progress & "%"">" & " <div class=""w3-center w3-text-white"">" & Progress & "%</div> </div>"
            Submitbtn.Visible = False
            GridView1.Visible = True
            ShowAnsbtn.Visible = False
            ShowAnstxt.Visible = False
            '----------------------------------------------------------------
            'Display results of query in GridView
            Dim sqlCommand As String = UserInputtxt.Text
            ExercisesDB.DataSourceMode = SqlDataSourceMode.DataReader
            ExercisesDB.SelectCommand = sqlCommand
            GridView1.DataSource = ExercisesDB
            GridView1.AutoGenerateColumns = True
            GridView1.DataBind()
            '----------------------------------------------------------------
            'Test if this is the last task of the activity
            If IndexHF.Value >= 2 Then
                UserInputtxt.ReadOnly = True
                IndexHF.Value = 0
                WrongPanel.Visible = False
                CorrectPanel.Visible = True
                CorrectFeedbacklbl.Text = "Hooray you are correct!" & "<br />" & "You are ready to take the quiz."
                NextQuestbtn.Visible = False
                TakeQuizbtn.Visible = True

            Else
                WrongPanel.Visible = False
                CorrectPanel.Visible = True
                CorrectFeedbacklbl.Text = "Hooray you are correct!"
                NextQuestbtn.Visible = True
            End If
            '---------------------------------------------------------------
        Else
            NumTaskAttemptsHF.Value = NumTaskAttemptsHF.Value + 1
            If NumTaskAttemptsHF.Value = 5 Then
                ShowAnsbtn.Visible = True
            End If
            CorrectPanel.Visible = False
            WrongPanel.Visible = True
            WrongFeedbacklbl.Text = "Sorry, please try again." & "<br />" & "Pay careful attention to the hints below and look out for extra spaces in your response."

        End If
    End Sub

    Protected Sub NextQuestbtn_Click(sender As Object, e As EventArgs) Handles NextQuestbtn.Click
        InfoPanel.Visible = False
        WrongPanel.Visible = False
        CorrectPanel.Visible = False
        GridView1.Visible = False
        ShowAnsbtn.Visible = False
        ShowAnstxt.Visible = False
        Submitbtn.Visible = True
        UserInputtxt.Text = ""
        ShowAnstxt.Text = ""
        '----------------------------------------------------------------------------------------------------------------------------
        'Check if this is the last task of the activity
        If IndexHF.Value >= 2 Then
            UserInputtxt.ReadOnly = True
            IndexHF.Value = 0
            TaskNumberHF.Value = 0
            NextQuestbtn.Visible = False
            TakeQuizbtn.Visible = True
            Submitbtn.Visible = False
        Else
            IndexHF.Value = IndexHF.Value + 1
            TaskNumberHF.Value = TaskNumberHF.Value + 1
        End If
        ActivityNumberlbl.Text = TaskNumberHF.Value

        '----------------------------------------------------------------------------------------------------------------------------
        'Get the next task's instruction from the database 
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

        ActivityInstructionlbl.Text = Instruction
        NextQuestbtn.Visible = False
        '-------------------------------------------------------------------------------------------------------------------------
    End Sub

    Protected Sub NextPagebtn_Click(sender As Object, e As EventArgs) Handles NextPagebtn.Click

        'create database connection to the SQLInteractDB database
        Dim oleDbConn As New OleDb.OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)

        'opens DB connection
        oleDbConn.Open()
        'create SQL command
        Dim CurrentLessonSql As String = "SELECT * FROM CurrentLesson WHERE userName=@UserName"
        'create DB command
        Dim CurrentLessonCmd As OleDbCommand = New OleDbCommand(CurrentLessonSql, oleDbConn)
        'define parameters
        CurrentLessonCmd.Parameters.AddWithValue("@UserName", User.Identity.Name)
        'create Data Adapter
        Dim CurrentLessonAdapter As New OleDbDataAdapter
        'create Data Set
        Dim CurrentLessonData As New DataSet
        'use Data Adapter to run the command
        CurrentLessonAdapter.SelectCommand = CurrentLessonCmd
        'Data Adapter fills the Data Set with the resulting data
        CurrentLessonAdapter.Fill(CurrentLessonData)
        'create variables from data values in data set results
        Dim currentLessonID = CurrentLessonData.Tables(0).Rows(0).Item("currentLessonID").ToString
        oleDbConn.Close()

        If String.Equals(currentLessonID, "1") Then
            'create variable to store SQL statement for inserting record
            Dim DeleteSql As String = "Delete From CurrentLesson Where userName=@userName"
            'command links SQL statement and database connection
            Dim Deletecmd As OleDbCommand = New OleDbCommand(DeleteSql, oleDbConn)

            Deletecmd.CommandType = CommandType.Text
            'parameters point to actual values stored in front end controls
            Deletecmd.Parameters.AddWithValue("@userName", User.Identity.Name)

            'open, run SQL statement and close database connection
            oleDbConn.Open()
            Deletecmd.ExecuteNonQuery()
            oleDbConn.Close()

            '----------------------------------------------------------------------------------------------------------------------------

            'create variable to store SQL statement for inserting record
            Dim AddInitialLessonSql As String = "Insert into CurrentLesson(userName,currentLessonID,numLessonsCompleted) Values(@userName,@currentLessonID,@numCompletedLessons)"
            'command links SQL statement and database connection
            Dim addInitialLessoncmd As OleDbCommand = New OleDbCommand(AddInitialLessonSql, oleDbConn)

            Dim chosenID As Integer = Convert.ToInt32(HFLessonID.Value)
            chosenID = chosenID + 1

            addInitialLessoncmd.CommandType = CommandType.Text
            'parameters point to actual values stored in front end controls
            addInitialLessoncmd.Parameters.AddWithValue("@userName", User.Identity.Name)
            addInitialLessoncmd.Parameters.AddWithValue("@currentLessonID", chosenID)
            addInitialLessoncmd.Parameters.AddWithValue("@numCompletedLessons", 1)

            'open, run SQL statement and close database connection
            oleDbConn.Open()
            addInitialLessoncmd.ExecuteNonQuery()
            oleDbConn.Close()
        End If

        Response.Redirect("Lesson.aspx?lessonID=2")
    End Sub

    Protected Sub ShowAnsbtn_Click(sender As Object, e As EventArgs) Handles ShowAnsbtn.Click

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
        GridView1.Visible = True
        CorrectPanel.Visible = False
        WrongPanel.Visible = False
        InfoPanel.Visible = True

    End Sub

    Protected Sub TakeQuizbtn_Click(sender As Object, e As EventArgs) Handles TakeQuizbtn.Click
        Response.Redirect("Quiz.aspx?lessonID=" & HFLessonID.Value)
    End Sub

    Protected Sub ExplanationLink_Click(sender As Object, e As EventArgs) Handles ExplanationLink.Click
        ExplanationLink.BackColor = Drawing.Color.Red
        SyntaxLink.BackColor = Drawing.Color.Gray
        ExampleLink.BackColor = Drawing.Color.Gray
        VideoLink.BackColor = Drawing.Color.Gray
        ExplanationPanel.Visible = True
        SyntaxPanel.Visible = False
        ExamplePanel.Visible = False
        VideoPanel.Visible = False
    End Sub

    Protected Sub SyntaxLink_Click(sender As Object, e As EventArgs) Handles SyntaxLink.Click
        ExplanationLink.BackColor = Drawing.Color.Gray
        SyntaxLink.BackColor = Drawing.Color.Red
        ExampleLink.BackColor = Drawing.Color.Gray
        VideoLink.BackColor = Drawing.Color.Gray
        ExplanationPanel.Visible = False
        SyntaxPanel.Visible = True
        ExamplePanel.Visible = False
        VideoPanel.Visible = False
    End Sub

    Protected Sub ExampleLink_Click(sender As Object, e As EventArgs) Handles ExampleLink.Click
        ExplanationLink.BackColor = Drawing.Color.Gray
        SyntaxLink.BackColor = Drawing.Color.Gray
        ExampleLink.BackColor = Drawing.Color.Red
        VideoLink.BackColor = Drawing.Color.Gray
        ExplanationPanel.Visible = False
        SyntaxPanel.Visible = False
        ExamplePanel.Visible = True
        VideoPanel.Visible = False
    End Sub

    Protected Sub VideoLink_Click(sender As Object, e As EventArgs) Handles VideoLink.Click
        ExplanationLink.BackColor = Drawing.Color.Gray
        SyntaxLink.BackColor = Drawing.Color.Gray
        ExampleLink.BackColor = Drawing.Color.Gray
        VideoLink.BackColor = Drawing.Color.Red
        ExplanationPanel.Visible = False
        SyntaxPanel.Visible = False
        ExamplePanel.Visible = False
        VideoPanel.Visible = True
    End Sub
End Class