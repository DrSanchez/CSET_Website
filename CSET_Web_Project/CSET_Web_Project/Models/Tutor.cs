/*******************************************************************************
 *	Class:		Tutor
 *
 *	Purpose:	To retrieve and store the data associated with a Tutor.
 *
 *	Manager Functions:
 *		Tutor(int id)
 *		    Constructs the Tutor with the given id from the database.
 * 
 *	Mutators:
 *	    FillTutorById(int id)
 *	        Fills the Tutor from the database using the given id.
 * 
 *  Methods:
 *	    GetCourses()
 *	        Fills the list of courses that this Tutor can help with.
 *		
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace csetMVC2011.Models
{
    public class Tutor
    {
        #region Members - Properties for the Tutor class.
        private int mId;
        private string mFirst;
        private string mLast;
        private string mPic;
        private List<Course> mCourses;

        public string FullName
        { get { return mFirst + " " + mLast; } }
        public string FirstName
        { get { return mFirst; } }
        public string LastName
        { get { return mLast; } }
        public string Picture
        { get { return mPic; } }
        public List<Course> Courses
        { get { return mCourses; } }
        #endregion

        public Tutor(int id = 32)
        {
            FillTutorById(id);
        }

        private void FillTutorById(int id)
        {
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
                myParam.Value = id;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT  StuID, FirstName, LastName, StudentPic
                    FROM    crshelp_Tutor
                    WHERE   (StuID = @myParam)", tempConn.myConnection);
                myCommand.Parameters.Add(myParam);

                myReader = myCommand.ExecuteReader();
                if (!myReader.HasRows)
                    throw new Exception("No record exists of a Tutor with the given id.");
                while (myReader.Read())
                {
                    mId = int.Parse(myReader[0].ToString());
                    mFirst = myReader[1].ToString();
                    mLast = myReader[2].ToString();
                    mPic = myReader[3].ToString();
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

            GetCourses();
        }

        private void GetCourses()
        {
            mCourses = new List<Course>();

            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
                myParam.Value = mId;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT  CourseID
                    FROM    crshelp_TutoringCourses
                    WHERE   (StuID = @myParam)", tempConn.myConnection);
                myCommand.Parameters.Add(myParam);

                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    mCourses.Add(new Course(int.Parse(myReader[0].ToString())));
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
    }
}