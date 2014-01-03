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
    public class MRSearchController : ApiController
    {
        private MovieReviewEntities db = new MovieReviewEntities();

        // GET api/MRSearch
        public IEnumerable<MovieReview> GetMovieReviews()
        {
            var moviereviews = db.MovieReviews.Include(m => m.Language).Include(m => m.MovieReview_StatusType).Include(m => m.Person);
            return moviereviews.AsEnumerable();
        }

        // GET api/MRSearch/5
        public MovieReview GetMovieReview(int id)
        {
            MovieReview moviereview = db.MovieReviews.Find(id);
            if (moviereview == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return moviereview;
        }

        // PUT api/MRSearch/5
        public HttpResponseMessage PutMovieReview(int id, MovieReview moviereview)
        {
            if (ModelState.IsValid && id == moviereview.MovieReview_ID)
            {
                db.Entry(moviereview).State = EntityState.Modified;

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

        // POST api/MRSearch
        public IEnumerable<MovieReview> PostMovieReview(object mrSearchJson)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<MovieReview> moviereviews = null;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                mrSearchJson = mrSearchJson.ToString().Replace('"', '\"');
                var movie = serializer.Deserialize<MovieReview>(mrSearchJson as string);
                if (movie != null)
                    moviereviews = db.MovieReviews.Include(m => m.Language).Include(m => m.MovieReview_StatusType).Include(m => m.Person).Where(m => m.MovieReview_StatusType_ID == 1 && m.Movie_Name.Contains(movie.Movie_Name));
                if (moviereviews == null || moviereviews.Count() <= 0) return null;
                foreach (MovieReview mr in moviereviews)
                {
                    mr.ThumbsUpCount = mr.MovieReviewScores.Where(s => s.ThumbsUp == true).Count();
                    mr.ThumbsDownCount = mr.MovieReviewScores.Where(s => s.ThumbsUp == false).Count();
                }
                return moviereviews.AsEnumerable();  
            }
            else
            {
                return null;  
            }
        }

        // DELETE api/MRSearch/5
        public HttpResponseMessage DeleteMovieReview(int id)
        {
            MovieReview moviereview = db.MovieReviews.Find(id);
            if (moviereview == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.MovieReviews.Remove(moviereview);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, moviereview);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}