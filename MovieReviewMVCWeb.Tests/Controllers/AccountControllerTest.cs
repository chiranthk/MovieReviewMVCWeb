using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieReviewDataLayer;
using MovieReviewMVCWeb.Controllers;

namespace MovieReviewMVCWeb.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        HttpClient client = new HttpClient();

        [TestMethod]
        public void GetPerson()
        {

            AccountController con = new AccountController();
            Person p = con.GetPerson(1);
        }

        [TestMethod]
        public void RegisterPostJSONCall()
        {
           // var baseAddress = "http://mrservice.allthatsindian.com/api/account/";
           var baseAddress = "http://localhost:8816/api/account/";
              
            // New way of doing 
            Person p = CreatePersonObject();
            var jsonObj = new JavaScriptSerializer().Serialize(p);
            string str = @jsonObj;

            string test = "{\"First_Name\":\"Parmeshwar\",\"Middle_Name\":\"\",\"Last_Name\":\"ahemad\",\"User_ID\":\"mannu.ims@gmail.com\",\"Home_Phone\":\"nil\",\"Mobile_Phone\":\"\",\"Gender\":\"M\",\"DOB\":\"\\/Date(829638000000)\\/\",\"MaxInvalidAttempts\":0,\"Is_Locked\":\"F\",\"Role_ID\":2,\"Modified_User\":\"sadas\",\"Modified_Date\":\"\\/Date(1387783608886)\\/\",\"Country\":\"india\",\"Password\":\"qwert\",\"Person_ID\":0,\"Role\":null}";
            //string withoutescape = "{"First_Name":"Parmeshwar","Middle_Name":"","Last_Name":"ahemad","User_ID":"mannu.ims@gmail.com","Home_Phone":"nil","Mobile_Phone":"","Gender":"M","DOB":"\/Date(829638000000)\/","MaxInvalidAttempts":0,"Is_Locked":"F","Role_ID":2,"Modified_User":"sadas","Modified_Date":"\/Date(1387730388574)\/","Country":"india","Password":"qwert","Person_ID":0,"Role":null}";
            
            str = str.ToString().Replace('"', '\"');
          //  string withoutslash = "{\"First_Name\":\"Parmeshwar\",\"Middle_Name\":\"\",\"Last_Name\":\"ahemad\",\"User_ID\":\"mannu.ims@gmail.com\",\"Home_Phone\":\"nil\",\"Mobile_Phone\":\"\",\"Gender\":\"M\",\"DOB\":\"\/Date(829638000000)\/\",\"MaxInvalidAttempts\":0,\"Is_Locked\":\"F\",\"Role_ID\":2,\"Modified_User\":\"sadas\",\"Modified_Date\":\"\/Date(1387730388574)\/\",\"Country\":\"india\",\"Person_ID\":0,\"Role\":null}";

            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

            client.PostAsJsonAsync(baseAddress, str)
                 .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
             
        }

        [TestMethod]
        public void RegisterTestPostCall()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://mrservice.allthatsindian.com/api/account/");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                Person p = CreatePersonObject();

                //MovieReview mr = SetMovieReview();
                var jsonObj = new JavaScriptSerializer().Serialize(p);

                streamWriter.Write(jsonObj);
                streamWriter.Flush();
                streamWriter.Close();


                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }

        }



        [TestMethod]
        public void RegisterTest()
        {
            AccountController con = new AccountController();
            Person p = new Person();
            p.DOB = DateTime.Today;
            p.First_Name = "testregisterfn";
            p.Last_Name = "testregln";
            p.Mobile_Phone = "2221112222";
            p.Password = "password123";// Crypto.EncryptStringAES("password999", "moviereview");
            p.Role_ID = 1;
            p.Gender = "M";
            p.Country = "united states";
            p.Modified_Date = DateTime.Now;
            p.Modified_User = "chiranthk";
            p.User_ID = "testregister";
            p.Is_Locked = "N";
            p.Person_ID = 90;

            Person p1 = CreatePersonObject();
            //p1.Person_ID = 95;

            //con.PostPerson(p1);
        }

        private static Person CreatePersonObject()
        {
            Person p1 = new Person();

            p1.Country = "india";
            p1.DOB = Convert.ToDateTime("Apr 16, 1996");// DateTime.Now; // "Apr 16, 1996";
            p1.First_Name = "Parmeshwar";
            p1.Gender = "M";
            p1.Home_Phone = "nil";
            p1.Is_Locked = "F";
            p1.Last_Name = "ahemad";
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

        private static Person CreateUserNamePasswordObject()
        {
            Person p1 = new Person();

            p1.Password = "qwertss";
            
            p1.User_ID = "mannu.ims@gmail.com";
            return p1;
        }

        

        [TestMethod]
        public void SendEmail()
        {
            AccountController con = new AccountController();
            Person person = CreatePersonObject();
            
             
        }

        private MovieReview SetMovieReview()
        {
            MovieReview review = new MovieReview();
            review.Movie_Name = "";

            review.Language_ID = 1;
            review.Person_ID = 1;


            review.Starcast = "AAMIR";
            review.Music = "ARREH";
            review.Generation = "AAA";

            review.Banner = "";


            review.Review_Text = "TEST";
            review.Star_Rating = 4;
            review.Trailer_Link = "www.utube.com";
            review.Modified_User = "Me";
            review.Modified_Date = DateTime.Now;
            return review;
        }

        [TestMethod]
        public void MRInsertTest()
        {

            //var baseAddress = "http://mrservice.allthatsindian.com/api/account/";
            var baseAddress = "http://localhost:8816/api/mr/";
            // New way of doing 
            MovieReview mr = SetMovieReview();
             var jsonObj = new JavaScriptSerializer().Serialize(mr);

            // Create the JSON formatter.
            MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();
 
            // Use the JSON formatter to create the content of the request body.
            HttpContent content = new ObjectContent<MovieReview>(mr, jsonFormatter);

            //client.DefaultRequestHeaders.Accept.Add(
            //        new MediaTypeWithQualityHeaderValue("application/json"));

            client.PostAsync(baseAddress, content)
                 .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
             
        }

        [TestMethod]
        public void LoginTest()
        {
            //var baseAddress = "http://mrservice.allthatsindian.com/api/Validate/";
            var baseAddress = "http://localhost:8816/api/Validate/";

            // New way of doing 
            Person p = CreateUserNamePasswordObject();
            var jsonObj = new JavaScriptSerializer().Serialize(p);

            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));


            ValidateController cont = new ValidateController();
             var resp = cont.PostPerson(jsonObj);
            client.PostAsJsonAsync(baseAddress, jsonObj)
                 .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
             
        }

        [TestMethod]
        public void TestMRScoreInserts()
        {
            MRScoreController cont = new MRScoreController();
            var baseAddress = "http://mrservice.allthatsindian.com/api/MRScore/";
            //var baseAddress = "http://localhost:8816/api/MRScore/";

            // New way of doing 
            MovieReviewScore score = CreateMRSCore();
            var jsonObj = new JavaScriptSerializer().Serialize(score);

            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
             
            client.PostAsJsonAsync(baseAddress, jsonObj)
                 .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
 
         }

        [TestMethod]
        public void SearchMR()
        {
            MRScoreController cont = new MRScoreController();
            //var baseAddress = "http://mrservice.allthatsindian.com/api/MRSearch/";
            var baseAddress = "http://localhost:8816/api/MRSearch/";

            // New way of doing 
            MovieReview review = new MovieReview();
            review.Movie_Name = "Dil";

            var jsonObj = new JavaScriptSerializer().Serialize(review);

            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

             client.PostAsJsonAsync(baseAddress, jsonObj)
                 .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
        }
        [TestMethod]
        public void SearchMRGET()
        {
            MRSearchController cont = new MRSearchController();
            MovieReview review = new MovieReview();
            review.Movie_Name = "Dil";
            var jsonObj = new JavaScriptSerializer().Serialize(review);


            var coll =  cont.PostMovieReview(jsonObj);
        }


        private static MovieReviewScore CreateMRSCore()
        {
            MovieReviewScore score = new MovieReviewScore();
            score.DeviceID = "CKIPHONE123";
            score.MovieReviewID = 13;
            score.PersonID = 1;
            score.ThumbsUp = true;
            return score;
        }
    }
}
