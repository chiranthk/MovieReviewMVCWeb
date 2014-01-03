using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
//using MovieReviewWeb.MovieReviewService;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using MovieReviewDataLayer;
using System.Text;

namespace MovieReviewWeb
{
    public partial class MovieReviewList : System.Web.UI.Page
    {
        public string SortExpression
        {
            get 
            {
                if (ViewState["SortExpression"] != null)
                    return ViewState["SortExpression"].ToString();
                else
                    return null;
            }
            set
            {
                ViewState["SortExpression"] = value;
            }
        }

        HttpClient client = new HttpClient();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            if (!IsPostBack)
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings.Get("MRAPIURL"));
                var bytes = Encoding.UTF8.GetBytes("ZgurJ420:73@vko1890");
                var base64 = Convert.ToBase64String(bytes);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                AuthenticationHeaderValue headerValue = new AuthenticationHeaderValue("Credentials", base64.ToString());
                client.DefaultRequestHeaders.Authorization = headerValue;
                BindMovieReviews();
            }
        }

        private void BindMovieReviews()
        {
            BindDataGrid();
        }

        private void BindDataGrid()
        {
            // List all products.
            HttpResponseMessage response = client.GetAsync(ConfigurationManager.AppSettings.Get("MRAPIURL") + "/api/mr").Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                var products = response.Content.ReadAsAsync<IEnumerable<MovieReview>>().Result;
                gvMovieReviews.DataSource = products.ToList();
                gvMovieReviews.DataBind();
            }
            else
            {
                lblMessage.Text = "Un-Authurized Call to the service.";
            }

            //using (MovieReviewEntityManager manager = new MovieReviewEntityManager())
            //{
            //      var reviews = manager.GetAllMovieReview();
            //      gvMovieReviews.DataSource = reviews;
            //      gvMovieReviews.DataBind();
            
            //}
        }

     

        
        protected void gvMovieReviews_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                // List all products.
                HttpResponseMessage response = client.DeleteAsync(ConfigurationManager.AppSettings.Get("MRAPIURL")+"/api/mr/" + Convert.ToInt32(e.CommandArgument) + "").Result;  // Blocking call!
                if (response.IsSuccessStatusCode)
                {
                    BindDataGrid();
                }
                else
                {
                    lblMessage.Text = "The Movie Review was not deleted. Please contact admin." ;
                }
               
                //using (MovieReviewServiceClient proxy = new MovieReviewServiceClient())
                //{
                //    ValidationResult result = proxy.DeleteMovieReview(Convert.ToInt32(e.CommandArgument));
                //    if (result.ResultType == ValidationResult.Result.Success)
                //    {
                //        lblMessage.Text = "The Movie Review is deleted successfully.";
                //        ViewState["DataCollection"] = null;
                //        BindMovieReviews();
                //    }
                //    else
                //    {
                //        lblMessage.Text = "The Movie Review was not deleted. Please contact admin. "+result.Message+"";
                //    }
                //}
            }
            if (e.CommandName == "Edit")
            {
                Response.Redirect("AddModifyMovieReview.aspx?MovieReviewID="+e.CommandArgument+"");
            }
        }

        protected void gvMovieReviews_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void lnkAddNewMovieReview_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("AddModifyMovieReview.aspx?MovieReviewID=0");
        }
        protected void gvMovieReviews_Sorting(object sender, GridViewSortEventArgs e)
        {
            List<MovieReview> movieReviews = ViewState["DataCollection"] as List<MovieReview>;
            if (ViewState["SortDirection"] == null || ViewState["SortDirection"] as string == SortDirection.Descending.ToString())
            {
                SortExpression = e.SortExpression;
                switch (e.SortExpression)
                {                    
                    case "MovieName":
                        var x = from movieReview in movieReviews
                                orderby movieReview.Movie_Name ascending
                                select movieReview;
                        movieReviews = x.ToList<MovieReview>();
                        break;
                    case "Starcast":
                        var x1 = from movieReview in movieReviews
                                orderby movieReview.Starcast ascending
                                select movieReview;
                        movieReviews = x1.ToList<MovieReview>();
                        break;
                    case "StarRating":
                        var x2 = from movieReview in movieReviews
                                orderby movieReview.Star_Rating ascending
                                select movieReview;
                        movieReviews = x2.ToList<MovieReview>();
                        break;
                    case "ReleaseDate":
                        var x3 = from movieReview in movieReviews
                                orderby movieReview.Release_Date ascending
                                select movieReview;
                        movieReviews = x3.ToList<MovieReview>();
                        break;
                    case "MovieReviewStatusTypeName":
                        var x4 = from movieReview in movieReviews
                                orderby movieReview.MovieReview_StatusType.MovieReview_StatusType_Name ascending
                                select movieReview;
                        movieReviews = x4.ToList<MovieReview>();
                        break;
                }

                ViewState["DataCollection"] = movieReviews;
                gvMovieReviews.DataSource = movieReviews;
                gvMovieReviews.DataBind();
                ViewState["SortDirection"] = SortDirection.Ascending.ToString();
            }
            else
            {

                switch (e.SortExpression)
                {
                    case "MovieName":
                        var x = from movieReview in movieReviews
                                orderby movieReview.Movie_Name descending
                                select movieReview;
                        movieReviews = x.ToList<MovieReview>();
                        break;
                    case "Starcast":
                        var x1 = from movieReview in movieReviews
                                 orderby movieReview.Starcast descending
                                 select movieReview;
                        movieReviews = x1.ToList<MovieReview>();
                        break;
                    case "StarRating":
                        var x2 = from movieReview in movieReviews
                                 orderby movieReview.Star_Rating descending
                                 select movieReview;
                        movieReviews = x2.ToList<MovieReview>();
                        break;
                    case "ReleaseDate":
                        var x3 = from movieReview in movieReviews
                                 orderby movieReview.Release_Date descending
                                 select movieReview;
                        movieReviews = x3.ToList<MovieReview>();
                        break;
                    case "MovieReviewStatusTypeName":
                        var x4 = from movieReview in movieReviews
                                 orderby movieReview.MovieReview_StatusType.MovieReview_StatusType_Name descending
                                 select movieReview;
                        movieReviews = x4.ToList<MovieReview>();
                        break;
                }

                ViewState["DataCollection"] = movieReviews;
                gvMovieReviews.DataSource = movieReviews;
                gvMovieReviews.DataBind();
                ViewState["SortDirection"] = SortDirection.Descending.ToString();
            
            }
        }

       protected void gvMovieReviews_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMovieReviews.PageIndex = e.NewPageIndex;
            BindMovieReviews();
        }



      

       
    }
}
