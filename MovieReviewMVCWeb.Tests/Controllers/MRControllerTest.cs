using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieReviewMVCWeb.Controllers;
using MovieReviewDataLayer;
using System.Collections.Generic;

namespace MovieReviewMVCWeb.Tests.Controllers
{
    [TestClass]
    public class MRControllerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            MRController controller = new MRController();

            // Act
            IEnumerable<MovieReview> result = controller.GetMovieReviews();

            // Assert
            //Assert.IsNotNull(result);
            //Assert.AreEqual(2, result.Count());
            //Assert.AreEqual("value1", result.ElementAt(0));
            //Assert.AreEqual("value2", result.ElementAt(1));
        }
        [TestMethod]
        public void Testmethod2()
        {
            MRController controller = new MRController();
            IEnumerable<MovieReview> result = controller.GetMovieReview("Dil");

        }
        [TestMethod]
        public void TestGetTopMovieReviews()
        {
            MRController controller = new MRController();
          //  IEnumerable<MovieReview> result = controller.GetTopMovieReviews(100);

        }
    }
}
