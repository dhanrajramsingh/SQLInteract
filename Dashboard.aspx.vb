Imports System.Data.OleDb
Imports System.Configuration
Imports System.Data

Public Class WebForm2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        UserNameHF.Value = User.Identity.Name

        'create DB connection to ASPNetDB database
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("ASPNetDB").ConnectionString)

        'Retrieve Last Login Date
        'opens DB connection
        oleDbCon.Open()
        'create SQL command
        Dim GetDateSql As String = "SELECT * FROM [UserActivity] WHERE userName=@userName"
        'create DB command
        Dim GetDateCmd As OleDbCommand = New OleDbCommand(GetDateSql, oleDbCon)
        'define parameters
        GetDateCmd.Parameters.AddWithValue("@userName", User.Identity.Name)
        'create Data Adapter
        Dim GetDateAdapter As New OleDbDataAdapter
        'create Data Set
        Dim GetDateData As New DataSet
        'use Data Adapter to run the command
        GetDateAdapter.SelectCommand = GetDateCmd
        'Data Adapter fills the Data Set with the resulting data
        GetDateAdapter.Fill(GetDateData)
        oleDbCon.Close()

        'process the date value into a statement and assign to label
        Dim LastLoginDateValue = DateDiff(DateInterval.Minute, GetDateData.Tables(0).Rows(0).Item("CreateDate"), System.DateTime.Now)
        If LastLoginDateValue > 1439 Then
            DateCreatedlbl.Text = LastLoginDateValue \ 1440 & " days ago"
        ElseIf LastLoginDateValue > 60 Then
            DateCreatedlbl.Text = LastLoginDateValue \ 60 & " hours ago"
        Else
            DateCreatedlbl.Text = LastLoginDateValue & " minutes ago"
        End If

        Dim LessonID As Integer = 2
        If String.Equals(LessonNameDDL.SelectedValue, "") Then
            ChooseLessonlbl.Visible = False
            LessonNameDDL.Visible = False
            QuizHistoryPanel.Visible = False
            Feedbacklbl.Visible = True
            Feedbacklbl.Text = "Sorry no quiz history to display."
            LessonID = 2
        Else
            ChooseLessonlbl.Visible = True
            LessonNameDDL.Visible = True
            QuizHistoryChart.Visible = True
            QuizHistoryPanel.Visible = True
            Feedbacklbl.Visible = False
            Dim getDDLValue = LessonNameDDL.SelectedValue
            LessonID = CInt(getDDLValue)
        End If

        'create DB connection to SQLInteractDB database
        Dim oleDbConn1 As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)

        'Bind QuizHistoryRepeater to datasource
        'open connection
        oleDbConn1.Open()
        'define SQL
        Dim QuizSql As String = "SELECT * FROM [QuizHistory] WHERE userName = @userName AND lessonID = @lessonID"
        'define command
        Dim QuizCmd As OleDbCommand = New OleDbCommand(QuizSql, oleDbConn1)
        QuizCmd.Parameters.AddWithValue("@userName", User.Identity.Name)
        QuizCmd.Parameters.AddWithValue("@lessonID", LessonID)
        Dim QuizAdapter As New OleDbDataAdapter
        Dim QuizData As New DataSet
        QuizAdapter.SelectCommand = QuizCmd
        QuizAdapter.Fill(QuizData)
        'creating connection between repeater and dataset
        QuizHistoryRepeater.DataSource = QuizData.Tables(0)
        QuizHistoryRepeater.DataBind()
        oleDbConn1.Close()
        If QuizData.Tables(0).Rows.Count = 0 Then
            ChooseLessonlbl.Visible = False
            LessonNameDDL.Visible = False
            QuizHistoryChart.Visible = False
            QuizHistoryPanel.Visible = False
            Feedbacklbl.Visible = True
        Else
            ChooseLessonlbl.Visible = True
            LessonNameDDL.Visible = True
            QuizHistoryChart.Visible = True
            Feedbacklbl.Visible = False
            QuizHistoryPanel.Visible = True
        End If

        'Calculate course progress for Progress Bar

        'open database connection
        oleDbConn1.Open()

        'creates SQL statement to obtain records
        Dim GetLessonsCompletedSql As String = "SELECT * FROM [CurrentLesson] WHERE userName=@userName"
        Dim GetLessonsCompletedCmd As OleDbCommand = New OleDbCommand(GetLessonsCompletedSql, oleDbConn1)
        'define parameters for SQL command
        GetLessonsCompletedCmd.Parameters.AddWithValue("@userName", User.Identity.Name)

        'create Adapter that grabs data from DB
        Dim GetLessonsCompletedAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim GetLessonsCompletedData As New DataSet

        GetLessonsCompletedAdapter.SelectCommand = GetLessonsCompletedCmd
        GetLessonsCompletedAdapter.Fill(GetLessonsCompletedData)

        Dim numLessons = GetLessonsCompletedData.Tables(0).Rows(0).Item("numLessonsCompleted").ToString

        Dim numCompleted As Integer = Convert.ToInt32(numLessons)

        'close database connection
        oleDbConn1.Close()

        'open database connection
        oleDbConn1.Open()

        'creates SQL statement to obtain records
        Dim GetTotalLessonsSql As String = "SELECT * FROM [Lessons]"
        Dim GetTotalLessonsCmd As OleDbCommand = New OleDbCommand(GetTotalLessonsSql, oleDbConn1)

        'create Adapter that grabs data from DB
        Dim GetTotalLessonsAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim GetTotalLessonsData As New DataSet

        GetTotalLessonsAdapter.SelectCommand = GetTotalLessonsCmd
        GetTotalLessonsAdapter.Fill(GetTotalLessonsData)

        'close database connection
        oleDbConn1.Close()

        Dim Progress As Integer = CInt((numCompleted / GetTotalLessonsData.Tables(0).Rows.Count) * 100)

        Progresslbl.Text = "<div class=""w3-progressbar w3-blue w3-round-xlarge"" style=""width:" & Progress & "%"">" & " <div class=""w3-center w3-text-white"">" & Progress & "%</div> </div>"

    End Sub

    Public Sub QuizHistorySub(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        'create variables for Front End items
        Dim AttemptNoLabel As Label = CType(e.Item.FindControl("AttemptNolbl"), Label)
        Dim ScoreLabel As Label = CType(e.Item.FindControl("Scorelbl"), Label)
        Dim DateTakenLabel As Label = CType(e.Item.FindControl("DateTakenlbl"), Label)

        'define variables for data items
        Dim row As DataRowView = CType(e.Item.DataItem, DataRowView)
        Dim attemptOrderDB As String = row("attemptOrder").ToString()
        Dim scoreDB As String = row("score").ToString()
        
        'join data values with front end items
        AttemptNoLabel.Text = attemptOrderDB
        ScoreLabel.Text = scoreDB

        'process the date value into a statement and assign to label
        Dim dateTakenValue = DateDiff(DateInterval.Minute, row("dateTaken"), System.DateTime.Now)
        If dateTakenValue > 1439 Then
            DateTakenLabel.Text = dateTakenValue \ 1440 & " days ago"
        ElseIf dateTakenValue > 60 Then
            DateTakenLabel.Text = dateTakenValue \ 60 & " hours ago"
        Else
            DateTakenLabel.Text = dateTakenValue & " minutes ago"
        End If

    End Sub

End Class