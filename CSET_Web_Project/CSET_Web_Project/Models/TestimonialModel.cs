/*******************************************************************************
 *	Class:		Testimonial
 *
 *	Purpose:	To retrieve and store the data associated with a testimonial.
 *
 *	Manager Functions:
 *		Testimonial(int id)
 *		    Constructs the Testimonial from the database with the given id.		    
 * 
 *	Mutators:
 *	    FillTestimonialById(int id)
 *	        Fills the Testimonial from the database with the given id.
 *	    FillDegrees()
 *	        Fills the Testimonial's degree listing from the database with the
 *	        current Testimonial's id.
 * 
 *  Methods:
 *	    GetTestimonials()
 *	        Returns a list of all Testimonials to display on the website.
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
    public class Testimonial
    {
        #region Members - Properties for the Testimonial class
        private int mId;
        private string mFirstName;
        private string mLastName;
        private string mTestimonial;
        private int mGradYear;
        private string mEmployer;
        private Image mPicture;
        private List<String> mDegrees;

        public string FullName
        { get { return mFirstName + " " + mLastName; } }
        public string FirstName
        { get { return mFirstName; } }
        public string LastName
        { get { return mLastName; } }
        public string TestimonialText
        { get { return mTestimonial; } }
        public int GradYear
        { get { return mGradYear; } }
        public string Employer
        { get { return mEmployer; } }
        public Image Picture
        { get { return mPicture; } }
        public List<String> Degrees
        { get { return mDegrees; } }
        #endregion

        public Testimonial(int id = 2)
        {
            FillTestimonialById(id);
        }

        private void FillTestimonialById(int id)
        {
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
                myParam.Value = id;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT  StuTID, Fname, Lname, Testimonial, GradYear, Employer, PictureURL, ThumbURL
                    FROM    askstu_Testimonial
                    WHERE   (StuTID = @myParam)", tempConn.myConnection);
                myCommand.Parameters.Add(myParam);

                myReader = myCommand.ExecuteReader();
                if (!myReader.HasRows)
                    throw new Exception("No record exists of a Testimonial with the given id.");
                while (myReader.Read())
                {
                    mId = int.Parse(myReader[0].ToString());
                    mFirstName = myReader[1].ToString();
                    mLastName = myReader[2].ToString();
                    mTestimonial = myReader[3].ToString();
                    mGradYear = int.Parse(myReader[4].ToString());
                    mEmployer = myReader[5].ToString();
                    mPicture = new Image(myReader[6].ToString());
                    //mThumb = myReader[7].ToString();
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
            FillDegrees();
        }

        private void FillDegrees()
        {
            mDegrees = new List<String>();

            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
                myParam.Value = mId;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT  askstu_Major.Mname
                    FROM    askstu_MajorLevel INNER JOIN
                                askstu_Major ON askstu_MajorLevel.MajorID = askstu_Major.MajorID
                    WHERE   (askstu_MajorLevel.StuTID = @myParam)", tempConn.myConnection);
                myCommand.Parameters.Add(myParam);

                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    mDegrees.Add(myReader[0].ToString());
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

        public static List<Testimonial> GetTestimonials()
        {
            List<Testimonial> temp = new List<Testimonial>();

            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT        StuTID
                    FROM            askstu_Testimonial
                    ORDER BY GradYear DESC", tempConn.myConnection);

                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    temp.Add(new Testimonial(int.Parse(myReader[0].ToString())));
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