Imports System.Data.OleDb
Imports System.Configuration
Imports System.Data

Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        CheckLessonIDHF.Value = Request.Params("LessonCheck")
        If String.Equals(CheckLessonIDHF.Value, "1") Then
            LessonFeedbacklbl.Visible = True
            LessonFeedbacklbl.Text = "This lesson you clicked is not accessible until all preceding lessons are completed!"
        Else
            LessonFeedbacklbl.Visible = False
        End If

        ScrollBarHF.Value = Request.Params("ScrollPos")


        'create connection to database
        Dim oleDbCon As New OleDbConnection(ConfigurationManager.ConnectionStrings("SQLInteractDB").ConnectionString)
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

    End Sub

End Class