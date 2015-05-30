using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace CSET_Web_Project.Models
{
    //This class was created to simplify connecting to the Server SOVEREIGN
    /* Use instructions:
     * 1) Instantiate a ConnectSovereignDB variable feeding it the catalogue you want to open.
     *      Note: Catalogue is synonymous with a specific database in this case. Ex. csetlabs, csetweb, or PrintManager.
     * 2) call OpenConnection to open the connection to the DB
     * 3) use connection variable to do read and/or write opperations on the DB
     *      Note: remember to enclose your opperations with a try catch statement
     * 4) call CloseConnection to close the connection to the DB
     */
    public class ConnectSovereignDB
    {
        private string catalogue; // The catalogue to connect to
        public SqlConnection myConnection; // A public connection variable so that code outside of this class can use the connection

        public string Catalogue { get { return catalogue; } set { catalogue = Catalogue; } }


        //Constructor
        //catalogue is required because that will change every time the program connects to the DB.
        public ConnectSovereignDB(string catalogue)
        {
            this.catalogue = catalogue;
        }

        //Method
        //Used to actually connect to the Server and DB
        public void OpenConnection()
        {
            //string temp = "data source=SOVEREIGN-NEW; initial catalog=" + catalogue + "; integrated security=no; user id=sa; pwd=archer->;";
			string temp = "Data Source=DRSANCHEZ;Initial Catalog=csetweb;Integrated Security=True";
            //Create connection string and attempt to connect
            myConnection = new SqlConnection(temp);
            try
            {
                myConnection.Open();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.ToString());
            }
        }

        //Method
        //Used to safely close the connection to the DB and Server
        public void CloseConnection()
        {
            //Close the connection
            try
            {
                myConnection.Close();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.ToString());
            }
        }
    }
}