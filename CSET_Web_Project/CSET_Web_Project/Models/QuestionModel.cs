/*******************************************************************************
 *	Class:		Question
 *
 *	Purpose:	To store the information associated with a Question from the
 *	            database.
 *
 *	Manager Functions:
 *		Question(int id)
 *		    Constructs the Quesiton from the database using the given id.
 * 
 *	Mutators:
 *	    FillQuestionById(int id)
 *	        Fills the Question from the database using the given id.
 * 
 *  Methods:
 *		InsertQuestion(string question, string first, string last, string email)
 *		    Inserts a new Question with the given information into the database.
 *		
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace CSET_Web_Project.Models
{
    public class Question
    {
        #region Members - Properties for the Question class
        private int mId;
        private string mQuestion;
        private string mAnswer;

        public int ID
        { get { return mId; } }
        public string QuestionText
        { get { return mQuestion; } }
        public string AnswerText
        { get { return mAnswer; } }
        #endregion

        public Question(int id = 8)
        {
            FillQuestionById(id);
        }

        private void FillQuestionById(int id)
        {
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
                myParam.Value = id;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT        QuesID, Question, Answer
                    FROM            askstu_Question
                    WHERE        (QuesID = @myParam)", tempConn.myConnection);
                myCommand.Parameters.Add(myParam);

                myReader = myCommand.ExecuteReader();
                if (!myReader.HasRows)
                    throw new Exception("No record exists for a Question that has the given id.");
                while (myReader.Read())
                {
                    mId = int.Parse(myReader[0].ToString());
                    mQuestion = myReader[1].ToString();
                    mAnswer = myReader[2].ToString();
                }
            }
            catch (Exception f)
            {
                //Add log exception
                throw;
            }
            finally
            {
                tempConn.CloseConnection();
            }
        }

        public static void InsertQuestion(string question, string first, string last, string email)
        {
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlParameter questionP = new SqlParameter("@question", SqlDbType.Text);
                questionP.Value = question;
                SqlParameter fName = new SqlParameter("@first", SqlDbType.VarChar);
                fName.Value = first;
                SqlParameter lName = new SqlParameter("@last", SqlDbType.VarChar);
                lName.Value = last;
                SqlParameter emailP = new SqlParameter("@email", SqlDbType.VarChar);
                emailP.Value = email;

                SqlCommand myCommand = new SqlCommand(@"
                    INSERT INTO askstu_Question (CategoryID, Question, Fname, Lname, Email, SubmitFAQ, DateSubmit)
                    VALUES (1, @question, @first, @last, @email, 0, GETDATE())", tempConn.myConnection);
                myCommand.Parameters.Add(questionP);
                myCommand.Parameters.Add(fName);
                myCommand.Parameters.Add(lName);
                myCommand.Parameters.Add(emailP);
                myCommand.ExecuteNonQuery();
            }
            catch (Exception f)
            {
                //Add log exception
                throw;
            }
            finally
            {
                tempConn.CloseConnection();
            }
        }
    }
}