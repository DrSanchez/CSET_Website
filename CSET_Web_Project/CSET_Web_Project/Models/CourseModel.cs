/*******************************************************************************
 *	Class:		Course
 *
 *	Purpose:	To model the data associated with an Course in the database.
 *
 *	Manager Functions:
 *		Course(int id)
 *		    Constructs a Course from the database based on the given id.
 *		
 *	Mutators:
 *	    GetCourseByID(int id)
 *	        Fills the Course information from the database based on the given
 *	        id.	    
 *          
 *  Methods:
 *		GetListCSTCourses()
 *		    Returns a list of all CST courses in the database.		
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
    public class Course
    {
        #region Members - Properties for the CourseModel class.
        private int mId;
        private string mShortName;
        private string mLongName;
        private int mCreditHours;
        private int mLectureHours;
        private int mLabHours;
        private string mOfficialDesc;
        private bool mActive;

        public int Id
        {
            get { return mId; }
        }
        public string ShortName
        {
            get { return mShortName; }
            set { mShortName = value; }
        }
        public string LongName
        {
            get { return mLongName; }
            set { mLongName = value; }
        }
        public int CreditHours
        {
            get { return mCreditHours; }
            set { mCreditHours = value; }
        }
        public int LectureHours
        {
            get { return mLectureHours; }
            set { mLectureHours = value; }
        }
        public int LabHours
        {
            get { return mLabHours; }
            set { mLabHours = value; }
        }
        public string OfficialDescription
        {
            get { return mOfficialDesc; }
            set { mOfficialDesc = value; }
        }
        public bool Active
        {
            get { return mActive; }
            set { mActive = value; }
        }
        #endregion

        public Course(int id = 1)
        {
            GetCourseByID(id);
        }

        private void GetCourseByID(int id)
        {
            mId = id; //Set the object.mId to the id we want to read from the DB if available.

            //Create connection string and attempt to connect
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();
            //Attempt to read the data from server and if possible tie data from server to the object's corresponding properties.
            try
            {
                //Create reader
                SqlDataReader myReader = null;
                //Create any parameters that we need to look for in DB
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
                myParam.Value = mId;
                //Create an SQL command
                SqlCommand myCommand = new SqlCommand(@"
                    SELECT short_name, long_name, credit_hours, lecture_hours, 
                        lab_hours, official_description, active 
                    FROM Courses 
                    Where course_id = @myParam", tempConn.myConnection);
                //Add parameters to SQL command
                myCommand.Parameters.Add(myParam);

                //Begin reading from DB
                myReader = myCommand.ExecuteReader();

                //While there are more rows to read continue to do so. 
                //Due to this if there are more than one record with the same ID then the last one 
                //retrieved from the DB will be the one that will reside in the object. 
                //This should not happen if DB is done properly.
                if (!myReader.HasRows)
                    throw new Exception("No Course Recorded with the given id.");
                while (myReader.Read())
                {
                    mShortName = myReader[0].ToString();
                    mLongName = myReader[1].ToString();
                    mCreditHours = int.Parse(myReader[2].ToString());
                    mLectureHours = int.Parse(myReader[3].ToString());
                    mLabHours = int.Parse(myReader[4].ToString());
                    mOfficialDesc = myReader[5].ToString();
                    mActive = (bool)myReader[6];
                }
            }
            catch (Exception f)
            {
                //Add log exception
                throw;
            }
            finally
            {
                //Close the connection
                tempConn.CloseConnection();
            }
        }

        public static List<Course> GetListCSTCourses()
        {
            List<Course> tempList = new List<Course>(); //The list that will be returned.
            Course tempCourse = new Course();

            //Create connection string and attempt to connect
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            //Attempt to read the data from server and if possible tie data from server object's properties
            try
            {
                //Create reader variable
                SqlDataReader myReader = null;

                //Create command
                SqlCommand myCommand = new SqlCommand(@"
                    SELECT course_id, short_name, long_name, credit_hours, lecture_hours, 
                        lab_hours, official_description, active 
                    FROM Courses 
                    WHERE short_name like 'CST%' AND active=1 
                    ORDER BY short_name", tempConn.myConnection);

                //Start reading results
                myReader = myCommand.ExecuteReader();
                //While there are more rows to read continue to do so. 
                //Due to this if there are more than one record with the same ID then the last one 
                //retrieved from the DB will be the one that will reside in the object. 
                //This should not happen if DB is done properly.
                while (myReader.Read())
                {
                    tempCourse = new Course();
                    tempCourse.mId = int.Parse(myReader[0].ToString());
                    tempCourse.mShortName = myReader[1].ToString();
                    tempCourse.mLongName = myReader[2].ToString();
                    tempCourse.mCreditHours = int.Parse(myReader[3].ToString());
                    tempCourse.mLectureHours = int.Parse(myReader[4].ToString());
                    tempCourse.mLabHours = int.Parse(myReader[5].ToString());
                    tempCourse.mOfficialDesc = myReader[6].ToString();
                    tempCourse.mActive = (bool)myReader[7];

                    tempList.Add(tempCourse);
                }
            }
            catch (Exception f)
            {
                //Add log exception
                throw;
            }
            finally
            {
                //Close the connection
                tempConn.CloseConnection();
            }

            return tempList;
        }
    }
}