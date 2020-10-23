<%@ Page Title="" Language="C#" MasterPageFile="~/Miniportal.Master" AutoEventWireup="true" CodeBehind="UserExists.aspx.cs" Inherits="Es.Udc.DotNet.MiniPortal.Web.Pages.UserExists" meta:resourcekey="PageResource2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_MenuWelcome" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_MenuExplanation" runat="server">
    <asp:HyperLink ID="lnkMainPage" runat="server" NavigateUrl="~/Pages/MainPage.aspx" meta:resourcekey="lnkMainPageResource1">MainPage</asp:HyperLink> - <asp:Label ID="lblSearchUser" runat="server" Text="<%$Resources: , lblExplanation_txt %>"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_MenuLinks" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_BodyContent" runat="server">
    <form id="formUserExists" runat="server">
        <asp:Label ID="lblUserName" runat="server" meta:resourcekey="lblUserNameResource1"></asp:Label>
        <br />
        <asp:TextBox ID="txtUserName" runat="server" meta:resourcekey="txtUserNameResource1"></asp:TextBox>
        <br />
        <asp:Label ID="lblUserExists" runat="server" meta:resourcekey="lblUserExistsResource1" Visible="False"></asp:Label>
        <br />
        <asp:Label ID="lblUserNotExists" runat="server" meta:resourcekey="lblUserNotExistsResource1" Visible="False"></asp:Label>
        <br />
        <asp:Button ID="btnUserExists" runat="server" OnClick="btnUserExists_Click" Text="<%$Resources: Common , searchButton %>" />
    </form>
</asp:Content>