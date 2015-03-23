/*******************************************************************************
 *	Class:		PrintQuota
 *
 *	Purpose:	To store the data associated with a PrintQuota in the database.
 *
 *	Manager Functions:
 *		PrintQuota()
 *		    Constructs an empty PrintQuota.
 *		PrintQuota(string userName, string domain)
 *		    Constructs a PrintQuota from the database using the given information.
 * 
 *	Mutators:
 *	    GetQuotaInfo(string userName, string domain)
 *	        Fills the PrintQuota from the database using the given information.
 * 
 *  Methods:
 *		GetDomains(string currentlySelected = "")
 *		    Returns a list of all domains from the database that a user could
 *		    be a member of.
 *		
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace csetMVC2011.Models
{
    //Need to find way to secure the access that is made here so that the user has to input his/her password to view the data
    //Right now all you need to know is someone's username to access their print quota.
    //Due to the above this is still UNDER CONSTRUCTION but it is at least partially finished
    public class PrintQuota
    {
        #region Members - Properties for the PrintQuota class.
        private string mUserName;
        private int mTotalPagesPrinted;
        private double mBalance;
        private int mPagesLeft;
        private double mPaidBalance;
        private int mPaidPagesLeft;

        public string UserName
        {
            get { return mUserName; }
            //set { mUserName = value; }
        }
        public int TotalPagesPrinted
        {
            get { return mTotalPagesPrinted; }
        }
        public double Balance
        {
            get { return mBalance; }
            //set { mBalance = value; }
        }
        public int PagesLeft
        {
            get { return mPagesLeft; }
        }
        public double PaidBalance
        {
            get { return mPaidBalance; }
        }
        public int PaidPagesLeft
        {
            get { return mPaidPagesLeft; }
        }
        #endregion

        public PrintQuota()
        {
            mUserName = "";
            mTotalPagesPrinted = 0;
            mBalance = 0;
            mPagesLeft = 0;
            mPaidBalance = 0;
            mPaidPagesLeft = 0;
        }

        public PrintQuota(string userName, string domain)
        {
            GetQuotaInfo(userName, domain);
        }

        private void GetQuotaInfo(string userName, string domain)
        {
            ConnectSovereignDB tempConn = new ConnectSovereignDB("PrintManager");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.NVarChar);
                myParam.Value = userName;
                SqlCommand myCommand = new SqlCommand(
                    @"SELECT     UserName, TotalPagesPrinted, Balance, PaidBalance
                    FROM         UserQuotas
                    WHERE        (UserName = @myParam)",
                    tempConn.myConnection);
                myCommand.Parameters.Add(myParam);
                myReader = myCommand.ExecuteReader();
                if (!myReader.HasRows)
                    throw new Exception("No record exists of a PrintQuota with the given information.");
                while (myReader.Read())
                {
                    mUserName = myReader["UserName"].ToString();
                    mTotalPagesPrinted = int.Parse(myReader["TotalPagesPrinted"].ToString());
                    mBalance = Math.Round(double.Parse(myReader["Balance"].ToString()), 2);
                    mPagesLeft = (int)Math.Round((mBalance / .05));
                    mPaidBalance = Math.Round(double.Parse(myReader["PaidBalance"].ToString()), 2);
                    mPaidPagesLeft = (int)Math.Round((mPaidBalance / .05));
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

        public static List<SelectListItem> GetDomains(string currentlySelected = "")
        {
            List<SelectListItem> tempList = new List<SelectListItem>();

            ConnectSovereignDB tempConn = new ConnectSovereignDB("PrintManager");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;

                SqlCommand myCommand = new SqlCommand(
                    @"SELECT DISTINCT DomainName 
                    FROM UserQuotas",
                    tempConn.myConnection);

                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    if (myReader["DomainName"].ToString() != currentlySelected)
                    {
                        tempList.Add(new SelectListItem
                        {
                            Text = myReader["DomainName"].ToString(),
                            Value = myReader["DomainName"].ToString()
                        });
                    }
                    else
                    {
                        tempList.Add(new SelectListItem
                        {
                            Text = myReader["DomainName"].ToString(),
                            Value = myReader["DomainName"].ToString(),
                            Selected = true
                        });
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
                tempConn.CloseConnection();
            }

            return tempList;
        }
    }
}