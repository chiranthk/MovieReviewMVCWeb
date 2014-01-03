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
    public class MRScoreController : ApiController
    {
        private MovieReviewEntities db = new MovieReviewEntities();

        // GET api/MRScore
        public IEnumerable<MovieReviewScore> GetMovieReviewScores()
        {
            var moviereviewscores = db.MovieReviewScores.Include(m => m.MovieReviewID).Include(m => m.PersonID);
            return moviereviewscores.AsEnumerable();
        }

        // GET api/MRScore/5
        public MovieReviewScore GetMovieReviewScore(int id)
        {
            MovieReviewScore moviereviewscore = db.MovieReviewScores.Find(id);
            if (moviereviewscore == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return moviereviewscore;
        }

        // PUT api/MRScore/5
        public HttpResponseMessage PutMovieReviewScore(int id, MovieReviewScore moviereviewscore)
        {
            if (ModelState.IsValid && id == moviereviewscore.MovieReviewScoreID)
            {
                db.Entry(moviereviewscore).State = EntityState.Modified;

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

        // POST api/MRScore
        public HttpResponseMessage PostMovieReviewScore(object jsonMoviereviewscore)
        {
            if (ModelState.IsValid)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                jsonMoviereviewscore = jsonMoviereviewscore.ToString().Replace('"', '\"');
                var moviereviewscore = serializer.Deserialize<MovieReviewScore>(jsonMoviereviewscore as string);
                    
                db.MovieReviewScores.Add(moviereviewscore);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, moviereviewscore);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = moviereviewscore.MovieReviewScoreID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/MRScore/5
        public HttpResponseMessage DeleteMovieReviewScore(int id)
        {
            MovieReviewScore moviereviewscore = db.MovieReviewScores.Find(id);
            if (moviereviewscore == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.MovieReviewScores.Remove(moviereviewscore);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, moviereviewscore);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}