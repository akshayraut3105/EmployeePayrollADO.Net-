using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Day26_EmployeePayrollADO.Net
{
    class DBConnection
    {
        /// Gets the connection.
        public SqlConnection GetConnection()
        {
            //For sql authentication
            string connectionString = @"Server=DESKTOP-TSL9UFS; Initial Catalog =payroll_services; User ID = akshay; Password=1997";
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

    }
}