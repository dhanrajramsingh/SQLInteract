<%@ Page Title="" Language="vb" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Lesson.aspx.vb" Inherits="SQL_Interact.WebForm4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="IndexHF" runat="server" Value="0" />

    <asp:HiddenField ID="HFLessonID" runat="server" />

    <div class="row block01">
        <div class="col09">
        <section>
            <div class="lesson">
                <div class="title">
                    <asp:Label ID="LessonName" runat="server"></asp:Label>
                </div>
                <br />
                <div class="notes">
                    <asp:Label ID="DisplayLesson" runat="server"></asp:Label>
                </div>
            </div>
        </section>
        </div>
        <div class="col07">
            <asp:Label ID="Videolbl" runat="server" Text=""></asp:Label>
        </div>
    </div>

    <div class="row block02">
        <asp:Button ID="NextPagebtn" runat="server" Text="Next Lesson" 
            BackColor="Silver" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
        <br />
        <div class="col08">
            <asp:Panel ID="ActivityPanel" runat="server">
                <div class="heading">
                    <asp:Label ID="ActivityTitlelbl" runat="server"></asp:Label>
                </div>
                <br />
                <div class="task">
                    <b>Task:</b>
                    <asp:Label ID="ActivityInstructionlbl" runat="server"></asp:Label>
                    <br />
                </div>
                <div class="response">
                    Your Answer:
                    <br />
                    <asp:TextBox ID="UserInputtxt" runat="server" BorderColor="Black" 
                            BorderStyle="Solid" BorderWidth="1px" Rows="5" TextMode="MultiLine" 
                            ToolTip="Do not enter additional spaces in your response" 
                            Width="392px" BackColor="#D1D1A5" CssClass="textarea"></asp:TextBox>
                    <br />
                    <asp:Button ID="Submitbtn" runat="server" BackColor="Gray" 
                        BorderStyle="Solid" BorderWidth="1px" Text="Submit Answer" 
                        CssClass="button" BorderColor="#999999" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="NextQuestbtn" runat="server" BackColor="Gray" 
                        BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" Text="Next Question" 
                        Visible="False" CssClass="button" />
                    <br />
                </div>
                <div class="solution">
                    <asp:Label ID="Feedbacklbl" runat="server"></asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="ShowAnsbtn" runat="server" BackColor="Gray" 
                        BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CssClass="button" 
                        Text="Show Answer" ToolTip="Click to show answer for the current task." 
                        Visible="False" />
                    &nbsp;<br />
                    <br />
                    &nbsp;&nbsp;
                    <asp:TextBox ID="ShowAnstxt" runat="server" BackColor="#D1D1A5" 
                        BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" CssClass="textarea" 
                        ReadOnly="True" Rows="5" TextMode="MultiLine" Visible="False" Width="358px"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Button ID="TakeQuizbtn" runat="server" BackColor="Gray" 
                        BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" Text="Take Quiz" 
                        Visible="False" CssClass="button" />
                </div>
            </asp:Panel>
        
        </div>
        <div class="col08">
        
            <asp:Panel ID="ResultPanel" runat="server">
            </asp:Panel>
        
        </div>
    </div>
</asp:Content>
