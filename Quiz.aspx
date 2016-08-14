<%@ Page Title="" Language="vb" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Quiz.aspx.vb" Inherits="SQL_Interact.WebForm5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row block02">
        <div class="col16">
            <asp:HiddenField ID="LessonIDHF" runat="server" />
            <asp:HiddenField ID="IndexHF" runat="server" />
            <asp:HiddenField ID="NumCorrectHF" runat="server"/>
            <asp:HiddenField ID="QuestionNumberHF" runat="server" />
            <asp:HiddenField ID="MaxIndexHF" runat="server" />
            <asp:HiddenField ID="QuizIDHF" runat="server" />
            <asp:HiddenField ID="TotalQuestionsHF" runat="server" />
            <asp:Panel ID="QuizIntroPanel" runat="server">
                <div class="alert" style="text-align:center; font-size: 1.3em">
                    <p>Be aware if you decide to navigate away from this page during the quiz,<br />
                       any data submitted will not be saved! Click "OK" to confirm and display the quiz. </p><br />
                    <asp:Button ID="ConfirmButton" runat="server" Text="OK" CssClass="button" style="margin:auto"/>
                </div>
            </asp:Panel>
            <asp:Panel ID="QuizPanel" runat="server" Visible="False">
                <div class="heading">
                    <asp:Label ID="QuizTitlelbl" runat="server" />
                </div>
                <br />
                <div style="padding-left:26px;font-size: 1.1em;color: #555555;">
                    <asp:Image ID="QuizImage" runat="server" style="width: 90%" />
                    <br />
                    <p>
                        With reference to the above table, 
                        <b><asp:Label ID="TableNamelbl" runat="server" Text="" /></b>
                    </p>
                </div>
                <br />
                <div class="task" style="margin-top: 10px">
                    <b><asp:Label ID="QuestionNumberlbl" runat="server"></asp:Label></b>
                    <p><asp:Label ID="QuizQuestionlbl" runat="server" /></p>
                    <br />
                </div>
                <div class="col10" style="padding-left: 18px;">
                    <span style="font-size: 1.3em;color: #555555;"><p>Quiz Progress:</p></span>
                    <div class="w3-progress-container w3-round-xlarge">
                        <asp:Label ID="Progresslbl" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <div class="response">
                    <br />
                    <br />
                    <br />
                    Your Answer:
                    <br />
                    <div style="overflow-x:auto;height:auto;width:95%">
                        <asp:TextBox ID="UserInputtxt" runat="server" BorderColor="Black" 
                            BorderStyle="Solid" BorderWidth="1px" Rows="5" TextMode="MultiLine" 
                            ToolTip="Do not enter additional spaces in your response" 
                            Width="280px" BackColor="#D1D1A5" CssClass="textarea" Height="77px"></asp:TextBox>
                    </div>
                    <br />
                    <asp:Button ID="Submitbtn" runat="server" BackColor="Gray" 
                        BorderStyle="Solid" BorderWidth="1px" Text="Submit Answer" 
                        CssClass="button" BorderColor="#999999" />
                    <br />
                    <br />
                    <asp:Panel ID="QuizSummaryPanel" runat="server" Visible="False">
                        <asp:Label ID="SummaryTitlelbl" runat="server" Text="Quiz Summary:"></asp:Label>
                        <br />
                        <div class="datatable">
                            <div style="overflow-x:auto;">
                            <table class="table" style="font-size:0.85em">
                                <tr>
                                    <th>Question Number</th>
                                    <th>Question</th>
                                    <th>Your Response</th>
                                    <th>Response Status</th>
                                </tr>
                                <asp:Repeater ID="QuizReportRepeater" runat="server" 
                                        OnItemDataBound="QuizReportSub">
                                <ItemTemplate>
                                <tr>
                                    <td><asp:Label ID="QuestionNolbl" runat="server" Text=""></asp:Label></td>
                                    <td><asp:Label ID="Questionlbl" runat="server" Text=""></asp:Label></td>
                                    <td><asp:Label ID="Responselbl" runat="server" Text=""></asp:Label></td>
                                    <td><asp:Label ID="Statuslbl" runat="server" Text=""></asp:Label></td>
                                </tr>
                                </ItemTemplate>
                                </asp:Repeater>
                            </table>
                            </div>
                        </div>
                    </asp:Panel>
                    <br />
                    <asp:Label ID="Feedbacklbl" runat="server"></asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="NextLessonbtn" runat="server" BackColor="Gray" 
                        BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" Text="Next Lesson" 
                        Visible="False" CssClass="button" />
                </div>
            </asp:Panel>
       </div>
    </div>
</asp:Content>
