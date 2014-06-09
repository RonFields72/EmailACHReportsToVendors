using System;
using System.Configuration;
using System.IO;
using System.Text;
using EmailACHReportsToVendors.Email;
using EmailACHReportsToVendors.DAL;
using EmailACHReportsToVendors.Files;


namespace EmailACHReportsToVendors
{
    class EmailACHReports
    {
        /// <summary>
        /// Email ACH Reports Console Application Entry Point
        /// <param name="args">Command line arguments</param>
        /// <returns></returns>
        /// </summary>
        public static void Main()
        {
            //const string emailSubject = "EFT Deposit - Southwestern Energy Company";
            //StringBuilder emailBodyText = new StringBuilder();
            //emailBodyText.Append("Attention Vendor:");
            //emailBodyText.Append(Environment.NewLine);
            //emailBodyText.Append("Attached is a report containing the invoice detail information related to an upcoming EFT deposit in your bank account. ");
            //emailBodyText.Append("The date of deposit will appear in the body of the report.");
            //emailBodyText.Append(Environment.NewLine);
            //emailBodyText.Append(Environment.NewLine);
            //emailBodyText.Append("If you have questions regarding this report, please contact our Accounts Payable department at 479-582-8630.");
            //string emailBody = System.Convert.ToString(emailBodyText);


            string exceptionEmail = ConfigurationManager.AppSettings.Get("APExceptionEmailAddress");
            string testString = "hello world";

            //Get output paths
            string achDirectory = ConfigurationManager.AppSettings.Get("ACHReportDirectory");
            string auditDirectory = ConfigurationManager.AppSettings.Get("AuditReportDirectory");
            string achArchiveDir = ConfigurationManager.AppSettings.Get("ACHArchiveDirectory");
        
            // Check to see if ACH directory exists on the server
            if (FileHelper.DirectoryExists(achDirectory)) 
            {
                //Create a list of PDF's from the directory
                DirectoryInfo dir = new DirectoryInfo(achDirectory);
                FileInfo[] fileInfo = dir.GetFiles("*.pdf");
                string achFileName;
                string vendorEmailAddress;
                string vendorNumber;
                ODSProvider ods = new ODSProvider();   
      
            // For each PDF the vendor number must be extracted from the file name
            // Once the number is retrieved then the vendors email address will be pulled from
            // ODS and the PDF file will be emailed
            foreach (FileInfo achPDFFile in fileInfo)
            {
                achFileName = achPDFFile.Name;
                //Get the vendor number
                vendorNumber = FileHelper.ParseVendorNumber(achFileName);

                // Get the vendor's email address
                vendorEmailAddress = ods.GetVendorEmail(vendorNumber);
                 
                // If the email address is not found then the PDF will still be attached to an email
                // but it will be sent to the exception inbox
                //if (String.IsNullOrEmpty(vendorEmailAddress))
                if (!EmailHelper.IsValidEmailAddress(vendorEmailAddress)) 
                    
                {
                    EmailHelper.SendMailMessage(null,exceptionEmail, null, null, emailSubject, emailBody, achPDFFile.FullName);
                 
                    // Archive the pdf report that was emailed to the exception inbox
                    FileHelper.ArchiveException(achPDFFile, achArchiveDir);
                }
                else
                {
                    // Send the email with attached report (*.pdf) to the vendor
                    EmailHelper.SendMailMessage(null, vendorEmailAddress, null, null, emailSubject, emailBody, achPDFFile.FullName);
                     
                    // Archive the pdf report that was emailed to the vendor
                    FileHelper.ArchiveNormal(achPDFFile, achArchiveDir);
                }
                                 
                                 
            }
            
         }
 
        
        // Check to see if Audit directory exists on the server
        if (FileHelper.DirectoryExists(auditDirectory)) 
        {
            const string auditSubject = "EFT Deposit - Internal Review";
            StringBuilder auditBodyText = new StringBuilder();
            auditBodyText.Append("Attention Reviewer:");
            auditBodyText.Append(Environment.NewLine);
            auditBodyText.Append("Attached is a summary report containing information related to upcoming EFT payments that will be made by SWN. ");
            auditBodyText.Append("The date of payments will appear in the body of the report.");
            auditBodyText.Append(Environment.NewLine);
            string auditBody = System.Convert.ToString(auditBodyText);

            // Get outlook distribution list name
            string auditEmailGroup = ConfigurationManager.AppSettings.Get("ACHReviewEmailList");
             
            // Create a list of PDF's from the directory
            DirectoryInfo dir = new DirectoryInfo(auditDirectory);
            FileInfo[] fileInfo = dir.GetFiles("*.pdf");

            // Each PDF in the audit directory will be emailed to the members of the ACH Review Outlook 
            // distribution list and then archive it to the appropriate folder
            // file name format = [date]_[sequence number]_APVR17.pdf
            foreach (FileInfo auditPDFFile in fileInfo) 
             {
                // Send email with attachment to members of the ACH review group
                EmailHelper.SendMailMessage(null, auditEmailGroup, null, null, auditSubject, auditBody, auditPDFFile.FullName);

                // Archive the file in the same folder as the detail reports
                FileHelper.ArchiveNormal(auditPDFFile, achArchiveDir);
             }
         }
    
     }    

    }
}
