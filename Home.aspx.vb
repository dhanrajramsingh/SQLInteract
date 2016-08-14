Imports System.Data.OleDb
Imports System.Configuration
Imports System.Data

Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Retrieve LessonCheck parameter from query string to verify if the destination lesson was locked
        CheckLessonIDHF.Value = Request.Params("LessonCheck")

        'Check if any page redirected to homepage with string query parameter of 1
        If String.Equals(CheckLessonIDHF.Value, "1") Then
            WarningPanel.Visible = True
            LessonFeedbacklbl.Text = "This lesson you clicked is not accessible until all preceding lessons are completed!"
        Else
            WarningPanel.Visible = False
        End If

        'Retrieve a value to use to scroll the page lower
        ScrollBarHF.Value = Request.Params("ScrollPos")
        '----------------------------------------------------------------------------------------------------------------------------------

        'create connection to database
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)

        '----------------------------------------------------------------------------------------------------------------------------------
        'This code checks the QuizAttempt table for the latest quiz attempt to test if the score is zero and then checks
        'the CheckQuizComplete table to verify the number of responses given is equal to the number of questions in the quiz.
        'If the previous conditions are true then this quiz is incomplete and its details are deleted from the database.
        Dim Refreshpage As Boolean = False

        If Not (IsPostBack) Then
            'open database connection
            oleDbCon.Open()

            'creates SQL statement to obtain records
            Dim GetAttemptIDSql As String = "SELECT * FROM [QuizAttempt] WHERE userName=@userName ORDER BY dateTaken DESC"
            Dim GetAttemptIDCmd As OleDbCommand = New OleDbCommand(GetAttemptIDSql, oleDbCon)
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
            oleDbCon.Close()

            'open database connection
            oleDbCon.Open()

            'creates SQL statement to obtain records
            Dim GetResponsesSql As String = "SELECT * FROM [CheckQuizComplete] WHERE attemptID=@attemptID"
            Dim GetResponsesCmd As OleDbCommand = New OleDbCommand(GetResponsesSql, oleDbCon)
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
            oleDbCon.Close()

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
                        oleDbCon.Open()

                        'create variable to store SQL statement for inserting record
                        Dim DeleteSql As String = "Delete From Responses Where attemptID=@attemptID"
                        'command links SQL statement and database connection
                        Dim Deletecmd As OleDbCommand = New OleDbCommand(DeleteSql, oleDbCon)

                        Deletecmd.CommandType = CommandType.Text
                        'parameters point to actual values stored in front end controls
                        Deletecmd.Parameters.AddWithValue("@attemptID", AttemptID)

                        'run SQL statement and close database connection
                        Deletecmd.ExecuteNonQuery()
                        oleDbCon.Close()

                        Refreshpage = True
                    End If
                Else
                    'open database connection
                    oleDbCon.Open()

                    'create variable to store SQL statement for inserting record
                    Dim DeleteSql As String = "Delete From QuizAttempt Where attemptID=@attemptID"
                    'command links SQL statement and database connection
                    Dim Deletecmd As OleDbCommand = New OleDbCommand(DeleteSql, oleDbCon)

                    Deletecmd.CommandType = CommandType.Text
                    'parameters point to actual values stored in front end controls
                    Deletecmd.Parameters.AddWithValue("@attemptID", AttemptID)

                    'run SQL statement and close database connection
                    Deletecmd.ExecuteNonQuery()
                    oleDbCon.Close()
                End If
            End If
        End If

        If Refreshpage Then
            Response.Redirect(Request.RawUrl)
        End If
        '----------------------------------------------------------------------------------------------------------------------
        'Get the user's first and last name from the database using the user's username.

        'open database connection
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

        'create variables to store specific DataBase data
        Dim FirstName = NameData.Tables(0).Rows(0).Item("firstName").ToString
        Dim LastName = NameData.Tables(0).Rows(0).Item("lastName").ToString

        'close database connection
        oleDbCon.Close()

        Userlbl.Text = FirstName & " " & LastName

        '---------------------------------------------------------------------------------------------------------------------------------
        'Get the user's current lesson information and place as query string's parameters to transfer to the lesson page.

        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetLessonNameSql As String = "SELECT * FROM [UserCurrentLessonName] WHERE userName=@userName"
        Dim GetLessonNameCmd As OleDbCommand = New OleDbCommand(GetLessonNameSql, oleDbCon)
        'define parameters for SQL command
        GetLessonNameCmd.Parameters.AddWithValue("@userName", User.Identity.Name)

        'create Adapter that grabs data from DB
        Dim GetLessonNameAdapter As New OleDbDataAdapter

        'creates DataSet that stores captured DB data
        Dim GetLessonNameData As New DataSet
        GetLessonNameAdapter.SelectCommand = GetLessonNameCmd
        GetLessonNameAdapter.Fill(GetLessonNameData)

        'create variables to store specific DataBase data
        Dim LessonName = GetLessonNameData.Tables(0).Rows(0).Item("lessonName").ToString
        Dim UnitNumber = GetLessonNameData.Tables(0).Rows(0).Item("unitID").ToString
        Dim LessonID = GetLessonNameData.Tables(0).Rows(0).Item("currentLessonID").ToString
        Dim LessonOrder = GetLessonNameData.Tables(0).Rows(0).Item("lessonOrder").ToString

        'close database connection
        oleDbCon.Close()

        'Display current lesson name
        CurrentLessonNamelbl.Text = "<a href=""Lesson.aspx?lessonID=" & LessonID & """>" & "Unit " & UnitNumber & ": <br />" & "Lesson " & LessonOrder & ": " & LessonName & "</a>"

        '--------------------------------------------------------------------------------------------------------------------------
        'Get the number of lessons completed and the total number of lessons to find a percentage to represent the user's 
        'progress. This percentage is used to update the progress bar on the home page.

        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetLessonsCompletedSql As String = "SELECT * FROM [CurrentLesson] WHERE userName=@userName"
        Dim GetLessonsCompletedCmd As OleDbCommand = New OleDbCommand(GetLessonsCompletedSql, oleDbCon)
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
        oleDbCon.Close()

        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetTotalLessonsSql As String = "SELECT * FROM [Lessons]"
        Dim GetTotalLessonsCmd As OleDbCommand = New OleDbCommand(GetTotalLessonsSql, oleDbCon)

        'create Adapter that grabs data from DB
        Dim GetTotalLessonsAdapter As New OleDbDataAdapter
        'creates DataSet that stores captured DB data
        Dim GetTotalLessonsData As New DataSet

        GetTotalLessonsAdapter.SelectCommand = GetTotalLessonsCmd
        GetTotalLessonsAdapter.Fill(GetTotalLessonsData)

        'close database connection
        oleDbCon.Close()

        Dim Progress As Integer = CInt((numCompleted / GetTotalLessonsData.Tables(0).Rows.Count) * 100)

        Progresslbl.Text = "<div class=""w3-progressbar w3-blue w3-round-xlarge"" style=""width:" & Progress & "%"">" & " <div class=""w3-center w3-text-white"">" & Progress & "%</div> </div>"

        '-----------------------------------------------------------------------------------------------------------------------------

        'Bind Repeater1 to datasource
        'open connection
        oleDbCon.Open()
        'define SQL
        Dim GetLessonsSql As String = "SELECT * FROM [Unit1_Lessons] ORDER BY lessonOrder ASC"
        'define command
        Dim GetLessonsCmd As OleDbCommand = New OleDbCommand(GetLessonsSql, oleDbCon)

        Dim GetLessonsAdapter As New OleDbDataAdapter
        Dim GetLessonsData As New DataSet
        GetLessonsAdapter.SelectCommand = GetLessonsCmd
        GetLessonsAdapter.Fill(GetLessonsData)
        'creating connection between repeater and dataset
        Repeater1.DataSource = GetLessonsData.Tables(0)
        Repeater1.DataBind()
        oleDbCon.Close()

        '--------------------------------------------------------------------------------------------------------------------------------

        'Bind Repeater2 to datasource
        'open connection
        oleDbCon.Open()
        'define SQL
        Dim GetLessons2Sql As String = "SELECT * FROM [Unit2_Lessons] ORDER BY lessonOrder ASC"
        'define command
        Dim GetLessons2Cmd As OleDbCommand = New OleDbCommand(GetLessons2Sql, oleDbCon)

        Dim GetLessons2Adapter As New OleDbDataAdapter
        Dim GetLessons2Data As New DataSet
        GetLessons2Adapter.SelectCommand = GetLessons2Cmd
        GetLessons2Adapter.Fill(GetLessons2Data)
        'creating connection between repeater and dataset
        Repeater2.DataSource = GetLessons2Data.Tables(0)
        Repeater2.DataBind()
        oleDbCon.Close()

        '---------------------------------------------------------------------------------------------------------------------------

        'Bind QuizFeedRepeater to datasource
        'open connection
        oleDbCon.Open()
        'define SQL
        Dim QuizInfoSql As String = "SELECT * FROM [QuizFeed] WHERE userName=@userName"
        'define command
        Dim QuizInfoCmd As OleDbCommand = New OleDbCommand(QuizInfoSql, oleDbCon)
        QuizInfoCmd.Parameters.AddWithValue("@userName", User.Identity.Name)

        Dim QuizInfoAdapter As New OleDbDataAdapter
        Dim QuizInfoData As New DataSet
        QuizInfoAdapter.SelectCommand = QuizInfoCmd
        QuizInfoAdapter.Fill(QuizInfoData)

        If QuizInfoData.Tables(0).Rows.Count = 0 Then
            NoQuizlbl.Text = "Sorry you did not attempt any quiz thus far."
        Else
            NoQuizlbl.Visible = False
        End If

        'creating connection between repeater and dataset
        QuizFeedRepeater.DataSource = QuizInfoData.Tables(0)
        QuizFeedRepeater.DataBind()
        oleDbCon.Close()
    End Sub

    Public Sub Unit1Sub(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        'create variables for Front End items
        Dim LessonsLabel As Label = CType(e.Item.FindControl("Lessonslbl"), Label)
        Dim LockUnlockImg As Image = CType(e.Item.FindControl("LockUnlockImage"), Image)


        'define variables for data items
        Dim row As DataRowView = CType(e.Item.DataItem, DataRowView)
        Dim lessonID_DB As String = row("lessonID").ToString()
        Dim lessonName_DB As String = row("lessonName").ToString()

        'join data values with front end items
        LessonsLabel.Text = "<a href=""Lesson.aspx?lessonID=" & lessonID_DB & """>" & lessonName_DB & "</a>"

        'create connection to database
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)
        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetCurrentIDSql As String = "SELECT * FROM [CurrentLesson] WHERE userName=@userName"
        Dim GetCurrentIDCmd As OleDbCommand = New OleDbCommand(GetCurrentIDSql, oleDbCon)
        'define parameters for SQL command
        GetCurrentIDCmd.Parameters.AddWithValue("@userName", User.Identity.Name)

        'create Adapter that grabs data from DB
        Dim GetCurrentIDAdapter As New OleDbDataAdapter

        'creates DataSet that stores captured DB data
        Dim GetCurrentIDData As New DataSet
        GetCurrentIDAdapter.SelectCommand = GetCurrentIDCmd
        GetCurrentIDAdapter.Fill(GetCurrentIDData)

        'create variables to store specific DataBase data
        Dim currentID = GetCurrentIDData.Tables(0).Rows(0).Item("currentLessonID").ToString

        'close database connection
        oleDbCon.Close()

        If Convert.ToInt32(lessonID_DB) > Convert.ToInt32(currentID) Then
            LockUnlockImg.ImageUrl = "images/lock-it-07-icon.png"
        Else
            LockUnlockImg.ImageUrl = "images/unlocked.png"
        End If
    End Sub

    Public Sub Unit2Sub(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        'create variables for Front End items
        Dim LessonsLabel As Label = CType(e.Item.FindControl("Lessonslbl2"), Label)
        Dim LockUnlockImg As Image = CType(e.Item.FindControl("LockUnlockImage2"), Image)


        'define variables for data items
        Dim row As DataRowView = CType(e.Item.DataItem, DataRowView)
        Dim lessonID_DB As String = row("lessonID").ToString()
        Dim lessonName_DB As String = row("lessonName").ToString()

        'join data values with front end items
        LessonsLabel.Text = "<a href=""Lesson.aspx?lessonID=" & lessonID_DB & """>" & lessonName_DB & "</a>"

        'create connection to database
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)
        'open database connection
        oleDbCon.Open()

        'creates SQL statement to obtain records
        Dim GetCurrentIDSql As String = "SELECT * FROM [CurrentLesson] WHERE userName=@userName"
        Dim GetCurrentIDCmd As OleDbCommand = New OleDbCommand(GetCurrentIDSql, oleDbCon)
        'define parameters for SQL command
        GetCurrentIDCmd.Parameters.AddWithValue("@userName", User.Identity.Name)

        'create Adapter that grabs data from DB
        Dim GetCurrentIDAdapter As New OleDbDataAdapter

        'creates DataSet that stores captured DB data
        Dim GetCurrentIDData As New DataSet
        GetCurrentIDAdapter.SelectCommand = GetCurrentIDCmd
        GetCurrentIDAdapter.Fill(GetCurrentIDData)

        'create variables to store specific DataBase data
        Dim currentID = GetCurrentIDData.Tables(0).Rows(0).Item("currentLessonID").ToString

        'close database connection
        oleDbCon.Close()

        If Convert.ToInt32(lessonID_DB) > Convert.ToInt32(currentID) Then
            LockUnlockImg.ImageUrl = "images/lock-it-07-icon.png"
        Else
            LockUnlockImg.ImageUrl = "images/unlocked.png"
        End If
    End Sub

    Public Sub QuizFeedSub(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        'create variables for Front End items
        Dim QuizFeedLabel As Label = CType(e.Item.FindControl("QuizFeedLbl"), Label)

        'define variables for data items
        Dim row As DataRowView = CType(e.Item.DataItem, DataRowView)
        Dim lessonName_DB As String = row("lessonName").ToString()
        Dim numberOfAttempts As String = row("numAttempts").ToString()
        Dim MaxScore As String = row("MaxScore").ToString()


        If String.Equals(lessonName_DB, "") Then
            QuizFeedLabel.Text = "Sorry you did not attempt any quiz thus far."
        Else
            'join data values with front end items
            QuizFeedLabel.Text = "<b>" & lessonName_DB & " quiz</b>:" & "<br />Attempted: " & numberOfAttempts & " times" &
                "<br />Max Score: " & MaxScore & "<br />Last attempt: "
        End If

        'process the date value into a statement and assign to label
        Dim dateTakenValue = DateDiff(DateInterval.Minute, row("lastdateTaken"), System.DateTime.Now)
        If dateTakenValue > 1439 Then
            QuizFeedLabel.Text &= dateTakenValue \ 1440 & " days ago"
        ElseIf dateTakenValue > 60 Then
            QuizFeedLabel.Text &= dateTakenValue \ 60 & " hours ago"
        Else
            QuizFeedLabel.Text &= dateTakenValue & " minutes ago"
        End If

    End Sub
End Class