using System.Net.Mail;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace EmailACHReportsToVendors.Email
{
    public class EmailHelper
    {

        /// <summary>
        /// This helper class sends an email message using the System.Net.Mail namespace
        /// </summary>
        /// <param name="fromEmail">Sender email address</param>
        /// <param name="toEmail">Recipient email address</param>
        /// <param name="bcc">Blind carbon copy email address</param>
        /// <param name="cc">Carbon copy email address</param>
        /// <param name="subject">Subject of the email message</param>
        /// <param name="body">Body of the email message</param>
        /// <param name="attachment">File to attach</param>

        #region Static Members

        public static void SendMailMessage(string fromEmail, string toEmail, string bcc, string cc, string subject, string body, string attachment)
        {
            //  Create the MailMessage object
            MailMessage mMailMessage = new MailMessage();

            //  Set the sender address of the mail message
            if (!string.IsNullOrEmpty(fromEmail))
            {
                mMailMessage.From = new MailAddress(fromEmail);
            }

            //  Set the recipient address of the mail message
            mMailMessage.To.Add(new MailAddress(toEmail));
            
            //  Check if the bcc value is nothing or an empty string
            if (!string.IsNullOrEmpty(bcc))
            {
                mMailMessage.Bcc.Add(new MailAddress(bcc));
            }

            //  Check if the cc value is nothing or an empty value
            if (!string.IsNullOrEmpty(cc))
            {
                mMailMessage.CC.Add(new MailAddress(cc));
            }

            //  Set the subject of the mail message
            mMailMessage.Subject = subject;

            //  Set the body of the mail message
            mMailMessage.Body = body;

            //  Set the format of the mail message body
            mMailMessage.IsBodyHtml = false;

            // Set the priority
            mMailMessage.Priority = MailPriority.Normal;

            // Add any attachments from the filesystem
            Attachment mailAttachment = new Attachment(attachment);
            mMailMessage.Attachments.Add(mailAttachment);
                     
            //  Create the SmtpClient instance
            SmtpClient mSmtpClient = new SmtpClient();

            //  Send the mail message
            mSmtpClient.Send(mMailMessage);
            mailAttachment.Dispose();

        }

        /// <summary>
        /// Determines whether an email address is valid.
        /// </summary>
        /// <param name="emailAddress">The email address to validate.</param>
        /// <returns>
        /// 	<c>true</c> if the email address is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmailAddress(string emailAddress)
        {
            // An empty or null string is not valid
            if (String.IsNullOrEmpty(emailAddress))
            {
                return (false);
            }

            // Regular expression to match valid email address
            string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                                  
            // Match the email address using a regular expression
            Regex re = new Regex(emailRegex);
            if (re.IsMatch(emailAddress))
                return (true);
            else
                return (false);
        }

        #endregion

    }
}
