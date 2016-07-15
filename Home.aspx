<%@ Page Title="SQL INTERACT: HOME" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Home.aspx.vb" Inherits="SQL_Interact._Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <!--------------Slideshow--------------->

<section class="featured">
	<div class="rslides_container">
		<ul class="rslides" id="slider">
			<li><img src="images/slide1.jpg"/></li>
			<li><img src="images/slide2.jpg"/></li>
			<li><img src="images/slide3.jpg"/></li>
		</ul>
	</div>
</section>

<div class="row block02">
	<div class="col16">
		<section>
			<div class="heading">
                <h2>
                    Welcome <asp:Label ID="Userlbl" runat="server" Text=""></asp:Label> to SQL INTERACT!
                </h2>
            </div>
		</section>
	</div>
</div>

    
<p>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>    

    <ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="0"
        HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
        ContentCssClass="accordionContent" FadeTransitions="false" FramesPerSecond="40"
        TransitionDuration="250" AutoSize="None" RequireOpenedPane="false" SuppressHeaderPostbacks="true">
        <Panes>
            <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
        <Header>
            <div>UNIT 1: Data Definition</div>
        </Header>
        <Content>
            <a href="Introduction.aspx">Introduction</a><br />
            <a href="Introduction.aspx">Statements</a><br />
            <a href="Introduction.aspx">Create Statement</a><br />
            <a href="Introduction.aspx">Insert Statement</a><br />
            <a href="Introduction.aspx">Select Statement</a><br />
            <a href="Introduction.aspx">Update Statement</a><br />
        </Content>
        </ajaxToolkit:AccordionPane>
        
        <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
        <Header>
            <div>UNIT 2: Single Table Queries</div>
        </Header>
        <Content>
            <a href="Introduction.aspx">Select-1</a><br />
            <a href="Introduction.aspx">Select-2</a><br />
            <a href="Introduction.aspx">Select Distinct</a><br />
            <a href="Introduction.aspx">Where Clause</a><br />
        </Content>
        </ajaxToolkit:AccordionPane>
    </Panes>
    </ajaxToolkit:Accordion>
</p>
</asp:Content>
