<%@ Page Title="Admin Area" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CustomWebApp._Admin_Default" %>


<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
        <h2>Application Settings.</h2>
    </hgroup>
    <p class="message-info">
        Below we allow the administrator of the application to authorize a connection to a QuickBooks Online company, or enter tokens directly from the <a href="https://appcenter.intuit.com/Playground/OAuth/IA" target="_blank">IPP Playground</a>.
    </p>
    <h3>Use 1 of the following methods to connect this application to a single QuickBooks account.</h3>
    <ol class="round">
        <li class="one">
            <h5>Authorize a connection to your QuickBooks company inside your custom web application.</h5>
            <br />
            <% if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated && CustomWebApp.RestHelper.appTokensSet() && CustomWebApp.IppRealmOAuthProfile.Read().accessToken.Length > 0)
               { %>
            You're connected!  <a href="ManageConnection">Manage your connection</a>.
            <% }
               else
               { %>
            <a href="ManageConnection">Connect a QuickBooks company file</a> to this application using three-legged OAuth (Connect to QuickBooks button).
            <% } %><br />
        </li>
    </ol>
    <br />
    <ol class="round">
        <li class="two">
            <h5>Use the IPP Playground to authorize access and enter your tokens directly below.</h5>
            <% if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated && CustomWebApp.RestHelper.appTokensSet() && CustomWebApp.IppRealmOAuthProfile.Read().accessToken.Length > 0)
               { %><br />
            You're connected!  <a href="ManageConnection">Manage your connection</a>.
            <% }
               else
               { %>
            <p>
                <asp:Panel Visible="false" ID="errorMessage" runat="server" ForeColor="Red">An error occurred.  Please try again.</asp:Panel>
                Access Token:
                <asp:TextBox ID="accessToken" TextMode="Password" runat="server" /><br />
                Access Secret:
                <asp:TextBox ID="accessSecret" TextMode="Password" runat="server" /><br />
                Realm ID:
                <asp:TextBox Width="150" ID="realmId" TextMode="Password" runat="server" /><br />
                Data Source:
                <asp:DropDownList runat="server" ID="dataSource" Font-Size="Large" >
                    <asp:ListItem Selected="True">QBO</asp:ListItem>
                    <asp:ListItem>QBD</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Button runat="server" Text="Save" ID="saveButton" OnClick="saveButton_Click" />
            </p>
            <% } %><br />
        </li>
    </ol>
</asp:Content>
