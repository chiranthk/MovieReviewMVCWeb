<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master"
    CodeBehind="MovieReviewList.aspx.cs" Inherits="MovieReviewWeb.MovieReviewList" %>


<asp:Content ID="contentMain" runat="server" ContentPlaceHolderID="MainContent">
    <section>

        <table  style="width: auto">
            <tr>
                <td  style="width: auto">
                    <asp:ImageButton ID="lnkAddNewMovieReview" Width="5%" runat="server" ToolTip="Add Movie Review" ImageUrl="~/images/document_star.ico"
                      AlternateText="Add New Movie Review"   OnClick="lnkAddNewMovieReview_Click"></asp:ImageButton>
                </td>
            </tr>
            <tr>
                <td >
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text=""></asp:Label>


                    <asp:GridView ID="gvMovieReviews" runat="server" AutoGenerateColumns="False" OnRowCommand="gvMovieReviews_RowCommand"
                        OnRowEditing="gvMovieReviews_RowEditing" AllowPaging="True" OnSorting="gvMovieReviews_Sorting" AllowSorting="False"
                        OnPageIndexChanging="gvMovieReviews_PageIndexChanging" Width="100%">

                        <Columns>
                            <asp:BoundField HeaderText="Movie Review ID" SortExpression="MovieReviewID" DataField="MovieReviewID"
                                Visible="false" />
                            <asp:TemplateField HeaderText="Movie Name" ItemStyle-HorizontalAlign="Center"
                                SortExpression="Movie_Name">
                                <ItemTemplate>
                                    <asp:LinkButton ID="uxMovieReviewName" runat="server" CommandName="Edit" CommandArgument='<%# Bind("MovieReview_ID")%>'
                                        Text='<%# Bind("Movie_Name") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <ControlStyle />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Starcast" SortExpression="Starcast" DataField="Starcast"
                                ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Star Rating" SortExpression="StarRating" DataField="Star_Rating"
                                ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Release Date" SortExpression="Release_Date" DataField="Release_Date"
                                ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                               <asp:BoundField HeaderText="Thumbs Up"  SortExpression="ThumbsUpCount" DataField="ThumbsUpCount"
                                ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Thumbs Down"  SortExpression="ThumbsDownCount" DataField="ThumbsDownCount"
                                ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="4%">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/images/gnome_edit_delete.ico" OnClientClick="if (!window.confirm('Are you sure you want to delete this item?')) return false;"
                                        Width="22px" CommandArgument='<%# Bind("MovieReview_ID")%>' CommandName="Del" BackColor="#996600"
                                        BorderColor="#CC3300"></asp:ImageButton>
                                </ItemTemplate>
                                <ControlStyle BackColor="Gray" />
                                <ItemStyle HorizontalAlign="Center" Width="4%"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle BackColor="Silver" ForeColor="Black" />
                        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#FFCC66" ForeColor="Black" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                        <HeaderStyle BackColor="SlateGray"   Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle ForeColor="Black" BackColor="White" />

                    </asp:GridView>
                </td>
            </tr>
        </table>
    </section>

</asp:Content>

