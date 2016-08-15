<%@ Page Title="SQL INTERACT: HOME" Language="vb" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Home.aspx.vb" Inherits="SQL_Interact._Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript">
    $(document).ready(function () {
        var pos = $('.ypos input[type=hidden]').val();
        if (pos > 0) {
            $(window).scrollTop(document.body.scrollHeight);
        }
        else {
            $(window).scrollTop(0);
        }
    });
    </script>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <!--------------Slideshow--------------->

<section class="featured">
	<div class="rslides_container">
		<ul class="rslides" id="slider">
			<li><img src="images/slide1.jpg" alt="Image not found"/></li>
			<li><img src="images/slide2.jpg" alt="Image not found"/></li>
			<li><img src="images/slide3.jpg"alt="Image not found"/></li>
            <li><img src="images/Learn SQL.png"alt="Image not found"/></li>
            <li><img src="images/Learning.jpg"alt="Image not found"/></li>
		</ul>
	</div>
</section>

<div class="row block02">
    <asp:HiddenField ID="CheckLessonIDHF" runat="server" />
    <div class="col16">
        <section>
		    <div class="heading">
                    <h1>
                        Welcome <asp:Label ID="Userlbl" runat="server" Text=""></asp:Label> to SQL INTERACT!
                    </h1>
            </div>
        </section>
    </div>
</div>

<div class="row block03">
    <div id="main-content" class="col09">
        <div class="courseheading">
            <h1>COURSE BREAKDOWN:</h1>
        </div>
        
        <div class="content">    
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>    

            <ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="0"
            HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent" FadeTransitions="false" FramesPerSecond="40"
            TransitionDuration="250" AutoSize="None" RequireOpenedPane="false" 
            SuppressHeaderPostbacks="true">
                <Panes>
                    <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                        <Header>
                            <div>UNIT 1: SINGLE TABLE QUERIES</div>
                        </Header>
                        <Content>
                            <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Unit1Sub">
                            <ItemTemplate>
                                <div class="col07">
                                    <div class="repeaterelement">
                                        <span style=" float:left"><asp:Label ID="Lessonslbl" runat="server" Text=""></asp:Label></span>
                                        <span style=" float:right"><asp:Image ID="LockUnlockImage" runat="server" Width="35px" Height="35px" /></span>
                                    </div>
                                </div>
                            </ItemTemplate>
                            </asp:Repeater>
                        </Content>
                    </ajaxToolkit:AccordionPane>
        
                    <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                        <Header>
                            <div>UNIT 2: AGGREGATE FUNCTIONS</div>
                        </Header>
                        <Content>
                            <asp:Repeater ID="Repeater2" runat="server" OnItemDataBound="Unit2Sub">
                            <ItemTemplate>
                                <div class="col07">
                                    <div class="repeaterelement">
                                        <span style=" float:left"><asp:Label ID="Lessonslbl2" runat="server" Text=""></asp:Label></span>
                                        <span style=" float:right"><asp:Image ID="LockUnlockImage2" runat="server" Width="35px" Height="35px" /></span>
                                    </div>
                                </div>
                            </ItemTemplate>
                            </asp:Repeater>
                        </Content>
                    </ajaxToolkit:AccordionPane>
                </Panes>
            </ajaxToolkit:Accordion>
            <br />
            
            <div class="ypos">
                <asp:HiddenField ID="ScrollBarHF" runat="server" />
            </div>
            
            <asp:Panel ID="WarningPanel" runat="server" Visible="False">
                <div class="alert">
                    <span class="closebtn" onclick="this.parentElement.style.display='none';">&times;</span>
                    <asp:Label ID="LessonFeedbacklbl" runat="server" style="font-size:1.3em" />
                </div>
            </asp:Panel>
        </div>
    </div>
    
    <div id="sidebar" class="col07">
		<section>
			<div class="heading"><h2>Course Progress</h2></div>
			<div class="content">
				<div class="w3-progress-container w3-round-xlarge">
                    <asp:Label ID="Progresslbl" runat="server" Text=""></asp:Label>
                </div>
			</div>
		</section>

        <section>
			<div class="heading"><h2>Unit Progress</h2></div>
			<div class="content">
				<div class="w3-progress-container w3-round-xlarge">
                    <asp:Label ID="UnitProgresslbl" runat="server" Text=""></asp:Label>
                </div>
			</div>
		</section>

        <section>
			<div class="heading"><h2>Your current lesson is:</h2></div>
			<div class="content">
            <div class="courseheading">
                <asp:Label ID="CurrentLessonNamelbl" runat="server" Text=""></asp:Label>
            </div>
			</div>
		</section>

        <section>
			<div class="heading">
                <h2>Quiz Feed:</h2></div>
			    <div class="content" style="font-size: 1.3em">
                    <asp:Repeater ID="QuizFeedRepeater" runat="server" OnItemDataBound="QuizFeedSub">
                        <ItemTemplate>
                            <asp:Label ID="QuizFeedLbl" runat="server" Text=""></asp:Label><br /><br />
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Label ID="NoQuizlbl" runat="server"></asp:Label>
                </div>
		</section>
    </div>
</div>

</asp:Content>
