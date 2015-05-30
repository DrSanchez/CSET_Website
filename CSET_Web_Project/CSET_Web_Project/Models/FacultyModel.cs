/*******************************************************************************
 *	Class:		Faculty
 *
 *	Purpose:	To model the data associated with a Faculty member.
 *
 *	Manager Functions:
 *		Faculty()
 *		    Constructs an empty Faculty object.
 *		Faculty(int facultyId)
 *		    Constructs the Faculty from data in the database.
 *		
 *	Mutators:
 *	    FillFacultyById(int id)
 *	        Fills the Faculty from data in the database.
 *      FillListOfCoursesTaught()
 *          Fills mCoursesTaught with the Courses that this Faculty teaches.
 *      FillListOfDegrees()
 *          Fills mDegrees with the strings representing the degrees that
 *          this Faculty has earned.
 * 
 *  Methods:
 *		GetAllActiveFaculty()
 *		    Returns a list of all Faculty who are still marked as active in
 *		    the database.
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
    public class Faculty
    {
        #region Members - Properties for the Faculty class.
        private int mFacultyId;
        private int mUserId;
        private string mFirstName;
        private string mLastName;
        private string mPreferredName;
        private string mTitle;
        private string mPhone;
        private string mEmail;
        private List<String> mDegrees;
        private Image mImage;
        private string mOffice;
        private string mWebsite;
        private bool mActive;
        private List<Course> mCoursesTaught;

        public int FacultyId
        { get { return mFacultyId; } }
        public int UserId
        { get { return mUserId; } }
        public string FirstName
        { get { return mFirstName; } }
        public string LastName
        { get { return mLastName; } }
        public string PreferredName
        { 
            get { return mPreferredName; }
            set { mPreferredName = value; }
        }
        public string Title
        {
            get { return mTitle; }
            set { mTitle = value; }
        }
        public string Phone
        {
            get { return mPhone; }
            set { mPhone = value; }
        }
        public string Email
        { 
            get { return mEmail; }
            set { mEmail = value; }
        }
        public List<String> Degrees
        { get { return mDegrees; } }
        public Image ImageFile
        { get { return mImage; } }
        public string Office
        {
            get { return mOffice; }
            set { mOffice = value; }
        }
        public string Website
        {
            get { return mWebsite; }
            set { mWebsite = value; }
        }
        public bool Active
        { get { return mActive; } }
        public List<Course> CoursesTaught
        { get { return mCoursesTaught; } }
        #endregion

        public Faculty()
        { }

        public Faculty(int facultyId)
        {
            FillFacultyById(facultyId);
        }

        private void FillFacultyById(int id)
        {
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
                myParam.Value = id;

                SqlCommand myCommand = new SqlCommand(
                    @"SELECT UserID, FirstName, LastName, PreferredName, Title, Phone, 
                        Email, Degrees, ImageFile, Office, Website, Active 
                    FROM Faculty 
                    WHERE FacultyID = @myParam", tempConn.myConnection);
                myCommand.Parameters.Add(myParam);

                myReader = myCommand.ExecuteReader();
                if (!myReader.HasRows)
                    throw new Exception("No record for a Faculty with the given id exists.");
                while (myReader.Read())
                {
                    mFacultyId = id;
                    mUserId = int.Parse(myReader[0].ToString());
                    mFirstName = myReader[1].ToString();
                    mLastName = myReader[2].ToString();
                    mPreferredName = myReader[3].ToString();
                    mTitle = myReader[4].ToString();
                    mPhone = myReader[5].ToString();
                    mEmail = myReader[6].ToString();
                    //mDegrees = myReader[7].ToString();
                    mImage = new Image(myReader[8].ToString());
                    //mThumbFile = "th_" + mImageFile;
                    mOffice = myReader[9].ToString();
                    mWebsite = myReader[10].ToString();
                    mActive = (bool)myReader[11];
                    FillListOfCoursesTaught();
                    FillListOfDegrees();
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

        private void FillListOfCoursesTaught()
        {
            mCoursesTaught = new List<Course>();

            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
                myParam.Value = mFacultyId;

                SqlCommand myCommand = new SqlCommand(@"SELECT CourseID
                                                        FROM Faculty_Course
                                                        WHERE FacultyID = @myParam", tempConn.myConnection);
                myCommand.Parameters.Add(myParam);

                myReader = myCommand.ExecuteReader();
                Course tempCourse = null;
                while (myReader.Read())
                {
                    tempCourse = new Course(int.Parse(myReader[0].ToString()));
                    mCoursesTaught.Add(tempCourse);
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

        private void FillListOfDegrees()
        {
            mDegrees = new List<string>();

            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
                myParam.Value = mFacultyId;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT      Degree, Year
                    FROM        Faculty_Degrees
                    WHERE       (FacultyID = @myParam)
                    ORDER BY    Year DESC", tempConn.myConnection);
                myCommand.Parameters.Add(myParam);

                myReader = myCommand.ExecuteReader();
                string temp = "";
                string year = "";
                while (myReader.Read())
                {
                    temp = myReader[0].ToString();
                    year = myReader[1].ToString();
                    if (!string.IsNullOrEmpty(year))
                        temp += " (" + year + ")";
                    mDegrees.Add(temp);
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

        public static List<Faculty> GetAllActiveFaculty()
        {
            List<Faculty> tempList = new List<Faculty>();

            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;

                SqlCommand myCommand = new SqlCommand(
                    @"SELECT    FacultyID
                    FROM        Faculty
                    WHERE       Active = 'true'", tempConn.myConnection);

                myReader = myCommand.ExecuteReader();
                Faculty tempFaculty = null;
                while (myReader.Read())
                {
                    tempFaculty = new Faculty(int.Parse(myReader[0].ToString()));
                    tempList.Add(tempFaculty);
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

            return tempList;
        }
    }
}