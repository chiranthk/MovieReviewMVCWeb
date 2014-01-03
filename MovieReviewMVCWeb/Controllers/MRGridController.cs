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
using MovieReviewDataLayer;

namespace MovieReviewMVCWeb.Controllers
{
    public class MRGridController : ApiController
    {
        private MovieReviewEntities db = new MovieReviewEntities();

        // GET api/MRGrid
        public IEnumerable<MovieReview> GetMovieReviews()
        {
            var moviereviews = db.MovieReviews.Include(m => m.Language).Include(m => m.MovieReview_StatusType).Include(m => m.Person);
            return moviereviews.AsEnumerable();
        }

        // GET api/MR/5
        public IEnumerable<MovieReview> GetTopMovieReviews(int pageIndex)
        {
            int pageSize = 5;
            // Retrieve only published movie reviews.
            var moviereviews = db.MovieReviews.Include(m => m.Language).Include(m => m.MovieReview_StatusType).Include(m => m.Person).Where(m => m.MovieReview_StatusType_ID == 1);
            int count = moviereviews.Count();
            foreach (MovieReview mr in moviereviews)
            {
                mr.ThumbsUpCount = mr.MovieReviewScores.Where(s => s.ThumbsUp == true).Count();
                mr.ThumbsDownCount = mr.MovieReviewScores.Where(s => s.ThumbsUp == false).Count();
            }

            var skip = pageSize * (pageIndex - 1);
            var canPage = skip > count;
            if (canPage)
                return moviereviews.OrderByDescending(m => m.Release_Date).Take(pageSize);

            return moviereviews.OrderByDescending(m => m.Release_Date).Skip(skip)
             .Take(pageSize)
             .ToArray();
        }

        // PUT api/MRGrid/5
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

        // POST api/MRGrid
        public HttpResponseMessage PostMovieReview(MovieReview moviereview)
        {
            if (ModelState.IsValid)
            {
                db.MovieReviews.Add(moviereview);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, moviereview);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = moviereview.MovieReview_ID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/MRGrid/5
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