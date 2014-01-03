using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Membership.OpenAuth;
using MovieReviewDataLayer;

namespace MovieReviewPortal.Account
{
    public partial class Register : Page
    {
        HttpClient client = new HttpClient();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings.Get("MRAPIURL"));
                client.DefaultRequestHeaders.Accept.Add(
                     new MediaTypeWithQualityHeaderValue("application/json"));

            }
            RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {
            FormsAuthentication.SetAuthCookie(RegisterUser.UserName, createPersistentCookie: false);

            string continueUrl = RegisterUser.ContinueDestinationPageUrl;
            if (!OpenAuth.IsLocalUrl(continueUrl))
            {
                continueUrl = "~/";
            }
            Response.Redirect(continueUrl);
        }

        private Person SetPerson()
        {
            Person p1 = new Person();

            p1.Country = "india";
            p1.DOB = Convert.ToDateTime("Apr 16, 1996");// DateTime.Now; // "Apr 16, 1996";
            p1.First_Name = "Parmeshwar1";
            p1.Gender = "M";
            p1.Home_Phone = "222222222";
            p1.Is_Locked = "F";
            p1.Last_Name = "ahemad22";
            p1.MaxInvalidAttempts = 0;
            p1.Middle_Name = "";
            p1.Mobile_Phone = "";
            p1.Modified_Date = DateTime.Now;
            p1.Modified_User = "sadas";
            p1.Password = "qwert";
            p1.Role_ID = 2;
            p1.User_ID = "mannu.ims@gmail.com";
            return p1;
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            try
            {

                Person person = SetPerson();
                //Person p = CreatePersonObject();
                var jsonObj = new JavaScriptSerializer().Serialize(person);

                client.PostAsJsonAsync(ConfigurationManager.AppSettings.Get("MRAPIURL") + "/api/Account/", person)
         .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
                
                //lblMessage.Text = "Movie Review Saved Successfully.";
                //   Response.Redirect("MovieReviewList.aspx");
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Movie Review was not saved. Please try again. " + ex.InnerException.ToString();
            }
        }
    }
}