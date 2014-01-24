<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RunningThisSample.aspx.cs" Inherits="CustomWebApp._RunningThisSample" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3>Running this sample application:</h3>
    <ol class="round">
        <li class="one">
            <h5>Configure your IPP application</h5>
            <a href="Configure">View the app settings</a> required on developer.intuit.com to test subscription and connection flows.
        </li>
        <li class="two">
            <h5>Set your keys</h5>
            <% if (CustomWebApp.RestHelper.appTokensSet())
               { %>
            Your keys are set!
            <% }
               else
               { %>
            <a href="ApplicationKeys">Set your application keys</a> in the web.config.
            <% } %> 
        </li>
    </ol>
</asp:Content>
