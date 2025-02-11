using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Learning_Trivia_API.Models
{
    public class tbl_learning_user_log
    {
        public int IdLearningUserLog { get; set; }           // id_learning_user_log (Primary Key)
        public int CategoryId { get; set; }          // id_learning_category
        public int SubCategoryId { get; set; }       // id_learning_sub_category
        public int QuestionId { get; set; }          // id_learning_question
        public int orgid { get; set; }              // id_organization
        public int userid { get; set; }                      // id_user
        public int QuestionAnswerId { get; set; }   // id_learning_question_answer (nullable)
        public int Point { get; set; }                      // point (nullable)
        public string IsCorrectAnswer { get; set; }          // is_correct_answer (varchar)
        public int? Attempt { get; set; }                    // attempt (nullable)
        public string Status { get; set; }                 
        public DateTime CreatedDateTime { get; set; }
    }
}