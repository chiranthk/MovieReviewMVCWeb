using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using MovieReviewDataLayer;


namespace MovieReviewPortal.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterHyperLink.NavigateUrl = "Register.aspx";
            
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                //RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            using (MovieReviewEntityManager mrManager = new MovieReviewEntityManager())
            {
                //string encryptedPassword = Crypto.EncryptStringAES(MovieReviewLogin.Password, "moviereview");
                Person person = mrManager.ValidateUser(MovieReviewLogin.UserName, MovieReviewLogin.Password);
                if (person != null)
                {
                    Session["PersonID"] = person.Person_ID;
                    Session["UserID"] = person.User_ID;
                    MovieReviewLogin.FailureText = "Login credentials are incorrect. Please try again.";
                    FormsAuthenticationTicket tkt;
                    string cookiestr;
                    HttpCookie ck;
                    tkt = new FormsAuthenticationTicket(1, MovieReviewLogin.UserName, DateTime.Now,
              DateTime.Now.AddMinutes(30), MovieReviewLogin.RememberMeSet, "your custom data");
                    cookiestr = FormsAuthentication.Encrypt(tkt);
                    ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
                    if (MovieReviewLogin.RememberMeSet)
                        ck.Expires = tkt.Expiration;
                    ck.Path = FormsAuthentication.FormsCookiePath;
                    Response.Cookies.Add(ck);

                    //string strRedirect;
                    //strRedirect = Request["ReturnUrl"];
                    //if (strRedirect == null)
                    //    strRedirect = "default.aspx";
                    //Response.Redirect(strRedirect, true);
                                        
                    Response.Redirect("~/MovieReviewList.aspx",true);
                }
                else
                {
                    MovieReviewLogin.FailureText = "User-ID or Password is incorrect. Please enter the correct ones.";
                    Response.Redirect("Login.aspx", true);
                }
            }
        }
    }

}