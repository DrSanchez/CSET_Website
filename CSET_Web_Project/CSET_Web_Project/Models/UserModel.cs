using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace CSET_Web_Project.Models
{

    //UNDER CONSTRUCTION!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //May need to be scraped and started again from scratch.
    public class UserModel
    {
        private string m_loginName;
        private string m_firstName;
        private string m_lastName;
        private List<string> m_permissions;
        private List<Guid> m_permissionsID;
        private bool m_active;
        private string m_password;
        private Guid m_id;

        public UserModel()
        {

        }

        public UserModel(string loginName, string firstName, string lastName, List<string> permissions, bool active = true)
        {
            m_loginName = loginName;
            m_firstName = firstName;
            m_lastName = lastName;
            m_active = active;
            m_permissions = permissions;
        }

        public string loginName
        {
            get { return m_loginName; }
            set { m_loginName = value; }
        }
        public string firstName
        {
            get { return m_firstName; }
            set { m_firstName = value; }
        }
        public string lastName
        {
            get { return m_lastName; }
            set { m_lastName = value; }
        }
        public bool active
        {
            get { return m_active; }
            set { m_active = value; }
        }
        public string password
        {
            get { return m_password; }
            set { m_password = value; }
        }

        public List<UserModel> GetAllUsersFromDB()
        {
            //need to add this functionality
            List<UserModel> temp = new List<UserModel>();

            return temp;
        }

        public void GetUserByLogin(String login)
        {
            m_loginName = login;
            GetUserIDByLogin();
            GetUserByIDFromDB();
        }

        public void GetUserByID(Guid id)
        {
            m_id = id;
            GetUserByIDFromDB();
        }

        private void GetUserByIDFromDB()
        {
            //Connect to DB
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            //Attempt to read the data from server
            try
            {
                SqlDataReader myReader = null;

                SqlParameter myID = new SqlParameter("@myID", SqlDbType.UniqueIdentifier);
                myID.Value = m_id;

                SqlCommand myCommand = new SqlCommand("SELECT login_name, password, first_name, last_name, active FROM users WHERE UserId = @myID", tempConn.myConnection);

                myCommand.Parameters.Add(myID);

                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    m_loginName = myReader["login_name"].ToString();
                    m_password = myReader["password"].ToString();
                    m_firstName = myReader["first_name"].ToString();
                    m_lastName = myReader["last_name"].ToString();
                    m_active = (bool)myReader["active"];
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.ToString());
            }

            tempConn.CloseConnection();
        }

        private void GetUserIDByLogin()
        {
            //Connect to DB
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            //Attempt to read the data from server
            try
            {
                SqlDataReader myReader = null;

                SqlParameter myLogin = new SqlParameter("@myLogin", SqlDbType.VarChar);
                myLogin.Value = m_loginName;

                SqlCommand myCommand = new SqlCommand("SELECT aspnetID FROM users WHERE = @myLogin", tempConn.myConnection);

                myCommand.Parameters.Add(myLogin);

                myReader = myCommand.ExecuteReader();

                while (myReader.Read())
                {
                    m_id = (Guid)myReader["aspnetID"];
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.ToString());
            }

            tempConn.CloseConnection();
        }

        public static int GetIDByGuid(Guid id)
        {
            int temp = 1;

            //Create connection string and attempt to connect
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();


            //Attempt to read the data from server and if possible tie data from server to new list items
            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@Param", SqlDbType.UniqueIdentifier);
                myParam.Value = id;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT      id
                    FROM        users
                    WHERE       (aspnetID = @Param)", tempConn.myConnection);
                myCommand.Parameters.Add(myParam);

                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    temp = int.Parse(myReader[0].ToString());
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.ToString());
            }

            //Close the connection
            tempConn.CloseConnection();

            return temp;
        }
    }
}