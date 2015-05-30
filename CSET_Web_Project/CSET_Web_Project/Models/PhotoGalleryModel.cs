/*******************************************************************************
 *	Class:		PhotoGallery
 *
 *	Purpose:	To store the data associated with a Photo Gallery in the database.
 *
 *	Manager Functions:
 *		PhotoGallery()
 *		    Constructs an empty PhotoGallery.
 *		PhotoGallery(int id)
 *		    Constructs a PhotoGallery from the database at the given id.
 *		    
 *	Mutators:
 *	    GetListPhotos()
 *	        Fills mPhotoIds with the ids for the photos of this Gallery from the
 *	        database.
 *	    GetCaptionPublic()
 *	        Fills mCation and mPublic from the database.
 * 
 *  Methods:
 *		GetAllGalleries()
 *		    Returns a list of all PhotoGalleries in the database.
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
    public class PhotoGallery
    {
        #region Members - Properties for the PhotoGallery class
        private int mId;
        private List<Image> mPhotos;
        private string mCaption;
        private bool mIsPublic;

        public int Id
        { get { return mId; } }
        public List<Image> Photos
        { get { return mPhotos; } }
        public string Caption
        { get { return mCaption; } }
        public bool IsPublic
        { get { return mIsPublic; } }
        #endregion

        public PhotoGallery()
        { }

        public PhotoGallery(int id)
        {
            mId = id;
            GetListPhotos();
            GetCaptionPublic();
        }

        private void GetListPhotos()
        {
            mPhotos = new List<Image>();

            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
                myParam.Value = mId;
                SqlCommand myCommand = new SqlCommand(
                    @"SELECT PhotoID
                    FROM photos_Images
                    WHERE AlbumID = @myParam",
                    tempConn.myConnection);
                myCommand.Parameters.Add(myParam);
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    mPhotos.Add(new Image(int.Parse(myReader[0].ToString())));
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

        private void GetCaptionPublic()
        {
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
                myParam.Value = mId;
                SqlCommand myCommand = new SqlCommand(
                    @"SELECT Caption, IsPublic
                    FROM photos_Albums
                    WHERE AlbumID = @myParam",
                    tempConn.myConnection);
                myCommand.Parameters.Add(myParam);
                myReader = myCommand.ExecuteReader();
                if (!myReader.HasRows)
                    throw new Exception("No record exists of a PhotoGallery with the given id.");
                while (myReader.Read())
                {
                    mCaption = myReader[0].ToString();
                    mIsPublic = (bool)myReader[1];
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

        public static List<PhotoGallery> GetAllGalleries()
        {
            List<PhotoGallery> tempList = new List<PhotoGallery>();

            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlCommand myCommand = new SqlCommand(
                    @"SELECT AlbumID
                    FROM photos_Albums
                    WHERE (IsPublic='true')",
                    tempConn.myConnection);
                myReader = myCommand.ExecuteReader();
                PhotoGallery tempGallery = new PhotoGallery();
                while (myReader.Read())
                {
                    tempGallery = new PhotoGallery(int.Parse(myReader[0].ToString()));
                    tempList.Add(tempGallery);
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