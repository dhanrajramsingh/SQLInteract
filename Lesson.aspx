<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Lesson.aspx.vb" Inherits="SQL_Interact.WebForm4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="HFLessonID" runat="server" />
    <asp:Label ID="LessonName" runat="server"></asp:Label>
    <br />
    <asp:Label ID="DisplayLesson" runat="server"></asp:Label>
</asp:Content>
