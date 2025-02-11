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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TriviaSubmitController : ApiController
    {
        private TriviaManager _triviaManager;

        // Constructor
        public TriviaSubmitController()
        {
            _triviaManager = new TriviaManager();
        }

        [HttpOptions]
        public HttpResponseMessage Options()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type"); // Specify allowed headers here
            return response;
        }

        [Route("api/Trivia/SubmitUserLog")]
        [AllowAnonymous]
        [HttpPost]
        [HttpOptions]
        public IHttpActionResult SubmitUserLog(tbl_learning_user_log user_Log)
        {
            var UserAttempt = _triviaManager.SubmitUserLog(user_Log);

            if (UserAttempt == null)
            {
                return Content(HttpStatusCode.NotFound, "No data available");
            }

            return Ok(UserAttempt);
        }

  
       
    }
}

