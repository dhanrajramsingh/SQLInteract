Imports System.Data.OleDb
Imports System.Configuration
Imports System.Data

Public Class WebForm5
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        
        If Not (IsPostBack) Then
            InfoPanel.Visible = False
            QuizIntroPanel.Visible = True
            QuizPanel.Visible = False
            'Initialise Hidden Fields
            LessonIDHF.Value = Request.Params("lessonID")
            IndexHF.Value = 0
            NumCorrectHF.Value = 0
            MaxIndexHF.Value = 0
            QuestionNumberHF.Value = 1
            Dim Progress As Integer = CInt((IndexHF.Value / 3) * 100)
            Progresslbl.Text = "<div class=""w3-progressbar w3-blue w3-round-xlarge"" style=""width:" & Progress & "%"">" & " <div class=""w3-center w3-text-white"">" & Progress & "%</div> </div>"
        End If

        'create database connection
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)

        If Not IsPostBack Then
            'Getting Quiz Title Info
            'open database connection
            oleDbCon.Open()

            'creates SQL statement to obtain records
            Dim GetLessonDetailsSql As String = "SELECT * FROM [Lessons] WHERE lessonID=@lessonID"
            Dim GetLessonDetailsCmd As OleDbCommand = New OleDbCommand(GetLessonDetailsSql, oleDbCon)
            'define parameters for SQL command
            GetLessonDetailsCmd.Parameters.AddWithValue("@lessonID", LessonIDHF.Value)

            'create Adapter that grabs data from DB
            Dim GetLessonDetailsAdapter As New OleDbDataAdapter
            'creates DataSet that stores captured DB data
            Dim GetLessonDetailsData As New DataSet
            GetLessonDetailsAdapter.SelectCommand = GetLessonDetailsCmd
            GetLessonDetailsAdapter.Fill(GetLessonDetailsData)

            Dim lessonNumber As String = GetLessonDetailsData.Tables(0).Rows(0).Item("lessonOrder").ToString
            Dim lessonName As String = GetLessonDetailsData.Tables(0).Rows(0).Item("lessonName").ToString

            QuizTitlelbl.Text = "Lesson " & lessonNumber & ": " & lessonName & " Quiz"

            'close database connection
            oleDbCon.Close()
        End If

        If Not (IsPostBack) Then
            'Get Quiz database pic and name and total number of questions
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
            QuizIDHF.Value = GetQuizPicData.Tables(0).Rows(0).Item("quizID").ToString
            TotalQuestionsHF.Value = GetQuizPicData.Tables(0).Rows(0).Item("numQuestions").ToString
            Dim TableName As String = GetQuizPicData.Tables(0).Rows(0).Item("tableName").ToString

            QuizImage.ImageUrl = "~/" & PicURL
            TableNamelbl.Text = TableName
            QuestionNumberlbl.Text = "Question " & (QuestionNumberHF.Value) & " out of " & TotalQuestionsHF.Value

            'close database connection
            oleDbCon.Close()
        End If

        If Not (IsPostBack) Then 'Ensures it is not a page refresh from postback
            'open database connection
            oleDbCon.Open()

            'creates SQL statement to obtain records
            Dim GetAttemptOrderSql As String = "SELECT * FROM [QuizAttempt] WHERE quizID=@quizID AND userName=@userName ORDER BY dateTaken DESC"
            Dim GetAttemptOrderCmd As OleDbCommand = New OleDbCommand(GetAttemptOrderSql, oleDbCon)
            'define parameters for SQL command
            GetAttemptOrderCmd.Parameters.AddWithValue("@quizID", QuizIDHF.Value)
            GetAttemptOrderCmd.Parameters.AddWithValue("@userName", User.Identity.Name)

            'create Adapter that grabs data from DB
            Dim GetAttemptOrderAdapter As New OleDbDataAdapter
            'creates DataSet that stores captured DB data
            Dim GetAttemptOrderData As New DataSet
            GetAttemptOrderAdapter.SelectCommand = GetAttemptOrderCmd
            GetAttemptOrderAdapter.Fill(GetAttemptOrderData)

            Dim AttemptOrder As String
            If GetAttemptOrderData.Tables(0).Rows.Count = 0 Then
                AttemptOrder = 0
            Else
                AttemptOrder = GetAttemptOrderData.Tables(0).Rows(0).Item("attemptOrder").ToString
            End If

            Dim IncrementAttemptOrder As Integer = Convert.ToInt32(AttemptOrder) + 1

            'close database connection
            oleDbCon.Close()

            'write quiz details to the QuizAttempt table
            'open database connection
            oleDbCon.Open()

            'create variable to store SQL statement for inserting record
            Dim AddQuizDetailsSql As String = "Insert into QuizAttempt(attemptOrder,userName,quizID,dateTaken) Values(@attemptOrder,@userName,@quizID,@dateTaken)"
            'command links SQL statement and database connection
            Dim AddQuizDetailscmd As OleDbCommand = New OleDbCommand(AddQuizDetailsSql, oleDbCon)

            AddQuizDetailscmd.CommandType = CommandType.Text
            'parameters point to actual values stored in front end controls
            AddQuizDetailscmd.Parameters.AddWithValue("@attemptOrder", IncrementAttemptOrder)
            AddQuizDetailscmd.Parameters.AddWithValue("@userName", User.Identity.Name)
            AddQuizDetailscmd.Parameters.AddWithValue("@quizID", QuizIDHF.Value)
            AddQuizDetailscmd.Parameters.AddWithValue("@dateTaken", System.DateTime.Now())

            'run SQL statement and close database connection
            AddQuizDetailscmd.ExecuteNonQuery()
            'close database connection
            oleDbCon.Close()
        End If

            'Get the first question for the quiz
            'open database connection
            oleDbCon.Open()

            'creates SQL statement to obtain records
            Dim GetQuestionSql As String = "SELECT * FROM [Questions] WHERE quizID=@quizID"
            Dim GetQuestionCmd As OleDbCommand = New OleDbCommand(GetQuestionSql, oleDbCon)
            'define parameters for SQL command
            GetQuestionCmd.Parameters.AddWithValue("@quizID", QuizIDHF.Value)

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
        'create database connection
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)

        'Retrieve answer from database
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
        Dim attemptOrderDB As String = GetAttemptIDData.Tables(0).Rows(0).Item("attemptOrder").ToString

        'close database connection
        oleDbCon.Close()

        'Test if answer submitted is correct
        If String.Equals(UCase(UserInputtxt.Text), UCase(Answer)) Then
            NumCorrectHF.Value = NumCorrectHF.Value + 1
            questionStatus = "correct"

            'Test if this is the last question of the quiz
            If IndexHF.Value >= MaxIndexHF.Value Then
                UserInputtxt.ReadOnly = True
                Submitbtn.Visible = False
                QuizSummaryPanel.Visible = True

                'Calculate score of the quiz
                Dim Score As Integer = CInt((NumCorrectHF.Value / GetAnswerData.Tables(0).Rows.Count) * 100)

                If Score >= 50 Then
                    NextLessonbtn.Visible = True
                Else
                    InfoPanel.Visible = True
                End If

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
                Dim AddScoreSql As String = "Insert into QuizAttempt(attemptID,attemptOrder,userName,quizID,score,dateTaken) Values(@attemptID,@attemptOrder,@userName,@quizID,@score,@dateTaken)"
                'command links SQL statement and database connection
                Dim addScorecmd As OleDbCommand = New OleDbCommand(AddScoreSql, oleDbCon)

                addScorecmd.CommandType = CommandType.Text
                'parameters point to actual values stored in front end controls
                addScorecmd.Parameters.AddWithValue("@attemptID", attemptID)
                addScorecmd.Parameters.AddWithValue("@attemptOrder", attemptOrderDB)
                addScorecmd.Parameters.AddWithValue("@userName", User.Identity.Name)
                addScorecmd.Parameters.AddWithValue("@quizID", quizID)
                addScorecmd.Parameters.AddWithValue("@score", Score)
                addScorecmd.Parameters.AddWithValue("@dateTaken", System.DateTime.Now())

                'open, run SQL statement and close database connection
                oleDbCon.Open()
                addScorecmd.ExecuteNonQuery()
                oleDbCon.Close()
            Else
                IndexHF.Value = IndexHF.Value + 1
            End If
        Else
            questionStatus = "wrong"

            'Test if this is the last question of the quiz
            If IndexHF.Value >= MaxIndexHF.Value Then
                UserInputtxt.ReadOnly = True
                Submitbtn.Visible = False
                QuizSummaryPanel.Visible = True

                'Calculate score of the quiz
                Dim Score As Integer = CInt((NumCorrectHF.Value / GetAnswerData.Tables(0).Rows.Count) * 100)

                If Score >= 50 Then
                    NextLessonbtn.Visible = True
                Else
                    InfoPanel.Visible = True
                End If

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
                Dim AddScoreSql As String = "Insert into QuizAttempt(attemptID,attemptOrder,userName,quizID,score,dateTaken) Values(@attemptID,@attemptOrder,@userName,@quizID,@score,@dateTaken)"
                'command links SQL statement and database connection
                Dim addScorecmd As OleDbCommand = New OleDbCommand(AddScoreSql, oleDbCon)

                addScorecmd.CommandType = CommandType.Text
                'parameters point to actual values stored in front end controls
                addScorecmd.Parameters.AddWithValue("@attemptID", attemptID)
                addScorecmd.Parameters.AddWithValue("@attemptOrder", attemptOrderDB)
                addScorecmd.Parameters.AddWithValue("@userName", User.Identity.Name)
                addScorecmd.Parameters.AddWithValue("@quizID", quizID)
                addScorecmd.Parameters.AddWithValue("@score", Score)
                addScorecmd.Parameters.AddWithValue("@dateTaken", System.DateTime.Now())

                'open, run SQL statement and close database connection
                oleDbCon.Open()
                addScorecmd.ExecuteNonQuery()
                oleDbCon.Close()
            Else
                IndexHF.Value = IndexHF.Value + 1
            End If

            End If

        'Retrieve next question from database.
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

        'close database connection
        oleDbCon.Close()

        'Store user's response
        'open database connection
        oleDbCon.Open()

        'create variable to store SQL statement for inserting record
        Dim AddResponseSql As String = "Insert into Responses(attemptID,questionOrder,response,status) Values(@attemptID,@questionOrder,@response,@status)"
        'command links SQL statement and database connection
        Dim AddResponsecmd As OleDbCommand = New OleDbCommand(AddResponseSql, oleDbCon)

        AddResponsecmd.CommandType = CommandType.Text
        'parameters point to actual values stored in front end controls
        AddResponsecmd.Parameters.AddWithValue("@attemptID", attemptID)
        AddResponsecmd.Parameters.AddWithValue("@questionOrder", QuestionNumberHF.Value)
        AddResponsecmd.Parameters.AddWithValue("@response", UserInputtxt.Text)
        AddResponsecmd.Parameters.AddWithValue("@status", questionStatus)

        'run SQL statement and close database connection
        AddResponsecmd.ExecuteNonQuery()
        oleDbCon.Close()

        If QuestionNumberHF.Value < TotalQuestionsHF.Value Then
            QuestionNumberHF.Value = QuestionNumberHF.Value + 1
        End If

        UserInputtxt.Text = ""

        'Produce Quiz Summary in a Repeater
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

        If IndexHF.Value = MaxIndexHF.Value Then
            Dim Progress As Integer = CInt((QuestionNumberHF.Value / TotalQuestionsHF.Value) * 100)
            Progresslbl.Text = "<div class=""w3-progressbar w3-blue w3-round-xlarge"" style=""width:" & Progress & "%"">" & " <div class=""w3-center w3-text-white"">" & Progress & "%</div> </div>"
        Else
            Dim Progress As Integer = CInt((IndexHF.Value / TotalQuestionsHF.Value) * 100)
            Progresslbl.Text = "<div class=""w3-progressbar w3-blue w3-round-xlarge"" style=""width:" & Progress & "%"">" & " <div class=""w3-center w3-text-white"">" & Progress & "%</div> </div>"
        End If
        

        QuestionNumberlbl.Text = "Question " & (QuestionNumberHF.Value) & " out of " & TotalQuestionsHF.Value

    End Sub

    Protected Sub NextLessonbtn_Click(sender As Object, e As EventArgs) Handles NextLessonbtn.Click

        'create database connection
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)

        'Retrieve records from QuizAttempt table matching QuizID
        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetQuizAttemptSql As String = "SELECT * FROM [QuizAttempt] WHERE quizID=@quizID AND userName=@userName"
        Dim GetQuizAttemptCmd As OleDbCommand = New OleDbCommand(GetQuizAttemptSql, oleDbCon)
        'define parameters for SQL command
        GetQuizAttemptCmd.Parameters.AddWithValue("@quizID", QuizIDHF.Value)
        GetQuizAttemptCmd.Parameters.AddWithValue("@userName", User.Identity.Name)

        'create Adapter that grabs data from DB
        Dim GetQuizAttemptAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim GetQuizAttemptData As New DataSet
        GetQuizAttemptAdapter.SelectCommand = GetQuizAttemptCmd
        GetQuizAttemptAdapter.Fill(GetQuizAttemptData)

        'close database connection
        oleDbCon.Close()

        'Check if the quiz is attempted only once
        If GetQuizAttemptData.Tables(0).Rows.Count <= 1 Then
            'retrieve number of completed lessons
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
        End If

        Response.Redirect("Lesson.aspx?lessonID=" & LessonIDHF.Value + 1)
    End Sub

    Protected Sub Page_UnLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload


    End Sub

    Protected Sub ConfirmButton_Click(sender As Object, e As EventArgs) Handles ConfirmButton.Click
        QuizIntroPanel.Visible = False
        QuizPanel.Visible = True
    End Sub

    Protected Sub GoLessonbtn_Click(sender As Object, e As EventArgs) Handles GoLessonbtn.Click
        Response.Redirect("Lesson.aspx?lessonID=" & LessonIDHF.Value)
    End Sub
End Class