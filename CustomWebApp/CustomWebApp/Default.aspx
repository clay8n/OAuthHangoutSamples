<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CustomWebApp._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>A list of employees meant for internal use only.</h1>
            </hgroup>
            <p>
                This application reads the list of QuickBooks employeees and displays it on a sample intranet page.  Since the OAuth connection is not necesary for non-admins, it is in a protected area.
            </p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h4>Once your administrator has enabled access to your company's QuickBooks file, you will be able to list a list of employees in a grid.</h4>
    <ol class="round">
        <li class="one">
            <h5>Create or login to your account</h5>
            <% if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
               { %>
            You're signed in!
            <% }
               else
               { %>
            <a href="/Account/Login">Sign In with Intuit</a> or <a href="/Account/Register">register</a> for an account.
            <% } %> 
        </li>
        <li class="star">
            <h5>See all employees in a grid</h5>
            <% if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
               { %>
            You must be signed in to view the employee list.
            <% }
               else if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated && CustomWebApp.RestHelper.appTokensSet() && CustomWebApp.IppRealmOAuthProfile.Read().accessToken.Length > 0)
               { %>
            <a href="EmployeeList">Click here to see all employees in a grid</a>!
            <% }
               else
               { %>
            Your application is not connected to a QuickBooks file.  Ask your administrator to connect to QuickBooks.
            <% } %> 
            
        </li>
        <br />
        <li class="questionmark">
            <h5>Need help setting up this demo application?</h5>
            The admin login is admin / intuit<br />
            <a href="RunningThisSample">View our Running this Sample page for more information.</a>
        </li>
    </ol>
</asp:Content>
