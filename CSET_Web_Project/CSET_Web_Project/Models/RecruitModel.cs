/*******************************************************************************
 *	Class:		Recruit
 *
 *	Purpose:	To add new recruits to the database. ------------WILL CHANGE-----------
 *
 *	Manager Functions:
 *		
 * 
 *	Mutators:
 *	    
 * 
 *  Methods:
 *		InsertRecruit(string first, string last, string street,
 *              string city, string state, string zip, string phone, string email,
 *              string school)
 *          Adds a new recruit to the dabase with the given information.
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
    public class Recruit
    {
        #region Members - Properties for the Recruit class.
        #endregion

        public static void InsertRecruit(string first, string last, string street,
            string city, string state, string zip, string phone, string email,
            string school)
        {
            //Create connection string and attempt to connect
            ConnectSovereignDB tempConn = new ConnectSovereignDB("Recruiting");
            tempConn.OpenConnection();

            try
            {
                SqlParameter fName = new SqlParameter("@first", SqlDbType.VarChar);
                fName.Value = first;
                SqlParameter lName = new SqlParameter("@last", SqlDbType.VarChar);
                lName.Value = last;
                SqlParameter emailP = new SqlParameter("@email", SqlDbType.VarChar);
                emailP.Value = email;
                SqlParameter streetP = new SqlParameter("@street", SqlDbType.VarChar);
                streetP.Value = street;
                SqlParameter cityP = new SqlParameter("@city", SqlDbType.VarChar);
                cityP.Value = city;
                SqlParameter stateP = new SqlParameter("@state", SqlDbType.Char);
                stateP.Value = state;
                SqlParameter zipP = new SqlParameter("@zip", SqlDbType.VarChar);
                zipP.Value = zip;
                SqlParameter phoneP = new SqlParameter("@phone", SqlDbType.VarChar);
                phoneP.Value = phone;

                SqlCommand myCommand = new SqlCommand(@"
                    INSERT INTO Tour (FirstName, LastName, Street, City, State, Zip, Phone, Email)
                    VALUES (@first, @last, @street, @city, @state, @zip, @phone, @email)", tempConn.myConnection);
                myCommand.Parameters.Add(fName);
                myCommand.Parameters.Add(lName);
                myCommand.Parameters.Add(emailP);
                myCommand.Parameters.Add(streetP);
                myCommand.Parameters.Add(cityP);
                myCommand.Parameters.Add(stateP);
                myCommand.Parameters.Add(zipP);
                myCommand.Parameters.Add(phoneP);

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
    }
}