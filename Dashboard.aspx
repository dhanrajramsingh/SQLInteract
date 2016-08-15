<%@ Page Title="" Language="vb" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Dashboard.aspx.vb" Inherits="SQL_Interact.WebForm2" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!---------------------------------Display how long ago account was created----------------------------->
    <div class="row block01">
        <asp:HiddenField ID="UserNameHF" runat="server" />
        <asp:HiddenField ID="LessonIDHF" runat="server" />
        <div class="col16">
            <div class="dashboardsection">
                <div class="dashboard" style="text-align: center; margin-bottom: 8px">
                    <p>ACCOUNT CREATED</p>
                </div>
                <div class="dashboarddata" style="text-align: center">
                    <asp:Label ID="DateCreatedlbl" runat="server"></asp:Label>
                </div>            
            </div>
        </div>
    </div>
    <!---------------------------------Display overall course progress----------------------------->
    <div class="row block02">
        <div class="col02"></div>
        <div class="col13">
             <div class="dashboard" style="text-align: center; margin-bottom: 20px">
                    <p>OVERALL COURSE PROGRESS</p>
             </div>
            <div class="w3-progress-container w3-round-xlarge">
                <asp:Label ID="Progresslbl" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div class="col02"></div>
    </div>
    <!-----------------------------------Display unit progress------------------------------>
    <div class="row block02">
        <div class="col02"></div>
        <div class="col13">
             <div class="dashboard" style="text-align: center; margin-bottom: 20px">
                    <p>UNIT PROGRESS</p>
             </div>
            <div class="w3-progress-container w3-round-xlarge">
                <asp:Label ID="UnitProgresslbl" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <div class="col02"></div>
    </div>
    <!--------------------------------------------Quiz History row------------------------------------>
    <div class="row block03">
        <div class="dashboard" style="text-align: center; margin-bottom: 8px;margin-top:20px">
                <p>QUIZ HISTORY</p>
        </div>
        <div class="warning">
            <asp:Label ID="Feedbacklbl" runat="server" Text=""></asp:Label>
        </div>
        <!---------------------------------Drop Down List to choose lesson name----------------------------->
        <div class="col08" style="padding-left: 15px">
            <div class="datatable_title">
                <span style="font-family:'Times New Roman' , Times, serif;font-size:1.4em;color:#5F4125;margin-right: 10px;">
                    <asp:Label ID="ChooseLessonlbl" runat="server" Text="Choose Quiz Lesson:"></asp:Label>
                </span>
                <asp:DropDownList ID="LessonNameDDL" runat="server" 
                    DataSourceID="CompletedLessons" DataTextField="lessonName" 
                    DataValueField="lessonID" AutoPostBack="True" TabIndex="1" Height="27px" 
                    Width="158px">
                </asp:DropDownList>
                <asp:AccessDataSource ID="CompletedLessons" runat="server" DataFile="~/App_Data/SQLInteractDB.mdb" 
                    SelectCommand="SELECT [lessonName], [lessonID] FROM [CompletedLessons] WHERE ([userName] = ?) AND [lessonID]&lt;&gt;1">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="UserNameHF" Name="userName" PropertyName="Value" Type="String" />
                    </SelectParameters>
                </asp:AccessDataSource>
            </div>
            <asp:AccessDataSource ID="QuizHistory" runat="server" DataFile="~/App_Data/SQLInteractDB.mdb" 
                SelectCommand="SELECT [attemptOrder], [score] FROM [QuizHistory] WHERE (([userName] = ?) AND ([lessonID] = ?))">
                <SelectParameters>
                    <asp:ControlParameter ControlID="UserNameHF" Name="userName" 
                        PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="LessonNameDDL" Name="lessonID" 
                        PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
            </asp:AccessDataSource>
            <br />
            <!---------------------------------Display Quiz History for chosen lesson----------------------------->
            <asp:Panel ID="QuizHistoryPanel" runat="server">
                <div class="datatable">
                    <div style="overflow-x:auto;">
                        <table class="table" style="font-size:1.2em; width: 95%">
                            <tr>
                                <th>Attempt</th>
                                <th>Score</th>
                                <th>Date Taken</th>
                            </tr>
                            <asp:Repeater ID="QuizHistoryRepeater" runat="server" 
                                OnItemDataBound="QuizHistorySub">
                            <ItemTemplate>
                                <tr>
                                    <td><asp:Label ID="AttemptNolbl" runat="server" Text=""></asp:Label></td>
                                    <td><asp:Label ID="Scorelbl" runat="server" Text=""></asp:Label></td>
                                    <td><asp:Label ID="DateTakenlbl" runat="server" Text=""></asp:Label></td>
                                </tr>
                            </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
            </asp:Panel>   
        </div>
        <!-------------------Display quiz progress over attempts made using a bar chart----------------------->
        <div class="col07" style="padding-left: 40px">
            <asp:Chart ID="QuizHistoryChart" runat="server" DataSourceID="QuizHistory" 
                Palette="Grayscale" Width="350px">
                <Series>
                    <asp:Series Name="Series1" XValueMember="attemptOrder" YValueMembers="score" 
                        ChartArea="Quiz History">
                    </asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="Quiz History">
                        <AxisY Title="Score %">
                            <MinorGrid Enabled="True" LineDashStyle="Dash" />
                            <MinorTickMark Enabled="True" />
                        </AxisY>
                        <AxisX Title="Quiz Attempt">
                            <MinorGrid Enabled="True" LineDashStyle="Dash" />
                            <MinorTickMark Enabled="True" />
                        </AxisX>
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
        </div>
    </div>
    <div class="row block03">
        <div class="col16" style="padding-left: 15px">
            <div class="datatable_title">
                <span style="font-family:'Times New Roman' , Times, serif;font-size:1.4em;color:#5F4125;margin-right: 10px;">
                    <asp:Label ID="QuizAttemptlbl" runat="server" Text="Choose Quiz Attempt Number:"></asp:Label>
                <asp:DropDownList ID="QuizAttemptDLL" runat="server" AutoPostBack="True" 
                    DataSourceID="QuizAttemptOrder" DataTextField="attemptOrder" 
                    DataValueField="attemptOrder">
                </asp:DropDownList>
                <asp:AccessDataSource ID="QuizAttemptOrder" runat="server" 
                    DataFile="~/App_Data/SQLInteractDB.mdb" 
                    SelectCommand="SELECT [attemptOrder] FROM [QuizAttemptOrder] WHERE (([userName] = ?) AND ([lessonID] = ?))">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="UserNameHF" Name="userName" 
                            PropertyName="Value" Type="String" />
                        <asp:ControlParameter ControlID="LessonIDHF" Name="lessonID" 
                            PropertyName="Value" Type="Int32" />
                    </SelectParameters>
                </asp:AccessDataSource>
                </span>
            </div>
                <asp:AccessDataSource ID="AttemptOrder" runat="server" 
                    DataFile="~/App_Data/SQLInteractDB.mdb" 
                    
                SelectCommand="SELECT [attemptOrder] FROM [QuizAttemptOrder] WHERE (([userName] = ?) AND ([lessonID] = ?))">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="UserNameHF" Name="userName" 
                            PropertyName="Value" Type="String" />
                        <asp:ControlParameter ControlID="LessonIDHF" Name="lessonID" 
                            PropertyName="Value" Type="Int32" DefaultValue="" />
                    </SelectParameters>
                </asp:AccessDataSource>
                <br />
                <asp:Panel ID="QuizAttemptPanel" runat="server">
                <div class="datatable">
                    <div style="overflow-x:auto;">
                        <table class="table" style="font-size:1.0em; width: 95%">
                            <tr>
                                <th>Question Order</th>
                                <th>Question</th>
                                <th>Response</th>
                                <th>Status</th>
                            </tr>
                            <asp:Repeater ID="QuizAttemptRepeater" runat="server" 
                                OnItemDataBound="QuizAttemptSub">
                            <ItemTemplate>
                                <tr>
                                    <td><asp:Label ID="QuestionOrderlbl" runat="server" Text=""></asp:Label></td>
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
        </div>
    </div>
    <!--------------------------Display Lessons completed and Lessons still to complete----------------------->
    <div class="row block02">
         <div class="dashboard" style="text-align: center; margin-bottom: 8px; margin-top: 15px">
            <p>LESSONS</p>
        </div>
    <div class="col02"></div>
    <div class="col06">
        <asp:Panel ID="panel1" runat="server" Height="300px" Width="100%"  ScrollBars="Vertical">
            <asp:GridView ID="CompletedGridView" runat="server" 
            DataSourceID="LessonsCompleted" AutoGenerateColumns="False">
            
            <Columns>
                <asp:BoundField DataField="LESSONS COMPLETED" HeaderText="LESSONS COMPLETED" 
                    SortExpression="LESSONS COMPLETED" />
            </Columns>
            </asp:GridView>
        
            <asp:AccessDataSource ID="LessonsCompleted" runat="server" 
            DataFile="~/App_Data/SQLInteractDB.mdb" 
            SelectCommand="SELECT [lessonName] AS [LESSONS COMPLETED] FROM [CompletedLessons] WHERE ([userName] = ?)">
            <SelectParameters>
                <asp:ControlParameter ControlID="UserNameHF" Name="userName" 
                    PropertyName="Value" Type="String" />
            </SelectParameters>
            </asp:AccessDataSource>
        </asp:Panel>
    </div>
    <div class="col06">
            <asp:Panel ID="panelContainer" runat="server" Height="300px" Width="100%"  ScrollBars="Vertical">
            <asp:GridView ID="NotCompletedGridView" runat="server" 
            AutoGenerateColumns="False" DataSourceID="NotCompleted">
            
            <Columns>
                <asp:BoundField DataField="LESSONS NOT COMPLETED" HeaderText="LESSONS NOT COMPLETED" 
                    SortExpression="LESSONS NOT COMPLETED" />
            </Columns>
            </asp:GridView>
        
        <asp:AccessDataSource ID="NotCompleted" runat="server" 
            DataFile="~/App_Data/SQLInteractDB.mdb" 
            
            SelectCommand="SELECT [lessonName] AS[LESSONS NOT COMPLETED] FROM [LessonsNotCompleted] WHERE ([userName] = ?)">
            <SelectParameters>
                <asp:ControlParameter ControlID="UserNameHF" Name="userName" 
                    PropertyName="Value" Type="String" />
            </SelectParameters>
        </asp:AccessDataSource>
        </asp:Panel>
    </div>
    <div class="col02"></div>
    </div>
</asp:Content>
