<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AddModifyMovieReview.aspx.cs" Inherits="MovieReviewPortal.AddModifyMovieReview" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc2" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit.HTMLEditor" tagprefix="cc1" %>


<asp:Content ContentPlaceHolderID="MainContent" ID="contentMain" runat="server">
    
          <%--  <script type='text/javascript'>
                $(function () {
                    $('#txtReleaseDate').datepicker({ dateFormat: 'mm-dd-yy' })
                });
    </script>--%>
       <script type="text/javascript" >
           $(document).ready(function () {
               $("#<%=txtReleaseDate.ClientID %>").datepicker();

                        });
</script>
  

    <div >
        <h2 style="font-style: italic"><font color="Black" face="georgia">
            Add New Movie Review</h2></font>
          
        <table >
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" ></asp:Label>
                   
                </td>
            </tr>
            <tr>
                <th  >
                    <asp:Label ID="lblMovieName" Font-Names="Georgia" runat="server" Text="Movie Name" 
                        CssClass="box-content"></asp:Label>
                </th>
                <td >
                    <asp:TextBox ID="txtMovieName" runat="server" Width="300px" MaxLength="250"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RFVName" runat="server" 
                        ControlToValidate="txtMovieName"  ForeColor="Red"
                        Font-Bold="True" Font-Italic="True" SetFocusOnError="True">Please enter a Movie Name</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th >
                    <asp:Label ID="lblLanguage" Font-Names="Georgia" runat="server" Text="Language"></asp:Label>
                </th>
                <td >
                    <asp:DropDownList runat="server" ID="ddlLanguages" Width="155px">
                    </asp:DropDownList>
               
                </td>
            </tr>
            <tr>
                <th >
                    <asp:Label ID="lblReleaseDate"  Font-Names="Georgia" runat="server" Text="Release Date(Please select a date MM/DD/YYYY from Calendar control.) "></asp:Label>
                </th>
                <td >
                  <asp:TextBox ID="txtReleaseDate"     runat="server" Width="300px" MaxLength="50"></asp:TextBox>
    
                  
                </td>
                
            </tr>
            <tr>
                <th >
                    <asp:Label ID="lblStarCast" Font-Names="Georgia" runat="server" Text="Star Cast"></asp:Label>
                </th>
                <td >
                    <asp:TextBox ID="txtStarCast" runat="server"   Width="300px" MaxLength="1000"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th >
                    <asp:Label ID="lblMusic" Font-Names="Georgia" runat="server" Text="Music"></asp:Label>
                </th>
                <td >
                    <asp:TextBox ID="txtMusic" runat="server" MaxLength="1000" Width="300px"></asp:TextBox>
                </td>
            </tr>
             <tr>
                <th >
                    <asp:Label ID="lblGeneration" Font-Names="Georgia" runat="server" Text="Generation"></asp:Label>
                </th>
                <td >
                    <asp:TextBox ID="txtGeneration" runat="server" MaxLength="255" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th >
                    <asp:Label ID="lblBanner" Font-Names="Georgia" runat="server" Text="Banner"></asp:Label>
                </th>
                <td >
                    <asp:FileUpload ID="fileUploadBanner" runat="server" CssClass="button" />
                    <asp:Label ID="BannerLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th >
                    <asp:Label ID="lblReviewText" Font-Names="Georgia" runat="server" Text="Review Text"></asp:Label>
                </th>
                <td  >
                  <%--  <asp:TextBox ID="txtReviewText" TextMode="MultiLine" runat="server" 
                        Width="98%" Height="414px"></asp:TextBox>--%>
                  <cc1:Editor runat="server" ID="txtReviewText"
        Height="300px" 
        Width="100%"
        AutoFocus="true"
/>
       
                </td>
            </tr>
            <tr>
                <th >
                    <asp:Label ID="lblStarRating" runat="server" Text="Star Rating(0-5)"></asp:Label>
                </th>
                <td >
                    <asp:TextBox ID="txtStarrating" runat="server" MaxLength="3" Width="50px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtStarrating" ForeColor="Red" ErrorMessage="Please enter valid rating from 0-5" ValidationExpression="^\d{0,2}"  runat="server" ></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <th >
                    <asp:Label ID="lblTrailerLink"  runat="server" Text="Trailer Link"></asp:Label>
                </th>
                <td >
                    <asp:TextBox ID="txtTrailerLink" MaxLength="1000" runat="server" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th >
                    <asp:Label ID="lblImageOne" Font-Names="Georgia" runat="server" Text="Image One"></asp:Label>
                </th>
                <td >
                    <asp:FileUpload ID="ImageOneFileUpload" runat="server" CssClass="button" />
                    <asp:Label ID="FileNameOne" runat="server"></asp:Label>
                </td>
            </tr>
             <tr>
                <th >
                    <asp:Label ID="Label1" Font-Names="Georgia" runat="server" Text="Image Two"></asp:Label>
                </th>
                <td >
                    <asp:FileUpload ID="ImageTwoFileUpload" runat="server" CssClass="button" />
                <asp:Label ID="FileNameTwo" runat="server"></asp:Label>
                </td>
            </tr>
             <tr>
                <th >
                    <asp:Label ID="Label2" Font-Names="Georgia" runat="server" Text="Image Three"></asp:Label>
                </th>
                <td >
                    <asp:FileUpload ID="ImageThreeFileUpload" runat="server" CssClass="button" />
                    <asp:Label ID="FileNameThree" runat="server"></asp:Label>
                </td>
            </tr>
             <tr>
                <th >
                    <asp:Label ID="Label3" Font-Names="Georgia" runat="server" Text="Image Four"></asp:Label>
                </th>
                <td >
                    <asp:FileUpload ID="ImageFourFileUpload" runat="server" CssClass="button" />
                    <asp:Label ID="FileNameFour" runat="server"></asp:Label>
                </td>
            </tr>
             <tr>
                <th >
                    <asp:Label ID="Label4" Font-Names="Georgia" runat="server" Text="Image Five"></asp:Label>
                </th>
                <td >
                    <asp:FileUpload ID="ImageFiveFileUpload" runat="server" CssClass="button" />
                    <asp:Label ID="FileNameFive" runat="server"></asp:Label>
                </td>
            </tr>
           
        </table>
        <div >
                    <asp:Button ID="btnAddMovieReview" CssClass="button" runat="server" Text="Save Movie Review" OnClick="btnAddMovieReview_Click" />
                    <asp:Button ID="btnPublish" runat="server" CssClass="button" 
            Text="Publish Movie Review" onclick="btnPublish_Click" />
                    <asp:Button ID="btnMovieList" runat="server" CssClass="button" 
            Text="Back to Movie Review List"  CausesValidation="false" onclick="btnMovieList_Click" />
         </div>
    </div>
     
</asp:Content>
