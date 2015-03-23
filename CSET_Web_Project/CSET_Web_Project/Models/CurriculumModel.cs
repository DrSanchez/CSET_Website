/*******************************************************************************
 *	Class:		Curriculum
 *
 *	Purpose:	To model the curriculum data for a given year of a given degree.
 *
 *	Manager Functions:
 *		Curriculum()
 *		    Constructs the Curriculum to an empty state.
 *		Curriculum(int degreeId, int yearId)
 *		    Constructs the Curriculum to have the data associated with the
 *		    given degree and year.
 *		
 *	Mutators:
 *	    FillCurriculum(int degreeId, int yearId)
 *	        Sets the Curriculum to have the data associated with the given
 *	        degree and year.
 *          
 *  Methods:
 *		GetYears(int degreeId, int selectedYear)
 *		    Returns a list of the available years for the given degree. Mainly
 *		    used for drop down lists on webpages.
 *		
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;

namespace csetMVC2011.Models
{
    public class Curriculum
    {
        #region Members - Properties for the Curriculum class.
        private List<Course> mFallTerm;
        private List<Course> mWinterTerm;
        private List<Course> mSpringTerm;

        public List<Course> FallTerm
        {
            get { return mFallTerm; }
        }
        public List<Course> WinterTerm
        {
            get { return mWinterTerm; }
        }
        public List<Course> SpringTerm
        {
            get { return mSpringTerm; }
        }
        #endregion

        public Curriculum()
        {
            mFallTerm = new List<Course>();
            mWinterTerm = new List<Course>();
            mSpringTerm = new List<Course>();
        }

        public Curriculum(int degreeId, int yearId)
        {
            mFallTerm = new List<Course>();
            mWinterTerm = new List<Course>();
            mSpringTerm = new List<Course>();
            FillCurriculum(degreeId, yearId);
        }

        public static List<SelectListItem> GetYears(int degreeId, int selectedYear)
        {
            List<SelectListItem> tempYears = new List<SelectListItem>();

            if (selectedYear == 1)
                tempYears.Add(new SelectListItem
                {
                    Text = "Freshman",
                    Value = "1",
                    Selected = true
                });
            else
                tempYears.Add(new SelectListItem
                {
                    Text = "Freshman",
                    Value = "1"
                });

            if (selectedYear == 2)
                tempYears.Add(new SelectListItem
                {
                    Text = "Sophomore",
                    Value = "2",
                    Selected = true
                });
            else
                tempYears.Add(new SelectListItem
                {
                    Text = "Sophomore",
                    Value = "2"
                });

            if (degreeId != 3 && degreeId != 4)
            {
                if (selectedYear == 3)
                    tempYears.Add(new SelectListItem
                    {
                        Text = "Junior",
                        Value = "3",
                        Selected = true
                    });
                else
                    tempYears.Add(new SelectListItem
                    {
                        Text = "Junior",
                        Value = "3"
                    });

                if (selectedYear == 4)
                    tempYears.Add(new SelectListItem
                    {
                        Text = "Senior",
                        Value = "4",
                        Selected = true
                    });
                else
                    tempYears.Add(new SelectListItem
                    {
                        Text = "Senior",
                        Value = "4"
                    });
            }

            if (degreeId == 6)
            {
                if (selectedYear == 5)
                    tempYears.Add(new SelectListItem
                    {
                        Text = "Super Senior",
                        Value = "5",
                        Selected = true
                    });
                else
                    tempYears.Add(new SelectListItem
                    {
                        Text = "Super Senior",
                        Value = "5"
                    });
            }

            return tempYears;
        }

        public void FillCurriculum(int degreeId, int yearId)
        {
            mFallTerm.Clear();
            mWinterTerm.Clear();
            mSpringTerm.Clear();
            //Create connection string and attempt to connect
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            //Attempt to read the data from server and if possible tie data to object's properties
            try
            {
                //Create reader variable
                SqlDataReader myReader = null;
                //Create Parameters we need
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
                myParam.Value = degreeId;
                SqlParameter myParam2 = new SqlParameter("@myParam2", SqlDbType.Int);
                myParam2.Value = yearId;

                //create command variable
                SqlCommand myCommand = new SqlCommand(@"
                    SELECT CoursesInDegrees.course_id, CoursesInDegrees.degree_id, 
                        CoursesInDegrees.year_id, CoursesInDegrees.quarter_id
                    FROM CoursesInDegrees 
                    INNER JOIN Courses ON CoursesInDegrees.course_id = Courses.course_id
                    WHERE (CoursesInDegrees.degree_id = @myParam) AND (CoursesInDegrees.year_id = @myParam2) 
                    ORDER BY CoursesInDegrees.quarter_id, Courses.short_name", tempConn.myConnection);
                //add parameters to command
                myCommand.Parameters.Add(myParam);
                myCommand.Parameters.Add(myParam2);

                int tempCheck = 0;

                //Begin reading results row by row
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    tempCheck = int.Parse(myReader[3].ToString());
                    switch (tempCheck)
                    {
                        case 1:
                            mFallTerm.Add(new Course(int.Parse(myReader[0].ToString())));
                            break;
                        case 2:
                            mWinterTerm.Add(new Course(int.Parse(myReader[0].ToString())));
                            break;
                        case 3:
                            mSpringTerm.Add(new Course(int.Parse(myReader[0].ToString())));
                            break;
                        default:
                            throw new Exception("A course could not be sorted into a term.");
                    }
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
    }
}