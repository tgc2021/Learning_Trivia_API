using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Learning_Trivia_API.Models
{
    public class QuestionWithAnswers
    {
        public Question _question { get; set; }
        public List<_answeroption> _answeroption { get; set; }
    }
    public class Question
    {
        public int IdQuestion { get; set; }
        public int IdOrganization { get; set; }
        public int IdLearningCategory { get; set; }
        public int IdLearningSubCategory { get; set; }
        public string Title { get; set; }
        public string QuestionText { get; set; }
        public string ImagePath { get; set; }
        public string YoutubeUrl { get; set; }
        public string VideoUrl { get; set; }
        public string Points { get; set; }
       // Assuming GetAllQuestionAnswer1 returns a list of AnswerOptions
    }
    public class _answeroption
    {
        public int IdLearningQuestionAnswer { get; set; }
        public int IdLearningQuestion { get; set; }
        public string OptionAnswer { get; set; }
        public string IsCorrectAnswer { get; set; }
        public int option_number { get; set; }
    }

}