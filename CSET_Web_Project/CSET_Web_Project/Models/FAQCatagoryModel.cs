/*******************************************************************************
 *	Class:		FAQCategory
 *
 *	Purpose:	To store a FAQ Category and associated Questions.
 *
 *	Manager Functions:
 *		FAQCatagory(int id)
 *		    Constructs the category based on id from the database.
 *		
 *	Mutators:
 *	    FillByIdForDisplay(int id)
 *	        Fills the category based on the id from the database.
 *	    FillSelf(int id)
 *	        Fills the catagory information from the database.
 *	    FillQuestionsForDisplay(int id)
 *	        Fills the list of Questions from the database.
 * 
 *  Methods:
 *		GetAllCategories()
 *		    Returns a list of all categories and associated questions in the
 *		    database.
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
    public class FAQCatagory
    {
        #region Members - Properties for the FAQCategory class.
        private int mId;
        private string mName;
        private List<Question> mQuestions;

        public string Name
        { get { return mName; } }
        public List<Question> Questions
        { get { return mQuestions; } }
        #endregion

        public FAQCatagory(int id = 1)
        {
            FillByIdForDisplay(id);
        }

        private void FillByIdForDisplay(int id)
        {
            FillSelf(id);
            FillQuestionsForDisplay(id);
        }

        private void FillSelf(int id)
        {
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
                myParam.Value = id;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT        CategoryID, CategoryName
                    FROM            askstu_Category
                    WHERE        (CategoryID = @myParam)", tempConn.myConnection);
                myCommand.Parameters.Add(myParam);

                myReader = myCommand.ExecuteReader();
                if (!myReader.HasRows)
                    throw new Exception("No record of a Category with the given id exists.");
                while (myReader.Read())
                {
                    mId = int.Parse(myReader[0].ToString());
                    mName = myReader[1].ToString();
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

        private void FillQuestionsForDisplay(int id)
        {
            mQuestions = new List<Question>();

            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
                myParam.Value = id;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT        QuesID
                    FROM            askstu_Question
                    WHERE        (CategoryID = @myParam) AND (SubmitFAQ = 1)", tempConn.myConnection);
                myCommand.Parameters.Add(myParam);

                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    mQuestions.Add(new Question(int.Parse(myReader[0].ToString())));
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.ToString());
            }
            finally
            {
                tempConn.CloseConnection();
            }
        }

        public static List<FAQCatagory> GetAllCategories()
        {
            List<FAQCatagory> temp = new List<FAQCatagory>();

            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT      CategoryID
                    FROM        askstu_Category", tempConn.myConnection);

                myReader = myCommand.ExecuteReader();
                if (!myReader.HasRows)
                    throw new Exception("No records for any Category exist in the database.");
                while (myReader.Read())
                {
                    temp.Add(new FAQCatagory(int.Parse(myReader[0].ToString())));
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

            return temp;
        }
    }
}