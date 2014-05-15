using System;
using System.Collections.Generic;
using System.Text;

namespace EmailACHReportsToVendors.DAL
{
                                                                                   
    /// <summary>
    /// Abstract base class for all DAL objects.
    /// </summary>    
    public abstract class DataAccessBase
    {
        #region Private Members


        /// <summary>
        /// Connection string property
        /// </summary>
        private string _connectionString = "";

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the database connection string.
        /// </summary>
        /// <value>The connection string.</value>
        protected string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        #endregion
    }
}
