Imports System.Data.OleDb
Imports System.Configuration
Imports System.Data

Public Class WebForm2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'To be used in drop down list data source queries
        UserNameHF.Value = User.Identity.Name

        'create DB connection to SQLInteractDB database
        Dim oleDbConn1 As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)

        '----------------------------------------------------------------------------------------------------------------------------------
        'This code checks the QuizAttempt table for the latest quiz attempt to test if the score is zero and then checks
        'the CheckQuizComplete table to verify the number of responses given is equal to the number of questions in the quiz.
        'If the previous conditions are true then this quiz is incomplete and its details are deleted from the database.
        Dim Refreshpage As Boolean = False

        If Not (IsPostBack) Then
            'open database connection
            oleDbConn1.Open()

            'creates SQL statement to obtain records
            Dim GetAttemptIDSql As String = "SELECT * FROM [QuizAttempt] WHERE userName=@userName ORDER BY dateTaken DESC"
            Dim GetAttemptIDCmd As OleDbCommand = New OleDbCommand(GetAttemptIDSql, oleDbConn1)
            'define parameters for SQL command
            GetAttemptIDCmd.Parameters.AddWithValue("@userName", User.Identity.Name)

            'create Adapter that grabs data from DB
            Dim GetAttemptIDAdapter As New OleDbDataAdapter
            'creates DataSet that stores captured DB data
            Dim GetAttemptIDData As New DataSet
            GetAttemptIDAdapter.SelectCommand = GetAttemptIDCmd
            GetAttemptIDAdapter.Fill(GetAttemptIDData)

            Dim AttemptID As String = ""
            Dim Score As String = ""

            If GetAttemptIDData.Tables(0).Rows.Count <> 0 Then
                'create variables to store specific DataBase data
                AttemptID = GetAttemptIDData.Tables(0).Rows(0).Item("attemptID").ToString
                Score = GetAttemptIDData.Tables(0).Rows(0).Item("score").ToString
            End If

            'close database connection
            oleDbConn1.Close()

            'open database connection
            oleDbConn1.Open()

            'creates SQL statement to obtain records
            Dim GetResponsesSql As String = "SELECT * FROM [CheckQuizComplete] WHERE attemptID=@attemptID"
            Dim GetResponsesCmd As OleDbCommand = New OleDbCommand(GetResponsesSql, oleDbConn1)
            'define parameters for SQL command
            GetResponsesCmd.Parameters.AddWithValue("@attemptID", AttemptID)

            'create Adapter that grabs data from DB
            Dim GetResponsesAdapter As New OleDbDataAdapter
            'creates DataSet that stores captured DB data
            Dim GetResponsesData As New DataSet
            GetResponsesAdapter.SelectCommand = GetResponsesCmd
            GetResponsesAdapter.Fill(GetResponsesData)

            Dim NumberAnswered As String = ""
            Dim NumQuestions As String = ""

            If GetResponsesData.Tables(0).Rows.Count <> 0 Then
                'create variables to store specific DataBase data
                NumberAnswered = GetResponsesData.Tables(0).Rows(0).Item("numberAnswered").ToString
                NumQuestions = GetResponsesData.Tables(0).Rows(0).Item("numQuestions").ToString
            End If

            'close database connection
            oleDbConn1.Close()

            Dim ScoreNumber = 0
            If String.Equals(Score, "") Then
                ScoreNumber = 0
            Else
                ScoreNumber = Convert.ToInt32(Score)
            End If

            If ScoreNumber = 0 Then
                If GetResponsesData.Tables(0).Rows.Count <> 0 Then
                    If Convert.ToInt32(NumberAnswered) <> Convert.ToInt32(NumQuestions) Then
                        'open database connection
                        oleDbConn1.Open()

                        'create variable to store SQL statement for inserting record
                        Dim DeleteSql As String = "Delete From Responses Where attemptID=@attemptID"
                        'command links SQL statement and database connection
                        Dim Deletecmd As OleDbCommand = New OleDbCommand(DeleteSql, oleDbConn1)

                        Deletecmd.CommandType = CommandType.Text
                        'parameters point to actual values stored in front end controls
                        Deletecmd.Parameters.AddWithValue("@attemptID", AttemptID)

                        'run SQL statement and close database connection
                        Deletecmd.ExecuteNonQuery()
                        oleDbConn1.Close()
                        Refreshpage = True
                    End If
                Else
                    'open database connection
                    oleDbConn1.Open()

                    'create variable to store SQL statement for inserting record
                    Dim DeleteSql As String = "Delete From QuizAttempt Where attemptID=@attemptID"
                    'command links SQL statement and database connection
                    Dim Deletecmd As OleDbCommand = New OleDbCommand(DeleteSql, oleDbConn1)

                    Deletecmd.CommandType = CommandType.Text
                    'parameters point to actual values stored in front end controls
                    Deletecmd.Parameters.AddWithValue("@attemptID", AttemptID)

                    'run SQL statement and close database connection
                    Deletecmd.ExecuteNonQuery()
                    oleDbConn1.Close()
                End If
            End If
        End If

        If Refreshpage Then
            Response.Redirect(Request.RawUrl)
        End If
        '----------------------------------------------------------------------------------------------------------------------

        'create DB connection to ASPNetDB database
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("ASPNetDB").ConnectionString)

        'Retrieve Account Create Date
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

        '-----------------------------------------------------------------------------------------------------------------------------
        Dim LessonID As Integer = 2

        If String.Equals(LessonNameDDL.SelectedValue, "") Then
            LessonID = 2
            LessonIDHF.Value = LessonID
        Else
            Dim getDDLValue = LessonNameDDL.SelectedValue
            LessonID = CInt(getDDLValue)
            LessonIDHF.Value = LessonID
        End If

        '------------------------------------------------------------------------------------------------------------------------

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
        Feedbacklbl.Text = "Sorry no quiz history to display."
        If QuizData.Tables(0).Rows.Count = 0 Then
            QuizAttemptPanel.Visible = False
            ChooseLessonlbl.Visible = False
            LessonNameDDL.Visible = False
            QuizHistoryChart.Visible = False
            QuizHistoryPanel.Visible = False
            QuizAttemptDLL.Visible = False
            QuizAttemptlbl.Visible = False
            Feedbacklbl.Visible = True
        Else
            QuizAttemptPanel.Visible = True
            ChooseLessonlbl.Visible = True
            LessonNameDDL.Visible = True
            QuizHistoryChart.Visible = True
            QuizAttemptDLL.Visible = True
            QuizAttemptlbl.Visible = True
            QuizHistoryPanel.Visible = True
            Feedbacklbl.Visible = False
        End If
        '-----------------------------------------------------------------------------------------------------------------------------
        'When Dropdown List is initially rendered its selected value is an empty string. Set attempt order to 1
        Dim attemptOrder As Integer = 1
        If String.Equals(QuizAttemptDLL.SelectedValue, "") Then
            attemptOrder = 1
        Else
            Dim getDDLValue = QuizAttemptDLL.SelectedValue
            attemptOrder = CInt(getDDLValue)
        End If

        'Bind QuizAttemptRepeater to datasource
        'open connection
        oleDbConn1.Open()
        'define SQL
        Dim QuizAttemptSql As String = "SELECT * FROM [QuestionResponse] WHERE userName = @userName AND attemptOrder = @attemptOrder AND lessonID=@lessonID ORDER BY questionOrder"
        'define command
        Dim QuizAttemptCmd As OleDbCommand = New OleDbCommand(QuizAttemptSql, oleDbConn1)
        QuizAttemptCmd.Parameters.AddWithValue("@userName", User.Identity.Name)
        QuizAttemptCmd.Parameters.AddWithValue("@attemptOrder", attemptOrder)
        QuizAttemptCmd.Parameters.AddWithValue("@lessonID", LessonID)
        Dim QuizAttemptAdapter As New OleDbDataAdapter
        Dim QuizAttemptData As New DataSet
        QuizAttemptAdapter.SelectCommand = QuizAttemptCmd
        QuizAttemptAdapter.Fill(QuizAttemptData)
        'creating connection between repeater and dataset
        QuizAttemptRepeater.DataSource = QuizAttemptData.Tables(0)
        QuizAttemptRepeater.DataBind()
        oleDbConn1.Close()

        '-----------------------------------------------------------------------------------------------------------------------------
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
        '--------------------------------------------------------------------------------------------------------------------------------------------------------
        'Calculate percentage of unit completed and display in unit progress bar
        'open database connection
        oleDbConn1.Open()

        'creates SQL statement to obtain records
        Dim GetLessonNameSql As String = "SELECT * FROM [UserCurrentLessonName] WHERE userName=@userName"
        Dim GetLessonNameCmd As OleDbCommand = New OleDbCommand(GetLessonNameSql, oleDbConn1)
        'define parameters for SQL command
        GetLessonNameCmd.Parameters.AddWithValue("@userName", User.Identity.Name)

        'create Adapter that grabs data from DB
        Dim GetLessonNameAdapter As New OleDbDataAdapter

        'creates DataSet that stores captured DB data
        Dim GetLessonNameData As New DataSet
        GetLessonNameAdapter.SelectCommand = GetLessonNameCmd
        GetLessonNameAdapter.Fill(GetLessonNameData)

        'create variables to store specific DataBase data
        Dim UnitNumber = GetLessonNameData.Tables(0).Rows(0).Item("unitID").ToString
        
        'close database connection
        oleDbConn1.Close()

        'open database connection
        oleDbConn1.Open()

        'creates SQL statement to obtain records
        Dim GetTotalUnitLessonsSql As String = "SELECT * FROM [Lessons] WHERE unitID = @unitID"
        Dim GetTotalUnitLessonsCmd As OleDbCommand = New OleDbCommand(GetTotalUnitLessonsSql, oleDbConn1)
        GetTotalUnitLessonsCmd.Parameters.AddWithValue("@unitID", UnitNumber)

        'create Adapter that grabs data from DB
        Dim GetTotalUnitLessonsAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim GetTotalUnitLessonsData As New DataSet

        GetTotalUnitLessonsAdapter.SelectCommand = GetTotalUnitLessonsCmd
        GetTotalUnitLessonsAdapter.Fill(GetTotalUnitLessonsData)

        'close database connection
        oleDbConn1.Close()

        Dim UnitProgress As Integer = CInt((numCompleted / GetTotalUnitLessonsData.Tables(0).Rows.Count) * 100)

        UnitProgresslbl.Text = "<div class=""w3-progressbar w3-blue w3-round-xlarge"" style=""width:" & UnitProgress & "%"">" & " <div class=""w3-center w3-text-white"">" & UnitProgress & "%</div> </div>"
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

    Public Sub QuizAttemptSub(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        'create variables for Front End items
        Dim QuestionOrderLabel As Label = CType(e.Item.FindControl("QuestionOrderlbl"), Label)
        Dim QuestionLabel As Label = CType(e.Item.FindControl("Questionlbl"), Label)
        Dim ResponseLabel As Label = CType(e.Item.FindControl("Responselbl"), Label)
        Dim StatusLabel As Label = CType(e.Item.FindControl("Statuslbl"), Label)


        'define variables for data items
        Dim row As DataRowView = CType(e.Item.DataItem, DataRowView)
        Dim questionOrderDB As String = row("questionOrder").ToString()
        Dim questionDB As String = row("questionText").ToString()
        Dim responseDB As String = row("response").ToString()
        Dim statusDB As String = row("status").ToString()

        'join data values with front end items
        QuestionOrderLabel.Text = questionOrderDB
        QuestionLabel.Text = questionDB
        ResponseLabel.Text = responseDB
        StatusLabel.Text = statusDB
    End Sub
End Class