using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using MovieReviewDataLayer;
using System.Runtime.Serialization;
using System.Web.Helpers;
using System.Text;
namespace MovieReviewMVCWeb.Controllers
{
    public class AccountController : ApiController
    {
        private MovieReviewEntities db = new MovieReviewEntities();

        // GET api/Account
        public IEnumerable<Person> GetPeople()
        {
            var people = db.People.AsEnumerable();
            return people;
        }

        // GET api/Account/5
        public Person GetPerson(int id)
        {
            Person person = db.People.Find(id);
            if (person == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return person;
        }



        // PUT api/Account/5
        public HttpResponseMessage PutPerson(int id, Person person)
        {
            if (ModelState.IsValid && id == person.Person_ID)
            {
                db.Entry(person).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        } 

      
        // POST api/Account
         
        public HttpResponseMessage PostPerson(object jsonPerson)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    jsonPerson = jsonPerson.ToString().Replace('"', '\"');
                    HttpResponseMessage response = null;
                    MovieReviewEntityManager manager = new MovieReviewEntityManager();
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var person = serializer.Deserialize<Person>(jsonPerson as string);

                    if (person == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.PartialContent, "Data is incomplete.");
                        response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = 0 }));
                        return response;
                    }
                    if (person != null)
                    { 
                        // check if the user_id is already registered or not
                        if (manager.IsUserIDDuplicate(person.User_ID))
                        { 
                            response = Request.CreateResponse(HttpStatusCode.Accepted,"This User ID already exists.");
                            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = 0 }));
                            return response;
                        }
                    }
                    
                    
                    person.Password = MovieReviewDataLayer.Crypto.Encrypt(person.Password, true);
                    db.People.Add(person);
                    db.SaveChanges();

                    response = Request.CreateResponse(HttpStatusCode.Created, person);
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = person.Person_ID }));
                    return response;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception EX)
            {
                return null;
            }
        }

        // POST api/Account
         
        public HttpResponseMessage PostPerson( string jsonPerson)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var person = (Person)serializer.DeserializeObject(jsonPerson);

                    MovieReviewEntityManager manager = new MovieReviewEntityManager();
                    person.Password = MovieReviewDataLayer.Crypto.Encrypt(person.Password, true);
                    db.People.Add(person);
                    db.SaveChanges();

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, person);
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = person.Person_ID }));
                    return response;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception EX)
            {
                return null;
            }
        }

        // DELETE api/Account/5
        public HttpResponseMessage DeletePerson(int id)
        {
            Person person = db.People.Find(id);
            if (person == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.People.Remove(person);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, person);
        }

        

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}