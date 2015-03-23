/*******************************************************************************
 *	Class:		TutorEvenet
 *
 *	Purpose:	To retrieve and store the data associated with a TutorEvent.
 *
 *	Manager Functions:
 *		TutorEvent(int id)
 *		    Constructs the TutorEvent with the given id from the database.
 * 
 *	Mutators:
 *	    FillTutorEventById(int id)
 *	        Fills the TutorEvent from the database using the given id.
 * 
 *  Methods:
 *	    GetEventsByMonth(DateTime month)
 *	        Returns a list of all tutoring events occuring in a given month.
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
    public class TutorEvent
    {
        #region Members - Properties for the TutorEvent class.
        private int mId;
        private Tutor mTutor;
        private DateTime mDate;
        private DateTime mStart;
        private DateTime mEnd;
        private string mTitle;
        private string mLocation;

        public int ID
        { get { return mId; } }
        public Tutor TheTutor
        { get { return mTutor; } }
        public DateTime Date
        { get { return mDate; } }
        public DateTime Start
        { get { return mStart; } }
        public DateTime End
        { get { return mEnd; } }
        public string Title
        { get { return mTitle; } }
        public string Location
        { get { return mLocation; } }
        #endregion

        public TutorEvent(int id = 74)
        {
            FillTutorEventById(id);
        }

        private void FillTutorEventById(int id)
        {
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
                myParam.Value = id;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT        TutID, StuID, TutDate, StartTime, EndTime, Title, Location
                    FROM            crshelp_Tutoring
                    WHERE        (TutID = @myParam)", tempConn.myConnection);
                myCommand.Parameters.Add(myParam);

                myReader = myCommand.ExecuteReader();
                if (!myReader.HasRows)
                    throw new Exception("No record of a tutor event with the given id exists.");
                while (myReader.Read())
                {
                    mId = int.Parse(myReader[0].ToString());
                    mTutor = new Tutor(int.Parse(myReader[1].ToString()));
                    mDate = DateTime.Parse(myReader[2].ToString());
                    mStart = DateTime.Parse(myReader[3].ToString());
                    mEnd = DateTime.Parse(myReader[4].ToString());
                    mTitle = myReader[5].ToString();
                    mLocation = myReader[6].ToString();
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

        public static List<TutorEvent> GetEventsByMonth(DateTime month)
        {
            DateTime start = new DateTime(month.Year, month.Month, 1);
            DateTime end = start.AddMonths(1);
            List<TutorEvent> temp = new List<TutorEvent>();

            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter lStart = new SqlParameter("@start", SqlDbType.DateTime);
                lStart.Value = start;
                SqlParameter lEnd = new SqlParameter("@end", SqlDbType.DateTime);
                lEnd.Value = end;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT  TutID
                    FROM    crshelp_Tutoring
                    WHERE   (TutDate > @start) AND (TutDate < @end)", tempConn.myConnection);
                myCommand.Parameters.Add(lStart);
                myCommand.Parameters.Add(lEnd);

                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    temp.Add(new TutorEvent(int.Parse(myReader[0].ToString())));
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

            return temp;
        }
    }
}