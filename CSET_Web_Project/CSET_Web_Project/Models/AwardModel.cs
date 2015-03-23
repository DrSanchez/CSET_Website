/*******************************************************************************
 *	Class:		Award
 *
 *	Purpose:	To model the data associated with an Award in the Database.
 *
 *	Manager Functions:
 *		Award(int id)
 *		    Constructs the Award from the database based on the given id.
 *		
 *	Mutators:
 *	    GetAwardById(int id)
 *          Fills the Award from the database based on the given id.
 *          
 *  Methods:
 *		GetAllAwards()
 *		    Gets a List of Awards from the database and returns it.
 *		
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
    public class Award
    {
        #region Members - Properties for the AwardModel class.
        private int mAwardEntryId;
        private string mStudentName;
        private string mAwardTitle;
        private string mAwardDescription;
        private int mYear;

        public int AwardEntryId
        { get { return mAwardEntryId; } }
        public string StudentName
        { get { return mStudentName; } }
        public string AwardTitle
        { get { return mAwardTitle; } }
        public string AwardDescription
        { get { return mAwardDescription; } }
        public int Year
        { get { return mYear; } }
        #endregion

        public Award(int id = 1)
        {
            GetAwardById(id);
        }

        public void GetAwardById(int id)
        {
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                //Create reader
                SqlDataReader myReader = null;
                //Create any parameters that we need to look for in DB
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
                myParam.Value = id;
                //Create an SQL command
                SqlCommand myCommand = new SqlCommand(@"
                    SELECT        awards_Awards_Students.StudentAwardID, awards_Awards.Title, awards_Awards.Description, awards_Students.FirstName, awards_Students.LastName, 
                                awards_Awards_Students.Year, awards_Awards_Students.Description AS SpecDescription
                    FROM        awards_Awards_Students INNER JOIN
                                    awards_Students ON awards_Awards_Students.StudentID = awards_Students.StudentID INNER JOIN
                                    awards_Awards ON awards_Awards_Students.AwardID = awards_Awards.AwardID
                    WHERE       (awards_Awards_Students.StudentAwardID = @myParam)
                    ORDER BY    awards_Awards_Students.StudentAwardID", tempConn.myConnection);
                //Add parameters to SQL command
                myCommand.Parameters.Add(myParam);

                //Begin reading from DB
                myReader = myCommand.ExecuteReader();

                //While there are more rows to read continue to do so. 
                //Due to this if there are more than one record with the same ID then the last one 
                //retrieved from the DB will be the one that will reside in the object. 
                //This should not happen if DB is done properly.
                if (!myReader.HasRows)
                    throw new Exception("No Award recorded with the given id.");
                while (myReader.Read())
                {
                    mAwardEntryId = int.Parse(myReader[0].ToString());
                    mAwardTitle = myReader[1].ToString();
                    mAwardDescription = myReader[2].ToString();
                    mStudentName = myReader[3].ToString() + " " + myReader[4].ToString();
                    mYear = int.Parse(myReader[5].ToString());
                }
            }
            catch (Exception f)
            {
                //Add log exception
                throw;
            }
            finally //Always executes
            {
                //Close the connection
                tempConn.CloseConnection();
            }
        }

        public static List<Award> GetAllAwards()
        {
            List<Award> awards = new List<Award>();

            //Create connection string and attempt to connect
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                //Create reader
                SqlDataReader myReader = null;
                //Create an SQL command
                SqlCommand myCommand = new SqlCommand(@"
                    SELECT      StudentAwardID
                    FROM        awards_Awards_Students
                    ORDER BY    StudentAwardID", tempConn.myConnection);

                //Begin reading from DB
                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    awards.Add(new Award(int.Parse(myReader[0].ToString())));
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

            return awards;
        }
    }
}