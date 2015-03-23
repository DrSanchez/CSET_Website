/*******************************************************************************
 *	Class:		Degree
 *
 *	Purpose:	To model a Degree from the database.
 *
 *	Manager Functions:
 *		Degree(int id)
 *		    Constructs the Degree with the associated data from
 *		    the database.
 *		
 *	Mutators:
 *	    GetDegreeById(int id)
 *	        Fills the Degree with the associated data from the database.
 *          
 *  Methods:
 *		GetListOfDegrees(int currentDegreeId = 1)
 *		    Returns a list of degrees that can be used for drop down lists.
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
	public class Degree
	{
		#region Members - Properties for the Degree class.
		private int mId;
		private string mShortName;
		private string mLongName;
		private string mOfficialDesc;
		private string mShortNameText;
		private bool mActive;
		private DegreeType mDegreeType;

		public string ShortName
		{
			get { return mShortName; }
			set { mShortName = value; }
		}
		public string LongName
		{
			get { return mLongName; }
			set { mLongName = value; }
		}
		public string OfficialDesc
		{
			get { return mOfficialDesc; }
			set { mOfficialDesc = value; }
		}
		public string ShortNameTxt
		{
			get { return mShortNameText; }
			set { mShortNameText = value; }
		}
		public bool Active
		{
			get { return mActive; }
			set { mActive = value; }
		}
		public DegreeType TypeOfDegree
		{
			get { return mDegreeType; }
			set { mDegreeType = value; }
		}
		#endregion

		//public Degree(int id)
		//{
		//	GetDegreeById(id);
		//}

//		public void GetDegreeById(int id)
//		{
//			mId = id;

//			ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
//			tempConn.OpenConnection();

//			try
//			{
//				SqlDataReader myReader = null;

//				SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
//				myParam.Value = mId;

//				SqlCommand myCommand = new SqlCommand(@"
//                    SELECT short_name, long_name, official_description, active, degree_type_id, 
//                        short_name_text 
//                    FROM Degrees 
//                    WHERE degree_id = @myParam", tempConn.myConnection);
//				myCommand.Parameters.Add(myParam);

//				myReader = myCommand.ExecuteReader();
//				if (!myReader.HasRows)
//					throw new Exception("No Degree is recorded with the given id.");
//				while (myReader.Read())
//				{
//					mShortName = myReader[0].ToString();
//					mLongName = myReader[1].ToString();
//					mOfficialDesc = myReader[2].ToString();
//					mActive = (bool)myReader[3];
//					mDegreeType = new DegreeType(int.Parse(myReader[4].ToString()));
//					mShortNameText = myReader[5].ToString();
//				}
//			}
//			catch (Exception f)
//			{
//				//Add log exception
//				throw;
//			}
//			finally
//			{
//				tempConn.CloseConnection();
//			}
//		}

//		public static List<SelectListItem> GetListOfDegrees(int currentDegreeId = 1)
//		{
//			List<SelectListItem> tempList = new List<SelectListItem>();

//			ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
//			tempConn.OpenConnection();

//			try
//			{
//				SqlDataReader myReader = null;

//				SqlCommand myCommand = new SqlCommand(@"
//                    SELECT short_name_text, degree_id 
//                    FROM Degrees", tempConn.myConnection);

//				myReader = myCommand.ExecuteReader();

//				int tempCheck = 0;

//				while (myReader.Read())
//				{
//					tempCheck = int.Parse(myReader[1].ToString());
//					if (tempCheck != currentDegreeId)
//					{
//						tempList.Add(new SelectListItem
//						{
//							Text = myReader[0].ToString(),
//							Value = myReader[1].ToString()
//						});
//					}
//					else
//					{
//						tempList.Add(new SelectListItem
//						{
//							Text = myReader[0].ToString(),
//							Value = myReader[1].ToString(),
//							Selected = true
//						});
//					}
//				}
//			}
//			catch (Exception f)
//			{
//				//Add log exception
//				throw;
//			}
//			finally
//			{
//				tempConn.CloseConnection();
//			}
//			return tempList;
//		}
	}
}