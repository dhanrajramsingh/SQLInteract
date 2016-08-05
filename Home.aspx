<%@ Page Title="SQL INTERACT: HOME" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Home.aspx.vb" Inherits="SQL_Interact._Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <!--------------Slideshow--------------->

<section class="featured">
	<div class="rslides_container">
		<ul class="rslides" id="slider">
			<li><img src="images/slide1.jpg" alt="Image not found"/></li>
			<li><img src="images/slide2.jpg" alt="Image not found"/></li>
			<li><img src="images/slide3.jpg"alt="Image not found"/></li>
		</ul>
	</div>
</section>

<div class="row block02">
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
    <div id="main-content" class="col-2-3">
        <div class="heading">
            <h1>COURSE BREAKDOWN:</h1>
        </div>
        <div class="content">    
            <asp:AccessDataSource ID="UNIT1" runat="server" 
                DataFile="~/App_Data/SQLInteractDB.mdb" SelectCommand="SELECT * FROM [Unit1_Lessons]">
            </asp:AccessDataSource>

            <asp:AccessDataSource ID="UNIT2" runat="server" 
                DataFile="~/App_Data/SQLInteractDB.mdb" SelectCommand="SELECT * FROM [Unit2_Lessons]">
            </asp:AccessDataSource>

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
                            <asp:Repeater ID="Repeater1" runat="server" DataSourceID="UNIT1" >
                            <ItemTemplate>
                                <a href="Lesson.aspx?lessonID=<%# Eval("lessonID") %>"> <%# Eval("lessonName")%> </a> <br />
                            </ItemTemplate>
                            </asp:Repeater>
                        </Content>
                    </ajaxToolkit:AccordionPane>
        
                    <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                        <Header>
                            <div>UNIT 2: AGGREGATE FUNCTIONS</div>
                        </Header>
                        <Content>
                            <asp:Repeater ID="Repeater2" runat="server" DataSourceID="UNIT2">
                            <ItemTemplate>
                                <a href="Lesson.aspx?lessonID=<%# Eval("lessonID") %>"> <%# Eval("lessonName")%> </a> <br />
                            </ItemTemplate>
                            </asp:Repeater>
                        </Content>
                    </ajaxToolkit:AccordionPane>
                </Panes>
            </ajaxToolkit:Accordion>
        </div>
    </div>
    
    <div id="sidebar" class="col-1-3">
		<section>
			<div class="heading"><h2>Course Progress</h2></div>
			<div class="content">
				<div class="w3-progress-container w3-round-xlarge">
                    <div class="w3-progressbar w3-blue w3-round-xlarge" style="width:50%">
                        <div class="w3-center w3-text-white">50%</div>
                    </div>
                </div>
			</div>
		</section>
    </div>
</div>

</asp:Content>
