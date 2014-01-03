<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MovieReviewPortal._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>Find more about India.</h2>
            </hgroup>
            <p>
                Browse through Recipes, Movie Reviews etc...
            </p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3>Get to know more about:</h3>
    <ol class="round">
        <li class="one">
            <h5>Upcoming Movies and Reviews</h5>
            
        </li>
        <li class="two">
            <h5>Hot and Spicy Recipes</h5>
        </li>
        <li class="three">
            <h5>Music</h5>
        </li>
    </ol>
</asp:Content>
