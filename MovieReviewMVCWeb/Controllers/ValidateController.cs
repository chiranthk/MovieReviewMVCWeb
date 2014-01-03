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

namespace MovieReviewMVCWeb.Controllers
{
    public class ValidateController : ApiController
    {
        private MovieReviewEntities db = new MovieReviewEntities();

        // GET api/Validate
        public IEnumerable<Person> GetPeople()
        {
            var people = db.People.Include(p => p.Role);
            return people.AsEnumerable();
        }

        // GET api/Validate/5
        public Person GetPerson(int id)
        {
            Person person = db.People.Find(id);
            if (person == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return person;
        }

        // PUT api/Validate/5
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

        // POST api/Validate
        public bool PostPerson(object jsonPerson)
        {
            if (ModelState.IsValid)
            {
                var isSuccess = false;
                
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                jsonPerson = jsonPerson.ToString().Replace('"', '\"');
                var person = serializer.Deserialize<Person>(jsonPerson as string);
                if (person == null)
                {
                    return false;
                }
                using (MovieReviewEntityManager manager = new MovieReviewEntityManager())
                {
                     isSuccess = manager.IsValidateUser(person.User_ID, person.Password);
                } 
                return isSuccess;
            }
            else
            {
                return false;
            }
        }

        // DELETE api/Validate/5
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