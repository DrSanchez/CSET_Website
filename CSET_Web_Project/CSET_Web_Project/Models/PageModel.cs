/*******************************************************************************
 *	Class:		Page
 *
 *	Purpose:	To store the data associated with a NewsEvent in the database.
 *
 *	Manager Functions:
 *		Page(int id)
 *		    Constructs the page by id from the database.
 *		    
 *	Mutators:
 *	    GetPageById(int id)
 *	        Fills the page by id from the database.
 * 
 *  Methods:
 *		CommitChanges()
 *		    Sends the updated object back to the database.
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
    public class Page
    {
        #region Members - Properties for the Page class
        private string mText;
        private int mId;
        private bool mFromDB;

        public string Text
        {
            get { return mText; }
            set { mText = value; }
        }
        public int Id
        { get { return mId; } }
        #endregion

        public Page(int id = 1)
        {
            mFromDB = false;
            GetPageById(id);
        }

        private void GetPageById(int id)
        {
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@Param", SqlDbType.Int);
                myParam.Value = id;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT      id, text
                    FROM        Pages
                    WHERE       (id = @Param)",
                    tempConn.myConnection);
                myCommand.Parameters.Add(myParam);

                myReader = myCommand.ExecuteReader();
                if (!myReader.HasRows)
                    throw new Exception("No record exists of a Page with the given id.");
                while (myReader.Read())
                {
                    mId = int.Parse(myReader[0].ToString());
                    mText = myReader[1].ToString();
                }
                mFromDB = true;
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

        public void CommitChanges()
        {
            if (mFromDB)
            {
                ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
                tempConn.OpenConnection();

                try
                {
                    SqlParameter lId = new SqlParameter("@id", SqlDbType.Int);
                    lId.Value = mId;
                    SqlParameter lText = new SqlParameter("@text", SqlDbType.Text);
                    lText.Value = mText;

                    SqlCommand myCommand = new SqlCommand(@"
                    UPDATE  Pages
                    SET     text = @text
                    WHERE   (id = @id)", tempConn.myConnection);
                    myCommand.Parameters.Add(lText);
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
    }
}