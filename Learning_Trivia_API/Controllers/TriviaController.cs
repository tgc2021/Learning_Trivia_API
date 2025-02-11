using Learning_Trivia_API.DataAccessLayer;
using Learning_Trivia_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Learning_Trivia_API.Controllers
{
    [EnableCorsAttribute(origins: "*", headers: "*", methods: "*")]
    public class TriviaController : ApiController
    {
        [HttpOptions]
        public HttpResponseMessage Options()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST,OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type"); // Specify allowed headers here
            return response;
        }


        private TriviaManager _triviaManager = new TriviaManager();

        [HttpGet]
        [Route("api/Trivia/GetAllCategory")]
        public IHttpActionResult GetAllCategory(int orgid, int userid)
        {
            var categories = _triviaManager.GetAllCategory(orgid, userid);

            if (categories == null || !categories.Any())
            {
                return Content(HttpStatusCode.NotFound, "No data available");
            }

            return Ok(categories); 
        }


        [HttpGet]
        [Route("api/Trivia/GetAllSubCategory")]
        public IHttpActionResult GetAllSubCategory(int CategoryId, int orgid,int userid)
        {
            var Subcategories = _triviaManager.GetAllSubCategory(CategoryId, orgid, userid);


            if (Subcategories == null || !Subcategories.Any())
            {
                return Content(HttpStatusCode.NotFound, "No data available");
            }

            return Ok(Subcategories);

        }

        [HttpGet]
        [Route("api/Trivia/GetAllQuestion")]
        public IHttpActionResult GetAllQuestion(int CategoryId, int SubCategoryId, int orgid)
        {
            var Question = _triviaManager.GetAllQuestion(CategoryId, SubCategoryId, orgid);


            if (Question == null || !Question.Any())
            {
                return Content(HttpStatusCode.NotFound, "No data available");
            }

            return Ok(Question);

        }


        [HttpGet]
        [Route("api/Trivia/GetAllQuestionAnswer")]
        public IHttpActionResult GetAllQuestionAnswer(int QuestionId)
        {
            var QuestionAnswer = _triviaManager.GetAllQuestionAnswer(QuestionId);


            if (QuestionAnswer == null || !QuestionAnswer.Any())
            {
                return Content(HttpStatusCode.NotFound, "No data available");
            }

            return Ok(QuestionAnswer);

        }

        [HttpGet]
        [Route("api/Trivia/GetUserAttempt")]
        public IHttpActionResult GetUserAttempt(int orgid, int userid, int QuestionId)
        {
            var UserAttempt = _triviaManager.GetUserAttempt(orgid, userid, QuestionId);


            if (UserAttempt == null || !UserAttempt.Any())
            {
                return Content(HttpStatusCode.NotFound, "No data available");
            }

            return Ok(UserAttempt);

        }

        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //[HttpPost]
        //[Route("api/Trivia/SubmitUserLog")]
        //[HttpOptions]
        //public IHttpActionResult SubmitUserLog(tbl_learning_user_log user_Log)
        //{
        //    var UserAttempt = _triviaManager.SubmitUserLog(user_Log);


        //    if (UserAttempt == null || !UserAttempt.Any())
        //    {
        //        return Content(HttpStatusCode.NotFound, "No data available");
        //    }

        //    return Ok(UserAttempt);

        //}


        [HttpGet]
        [Route("api/Trivia/GetProfile")]
        public IHttpActionResult GetProfile(int orgid, int userid)
        {
            var categories = _triviaManager.GetProfile(orgid, userid);


            if (categories == null || !categories.Any())
            {
                return Content(HttpStatusCode.NotFound, "No data available");
            }

            return Ok(categories);
        }


        [HttpGet]
        [Route("api/Trivia/Leaderboard")]
        public IHttpActionResult GetLeaderboard(int orgid, int userid)
        {
            var leaderboard = _triviaManager.GetLeaderboard(orgid, userid);

            if (leaderboard == null || leaderboard.TopUsers == null || !leaderboard.TopUsers.Any())
            {
                return Content(HttpStatusCode.NotFound, "No data available");
            }

            return Ok(leaderboard);
        }

        [HttpGet]
        [Route("api/Trivia/CategoryScore")]
        public IHttpActionResult CategoryScore(int orgid, int userid, int CategoryId)
        {
            var CategoryScore = _triviaManager.CategoryScore(orgid, userid, CategoryId);

            if (CategoryScore == null)
            {
                return Content(HttpStatusCode.NotFound, "No data available");
            }

            return Ok(CategoryScore);
        }

        [HttpGet]
        [Route("api/Trivia/OverallScore")]
        public IHttpActionResult OverallScore(int orgid, int userid)
        {
            var CategoryScore = _triviaManager.OverallScore(orgid, userid);

            if (CategoryScore == null)
            {
                return Content(HttpStatusCode.NotFound, "No data available");
            }

            return Ok(CategoryScore);
        }

    }
}
