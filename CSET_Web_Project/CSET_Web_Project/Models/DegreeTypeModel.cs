/*******************************************************************************
 *	Class:		DegreeType
 *
 *	Purpose:	To model a DegreeType from the database.
 *
 *	Manager Functions:
 *		DegreeType()
 *		    Constructs the DegreeType to an empty state.
 *		DegreeType(int id)
 *		    Constructs the DegreeType with the associated data from
 *		    the database.
 *		
 *	Mutators:
 *	    GetDegreeTypeById(int id)
 *	        Fills the DegreeType with the information from the database.
 *          
 *  Methods:
 *		
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
	public class DegreeType
	{
		#region Members - Properties for the DegreeType class.
		private int mDegreeTypeId; //Id of degre type
		private string mShortName; //Short name of degree type ex. BS
		private string mLongName; //Long name of degree type ex. Bachelor of Science
		private string mDescription; //Description of degree type

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
		public string Description
		{
			get { return mDescription; }
			set { mDescription = value; }
		}
		#endregion

		public DegreeType()
		{ }

		//public DegreeType(int id)
		//{
		//	GetDegreeTypeById(id);
		//}

//		private void GetDegreeTypeById(int id)
//		{
//			mDegreeTypeId = id;

//			ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
//			tempConn.OpenConnection();

//			try
//			{
//				SqlDataReader myReader = null;
//				SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
//				myParam.Value = mDegreeTypeId;

//				SqlCommand myCommand = new SqlCommand(@"SELECT short_name, long_name, description
//                                                        FROM DegreeTypes
//                                                        WHERE degree_type_id = @myParam", tempConn.myConnection);
//				myCommand.Parameters.Add(myParam);

//				myReader = myCommand.ExecuteReader();
//				if (!myReader.HasRows)
//					throw new Exception("No record of a DegreeType with given id exists.");
//				while (myReader.Read())
//				{
//					mShortName = myReader[0].ToString();
//					mLongName = myReader[1].ToString();
//					mDescription = myReader[2].ToString();
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
	}
}