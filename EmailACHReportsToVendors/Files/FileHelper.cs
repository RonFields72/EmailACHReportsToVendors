using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace EmailACHReportsToVendors.Files
{
    public class FileHelper
    {
        #region Static Methods

        /// <summary>
        /// Determines if the directory exist.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>True or False.</returns>
        public static bool DirectoryExists(string directory)
        {
            if (System.IO.Directory.Exists(directory))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Gets the folder name by extracting the date portion of the file name.
        /// This is the subdirectory where the PDF file should be archived.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static string ParseFolderName(string fileName)
        {
            // Parse the date string from the file name
            // file name format = [date]_[sequence number]_[vendor number].pdf
            int strPos = 0;
            int len = fileName.IndexOf("_");
            string folder = fileName.Substring(strPos, len);

            return folder;
        }

        /// <summary>
        /// The normal PDF archival method.
        /// </summary>
        /// <param name="pdfFile">The PDF file.</param>
        /// <param name="archiveDirectory">The archive directory.</param>
        public static void ArchiveNormal(FileInfo pdfFile, string archiveDirectory)
        {
            // Create a new subdirectory in the archive directory 
            // This is based on the date of the report being archived
            DirectoryInfo di = new DirectoryInfo(archiveDirectory);
            string archiveFileName = pdfFile.Name;
            string newSubFolder = ParseFolderName(archiveFileName);
            try
            {
                di.CreateSubdirectory(newSubFolder);
            }
            catch (Exception ex)
            {
                // The folder already exists so don't create it
                
            }

            // Create destination path
            string destFullPath = archiveDirectory + "\\" + newSubFolder + "\\" + pdfFile.Name;

            // Move the file to the archive directory
            try
            {
                pdfFile.MoveTo(destFullPath);
            }
            catch (Exception ex)
            {
                // Unable to move the PDF to the archive
            }
        }


        /// <summary>
        /// Archives the exception report.
        /// The name of the PDF file is modified to make it easier to identify.
        /// </summary>
        /// <param name="pdfFile">The PDF file.</param>
        /// <param name="archiveDirectory">The archive directory.</param>
        public static void ArchiveException(FileInfo pdfFile, string archiveDirectory)
        {
        
            // Create a new subdirectory in the archive directory 
            // This is based on the date of the report being archived
            DirectoryInfo di = new DirectoryInfo(archiveDirectory);
            string archiveFileName = pdfFile.Name;
            string newSubFolder = ParseFolderName(archiveFileName);
            try
            {
                di.CreateSubdirectory(newSubFolder);
            }
            catch (Exception ex)
            {
                // The folder already exists so don't create it
            }

            // Create destination path
            // Insert _EXCEPT into file name
            // This will make it easier to identify as an exception in the archive folder
            string destFileName = archiveFileName.Insert(archiveFileName.IndexOf("."), "_EXCEPT");
            string destFullPath = archiveDirectory + "\\" + newSubFolder + "\\" + destFileName;
           
            // Move the file to the archive directory
            try
            {
                pdfFile.MoveTo(destFullPath);
            }
            catch (Exception ex)
            {
                
              
            }
        }

        /// <summary>
        /// Parse the vendor number from the PDF file name.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>Vendor number.</returns>
        public static string ParseVendorNumber(string filename)
        {
            string vendorNumber;

            // Parse the vendor number from the file name string
            // file name format = [date]_[sequence number]_[vendor number].pdf
            int endPos = filename.IndexOf(".");
            int strPos = filename.LastIndexOf("_") + 1;
            int len = endPos - strPos;
            vendorNumber = filename.Substring(strPos, len);

            return vendorNumber;
        }

  
        #endregion

    }
}
