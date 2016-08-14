Public Class WebForm3
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            HiddenField1.Value = 0
        End If
        Label1.Text = "Page load = " & HiddenField1.Value


    End Sub



    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label2.Text = "Submit Button click = " & HiddenField1.Value
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        HiddenField1.Value = HiddenField1.Value + 1
        Label3.Text = "Next Button click = " & HiddenField1.Value
    End Sub
End Class