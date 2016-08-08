Imports System.Data.OleDb
Imports System.Configuration
Imports System.Data

Public Class WebForm5
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Initialise Hidden Fields
        LessonIDHF.Value = Request.Params("lessonID")

        If Not (IsPostBack) Then
            IndexHF.Value = 0
            NumCorrectHF.Value = 0
            MaxIndexHF.Value = 0
        End If

        'create database connection
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)
        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetQuizPicSql As String = "SELECT * FROM [Quiz] WHERE lessonID=@lessonID"
        Dim GetQuizPicCmd As OleDbCommand = New OleDbCommand(GetQuizPicSql, oleDbCon)
        'define parameters for SQL command
        GetQuizPicCmd.Parameters.AddWithValue("@lessonID", LessonIDHF.Value)

        'create Adapter that grabs data from DB
        Dim GetQuizPicAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim GetQuizPicData As New DataSet
        GetQuizPicAdapter.SelectCommand = GetQuizPicCmd
        GetQuizPicAdapter.Fill(GetQuizPicData)

        'create variables to store specific DataBase data
        Dim PicURL As String = GetQuizPicData.Tables(0).Rows(0).Item("picURL").ToString
        Dim quizID As String = GetQuizPicData.Tables(0).Rows(0).Item("quizID").ToString
        Dim NumQuestions As String = GetQuizPicData.Tables(0).Rows(0).Item("numQuestions").ToString

        QuizTitlelbl.Text = "Lesson " & LessonIDHF.Value & " Quiz"
        QuizPiclbl.Text = "<img src=""" & PicURL & """ height=500 width=700 />"
        QuestionNumberlbl.Text &= (IndexHF.Value + 1) & " out of " & NumQuestions

        'close database connection
        oleDbCon.Close()

        If Not (IsPostBack) Then
            'write quiz details to the QuizAttempt table
            'open database connection
            oleDbCon.Open()

            'create variable to store SQL statement for inserting record
            Dim AddQuizDetailsSql As String = "Insert into QuizAttempt(userName,quizID,dateTaken) Values(@userName,@quizID,@dateTaken)"
            'command links SQL statement and database connection
            Dim AddQuizDetailscmd As OleDbCommand = New OleDbCommand(AddQuizDetailsSql, oleDbCon)

            AddQuizDetailscmd.CommandType = CommandType.Text
            'parameters point to actual values stored in front end controls
            AddQuizDetailscmd.Parameters.AddWithValue("@userName", User.Identity.Name)
            AddQuizDetailscmd.Parameters.AddWithValue("@quizID", quizID)
            AddQuizDetailscmd.Parameters.AddWithValue("@dateTaken", System.DateTime.Now())

            'run SQL statement and close database connection
            AddQuizDetailscmd.ExecuteNonQuery()
            oleDbCon.Close()
        End If


        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetQuestionSql As String = "SELECT * FROM [Questions] WHERE quizID=@quizID"
        Dim GetQuestionCmd As OleDbCommand = New OleDbCommand(GetQuestionSql, oleDbCon)
        'define parameters for SQL command
        GetQuestionCmd.Parameters.AddWithValue("@quizID", quizID)

        'create Adapter that grabs data from DB
        Dim GetQuestionAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim GetQuestionData As New DataSet
        GetQuestionAdapter.SelectCommand = GetQuestionCmd
        GetQuestionAdapter.Fill(GetQuestionData)

        Dim Question As String = GetQuestionData.Tables(0).Rows(IndexHF.Value).Item("questionText").ToString

        QuizQuestionlbl.Text = Question

        'close database connection
        oleDbCon.Close()

    End Sub

    Public Sub QuizReportSub(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        'create variables for Front End items
        Dim QuestionNoLabel As Label = CType(e.Item.FindControl("QuestionNolbl"), Label)
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
        QuestionNoLabel.Text = questionOrderDB
        QuestionLabel.Text = questionDB
        ResponseLabel.Text = responseDB
        StatusLabel.Text = statusDB

    End Sub

    Protected Sub Submitbtn_Click(sender As Object, e As EventArgs) Handles Submitbtn.Click
        Submitbtn.Visible = False

        'Retrieve answer from database
        'create database connection
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)
        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetAnswerSql As String = "SELECT * FROM [QuizQuery] WHERE lessonID=@lessonID ORDER BY questionOrder ASC"
        Dim GetAnswerCmd As OleDbCommand = New OleDbCommand(GetAnswerSql, oleDbCon)
        'define parameters for SQL command
        GetAnswerCmd.Parameters.AddWithValue("@lessonID", LessonIDHF.Value)

        'create Adapter that grabs data from DB
        Dim GetAnswerAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim GetAnswerData As New DataSet
        GetAnswerAdapter.SelectCommand = GetAnswerCmd
        GetAnswerAdapter.Fill(GetAnswerData)

        'create variables to store specific DataBase data
        Dim Answer As String = GetAnswerData.Tables(0).Rows(IndexHF.Value).Item("solutionText").ToString
        Dim quizID As String = GetAnswerData.Tables(0).Rows(0).Item("quizID").ToString

        'close database connection
        oleDbCon.Close()

        MaxIndexHF.Value = GetAnswerData.Tables(0).Rows.Count - 1

        Dim questionStatus As String

        'Get attemptedID of this Quiz attempt
        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetAttemptIDSql As String = "SELECT * FROM [QuizAttempt] WHERE userName=@userName AND quizID = @quizID ORDER BY dateTaken DESC"
        Dim GetAttemptIDCmd As OleDbCommand = New OleDbCommand(GetAttemptIDSql, oleDbCon)
        'define parameters for SQL command
        GetAttemptIDCmd.Parameters.AddWithValue("@userName", User.Identity.Name)
        GetAttemptIDCmd.Parameters.AddWithValue("@quizID", quizID)

        'create Adapter that grabs data from DB
        Dim GetAttemptIDAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim GetAttemptIDData As New DataSet
        GetAttemptIDAdapter.SelectCommand = GetAttemptIDCmd
        GetAttemptIDAdapter.Fill(GetAttemptIDData)

        'create variables to store specific DataBase data
        Dim attemptID As String = GetAttemptIDData.Tables(0).Rows(0).Item("attemptID").ToString

        'close database connection
        oleDbCon.Close()

        'Test if answer submitted is correct
        If String.Equals(UCase(UserInputtxt.Text), UCase(Answer)) Then
            NumCorrectHF.Value = NumCorrectHF.Value + 1
            questionStatus = "correct"
            'Test if this is the last question of the quiz
            If IndexHF.Value >= MaxIndexHF.Value Then
                QuizSummaryPanel.Visible = True
                UserInputtxt.ReadOnly = True
                NextQuestbtn.Visible = False
                NextLessonbtn.Visible = True

                'Calculate score of the quiz
                Dim Score As Integer = CInt((NumCorrectHF.Value / GetAnswerData.Tables(0).Rows.Count) * 100)
                Feedbacklbl.Text &= " Score = " & Score

                Feedbacklbl.Text = "Number of questions correct = " & NumCorrectHF.Value & " out of " & GetAnswerData.Tables(0).Rows.Count & " questions." &
                                    " Your score is " & Score & "%."

                'write score to the QuizAttempt table
                'open database connection
                oleDbCon.Open()

                'create variable to store SQL statement for inserting record
                Dim DeleteSql As String = "Delete From QuizAttempt Where attemptID=@attemptID"
                'command links SQL statement and database connection
                Dim Deletecmd As OleDbCommand = New OleDbCommand(DeleteSql, oleDbCon)

                Deletecmd.CommandType = CommandType.Text
                'parameters point to actual values stored in front end controls
                Deletecmd.Parameters.AddWithValue("@attemptID", attemptID)

                'run SQL statement and close database connection
                Deletecmd.ExecuteNonQuery()
                oleDbCon.Close()

                'create variable to store SQL statement for inserting record
                Dim AddScoreSql As String = "Insert into QuizAttempt(attemptID,userName,quizID,score,dateTaken) Values(@attemptID,@userName,@quizID,@score,@dateTaken)"
                'command links SQL statement and database connection
                Dim addScorecmd As OleDbCommand = New OleDbCommand(AddScoreSql, oleDbCon)

                addScorecmd.CommandType = CommandType.Text
                'parameters point to actual values stored in front end controls
                addScorecmd.Parameters.AddWithValue("@attemptID", attemptID)
                addScorecmd.Parameters.AddWithValue("@userName", User.Identity.Name)
                addScorecmd.Parameters.AddWithValue("@quizID", quizID)
                addScorecmd.Parameters.AddWithValue("@score", Score)
                addScorecmd.Parameters.AddWithValue("@dateTaken", System.DateTime.Now())

                'open, run SQL statement and close database connection
                oleDbCon.Open()
                addScorecmd.ExecuteNonQuery()
                oleDbCon.Close()

                'IndexHF.Value = 0
            Else
                NextQuestbtn.Visible = True
            End If
        Else
            questionStatus = "wrong"

            'Test if this is the last question of the quiz
            If IndexHF.Value >= MaxIndexHF.Value Then
                QuizSummaryPanel.Visible = True
                UserInputtxt.ReadOnly = True
                NextQuestbtn.Visible = False
                NextLessonbtn.Visible = True

                Dim Score As Integer = CInt((NumCorrectHF.Value / GetAnswerData.Tables(0).Rows.Count) * 100)

                Feedbacklbl.Text = "Number of questions correct = " & NumCorrectHF.Value & " out of " & GetAnswerData.Tables(0).Rows.Count & " questions." &
                                   " Your score is " & Score & "%."

                'write score to the QuizAttempt table
                'open database connection
                oleDbCon.Open()

                'create variable to store SQL statement for inserting record
                Dim DeleteSql As String = "Delete From QuizAttempt Where attemptID=@attemptID"
                'command links SQL statement and database connection
                Dim Deletecmd As OleDbCommand = New OleDbCommand(DeleteSql, oleDbCon)

                Deletecmd.CommandType = CommandType.Text
                'parameters point to actual values stored in front end controls
                Deletecmd.Parameters.AddWithValue("@attemptID", attemptID)

                'run SQL statement and close database connection
                Deletecmd.ExecuteNonQuery()
                oleDbCon.Close()

                'create variable to store SQL statement for inserting record
                Dim AddScoreSql As String = "Insert into QuizAttempt(attemptID,userName,quizID,score,dateTaken) Values(@attemptID,@userName,@quizID,@score,@dateTaken)"
                'command links SQL statement and database connection
                Dim addScorecmd As OleDbCommand = New OleDbCommand(AddScoreSql, oleDbCon)

                addScorecmd.CommandType = CommandType.Text
                'parameters point to actual values stored in front end controls
                addScorecmd.Parameters.AddWithValue("@attemptID", attemptID)
                addScorecmd.Parameters.AddWithValue("@userName", User.Identity.Name)
                addScorecmd.Parameters.AddWithValue("@quizID", quizID)
                addScorecmd.Parameters.AddWithValue("@score", Score)
                addScorecmd.Parameters.AddWithValue("@dateTaken", System.DateTime.Now())

                'open, run SQL statement and close database connection
                oleDbCon.Open()
                addScorecmd.ExecuteNonQuery()
                oleDbCon.Close()

                'IndexHF.Value = 0
            Else
                NextQuestbtn.Visible = True
            End If

        End If

        'Store user responses
        'open database connection
        oleDbCon.Open()

        'create variable to store SQL statement for inserting record
        Dim AddResponseSql As String = "Insert into Responses(attemptID,questionOrder,response,status) Values(@attemptID,@questionOrder,@response,@status)"
        'command links SQL statement and database connection
        Dim AddResponsecmd As OleDbCommand = New OleDbCommand(AddResponseSql, oleDbCon)

        AddResponsecmd.CommandType = CommandType.Text
        'parameters point to actual values stored in front end controls
        AddResponsecmd.Parameters.AddWithValue("@attemptID", attemptID)
        AddResponsecmd.Parameters.AddWithValue("@questionOrder", IndexHF.Value + 1)
        AddResponsecmd.Parameters.AddWithValue("@response", UserInputtxt.Text)
        AddResponsecmd.Parameters.AddWithValue("@status", questionStatus)

        'run SQL statement and close database connection
        AddResponsecmd.ExecuteNonQuery()
        oleDbCon.Close()
        UserInputtxt.Text = ""

        If IndexHF.Value >= MaxIndexHF.Value Then
            'Bind Repeater to datasource
            'open connection
            oleDbCon.Open()
            'define SQL
            Dim QuizSql As String = "SELECT * FROM [QuestionResponse] WHERE attemptID = @attemptID"
            'define command
            Dim QuizCmd As OleDbCommand = New OleDbCommand(QuizSql, oleDbCon)
            QuizCmd.Parameters.AddWithValue("@attemptID", attemptID)
            Dim QuizAdapter As New OleDbDataAdapter
            Dim QuizData As New DataSet
            QuizAdapter.SelectCommand = QuizCmd
            QuizAdapter.Fill(QuizData)
            'creating connection between repeater and dataset
            QuizReportRepeater.DataSource = QuizData.Tables(0)
            QuizReportRepeater.DataBind()
            oleDbCon.Close()
        End If

    End Sub

    Protected Sub NextQuestbtn_Click(sender As Object, e As EventArgs) Handles NextQuestbtn.Click
        Submitbtn.Visible = True

        If IndexHF.Value > MaxIndexHF.Value Then
            UserInputtxt.ReadOnly = True
            IndexHF.Value = 0
            NextQuestbtn.Visible = False
            NextLessonbtn.Visible = True
            Submitbtn.Visible = False
        Else
            IndexHF.Value = IndexHF.Value + 1
        End If

        'Retrieve subsequent question from database.
        'create database connection
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)
        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetInstructionSql As String = "SELECT * FROM [QuizQuery] WHERE lessonID=@lessonID ORDER BY questionOrder ASC"
        Dim GetInstructionCmd As OleDbCommand = New OleDbCommand(GetInstructionSql, oleDbCon)
        'define parameters for SQL command
        GetInstructionCmd.Parameters.AddWithValue("@lessonID", LessonIDHF.Value)

        'create Adapter that grabs data from DB
        Dim GetInstructionAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim GetInstructionData As New DataSet
        GetInstructionAdapter.SelectCommand = GetInstructionCmd
        GetInstructionAdapter.Fill(GetInstructionData)

        Dim Instruction As String = GetInstructionData.Tables(0).Rows(IndexHF.Value).Item("questionText").ToString

        QuizQuestionlbl.Text = Instruction
        NextQuestbtn.Visible = False
    End Sub

    Protected Sub NextLessonbtn_Click(sender As Object, e As EventArgs) Handles NextLessonbtn.Click
        'retrieve number of completed lessons
        'create database connection
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)
        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetCurrentLessonSql As String = "SELECT * FROM [CurrentLesson] WHERE userName=@userName"
        Dim GetCurrentLessonCmd As OleDbCommand = New OleDbCommand(GetCurrentLessonSql, oleDbCon)
        'define parameters for SQL command
        GetCurrentLessonCmd.Parameters.AddWithValue("@userName", User.Identity.Name)

        'create Adapter that grabs data from DB
        Dim GetCurrentLessonAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim GetCurrentLessonData As New DataSet
        GetCurrentLessonAdapter.SelectCommand = GetCurrentLessonCmd
        GetCurrentLessonAdapter.Fill(GetCurrentLessonData)

        'create variables to store specific DataBase data
        Dim numLessons As String = GetCurrentLessonData.Tables(0).Rows(0).Item("numLessonsCompleted").ToString
        Dim number As Integer = Convert.ToInt32(numLessons)
        number = number + 1

        'close database connection
        oleDbCon.Close()

        'delete user record from current lesson table
        'open database connection
        oleDbCon.Open()

        'create variable to store SQL statement for inserting record
        Dim DeleteSql As String = "Delete From CurrentLesson Where userName=@userName"
        'command links SQL statement and database connection
        Dim Deletecmd As OleDbCommand = New OleDbCommand(DeleteSql, oleDbCon)

        Deletecmd.CommandType = CommandType.Text
        'parameters point to actual values stored in front end controls
        Deletecmd.Parameters.AddWithValue("@userName", User.Identity.Name)

        'run SQL statement and close database connection
        Deletecmd.ExecuteNonQuery()
        oleDbCon.Close()

        'Insert new record with user's current lesson
        'open database connection
        oleDbCon.Open()

        'create variable to store SQL statement for inserting record
        Dim InsertCurrentLessonSql As String = "Insert into CurrentLesson(userName,currentLessonID,numLessonsCompleted) Values(@userName,@currentLessonID,@numLessonsCompleted)"
        'command links SQL statement and database connection
        Dim InsertCurrentLessoncmd As OleDbCommand = New OleDbCommand(InsertCurrentLessonSql, oleDbCon)

        InsertCurrentLessoncmd.CommandType = CommandType.Text
        'parameters point to actual values stored in front end controls
        InsertCurrentLessoncmd.Parameters.AddWithValue("@userName", User.Identity.Name)
        InsertCurrentLessoncmd.Parameters.AddWithValue("@currentLessonID", LessonIDHF.Value + 1)
        InsertCurrentLessoncmd.Parameters.AddWithValue("@numLessonsCompleted", number)

        'run SQL statement and close database connection
        InsertCurrentLessoncmd.ExecuteNonQuery()
        oleDbCon.Close()

        Response.Redirect("Lesson.aspx?lessonID=" & LessonIDHF.Value + 1)
    End Sub
End Class