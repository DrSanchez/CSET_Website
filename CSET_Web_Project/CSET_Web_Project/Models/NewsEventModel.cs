/*******************************************************************************
 *	Class:		NewsEvent
 *
 *	Purpose:	To store the data associated with a NewsEvent in the database.
 *
 *	Manager Functions:
 *		NewsEvent()
 *		    Constructs an empty NewsEvent.
 *		NewsEvent(int id)
 *		    Constructs a NewsEvent from information in the database at the
 *		    given id.
 *		    
 *	Mutators:
 *	    FillEventById(int id)
 *	        Fills the NewsEvent from information in the database at the given id.
 * 
 *  Methods:
 *		GetListEventsByType(int typeId)
 *		    Returns a list of all NewsEvents in the database that are of a given
 *		    type and do not expire prior to the current date.
 *		GetListEventsByTypeIgnoreDate(int typeId)
 *		    Returns a list of all NewsEvents in the database that are of a given
 *		    type.
 *		InsertNewsEvent(string title, string subtitle, string message,
 *		        string picture, DateTime startDate, DateTime endDate,
 *		        int userID, int type, bool notify)
 *		    Inserts a new NewsEvent into the database with the given information.
 *		CommitChanges(int userId)
 *		    If this NewsEvent was loaded from the database then all changes to
 *		    this NewsEvent will be synced to the database.
 *		Delete()
 *		    If this NewsEvent was loaded from the database then the corresponding
 *		    entry in the database will be deleted.
 *		GetTypes()
 *		    Returns a list of the different types of NewsEvents in the database.
 *		
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace CSET_Web_Project.Models
{
    public class NewsEvent
    {
        #region Members - Properties of the NewEvent class.
        private int mId;
        private string mTitle;
        private string mSubtitle;
        private string mMessage;
        private DateTime mStartDate;
        private DateTime mEndDate;
        private int mType;
        private string mUserName;
        private Image mPicture;
        private bool mWasFilled;
        private bool mAlumni;

        public int Id
        { get { return mId; } }
        public string Title
        {
            get { return mTitle; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    mTitle = value;
            }
        }
        public string Subtitle
        {
            get { return mSubtitle; }
            set { mSubtitle = value; }
        }
        public string Message
        {
            get { return mMessage; }
            set { mMessage = value; }
        }
        public DateTime StartDate
        {
            get { return mStartDate; }
        }
        public DateTime EndDate
        {
            get { return mEndDate; }
            set
            {
                if (value > mStartDate)
                {
                    mEndDate = value;
                }
            }
        }
        public string UserName
        { get { return mUserName; } }
        public Image Picture
        { get { return mPicture; } }
        public bool Alumni
        {
            get { return mAlumni; }
            set { mAlumni = value; }
        }
        public int Type
        { get { return mType; } }
        #endregion

        public NewsEvent()
        { mWasFilled = false; }

        public NewsEvent(int id)
        {
            mWasFilled = false;
            FillEventById(id);
        }

        private void FillEventById(int id)
        {
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@Param", SqlDbType.Int);
                myParam.Value = id;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT      news_events.id, news_events.title, news_events.subtitle, news_events.message, news_events.start_date, news_events.end_date, 
                                news_events.image_name, users.first_name, users.last_name, news_events.notify, news_events.type
                    FROM        news_events INNER JOIN
                                    users ON news_events.[user] = users.id
                    WHERE       (news_events.id = @Param)",
                    tempConn.myConnection);
                myCommand.Parameters.Add(myParam);

                myReader = myCommand.ExecuteReader();
                if (!myReader.HasRows)
                    throw new Exception("No record exists of a NewsEvent with the given id.");
                while (myReader.Read())
                {
                    mId = int.Parse(myReader[0].ToString());
                    mTitle = myReader[1].ToString();
                    mSubtitle = myReader[2].ToString();
                    mMessage = myReader[3].ToString();
                    mStartDate = DateTime.Parse(myReader[4].ToString());
                    mEndDate = DateTime.Parse(myReader[5].ToString());
                    mPicture = new Image(myReader[6].ToString());
                    mUserName = myReader[7].ToString() + " " + myReader[8].ToString();
                    //mAlumni = bool.Parse(myReader[9].ToString());
                    mAlumni = false;
                    mType = int.Parse(myReader[10].ToString());
                    mWasFilled = true;
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

        public static List<NewsEvent> GetListEventsByType(int typeId)
        {
            List<NewsEvent> tempList = new List<NewsEvent>();

            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@Param", SqlDbType.Int);
                myParam.Value = typeId;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT      id
                    FROM        news_events
                    WHERE       (type = @Param) AND (end_date > GETDATE()) AND (start_date < GETDATE())
                    ORDER BY    start_date DESC",
                    tempConn.myConnection);
                myCommand.Parameters.Add(myParam);

                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    tempList.Add(new NewsEvent(int.Parse(myReader[0].ToString())));
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

        public static List<NewsEvent> GetListEventsByTypeIgnoreDate(int typeId)
        {
            List<NewsEvent> tempList = new List<NewsEvent>();

            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@Param", SqlDbType.Int);
                myParam.Value = typeId;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT      id
                    FROM        news_events
                    WHERE       (type = @Param)
                    ORDER BY    start_date DESC",
                    tempConn.myConnection);
                myCommand.Parameters.Add(myParam);

                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    tempList.Add(new NewsEvent(int.Parse(myReader[0].ToString())));
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

        public static void InsertNewsEvent(string title, string subtitle, string message,
            string picture, DateTime startDate, DateTime endDate, int userID, int type,
            bool notify)
        {
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlParameter lTitle = new SqlParameter("@title", SqlDbType.VarChar);
                lTitle.Value = title;
                SqlParameter lSub = new SqlParameter("@subtitle", SqlDbType.VarChar);
                lSub.Value = subtitle;
                SqlParameter lMess = new SqlParameter("@message", SqlDbType.Text);
                lMess.Value = message;
                SqlParameter lStart = new SqlParameter("@start", SqlDbType.DateTime);
                lStart.Value = startDate;
                SqlParameter lEnd = new SqlParameter("@end", SqlDbType.DateTime);
                lEnd.Value = endDate;
                SqlParameter lUser = new SqlParameter("@user", SqlDbType.Int);
                lUser.Value = userID;
                SqlParameter lType = new SqlParameter("@type", SqlDbType.Int);
                lType.Value = type;
                SqlParameter lNot = new SqlParameter("@notify", SqlDbType.Bit);
                lNot.Value = notify;
                SqlParameter lPic = new SqlParameter("@picture", SqlDbType.VarChar);
                lPic.Value = picture;

                SqlCommand myCommand = new SqlCommand(@"
                    INSERT INTO news_events (title, subtitle, message, start_date, end_date, [user], type, notify, image_name)
                    VALUES (@title, @subtitle, @message, @start, @end, @user, @type, @notify, @picture)", tempConn.myConnection);
                myCommand.Parameters.Add(lTitle);
                myCommand.Parameters.Add(lSub);
                myCommand.Parameters.Add(lMess);
                myCommand.Parameters.Add(lStart);
                myCommand.Parameters.Add(lEnd);
                myCommand.Parameters.Add(lUser);
                myCommand.Parameters.Add(lType);
                myCommand.Parameters.Add(lNot);
                myCommand.Parameters.Add(lPic);

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

        public void CommitChanges(int userId)
        {
            if (mWasFilled)
            {
                ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
                tempConn.OpenConnection();

                try
                {
                    SqlParameter lId = new SqlParameter("@id", SqlDbType.Int);
                    lId.Value = mId;
                    SqlParameter lTitle = new SqlParameter("@title", SqlDbType.VarChar);
                    lTitle.Value = mTitle;
                    SqlParameter lSub = new SqlParameter("@subtitle", SqlDbType.VarChar);
                    lSub.Value = mSubtitle;
                    SqlParameter lMess = new SqlParameter("@message", SqlDbType.Text);
                    lMess.Value = mMessage;
                    SqlParameter lStart = new SqlParameter("@start", SqlDbType.DateTime);
                    lStart.Value = mStartDate;
                    SqlParameter lEnd = new SqlParameter("@end", SqlDbType.DateTime);
                    lEnd.Value = mEndDate;
                    SqlParameter lUser = new SqlParameter("@user", SqlDbType.Int);
                    lUser.Value = userId;
                    SqlParameter lType = new SqlParameter("@type", SqlDbType.Int);
                    lType.Value = mType;
                    SqlParameter lNot = new SqlParameter("@notify", SqlDbType.Bit);
                    lNot.Value = mAlumni;
                    SqlParameter lPic = new SqlParameter("@picture", SqlDbType.VarChar);
                    lPic.Value = mPicture;

                    SqlCommand myCommand = new SqlCommand(@"
                    UPDATE  news_events
                    SET     title = @title, subtitle = @subtitle, message = @message, start_date = @start,
                            end_date = @end, [user] = @user, type = @type, notify = @notify, image_name = @picture
                    WHERE   (id = @id)", tempConn.myConnection);
                    myCommand.Parameters.Add(lTitle);
                    myCommand.Parameters.Add(lSub);
                    myCommand.Parameters.Add(lMess);
                    myCommand.Parameters.Add(lStart);
                    myCommand.Parameters.Add(lEnd);
                    myCommand.Parameters.Add(lUser);
                    myCommand.Parameters.Add(lType);
                    myCommand.Parameters.Add(lNot);
                    myCommand.Parameters.Add(lPic);
                    myCommand.Parameters.Add(lId);

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
            else
            {
                throw new Exception("Update Failed Because Object not created from DB");
            }
        }

        public void Delete()
        {
            if (mWasFilled)
            {
                ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
                tempConn.OpenConnection();

                try
                {
                    SqlParameter lId = new SqlParameter("@id", SqlDbType.Int);
                    lId.Value = mId;

                    SqlCommand myCommand = new SqlCommand(@"
                    DELETE FROM news_events
                    WHERE   (id = @id)", tempConn.myConnection);
                    myCommand.Parameters.Add(lId);

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
            else
            {
                throw new Exception("Delete Failed Because Object not created from DB");
            }
        }

        public static List<SelectListItem> GetTypes()
        {
            List<SelectListItem> temp = new List<SelectListItem>();

            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT  id, description
                    FROM    news_types", tempConn.myConnection);

                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    temp.Add(new SelectListItem
                    {
                        Text = myReader[1].ToString(),
                        Value = myReader[0].ToString()
                    });
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