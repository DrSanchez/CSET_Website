/*******************************************************************************
 *	Class:		Lab
 *
 *	Purpose:	To store the information associated with a lab from the database.
 *
 *	Manager Functions:
 *		Lab()
 *		    Constructs an empty Lab.
 *		Lab(int labId)
 *		    Constructs the lab with the information from the database
 *		    corresponding to given id.
 *		Lab(string labNumber)
 *		    Constructs the lab with the information from the database
 *		    correspoding to the given lab name.
 *		    
 *	Mutators:
 *	    GetLabByID(int labId)
 *	        Fills the lab information from the database entries associated with
 *	        the given id.
 *	    GetLabByName(string labName)
 *	        Fills the lab information from the database entries associated with
 *	        the given lab name.
 *	    GetLabByNumber(int id)
 *	        Fills the lab information from the database entries associated with
 *	        the given lab number.
 *	    GetNameOfClassesDB(int labID)
 *	        Fills the array classNames from the database entries associated
 *	        with the given id and then fills backgroundColor based on the
 *	        information in classNames.
 * 
 *  Methods:
 *		GetListLabs(int labId)
 *		    Returns a list of all labs that are in the database.
 *		GetIDLabFromNameDB(string labName)
 *		    Returns the id of the lab associated with the given lab name in the
 *		    database.
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
    public class Lab
    {
        #region Members - Properties of the Lab class.
        //properties of Lab
        private string[,] classNames = new string[15, 7];
        private string[,] backgroundColor = new string[15, 7];

        //access points of Lab
        public string[,] ClassNames { get { return classNames; } }
        public string[,] BackgroundColor { get { return backgroundColor; } }
        #endregion

        public Lab()
        {
            for (int r = 0; r < 15; r++)
                for (int c = 0; c < 7; c++)
                {
                    classNames[r, c] = "CST 162";
                    backgroundColor[r, c] = "#FFF000";
                }
        }

        public Lab(int labId)
        {
            for (int r = 0; r < 15; r++)
                for (int c = 0; c < 7; c++)
                {
                    classNames[r, c] = "CST 162";
                    backgroundColor[r, c] = "#FFF000";
                }
            GetLabByID(labId);
        }

        public Lab(string labName)
        {
            for (int r = 0; r < 15; r++)
                for (int c = 0; c < 7; c++)
                {
                    classNames[r, c] = "CST 162";
                    backgroundColor[r, c] = "#FFF000";
                }
            GetLabByName(labName);
        }

        private void GetLabByID(int labId)
        {
            GetNameOfClassesDB(labId);
        }

        private void GetLabByName(string labName)
        {
            int labID = GetIDLabFromNameDB(labName);
            GetNameOfClassesDB(labID);
        }

        private void GetLabByNumber(int id)
        {
            string temp = "PV " + id;
            GetLabByName(temp);
        }

        public static List<SelectListItem> GetListLabs(int labId)
        {
            List<SelectListItem> listOfLabs = new List<SelectListItem>();

            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetlabs");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;

                SqlCommand myCommand = new SqlCommand("SELECT Lab, LabID FROM Lab ORDER BY Lab", tempConn.myConnection);

                myReader = myCommand.ExecuteReader();

                int tempCheck = 0;
                if (!myReader.HasRows)
                    throw new Exception("No records exist for labs in the database.");
                while (myReader.Read())
                {
                    tempCheck = int.Parse(myReader[1].ToString());
                    if (tempCheck != labId)
                    {
                        listOfLabs.Add(new SelectListItem
                        {
                            Text = myReader[0].ToString(),
                            Value = myReader[1].ToString()
                        });
                    }
                    else
                    {
                        listOfLabs.Add(new SelectListItem
                        {
                            Text = myReader[0].ToString(),
                            Value = myReader[1].ToString(),
                            Selected = true
                        });
                    }
                }
            }
            catch (Exception f)
            {
                //Add log exceptions
                throw;
            }
            finally
            {
                tempConn.CloseConnection();
            }

            return listOfLabs;
        }

        private void GetNameOfClassesDB(int labID)
        {
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetlabs");
            tempConn.OpenConnection();

            try
            {

                SqlDataReader myReader = null;
                SqlParameter myParam3 = new SqlParameter("@Param3", SqlDbType.Int);
                myParam3.Value = labID;

                SqlCommand myCommand = new SqlCommand("SELECT DayWeekID, TimeDayID, Status FROM TimeBlock Where LabID = @Param3", tempConn.myConnection);
                myCommand.Parameters.Add(myParam3);

                myReader = myCommand.ExecuteReader();
                int r = 0,
                    c = 0;
                if (!myReader.HasRows)
                    throw new Exception("No records exist for a lab with the given id.");
                while (myReader.Read())
                {
                    r = int.Parse(myReader[1].ToString()) - 1;
                    c = int.Parse(myReader[0].ToString()) - 1;
                    classNames[r, c] = myReader[2].ToString();
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

            for (int r = 0; r < 15; r++)
                for (int c = 0; c < 7; c++)
                {
                    if (classNames[r, c] == "Closed")
                    {
                        backgroundColor[r, c] = "#990000";
                    }
                    else if (classNames[r, c] == "Open")
                    {
                        backgroundColor[r, c] = "#006600";
                    }
                    else
                    {
                        backgroundColor[r, c] = "#0000FF";
                    }
                }
        }

        private int GetIDLabFromNameDB(string labName)
        {
            int labID = 1;

            //Connect to DB
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetlabs");
            tempConn.OpenConnection();

            //Attempt to read the data from server and if possible tie data from server to properties
            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam1 = new SqlParameter("@Param1", SqlDbType.Char);
                myParam1.Value = labName;

                SqlCommand myCommand = new SqlCommand("SELECT LabID FROM Lab Where Lab = @Param1", tempConn.myConnection);
                myCommand.Parameters.Add(myParam1);

                myReader = myCommand.ExecuteReader();
                if (!myReader.HasRows)
                    throw new Exception("No records exist for a Lab with the given name/number.");
                while (myReader.Read())
                {
                    labID = int.Parse(myReader[0].ToString());
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

            if (labID == 0)
                labID = 1;

            return labID;
        }
    }
}