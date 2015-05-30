/*******************************************************************************
 *	Class:		Image
 *
 *	Purpose:	To store the information associated with an image from the
 *	            database.
 *
 *	Manager Functions:
 *		Image(int id)
 *		    Constructs the image from the database.
 * 
 *	Mutators:
 *	    GetImageById(int id)
 *	        Sets the image from the database.
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
    public class Image
    {
        #region Members - Properties for the Image class
        private int mId;
        private string mFilename;
        private string mCaption;
        private string mThumbFile;
        private string mPath;

        public int ID
        { get { return mId; } }
        public string Filename
        { get { return mFilename; } }
        public string ThumbFile
        { get { return mThumbFile; } }
        public string Caption
        { get { return mCaption; } }
        public string Path
        { get { return mPath; } }
        public string PathName
        { get { return mPath + mFilename; } }
        public string PathThumb
        { get { return mPath + mThumbFile; } }
        #endregion

        public Image(int id = 20)
        {
            GetImageById(id);
        }

        public Image(string filename)
        {
            if(!String.IsNullOrEmpty(filename))
                GetImageByFilename(filename);
        }

        private void GetImageByFilename(string filename)
        {
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.VarChar);
                myParam.Value = filename;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT  PhotoID, Caption, Filename, Thumbnail, Path
                    FROM    photos_Images
                    WHERE   (Filename = @myParam)", tempConn.myConnection);

                myCommand.Parameters.Add(myParam);

                myReader = myCommand.ExecuteReader();

                if (!myReader.HasRows)
                    throw new Exception("No image exists in the db with the given id.");

                while (myReader.Read())
                {
                    mId = int.Parse(myReader[0].ToString());
                    mCaption = myReader[1].ToString();
                    mFilename = myReader[2].ToString();
                    mThumbFile = myReader[3].ToString();
                    mPath = myReader[4].ToString();
                    if (string.IsNullOrEmpty(mThumbFile))
                        mThumbFile = mFilename;
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

        private void GetImageById(int id)
        {
            mId = id;
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlDataReader myReader = null;
                SqlParameter myParam = new SqlParameter("@myParam", SqlDbType.Int);
                myParam.Value = id;

                SqlCommand myCommand = new SqlCommand(@"
                    SELECT  Caption, Filename, Thumbnail, Path
                    FROM    photos_Images
                    WHERE   (PhotoID = @myParam)", tempConn.myConnection);

                myCommand.Parameters.Add(myParam);

                myReader = myCommand.ExecuteReader();

                if (!myReader.HasRows)
                    throw new Exception("No image exists in the db with the given id.");

                while (myReader.Read())
                {
                    mCaption = myReader[0].ToString();
                    mFilename = myReader[1].ToString();
                    mThumbFile = myReader[2].ToString();
                    mPath = myReader[3].ToString();
                    if (string.IsNullOrEmpty(mThumbFile))
                        mThumbFile = mFilename;
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

        public static void InsertImage(int album, string caption, string filename, string thumb, string path)
        {
            ConnectSovereignDB tempConn = new ConnectSovereignDB("csetweb");
            tempConn.OpenConnection();

            try
            {
                SqlParameter lCaption = new SqlParameter("@caption", SqlDbType.VarChar);
                lCaption.Value = caption;
                SqlParameter lFilename = new SqlParameter("@filename", SqlDbType.VarChar);
                lFilename.Value = filename;
                SqlParameter lThumb = new SqlParameter("@thumb", SqlDbType.VarChar);
                lThumb.Value = thumb;
                SqlParameter lPath = new SqlParameter("@path", SqlDbType.VarChar);
                lPath.Value = path;
                SqlParameter lAlbum = new SqlParameter("@album", SqlDbType.Int);
                lAlbum.Value = album;

                SqlCommand myCommand = new SqlCommand(@"
                    INSERT INTO photos_Images (AlbumID, Caption, Filename, Thumbnail, Path)
                    VALUES (@album, @caption, @filename, @thumb, @path)", tempConn.myConnection);
                myCommand.Parameters.Add(lCaption);
                myCommand.Parameters.Add(lFilename);
                myCommand.Parameters.Add(lThumb);
                myCommand.Parameters.Add(lPath);
                myCommand.Parameters.Add(lAlbum);

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