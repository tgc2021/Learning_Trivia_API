using Learning_Trivia_API.BusinessLogicLayer;
using Learning_Trivia_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Learning_Trivia_API.BusinessLogicLayer.TriviaRepository;

namespace Learning_Trivia_API.DataAccessLayer
{
    public class TriviaManager
    {
        private TriviaRepository _triviaRepository = new TriviaRepository();

        public List<dynamic> GetAllCategory(int orgid,int userid)
        {
            return _triviaRepository.GetAllCategory(orgid, userid);
        }

        public List<dynamic> GetAllSubCategory(int cid,int orgid, int userid)
        {
            return _triviaRepository.GetAllSubCategory(cid,orgid, userid);
        }

        public List<QuestionWithAnswers> GetAllQuestion(int cid,int sb, int orgid)
        {
            return _triviaRepository.GetAllQuestionsWithAnswers1(cid,sb, orgid);
           // return _triviaRepository.GetAllQuestionsWithAnswers2(cid,sb, orgid);
        }

        public List<dynamic> GetAllQuestionAnswer(int idq)
        {
            return _triviaRepository.GetAllQuestionAnswer(idq);
        }

        public List<dynamic> GetUserAttempt(int orgid, int userid, int QuestionId)
        {
            return _triviaRepository.GetUserAttempt(orgid,userid,QuestionId);
        }

        public SubmitUserLogResponse SubmitUserLog(tbl_learning_user_log user_Log)
        {
            return _triviaRepository.SubmitUserLog(user_Log);
        }

        public List<dynamic> GetProfile(int orgid, int userid)
        {
            return _triviaRepository.GetProfile(orgid, userid);
        }

        public LeaderboardResult GetLeaderboard(int orgid, int userid)
        {
            return _triviaRepository.GetLeaderboard(orgid, userid);
        }

        public List<dynamic> CategoryScore(int orgid, int userid, int CategoryId)
        {
            return _triviaRepository.CategoryScore(orgid, userid, CategoryId);
        }

        public List<dynamic> OverallScore(int orgid, int userid)
        {
            return _triviaRepository.OverallScore(orgid, userid);
        }
    }
}