using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
//using MovieReviewWeb.MovieReviewService;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net.Http.Formatting;
using MovieReviewDataLayer;
using System.Globalization;
using System.Net;
using System.Text;

namespace MovieReviewPortal
{
    public partial class AddModifyMovieReview : System.Web.UI.Page
    {

        HttpClient client = new HttpClient();
        public int MovieReviewID
        {
            get
            {
                if (Request.QueryString["MovieReviewID"] != null)
                    return Convert.ToInt32(Request.QueryString["MovieReviewID"].ToString());
                else
                    return 0;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //txtReviewText.Attributes.Add("onkeyup", "checkMaxLength( " + txtReviewText.ClientID + " ,'4000')");

            if (!IsPostBack)
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings.Get("MRAPIURL"));
                client.DefaultRequestHeaders.Accept.Add(
                     new MediaTypeWithQualityHeaderValue("application/json"));

                BindLanguages();

                // Bind all the data to the controls.
                BindAllControls();

            }
        }

        private void BindAllControls()
        {
            MovieReview review = null;


            if (MovieReviewID > 0)
            {
                // List all products.
                HttpResponseMessage response = client.GetAsync(ConfigurationManager.AppSettings.Get("MRAPIURL") + "/api/mr/" + MovieReviewID + "").Result;  // Blocking call!
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    review = response.Content.ReadAsAsync<MovieReview>().Result;
                }
                if (review != null)
                {
                    txtMovieName.Text = review.Movie_Name;

                    if (review.Language != null)
                        ddlLanguages.SelectedItem.Value = review.Language_ID.ToString();

                    if (review.Release_Date.HasValue)
                        txtReleaseDate.Text = review.Release_Date.Value.ToShortDateString();
                    txtStarCast.Text = review.Starcast;
                    txtMusic.Text = review.Music;
                    txtGeneration.Text = review.Generation;
                    txtReviewText.Content = review.Review_Text;
                    txtStarrating.Text = review.Star_Rating.ToString();
                    txtTrailerLink.Text = review.Trailer_Link;
                    if (!string.IsNullOrEmpty(review.Banner))
                        BannerLabel.Text = "Image Already Exists " + review.Banner + " . You can replace if required.";
                    if (!string.IsNullOrEmpty(review.ImageOne))
                        FileNameOne.Text = "Image Already Exists " + review.ImageOne + " . You can replace if required.";
                    if (!string.IsNullOrEmpty(review.ImageTwo))
                        FileNameTwo.Text = "Image Already Exists " + review.ImageTwo + " . You can replace if required.";
                    if (!string.IsNullOrEmpty(review.ImageThree))
                        FileNameThree.Text = "Image Already Exists " + review.ImageThree + " . You can replace if required.";
                    if (!string.IsNullOrEmpty(review.ImageFour))
                        FileNameFour.Text = "Image Already Exists " + review.ImageFour + " . You can replace if required.";
                    if (!string.IsNullOrEmpty(review.ImageFive))
                        FileNameFive.Text = "Image Already Exists " + review.ImageFive + " . You can replace if required.";
                }
            }

        }



        private void BindLanguages()
        {
            using (MovieReviewEntityManager MRManager = new MovieReviewEntityManager())
            {
                ddlLanguages.DataSource = MRManager.GetAllLanguages();
                ddlLanguages.DataTextField = "Language_Name";
                ddlLanguages.DataValueField = "Language_ID";
                ddlLanguages.DataBind();
            }
        }

        protected void btnAddMovieReview_Click(object sender, EventArgs e)
        {
            MovieReview review = SetMovieReview();

            try
            {
                if (review.MovieReview_ID <= 0)
                {
                    client.PostAsJsonAsync(ConfigurationManager.AppSettings.Get("MRAPIURL") + "/api/mr/", review)
         .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
                }
                else
                {
                    client.PutAsJsonAsync(ConfigurationManager.AppSettings.Get("MRAPIURL") + "/api/mr/" + review.MovieReview_ID + "", review)
         .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
                }
                lblMessage.Text = "Movie Review Saved Successfully.";
                //   Response.Redirect("MovieReviewList.aspx");
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Movie Review was not saved. Please try again. " + ex.InnerException.ToString();
            }
        }

        private MovieReview SetMovieReview()
        {
            string uploadError = string.Empty;             
            string fileNameSaved = string.Empty;
            bool isFileSaved = false;
            MovieReview review = new MovieReview();
            if (MovieReviewID != 0)
            {
                // List all products.
                HttpResponseMessage response = client.GetAsync(ConfigurationManager.AppSettings.Get("MRAPIURL") + "/api/mr/" + MovieReviewID + "").Result;  // Blocking call!
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body. Blocking!
                    review = response.Content.ReadAsAsync<MovieReview>().Result;
                }
            }
            review.Movie_Name = txtMovieName.Text.Trim();

            review.Language_ID = Convert.ToInt32(ddlLanguages.SelectedItem.Value);
            review.Person_ID = (Session["PersonID"] != null) ? Convert.ToInt32(Session["PersonID"]) : 1;


            if (txtReleaseDate.Text != string.Empty)
            {
                review.Release_Date = ParseDate(txtReleaseDate.Text);
            }
            review.Starcast = txtStarCast.Text.Trim();
            review.Music = txtMusic.Text.Trim();
            review.Generation = txtGeneration.Text.Trim();

            isFileSaved = WriteFileToDisk(fileUploadBanner.PostedFile, Guid.NewGuid(), out  uploadError, out fileNameSaved);
            if (isFileSaved)
            {
                review.Banner = fileNameSaved;
            } 

            // Write all Images now .
            if (!string.IsNullOrEmpty(ImageOneFileUpload.FileName))
                SetMovieImage(ImageOneFileUpload, ref uploadError, ref fileNameSaved, ref isFileSaved, review, 1);
            if (!string.IsNullOrEmpty(ImageTwoFileUpload.FileName))
                SetMovieImage(ImageTwoFileUpload, ref uploadError, ref fileNameSaved, ref isFileSaved, review, 2);
            if (!string.IsNullOrEmpty(ImageThreeFileUpload.FileName))
                SetMovieImage(ImageThreeFileUpload, ref uploadError, ref fileNameSaved, ref isFileSaved, review, 3);
            if (!string.IsNullOrEmpty(ImageFourFileUpload.FileName))
                SetMovieImage(ImageFourFileUpload, ref uploadError, ref fileNameSaved, ref isFileSaved, review, 4);
            if (!string.IsNullOrEmpty(ImageFiveFileUpload.FileName))
                SetMovieImage(ImageFiveFileUpload, ref uploadError, ref fileNameSaved, ref isFileSaved, review, 5);

            review.Review_Text = txtReviewText.Content;
            if (!string.IsNullOrEmpty(txtStarrating.Text))
                review.Star_Rating = Convert.ToInt32(txtStarrating.Text);
            review.Trailer_Link = txtTrailerLink.Text.Trim();
            review.Modified_User = (Session["UserID"] != null) ? Session["UserID"].ToString() : "SysAdmin";
            review.Modified_Date = DateTime.Now;
            return review;
        }

        private static DateTime ParseDate(string input)
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.ShortDatePattern = "MM/dd/yyyy";
            dtfi.DateSeparator = "/";
            DateTime objDate = Convert.ToDateTime(input, dtfi);
            return objDate;

        }


        private void SetMovieImage(FileUpload uploadControl, ref string uploadError, ref string fileLocator, ref bool isFileSaved, MovieReview review, int displayOrder)
        {
            isFileSaved = WriteFileToDisk(uploadControl.PostedFile, Guid.NewGuid(), out  uploadError, out fileLocator);

            if (isFileSaved)
            {
                switch (displayOrder)
                {
                    case 1: review.ImageOne = fileLocator; break;
                    case 2: review.ImageTwo = fileLocator; break;
                    case 3: review.ImageThree = fileLocator; break;
                    case 4: review.ImageFour = fileLocator; break;
                    case 5: review.ImageFive = fileLocator; break;

                    default:
                        break;
                }

            }
        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {

            MovieReview review = SetMovieReview();
            review.MovieReview_StatusType_ID = 1;

            try
            {
                if (review.MovieReview_ID <= 0)
                {
                    client.PostAsJsonAsync(ConfigurationManager.AppSettings.Get("MRAPIURL") + "/api/mr/", review)
         .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
                }
                else
                {
                    client.PutAsJsonAsync(ConfigurationManager.AppSettings.Get("MRAPIURL") + "/api/mr/" + review.MovieReview_ID + "", review)
         .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
                }
                lblMessage.Text = "Movie Review Saved and Published Successfully.";

            }
            catch (Exception ex)
            {
                lblMessage.Text = "Movie Review was not Saved/Published. Please try again. " + ex.InnerException.ToString();
            }



            #region Old Code
            ////ValidationResult result = null;
            //string uploadError = string.Empty;
            //string fileLocator = string.Empty;
            //bool isFileSaved = false;
            //MovieReview review = new MovieReview();
            //if (MovieReviewID != 0)
            //{
            //    review.MovieReview_ID = MovieReviewID;
            //}
            //review.Movie_Name = txtMovieName.Text.Trim();
            //review.LanguageID = Convert.ToInt32(ddlLanguages.SelectedItem.Value);
            //review.PersonID = 1;
            //if(txtReleaseDate.Text != string.Empty)
            //    review.ReleaseDate = Convert.ToDateTime(txtReleaseDate.Text);
            //review.Starcast = txtStarCast.Text.Trim();
            //review.Music = txtMusic.Text.Trim();

            //if(fileUploadBanner.PostedFile.FileName != string.Empty)
            //    isFileSaved = WriteFileToDisk(fileUploadBanner.PostedFile, Guid.NewGuid(), out  uploadError, out fileLocator);

            //if (isFileSaved)
            //{
            //    review.Banner = fileLocator;
            //}
            //else
            //{
            //    review.Banner = "";
            //}

            //review.ReviewText = txtReviewText.Text.Trim();
            //review.StarRating = txtStarrating.Text;
            //review.TrailerLink = txtTrailerLink.Text.Trim();
            //review.ModifiedUser = "chira";

            //using (MovieReviewServiceClient proxy = new MovieReviewServiceClient())
            //{
            //    result = proxy.PublishMovieReview(review);
            //}
            //if (result.ResultType == ValidationResult.Result.Success)
            //{
            //    lblMessage.Text = "Movie Review Saved and Published Successfully.";
            //}
            //else
            //{
            //    lblMessage.Text = "Movie Review was not Saved/Published. Please try again. " + result.Message;
            //}

            #endregion Old Code.
        }

        private static void ConvertToDateTime(string value)
        {
            DateTime convertedDate;
            try
            {
                convertedDate = Convert.ToDateTime(value);
                Console.WriteLine("'{0}' converts to {1} {2} time.",
                                  value, convertedDate,
                                  convertedDate.Kind.ToString());
            }
            catch (FormatException)
            {
                Console.WriteLine("'{0}' is not in the proper format.", value);
            }
        }

        protected void btnMovieList_Click(object sender, EventArgs e)
        {
            Response.Redirect("MovieReviewList.aspx");
        }

        /// <summary>
        /// Write the File to the Hard Disk to the Upload Directory.
        /// </summary>
        /// <param name="uploadedFile">File to be uploaded.</param>
        /// <param name="guid">New Guid.</param>
        /// <param name="errorMessage">Validation error message.</param>
        /// <param name="fileLocator">Uploaded File Locator.</param>
        /// <param name="isReviewGuidelineDoc">If uploading a Review Guideline Document.</param>
        /// <returns>Result True or False</returns>
        public bool WriteFileToDisk(HttpPostedFile uploadedFile, Guid guid, out string errorMessage, out string fileNameSaved)
        {
            bool isSaved = false;
            errorMessage = string.Empty;
            string fileLocator = string.Empty;
            fileNameSaved = string.Empty;
            if ((uploadedFile != null))
            {
                if (uploadedFile.ContentLength < 0 || uploadedFile.FileName == string.Empty)
                    return false;

                string sFileDir = ConfigurationManager.AppSettings["UploadDirectory"].ToString();
               
                //determine file name
                string sFileName = System.IO.Path.GetFileName(uploadedFile.FileName);
                string sFileExtension = System.IO.Path.GetExtension(uploadedFile.FileName);
                fileNameSaved = guid + sFileExtension;

                fileLocator = sFileDir + "/" + guid + sFileExtension;

                try
                {
                    //Save File on disk
                    //uploadedFile.SaveAs(sFileDir + "\\" + guid + sFileExtension);
                    isSaved = ftpTransfer(uploadedFile,fileLocator);

                    isSaved = true;
                }
                catch (Exception)//in case of an error
                {
                    isSaved = false;
                    errorMessage = "An Error Occured. Please Try Again!";
                    DeleteFile(sFileDir + "\\" + guid + sFileExtension);
                }

            }
            return isSaved;
        }

        public bool ftpTransfer(HttpPostedFile file,string fileLocator)
        {
            try
            {
                string ftpAddress = ConfigurationManager.AppSettings.Get("FTPURL"); ;
                string username = ConfigurationManager.AppSettings.Get("FTPUserName");
                string password = ConfigurationManager.AppSettings.Get("FTPPassword");

                Stream stream = file.InputStream;

                Byte[] buffer = new Byte[file.ContentLength];
                stream.Read(buffer, 0, buffer.Length);

                string ftpUrl = string.Format("{0}/{1}", ftpAddress, fileLocator);
                FtpWebRequest request = FtpWebRequest.Create(ftpUrl) as FtpWebRequest;
                
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(username, password);
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(buffer, 0, buffer.Length);
                reqStream.Close();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Delete the file from the Location.
        /// </summary>
        /// <param name="fileName"></param>
        public bool DeleteFile(string fileName)
        {
            try
            {
                //Delete file from the server
                if (fileName.Trim().Length > 0)
                {
                    FileInfo fi = new FileInfo(fileName);
                    if (fi.Exists)//if file exists delete it
                    {
                        fi.Delete();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }




        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            //txtReleaseDate.Text = Calendar1.SelectedDate.ToShortDateString();
        }

    }
}