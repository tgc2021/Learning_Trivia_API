using Learning_Trivia_API.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Generator;
using System.Web.UI.WebControls;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;
using System.Xml.Linq;
using MySqlX.XDevAPI.Common;

namespace Learning_Trivia_API.BusinessLogicLayer
{
    public class TriviaRepository
    {
        private string _connectionString;

        // Constructor to initialize the connection string from Web.config or App.config
        public TriviaRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;
        }


        public List<dynamic> GetAllCategory(int orgid, int userid)
        {
            List<dynamic> categories = new List<dynamic>();

            try
            {

                   using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
             
                string query = @"SELECT DISTINCT a.id_organization, a.id_user,
                                c.id_learning_category, c.category_name
                             FROM tbl_learning_assigment a 
                             JOIN tbl_learning_category c 
                                ON a.id_learning_category = c.id_learning_category 
                             WHERE a.id_organization = @OrgId 
                             AND a.id_user = @UserId";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                // Add parameters to avoid SQL injection
                cmd.Parameters.AddWithValue("@OrgId", orgid);
                cmd.Parameters.AddWithValue("@UserId", userid);

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    // Create a dynamic object using ExpandoObject
                    dynamic category = new ExpandoObject();
                    //category.IdAssigment = Convert.ToInt32(reader["id_assigment"]);
                    category.IdOrganization = Convert.ToInt32(reader["id_organization"]);
                    category.IdUser = Convert.ToInt32(reader["id_user"]);
                    category.IdLearningCategory = Convert.ToInt32(reader["id_learning_category"]);
                    category.CategoryName = reader["category_name"].ToString();
                    //category.IdLearningSubCategory = Convert.ToInt32(reader["id_learning_sub_category"]);
                    //category.SubCategoryName = reader["sub_category_name"].ToString();

                    // Add the dynamic object to the list
                    categories.Add(category);
                }

                conn.Close();
            }
            }
            catch (MySqlException ex)
            {
                // Log the exception (for example, to a file or monitoring system)
                Console.WriteLine("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return categories;
        }

        public List<dynamic> GetAllSubCategory(int cid,int orgid, int userid)
        {
            List<dynamic> categories = new List<dynamic>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {

                    string query = @"SELECT  a.id_organization, a.id_user, a.id_learning_category,a.end_date,a.start_date,
                                sc.id_learning_sub_category, sc.sub_category_name,sc.sub_category_description
                             FROM tbl_learning_assigment a 
                             JOIN tbl_learning_sub_category sc 
                                ON a.id_learning_sub_category = sc.id_learning_sub_category 
                             WHERE a.id_organization = @OrgId
                             AND a.id_user = @UserId
                             AND sc.id_learning_category=@cid
                             AND a.start_date <= CURRENT_TIMESTAMP()
                             AND a.end_date >= CURRENT_TIMESTAMP();";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@cid", cid);
                    cmd.Parameters.AddWithValue("@OrgId", orgid);
                    cmd.Parameters.AddWithValue("@UserId", userid);

                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        dynamic category = new ExpandoObject();
                        //category.IdAssigment = Convert.ToInt32(reader["id_assigment"]);
                        category.IdOrganization = Convert.ToInt32(reader["id_organization"]);
                        category.IdUser = Convert.ToInt32(reader["id_user"]);
                        category.IdLearningCategory = Convert.ToInt32(reader["id_learning_category"]);
                        category.IdLearningSubCategory = Convert.ToInt32(reader["id_learning_sub_category"]);
                        category.SubCategoryName = reader["sub_category_name"].ToString();
                        category.SubCategoryDescription = reader["sub_category_description"].ToString();

                        // Add the dynamic object to the list
                        categories.Add(category);
                    }

                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                // Log the exception (for example, to a file or monitoring system)
                Console.WriteLine("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return categories;
        }

        public List<dynamic> GetAllQuestion(int cid, int sb,int orgid)
        {
            string serverPath = System.Configuration.ConfigurationManager.AppSettings["category_image"];
            List<dynamic> categories = new List<dynamic>();
            try
            {

                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {

                    //string query = @"select * from tbl_learning_question where id_learning_category=@cid AND
                    //             id_learning_sub_category=@sb AND id_organization=@OrgId;";
             string query = @"SELECT 
                            la.*, 
                            lq.* 
                        FROM 
                            tbl_learning_assigment la
                        JOIN 
                            tbl_learning_question lq 
                        ON 
                            la.id_learning_category = lq.id_learning_category
                        WHERE 
                            lq.id_learning_category = @cid 
                            AND lq.id_learning_sub_category = @sb
                            AND lq.id_organization = @OrgId
                            AND la.start_date < CURDATE() + INTERVAL 1 DAY;
                        ";


                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@cid", cid);
                    cmd.Parameters.AddWithValue("@OrgId", orgid);
                    cmd.Parameters.AddWithValue("@sb", sb);

                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        dynamic category = new ExpandoObject();
                        category.IdQuestion = Convert.ToInt32(reader["id_learning_question"]);
                        category.IdOrganization = Convert.ToInt32(reader["id_organization"]);
                        category.IdLearningCategory = Convert.ToInt32(reader["id_learning_category"]);
                        category.IdLearningSubCategory = Convert.ToInt32(reader["id_learning_sub_category"]);
                        category.Title = reader["title"].ToString();
                        category.Question = reader["question"].ToString();
                        category.Image_path = reader["image_path"] != DBNull.Value ? serverPath + reader["image_path"].ToString() : null;
                        category.Youtube_url = reader["youtube_url"] != DBNull.Value ? serverPath + reader["youtube_url"].ToString() : null;
                        category.Video_url = reader["video_url"] != DBNull.Value ? serverPath + reader["video_url"].ToString() : null;
                        category.Points = reader["points"].ToString();
                        categories.Add(category);
                    }

                    conn.Close();

                }
            }
            catch (MySqlException ex)
            {
                // Log the exception (for example, to a file or monitoring system)
                Console.WriteLine("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return categories;
        }

        public List<dynamic> GetAllQuestionAnswer(int idq)
        {
            List<dynamic> categories = new List<dynamic>();
            try
            {


                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {

                    string query = @"select * from tbl_learning_question_answer where id_learning_question=@idq;";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@idq", idq);


                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        dynamic category = new ExpandoObject();
                        category.IdLearningQuestionAnswer = Convert.ToInt32(reader["id_learning_question_answer"]);
                        category.IdLearningQuestion = Convert.ToInt32(reader["id_learning_question"]);
                        category.OptionAnswer = reader["option_answer"];
                        category.IscorrectAnswer = Convert.ToInt32(reader["is_correct_answer"]);
                        categories.Add(category);
                    }

                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                // Log the exception (for example, to a file or monitoring system)
                Console.WriteLine("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return categories;
        }

        public List<dynamic> GetAllQuestionsWithAnswers(int cid, int sb, int orgid)
        {
            string serverPath = System.Configuration.ConfigurationManager.AppSettings["category_image"];
            List<dynamic> questionsWithAnswers = new List<dynamic>();

            try
            {



                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    // Query to get questions
                    string questionQuery = @"SELECT * FROM tbl_learning_question 
                                 WHERE id_learning_category = @cid 
                                   AND id_learning_sub_category = @sb 
                                   AND id_organization = @OrgId;";

                    MySqlCommand questionCmd = new MySqlCommand(questionQuery, conn);
                    questionCmd.Parameters.AddWithValue("@cid", cid);
                    questionCmd.Parameters.AddWithValue("@OrgId", orgid);
                    questionCmd.Parameters.AddWithValue("@sb", sb);

                    conn.Open();
                    MySqlDataReader questionReader = questionCmd.ExecuteReader();

                    // Loop through questions
                    while (questionReader.Read())
                    {
                        dynamic question = new ExpandoObject();
                        question.IdQuestion = Convert.ToInt32(questionReader["id_learning_question"]);
                        question.IdOrganization = Convert.ToInt32(questionReader["id_organization"]);
                        question.IdLearningCategory = Convert.ToInt32(questionReader["id_learning_category"]);
                        question.IdLearningSubCategory = Convert.ToInt32(questionReader["id_learning_sub_category"]);
                        question.Title = questionReader["title"].ToString();
                        question.Question = questionReader["question"].ToString();
                        question.Image_path = questionReader["image_path"] != DBNull.Value ?
                            serverPath + questionReader["image_path"].ToString() : null;
                        question.Youtube_url = questionReader["youtube_url"] != DBNull.Value ?
                            serverPath + questionReader["youtube_url"].ToString() : null;
                        question.Video_url = questionReader["video_url"] != DBNull.Value ?
                            serverPath + questionReader["video_url"].ToString() : null;
                        question.Points = questionReader["points"].ToString();

                        question._answeroption = GetAllQuestionAnswer1(question.IdQuestion);


                        questionsWithAnswers.Add(question);

                    }

                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                // Log the exception (for example, to a file or monitoring system)
                Console.WriteLine("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return questionsWithAnswers; // Return the list of questions with answers
        }


        //
        //public List<Question> GetAllQuestionsWithAnswers1(int cid, int sb, int orgid)
        //{
        //    string serverPath = System.Configuration.ConfigurationManager.AppSettings["category_image"];
        //    List<Question> questionsWithAnswers = new List<Question>();

        //    using (MySqlConnection conn = new MySqlConnection(_connectionString))
        //    {
        //        // Query to get questions
        //        string questionQuery = @"SELECT * FROM tbl_learning_question 
        //                     WHERE id_learning_category = @cid 
        //                       AND id_learning_sub_category = @sb 
        //                       AND id_organization = @OrgId;";

        //        MySqlCommand questionCmd = new MySqlCommand(questionQuery, conn);
        //        questionCmd.Parameters.AddWithValue("@cid", cid);
        //        questionCmd.Parameters.AddWithValue("@OrgId", orgid);
        //        questionCmd.Parameters.AddWithValue("@sb", sb);

        //        conn.Open();
        //        MySqlDataReader questionReader = questionCmd.ExecuteReader();

        //        // Loop through questions
        //        while (questionReader.Read())
        //        {
        //            var question = new Question
        //            {
        //                IdQuestion = Convert.ToInt32(questionReader["id_learning_question"]),
        //                IdOrganization = Convert.ToInt32(questionReader["id_organization"]),
        //                IdLearningCategory = Convert.ToInt32(questionReader["id_learning_category"]),
        //                IdLearningSubCategory = Convert.ToInt32(questionReader["id_learning_sub_category"]),
        //                Title = questionReader["title"].ToString(),
        //                QuestionText = questionReader["question"].ToString(),
        //                ImagePath = questionReader["image_path"] != DBNull.Value ? serverPath + questionReader["image_path"].ToString() : null,
        //                YoutubeUrl = questionReader["youtube_url"] != DBNull.Value ? serverPath + questionReader["youtube_url"].ToString() : null,
        //                VideoUrl = questionReader["video_url"] != DBNull.Value ? serverPath + questionReader["video_url"].ToString() : null,
        //                Points = questionReader["points"].ToString(),
        //               //_answeroption = GetAllQuestionAnswer2(Convert.ToInt32(questionReader["id_learning_question"]))
        //            };
        //            var questionWithAnswers = new QuestionWithAnswers
        //            {
        //                _question = question,
        //                _answeroption = GetAllQuestionAnswer2(Convert.ToInt32(questionReader["id_learning_question"])) // Pass connection
        //            };


        //            questionsWithAnswers.Add(questionWithAnswers);
        //        }

        //        conn.Close();
        //    }

        //    return questionsWithAnswers; // Return the list of questions with answers
        //}

        //private List<_answeroption> GetAllQuestionAnswer2(int idq)
        //{
        //    List<_answeroption> answers = new List<_answeroption>();

        //    using (MySqlConnection conn = new MySqlConnection(_connectionString))
        //    {
        //        string answerQuery = @"SELECT * FROM tbl_learning_question_answer 
        //                   WHERE id_learning_question = @idq;";

        //        MySqlCommand answerCmd = new MySqlCommand(answerQuery, conn);
        //        answerCmd.Parameters.AddWithValue("@idq", idq);

        //        conn.Open();
        //        MySqlDataReader answerReader = answerCmd.ExecuteReader();

        //        while (answerReader.Read())
        //        {
        //            var _answeroption = new _answeroption
        //            {
        //                IdLearningQuestionAnswer = Convert.ToInt32(answerReader["id_learning_question_answer"]),
        //                IdLearningQuestion = Convert.ToInt32(answerReader["id_learning_question"]),
        //                OptionAnswer = answerReader["option_answer"].ToString(),
        //                IsCorrectAnswer = answerReader["is_correct_answer"] .ToString()// Assuming it's stored as an integer (0 or 1)
        //            };

        //            answers.Add(_answeroption);
        //        }

        //        conn.Close();
        //    }

        //    return answers; // Return the list of answers
        //}
        public List<QuestionWithAnswers> GetAllQuestionsWithAnswers2(int cid, int sb, int orgid)
        {
            string serverPath = System.Configuration.ConfigurationManager.AppSettings["category_image"];
            List<QuestionWithAnswers> questionsWithAnswers = new List<QuestionWithAnswers>();

            try
            {


                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    // Query to get questions and answers using a JOIN
                    string query = @"
                                    SELECT q.*, a.id_learning_question_answer, a.option_answer, a.is_correct_answer
                                    FROM tbl_learning_question q
                                    LEFT JOIN tbl_learning_question_answer a ON q.id_learning_question = a.id_learning_question
                                    WHERE q.id_learning_category = @cid 
                                    AND q.id_learning_sub_category = @sb 
                                    AND q.id_organization = @OrgId;";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@cid", cid);
                    cmd.Parameters.AddWithValue("@OrgId", orgid);
                    cmd.Parameters.AddWithValue("@sb", sb);

                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var question = new Question
                        {
                            IdQuestion = Convert.ToInt32(reader["id_learning_question"]),
                            IdOrganization = Convert.ToInt32(reader["id_organization"]),
                            IdLearningCategory = Convert.ToInt32(reader["id_learning_category"]),
                            IdLearningSubCategory = Convert.ToInt32(reader["id_learning_sub_category"]),
                            Title = reader["title"].ToString(),
                            QuestionText = reader["question"].ToString(),
                            ImagePath = reader["image_path"] != DBNull.Value ? serverPath + reader["image_path"].ToString() : null,
                            YoutubeUrl = reader["youtube_url"] != DBNull.Value ? serverPath + reader["youtube_url"].ToString() : null,
                            VideoUrl = reader["video_url"] != DBNull.Value ? serverPath + reader["video_url"].ToString() : null,
                            Points = reader["points"].ToString(),
                        };

                        var answers = new List<_answeroption>();

                        // Add answer if available
                        if (reader["id_learning_question_answer"] != DBNull.Value)
                        {
                            var answer = new _answeroption
                            {
                                IdLearningQuestionAnswer = Convert.ToInt32(reader["id_learning_question_answer"]),
                                IdLearningQuestion = question.IdQuestion,
                                OptionAnswer = reader["option_answer"].ToString(),
                                IsCorrectAnswer = reader["is_correct_answer"].ToString() // Convert to boolean
                            };

                            answers.Add(answer);
                        }

                        // Check if question already exists in the list
                        var existingQuestionWithAnswers = questionsWithAnswers.FirstOrDefault(q => q._question.IdQuestion == question.IdQuestion);
                        if (existingQuestionWithAnswers != null)
                        {
                            // Merge answers if the question already exists
                            existingQuestionWithAnswers._answeroption.AddRange(answers);
                        }
                        else
                        {
                            questionsWithAnswers.Add(new QuestionWithAnswers
                            {
                                _question = question,
                                _answeroption = answers
                            });
                        }
                    }

                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                // Log the exception (for example, to a file or monitoring system)
                Console.WriteLine("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return questionsWithAnswers; // Return the list of questions with answers
        }

        public List<QuestionWithAnswers> GetAllQuestionsWithAnswers1(int cid, int sb, int orgid)
        {
            string serverPath = System.Configuration.ConfigurationManager.AppSettings["category_image"];
            List<QuestionWithAnswers> questionsWithAnswers = new List<QuestionWithAnswers>();
            try
            {

                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    // Query to get questions
                    string questionQuery = @"SELECT * FROM tbl_learning_question 
                                 WHERE id_learning_category = @cid 
                                   AND id_learning_sub_category = @sb 
                                   AND id_organization = @OrgId;";

                    MySqlCommand questionCmd = new MySqlCommand(questionQuery, conn);
                    questionCmd.Parameters.AddWithValue("@cid", cid);
                    questionCmd.Parameters.AddWithValue("@OrgId", orgid);
                    questionCmd.Parameters.AddWithValue("@sb", sb);

                    conn.Open();
                    MySqlDataReader questionReader = questionCmd.ExecuteReader();

                    while (questionReader.Read())
                    {
                        var question = new Question
                        {
                            IdQuestion = Convert.ToInt32(questionReader["id_learning_question"]),
                            IdOrganization = Convert.ToInt32(questionReader["id_organization"]),
                            IdLearningCategory = Convert.ToInt32(questionReader["id_learning_category"]),
                            IdLearningSubCategory = Convert.ToInt32(questionReader["id_learning_sub_category"]),
                            Title = questionReader["title"].ToString(),
                            QuestionText = questionReader["question"].ToString(),
                            ImagePath = questionReader["image_path"] != DBNull.Value ? serverPath + questionReader["image_path"].ToString() : null,
                            YoutubeUrl = questionReader["youtube_url"] != DBNull.Value ? questionReader["youtube_url"].ToString() : null,
                            VideoUrl = questionReader["video_url"] != DBNull.Value ? serverPath + questionReader["video_url"].ToString() : null,
                            Points = questionReader["points"].ToString(),
                        };

                        var answers = GetAllQuestionAnswer2(Convert.ToInt32(questionReader["id_learning_question"]));

                        var questionWithAnswers = new QuestionWithAnswers
                        {
                            _question = question,
                            _answeroption = answers ?? new List<_answeroption>()  // Fallback to empty list if null
                        };

                        questionsWithAnswers.Add(questionWithAnswers);
                    }

                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                // Log the exception (for example, to a file or monitoring system)
                Console.WriteLine("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return questionsWithAnswers; 
        }

        private List<_answeroption> GetAllQuestionAnswer2(int idq)
        {
            List<_answeroption> answers = new List<_answeroption>();
            int option_number = 1;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    string answerQuery = @"SELECT * FROM tbl_learning_question_answer 
                               WHERE id_learning_question = @idq;";

                    MySqlCommand answerCmd = new MySqlCommand(answerQuery, conn);
                    answerCmd.Parameters.AddWithValue("@idq", idq);

                    conn.Open();
                    MySqlDataReader answerReader = answerCmd.ExecuteReader();

                    while (answerReader.Read())
                    {
                        var _answeroption = new _answeroption
                        {
                            IdLearningQuestionAnswer = Convert.ToInt32(answerReader["id_learning_question_answer"]),
                            IdLearningQuestion = Convert.ToInt32(answerReader["id_learning_question"]),
                            OptionAnswer = answerReader["option_answer"].ToString(),
                            IsCorrectAnswer = answerReader["is_correct_answer"].ToString(),
                            option_number  = option_number++

                        };

                        answers.Add(_answeroption);
                    }

                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                // Log the exception (for example, to a file or monitoring system)
                Console.WriteLine("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return answers; // Return the list of answers
        }


        private List<dynamic> GetAllQuestionAnswer1(int idq)
        {
            List<dynamic> answers = new List<dynamic>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    string answerQuery = @"SELECT * FROM tbl_learning_question_answer 
                               WHERE id_learning_question = @idq;";

                    MySqlCommand answerCmd = new MySqlCommand(answerQuery, conn);
                    answerCmd.Parameters.AddWithValue("@idq", idq);

                    conn.Open();
                    MySqlDataReader answerReader = answerCmd.ExecuteReader();

                    while (answerReader.Read())
                    {
                        dynamic answer = new ExpandoObject();
                        answer.IdLearningQuestionAnswer = Convert.ToInt32(answerReader["id_learning_question_answer"]);
                        answer.IdLearningQuestion = Convert.ToInt32(answerReader["id_learning_question"]);
                        answer.OptionAnswer = answerReader["option_answer"];
                        answer.IsCorrectAnswer = Convert.ToInt32(answerReader["is_correct_answer"]);
                        answers.Add(answer);
                    }

                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                // Log the exception (for example, to a file or monitoring system)
                Console.WriteLine("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return answers; // Return the list of answers
        }


        public List<dynamic> GetUserAttempt(int orgid, int userid, int QuestionId)
        {
            List<dynamic> categories = new List<dynamic>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    // First query to get user attempts
                    string Query = @"SELECT 
                            sl.id_user, 
                            MAX(sl.attempt) AS last_attempt, 
                            q.number_of_attempt
                        FROM 
                            tbl_learning_user_log sl
                        JOIN 
                            tbl_learning_question q 
                        ON 
                            sl.id_learning_question = q.id_learning_question
                        WHERE 
                            sl.id_learning_question = @QuestionId
                            AND sl.id_organization = @orgid 
                            AND sl.id_user = @userid
                        GROUP BY 
                            sl.id_user, 
                            q.number_of_attempt
                        ORDER BY 
                            MAX(sl.attempt) DESC";

                    MySqlCommand cmd = new MySqlCommand(Query, conn);
                    cmd.Parameters.AddWithValue("@orgid", orgid);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@QuestionId", QuestionId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dynamic category = new ExpandoObject();
                            category.Iduser = userid;
                            category.Attempt = reader["last_attempt"] != DBNull.Value ? Convert.ToInt32(reader["last_attempt"]) : 0;
                            category.Numberofattempt = reader["number_of_attempt"] != DBNull.Value ? Convert.ToInt32(reader["number_of_attempt"]) : 0;
                            categories.Add(category);
                        }
                    }

                    // If no records found, execute second query
                    if (categories.Count == 0)
                    {
                        string Query2 = @"SELECT number_of_attempt
                                  FROM tbl_learning_question
                                  WHERE id_learning_question = @QuestionId_1 
                                  AND id_organization = @orgid_1";

                        MySqlCommand cmd2 = new MySqlCommand(Query2, conn);
                        cmd2.Parameters.AddWithValue("@orgid_1", orgid);
                        cmd2.Parameters.AddWithValue("@QuestionId_1", QuestionId);

                        using (MySqlDataReader reader1 = cmd2.ExecuteReader())
                        {
                            while (reader1.Read())
                            {
                                dynamic category = new ExpandoObject();
                                category.Iduser = userid;
                                category.Attempt = 0;
                                category.Numberofattempt = reader1["number_of_attempt"] != DBNull.Value ? Convert.ToInt32(reader1["number_of_attempt"]) : 0;
                                categories.Add(category);
                            }
                        }
                    }

                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                // Log the exception (for example, to a file or monitoring system)
                Console.WriteLine("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return categories;
        }


        public class SubmitUserLogResponse
        {
            public string Result { get; set; }
            public int CorrectanswerId { get; set; }
            public string Response { get; set; }
            public int Score { get; set; }
            public int TotalPoint { get; set; }
        }

        public SubmitUserLogResponse SubmitUserLog(tbl_learning_user_log userLog)
        {
            var response = new SubmitUserLogResponse { Result = "Failure", Response = "", Score = 0 };

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    string query = @"INSERT INTO tbl_learning_user_log 
                        (id_learning_category, id_learning_sub_category, id_learning_question, 
                        id_organization, id_user, id_learning_question_answer, point, 
                        is_correct_answer, attempt)
                        VALUES (@IdLearningCategory, @IdLearningSubCategory, @IdLearningQuestion, 
                                @IdOrganization, @IdUser, @IdLearningQuestionAnswer, @Point, 
                                @IsCorrectAnswer, @Attempt);";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    // Add parameters to the MySqlCommand
                    cmd.Parameters.AddWithValue("@IdLearningCategory", userLog.CategoryId);
                    cmd.Parameters.AddWithValue("@IdLearningSubCategory", userLog.SubCategoryId);
                    cmd.Parameters.AddWithValue("@IdLearningQuestion", userLog.QuestionId);
                    cmd.Parameters.AddWithValue("@IdOrganization", userLog.orgid);
                    cmd.Parameters.AddWithValue("@IdUser", userLog.userid);
                    cmd.Parameters.AddWithValue("@IdLearningQuestionAnswer", userLog.QuestionAnswerId);
                    cmd.Parameters.AddWithValue("@Point", userLog.Point);
                    cmd.Parameters.AddWithValue("@IsCorrectAnswer", userLog.IsCorrectAnswer);
                    cmd.Parameters.AddWithValue("@Attempt", userLog.Attempt);

                    // Open the connection
                    conn.Open();

                    try
                    {
                        // Execute the command
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            var answers = GetRightAnswer(userLog.QuestionId, userLog.QuestionAnswerId, userLog.CategoryId);

                            if (answers.Count > 0)
                            {
                                response.Result = answers[0].result;
                                response.CorrectanswerId = Convert.ToInt32(answers[0].CorrectanswerId);
                                response.Response = answers[0].response;
                                response.Score = Convert.ToInt32(answers[0].score); 
                                response.TotalPoint = Convert.ToInt32(answers[0].totalpoint); 
                            }
                            else
                            {
                                response.Result = "Success";
                                response.Response = "No correct answer found";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log exception or handle error
                        response.Result = "Insertion failed";
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Log the exception (for example, to a file or monitoring system)
                Console.WriteLine("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return response;
        }

        public List<dynamic> GetRightAnswer(int questionId, int questionAnswerId,int CategoryId)
        {
            List<dynamic> answers = new List<dynamic>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
                {
                    connection.Open();

                    // First query
                    string query1 = @"
                                     SELECT qa.*, q.*, points_summary.total_points
FROM tbl_learning_question_answer qa
JOIN tbl_learning_question q ON q.id_learning_question = qa.id_learning_question
JOIN (
    SELECT SUM(CASE WHEN l.is_correct_answer = 1 THEN l.point ELSE 0 END) AS total_points
    FROM tbl_learning_user_log l
    WHERE l.id_learning_category = @CategoryId
) AS points_summary
WHERE qa.id_learning_question = @questionId 
  AND qa.id_learning_question_answer = @questionAnswerId
  AND qa.is_correct_answer = 1";

                    using (MySqlCommand command = new MySqlCommand(query1, connection))
                    {
                        command.Parameters.AddWithValue("@QuestionId", questionId);
                        command.Parameters.AddWithValue("@QuestionAnswerId", questionAnswerId);
                        command.Parameters.AddWithValue("@CategoryId", CategoryId);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    answers.Add(new
                                    {
                                        result = "Success",
                                        CorrectanswerId = reader["id_learning_question_answer"],
                                        response = reader["option_answer"] + " That's the correct choice",
                                        score = reader["points"],
                                        totalpoint = reader["total_points"]
                                    });
                                }
                                return answers; // Return if found
                            }
                        }
                    }

                        string query2 = @"
                                  SELECT qa.*, q.*, points_summary.total_points
                                FROM tbl_learning_question_answer qa
                                JOIN tbl_learning_question q ON q.id_learning_question = qa.id_learning_question
                                JOIN (
                                    SELECT SUM(CASE WHEN l.is_correct_answer = 1 THEN l.point ELSE 0 END) AS total_points
                                    FROM tbl_learning_user_log l
                                    WHERE l.id_learning_category = @CategoryId
                                ) AS points_summary
                                WHERE qa.id_learning_question = @questionId 
                                  AND qa.is_correct_answer = 1;"
;

                        using (MySqlCommand command2 = new MySqlCommand(query2, connection))
                        {
                            command2.Parameters.AddWithValue("@QuestionId", questionId);
                            command2.Parameters.AddWithValue("@CategoryId", CategoryId);

                            using (MySqlDataReader reader2 = command2.ExecuteReader())
                            {
                                while (reader2.Read())
                                {
                                    answers.Add(new
                                    {
                                        result = "Fail",
                                        CorrectanswerId = reader2["id_learning_question_answer"],
                                        response = reader2["option_answer"] + " is the correct choice",
                                        score = reader2["points"],
                                        totalpoint = reader2["total_points"]
                                    });
                                }
                            }
                        }
                    }
                
            }
            catch (MySqlException sqlEx)
            {
                // Log database-related errors
                Console.WriteLine($"MySQL error in GetProfile: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                // Log general errors
                Console.WriteLine($"An error occurred in GetProfile: {ex.Message}", ex);
            }
            return answers; // Return the correct answers found
        }

        public List<dynamic> GetProfile(int orgid, int userid)
        {
            List<dynamic> profiles = new List<dynamic>(); // List to hold multiple profiles

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
                {
                    conn.Open();

                    string query = @"SELECT * 
                             FROM tbl_user us 
                             LEFT JOIN tbl_profile pf ON us.id_user = pf.id_user 
                             WHERE us.id_user = @UserId AND us.STATUS = 'A' AND us.ID_ORGANIZATION = @orgid";

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@UserId", userid);
                        command.Parameters.AddWithValue("@orgid", orgid);

                        using (MySqlDataReader mySqlDataReader = command.ExecuteReader())
                        {
                            if (mySqlDataReader.Read())
                            {
                                dynamic profile = new ExpandoObject(); // Create a new dynamic object for the profile

                                profile.ID_USER = mySqlDataReader.GetInt32("ID_USER");
                                profile.USERID = mySqlDataReader.GetString("USERID");
                                profile.PASSWORD = mySqlDataReader.GetString("PASSWORD");
                                profile.ID_ORGANIZATION = mySqlDataReader.GetInt32("ID_ORGANIZATION");
                                profile.EMPLOYEEID = mySqlDataReader.GetString("EMPLOYEEID");
                                profile.user_designation = mySqlDataReader.GetString("USER_DESIGNATION");
                                profile.ID_ROLE = mySqlDataReader.GetInt32("Id_Role");
                                profile.user_grade = mySqlDataReader.GetString("USER_GRADE");
                                profile.user_function = mySqlDataReader.GetString("USER_FUNCTION");
                                profile.OFFICE_ADDRESS = mySqlDataReader.GetString("OFFICE_ADDRESS");
                                profile.GENDER = mySqlDataReader.GetString("GENDER");
                                profile.FullName = string.Concat(mySqlDataReader["firstname"], " ", mySqlDataReader["Lastname"]);

                                profiles.Add(profile); // Add the dynamic profile object to the list
                            }
                        }
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                // Log database-related errors
                Console.WriteLine($"MySQL error in GetProfile: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                // Log general errors
                Console.WriteLine($"An error occurred in GetProfile: {ex.Message}", ex);
            }

            return profiles; // Return the list of profiles
        }

        public LeaderboardResult GetLeaderboard(int orgid, int userid)
        {
            List<dynamic> profiles = new List<dynamic>();
            List<dynamic> designationUsers = new List<dynamic>();
            dynamic myRank = null;

            LeaderboardResult result = new LeaderboardResult();
            

            try
            {
                profiles = GetProfile(orgid, userid);

                foreach (var profile in profiles)
                {
                    if (profile.user_designation != null)
                    {
                        string roleNameLB = getRoleName_Lb(profile.ID_ROLE, profile.ID_ORGANIZATION);

                        if (!string.IsNullOrEmpty(roleNameLB))
                        {
                            designationUsers = getUserDesignationDetails(roleNameLB, orgid);

                            if (designationUsers.Count > 0)
                            {
                                myRank = designationUsers.Find(x => x.ID_USER == userid);
                                if (myRank != null)
                                {
                                    result = myRank.USERID;
                                    result= myRank.PointsScored;
                                    result=myRank.FullName;
                                    result=myRank.Dense_Rank;
                                }
                                else
                                {

                                    result=  myRank.USERID;
                                   
                                    result = myRank="User not found";
                                    
                                }

                                // Sort and assign top 20 users
                                result.TopUsers = designationUsers
                                    .OrderByDescending(x => x.PointsScored)
                                    .Take(20)
                                    .ToList();
                                return result;
                            }
                        }
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                Console.WriteLine($"MySQL error in GetLeaderboard: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetLeaderboard: {ex.Message}", ex);
            }

            result.TopUsers = designationUsers; // Return empty or populated list of top users
            return result;
        }


        public string getRoleName_Lb(int RoleId, int OrgId)
        {
            string RoleName_LB = string.Empty;

            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                conn.Open();
                string str = "SELECT substr(csst_role,1,2) as RoleName_LB FROM Tbl_csst_role WHERE Id_csst_role = @RoleId AND id_organization = @OrgId AND status = 'A'";

                using (MySqlCommand command = new MySqlCommand(str, conn))
                {
                    // Add parameters to prevent SQL injection
                    command.Parameters.AddWithValue("@RoleId", RoleId);
                    command.Parameters.AddWithValue("@OrgId", OrgId);

                    using (MySqlDataReader mySqlDataReader = command.ExecuteReader())
                    {
                        if (mySqlDataReader.Read())
                        {
                            RoleName_LB = mySqlDataReader["RoleName_LB"].ToString();
                        }
                    }
                }
            }

            return RoleName_LB;
        }

        public List<dynamic> getUserDesignationDetails(string RoleName_LB, int OrgId)
        {
            List<dynamic> UserDetails = new List<dynamic>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
                {
                    conn.Open();
                    string str = @"SELECT 
                            u.userid,
                            u.id_user,
                            p.firstname,
                            p.lastname,
                            SUM(l.point) AS points
                        FROM 
                            tbl_user u
                        JOIN 
                            tbl_csst_role r ON u.id_role = r.id_csst_role
                        JOIN 
                            tbl_learning_user_log l ON u.id_user = l.id_user
                        JOIN 
                            tbl_profile p ON u.id_user = p.id_user
                        WHERE 
                            r.id_organization = @OrgId
                            AND l.status = 'A'
                            AND u.status = 'A'
                            AND r.status = 'A'
                            AND l.is_correct_answer = 1
                            AND BINARY SUBSTR(r.csst_role, 1, 2) = @RoleName_LB
                        GROUP BY 
                            u.userid
                        HAVING 
                            points > 0
                        ORDER BY 
                            points DESC";
//                    string str = @"SELECT a.userid,a.ID_USER,d.firstname,d.Lastname,SUM(point) AS points 
//FROM tbl_user a,tbl_csst_role b, tbl_learning_user_log c,tbl_profile d WHERE a.id_role = b.id_csst_role 
//AND a.id_user = c.id_user AND a.id_user = d.id_user AND b.id_organization = @OrgId AND c.status = 'A' AND a.STATUS = 'A' 
//AND b.STATUS = 'A' AND BINARY SUBSTR(b.csst_role, 1, 2) = @RoleName_LB GROUP BY a.userid HAVING points > 0 ORDER BY points DESC;;";

                    using (MySqlCommand command = new MySqlCommand(str, conn))
                    {
                        command.Parameters.AddWithValue("@OrgId", OrgId);
                        command.Parameters.AddWithValue("@RoleName_LB", RoleName_LB);

                        using (MySqlDataReader mySqlDataReader = command.ExecuteReader())
                        {
                            int rank = 0;
                            double previousPoints = double.MaxValue;

                            while (mySqlDataReader.Read())
                            {
                                double currentPoints = Convert.ToDouble(mySqlDataReader["points"]);

                                if (currentPoints < previousPoints)
                                {
                                    rank++;
                                }
                                previousPoints = currentPoints;

                                UserDetails.Add(new
                                {
                                    ID_USER = Convert.ToInt32(mySqlDataReader["ID_USER"]),
                                    USERID = mySqlDataReader["USERID"].ToString(),
                                    FullName = $"{mySqlDataReader["firstname"]} {mySqlDataReader["Lastname"]}",
                                    PointsScored = currentPoints,
                                    Dense_Rank = rank
                                });
                            }
                        }
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                // Log MySQL-specific errors
                Console.WriteLine($"MySQL error in getUserDesignationDetails: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                // Log general errors
                Console.WriteLine($"An error occurred in getUserDesignationDetails: {ex.Message}", ex);
            }

            return UserDetails;
        }


        public List<dynamic> CategoryScore(int orgid, int userid, int CategoryId)
        {
            List<dynamic> categories = new List<dynamic>();

            try
            {

                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {

                    //                    string query = @"SELECT 
                    //    SUM(CASE WHEN qa.is_correct_answer = 1 THEN q.points ELSE 0 END) AS total_points,
                    //    COUNT(CASE WHEN qa.is_correct_answer = 1 THEN 1 END) AS total_correct_answers,
                    //    COUNT(CASE WHEN qa.is_correct_answer = 0 THEN 1 END) AS total_wrong_answers,
                    //    SUM(q.points) AS total_possible_points
                    //FROM tbl_learning_question q
                    //JOIN tbl_learning_question_answer qa ON q.id_learning_question = qa.id_learning_question
                    //WHERE q.id_learning_category = 10 AND q.id_organization = @OrgId 
                    //                             AND a.id_user = @UserId";

                    string query = @" SELECT
                                    SUM(CASE WHEN l.is_correct_answer = 1 THEN l.point ELSE 0 END) AS total_points,
                                    COUNT(CASE WHEN l.is_correct_answer = 1 THEN 1 END) AS total_correct_answers,
                                    COUNT(CASE WHEN l.is_correct_answer = 0 THEN 1 END) AS total_wrong_answers,
                                    SUM(l.point) AS total_possible_points
                                FROM tbl_learning_user_log l
                                WHERE l.id_learning_category =@CategoryId AND l.id_organization = @OrgId 
                                                                  AND l.id_user = @UserId";



                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    // Add parameters to avoid SQL injection
                    cmd.Parameters.AddWithValue("@OrgId", orgid);
                    cmd.Parameters.AddWithValue("@UserId", userid);
                    cmd.Parameters.AddWithValue("@CategoryId", CategoryId);
 
                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // Create a dynamic object using ExpandoObject
                        dynamic category = new ExpandoObject();
                        //category.IdAssigment = Convert.ToInt32(reader["id_assigment"]);
                        category.TotalPoints = Convert.ToInt32(reader["total_points"]);
                        category.TotalCorrectAnswers = Convert.ToInt32(reader["total_correct_answers"]);
                        category.TotalWrongAnswers = Convert.ToInt32(reader["total_wrong_answers"]);
                        category.TotalPossiblePoints = Convert.ToInt32(reader["total_possible_points"]);
                       
               
                        categories.Add(category);
                    }

                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                // Log the exception (for example, to a file or monitoring system)
                Console.WriteLine("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return categories;
        }

        public List<dynamic> OverallScore(int orgid, int userid)
        {
            List<dynamic> categories = new List<dynamic>();

            try
            {

                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {

                    string query = @" SELECT 
                                        YEAR(l.created_date_time) AS year,
                                        SUM(CASE WHEN l.is_correct_answer = 1 THEN l.point ELSE 0 END) AS total_points,
                                        COUNT(CASE WHEN l.is_correct_answer = 1 THEN 1 END) AS total_correct_answers,
                                        COUNT(CASE WHEN l.is_correct_answer = 0 THEN 1 END) AS total_wrong_answers,
                                        SUM(l.point) AS total_possible_points
                                    FROM 
                                        tbl_learning_user_log l
                                    WHERE 
                                        l.id_user = @UserId AND l.id_organization = @OrgId
                                        AND MONTH(l.created_date_time) BETWEEN 1 AND 12
                                    GROUP BY 
                                        YEAR(l.created_date_time)
                                    ORDER BY 
                                        YEAR(l.created_date_time);";



                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    // Add parameters to avoid SQL injection
                    cmd.Parameters.AddWithValue("@OrgId", orgid);
                    cmd.Parameters.AddWithValue("@UserId", userid);
                   

                    conn.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // Create a dynamic object using ExpandoObject
                        dynamic category = new ExpandoObject();
                     
                        category.Year = Convert.ToInt32(reader["year"]);
                        category.TotalPoints = Convert.ToInt32(reader["total_points"]);
                        category.TotalCorrectAnswers = Convert.ToInt32(reader["total_correct_answers"]);
                        category.TotalWrongAnswers = Convert.ToInt32(reader["total_wrong_answers"]);
                        category.TotalPossiblePoints = Convert.ToInt32(reader["total_possible_points"]);


                        categories.Add(category);
                    }

                    conn.Close();
                }
            }
            catch (MySqlException ex)
            {
                // Log the exception (for example, to a file or monitoring system)
                Console.WriteLine("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return categories;
        }
    }
}