using System.Data.SqlClient; 
using System.Configuration;
using EmailACHReportsToVendors.DAL;

namespace EmailACHReportsToVendors.DAL
{
    public class ODSProvider : DataAccessBase
    {
      
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ODSProvider"/> class.
        /// </summary>
        public ODSProvider()
        {
            this.ConnectionString = ConfigurationManager.ConnectionStrings["cnODS"].ConnectionString;
        }

        #endregion

        #region Public Members
        /// <summary>
        /// Gets the vendor email.
        /// </summary>
        /// <returns>Vendor email address.</returns>
        public string GetVendorEmail(string vendorNumber)
        {
            // Build query string
            string sqlString = "SELECT Entity_ID, Entity_Type, Email_Address FROM Entity_Contact WHERE Entity_Type = 'VENDOR' and Entity_ID = '" + vendorNumber + "'";
            string emailAddress = "";

            // Create and open the connection to ODS 
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlString, connection))
                {
                    SqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        // Retrieve the email address
                        emailAddress = dr["Email_Address"].ToString();
                    }
                    dr.Close();
                }
                
            }

            return emailAddress;
            
        }

        #endregion

       
    }
}