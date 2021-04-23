using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Day26_EmployeePayrollADO.Net
{
    public class EmployeeRepository
    {
        public static SqlConnection connection { get; set; }
        /// UC 2 : Gets all employees details.
        public void GetFullTableDetails()
        {
            string query = @"select * from dbo.employee_payroll";
            GetAllEmployees(query);
        }
        public void GetAllEmployees(string query)
        {
            //Creates a new connection for every method to avoid "ConnectionString property not initialized" exception
            DBConnection dbc = new DBConnection();
            connection = dbc.GetConnection();
            EmployeeModel model = new EmployeeModel();
            try
            {
                using (connection)
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            model.EmployeeID = reader.GetInt32(0);
                            model.EmployeeName = reader.GetString(1);
                            model.StartDate = reader.GetDateTime(2);
                            model.Gender = reader.GetString(3);
                            model.PhoneNumber = reader.GetInt64(4);
                            model.Address = reader.GetString(5);
                            model.Department = reader.GetString(6);
                            model.BasicPay = reader.GetDouble(7);
                            model.Deductions = reader.GetDouble(8);
                            model.TaxablePay = reader.GetDouble(9);
                            model.Income_Tax = reader.GetDouble(10);
                            model.NetPay = reader.GetDouble(11);
                            Console.WriteLine("EmpId:{0}\nEmpName:{1}\nStartDate:{2}\nGender:{3}\nPhoneNumber:{4}\nAddress:{5}\nDepartment:{6}\nBasicPay:{7}\nDeductions:{8}\nTaxablePay:{9}\nTax:{10}\nNetPay:{11}", model.EmployeeID, model.EmployeeName, model.StartDate.ToShortDateString(), model.Gender, model.PhoneNumber, model.Address, model.Department, model.BasicPay, model.Deductions, model.TaxablePay, model.Income_Tax, model.NetPay);
                            Console.WriteLine("\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found");
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State.Equals("Open"))
                    connection.Close();
            }
        }
        /// Adds the employee.
        public bool AddEmployee(EmployeeModel model)
        {
            DBConnection dbc = new DBConnection();
            connection = dbc.GetConnection();
            try
            {
                using (connection)
                {
                    SqlCommand command = new SqlCommand("dbo.SpAddEmployeeDetails", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@name", model.EmployeeName);
                    command.Parameters.AddWithValue("@start", model.StartDate);
                    command.Parameters.AddWithValue("@gender", model.Gender);
                    command.Parameters.AddWithValue("@phone_number", model.PhoneNumber);
                    command.Parameters.AddWithValue("@address", model.Address);
                    command.Parameters.AddWithValue("@department", model.Department);
                    command.Parameters.AddWithValue("@Basic_Pay", model.BasicPay);
                    command.Parameters.AddWithValue("@Deductions", model.Deductions);
                    command.Parameters.AddWithValue("@Taxable_pay", model.TaxablePay);
                    command.Parameters.AddWithValue("@Income_tax", model.Income_Tax);
                    command.Parameters.AddWithValue("@Net_pay", model.NetPay);
                    connection.Open();
                    var result = command.ExecuteNonQuery();
                    connection.Close();
                    if (result != 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State.Equals("Open"))
                    connection.Close();
            }
        }
        /// UC 3 : Updates the given empname with given salary into database.
        public bool UpdateSalaryIntoDatabase(string empName, double basicPay)
        {
            DBConnection dbc = new DBConnection();
            connection = dbc.GetConnection();
            try
            {
                using (connection)
                {
                    connection.Open();
                    string query = @"update dbo.employee_payroll set BasicPay=@p1 where EmpName=@p2";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@p1", basicPay);
                    command.Parameters.AddWithValue("@p2", empName);
                    var result = command.ExecuteNonQuery();
                    connection.Close();
                    if (result != 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        /// UC 4 : Reads the updated salary from database.
        public double ReadUpdatedSalaryFromDatabase(string empName)
        {
            DBConnection dbc = new DBConnection();
            connection = dbc.GetConnection();
            try
            {
                using (connection)
                {
                    string query = @"select BasicPay from dbo.employee_payroll where EmpName=@p";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    command.Parameters.AddWithValue("@p", empName);
                    return (Double)command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State.Equals("Open"))
                    connection.Close();
            }
        }
        /// UC 5 : Gets the employees details for a particular date range.
        public void GetEmployeesFromForDateRange(string date)
        {
            string query = $@"select * from dbo.employee_payroll where StartDate between cast('{date}' as date) and cast(getdate() as date)";
            GetAllEmployees(query);
        }
        /// UC 6 : Finds the grouped by gender data.
        public void FindGroupedByGenderData()
        {
            DBConnection dbc = new DBConnection();
            connection = dbc.GetConnection();
            try
            {
                using (connection)
                {
                    string query = @"select Gender,count(BasicPay) as EmpCount,min(BasicPay) as MinSalary,max(BasicPay) as MaxSalary,sum(BasicPay) as SalarySum,avg(BasicPay) as AvgSalary from dbo.employee_payroll where Gender='M' or Gender='F' group by Gender";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string gender = reader[0].ToString();
                            int empCount = reader.GetInt32(1);
                            double minSalary = reader.GetDouble(2);
                            double maxSalary = reader.GetDouble(3);
                            double salarySum = reader.GetDouble(4);
                            double avgSalary = reader.GetDouble(5);
                            Console.WriteLine($"Gender:{gender}\nEmpCount:{empCount}\nMinSalary:{minSalary}\nMaxSalary:{maxSalary}\nSalarySum:{salarySum}\nAvgSalary:{avgSalary}\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Data found");
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State.Equals("Open"))
                    connection.Close();
            }
        }
        /// UC 7 : Inserts data into multiple tables using transactions.
        public void InsertIntoMultipleTablesWithTransactions()
        {
            DBConnection dbc = new DBConnection();
            connection = dbc.GetConnection();

            Console.WriteLine("Enter EmployeeID");
            int empID = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter Name:");
            string empName = Console.ReadLine();

            DateTime startDate = DateTime.Now;

            Console.WriteLine("Enter Address:");
            string address = Console.ReadLine();

            Console.WriteLine("Enter Gender:");
            string gender = Console.ReadLine();

            Console.WriteLine("Enter PhoneNumber:");
            double phonenumber = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Enter BasicPay:");
            int basicPay = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter Deductions:");
            int deductions = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter TaxablePay:");
            int taxablePay = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter Tax:");
            int tax = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter NetPay:");
            int netPay = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter CompanyId:");
            int companyId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter CompanyName:");
            string companyName = Console.ReadLine();

            Console.WriteLine("Enter DeptId:");
            int deptId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter DeptName:");
            string deptName = Console.ReadLine();

            using (connection)
            {
                connection.Open();

                // Start a local transaction.
                SqlTransaction sqlTran = connection.BeginTransaction();

                // Enlist a command in the current transaction.
                SqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;

                try
                {
                    // Execute 1st command
                    command.CommandText = "insert into companies values(@company_id,@company_name)";
                    command.Parameters.AddWithValue("@company_id", companyId);
                    command.Parameters.AddWithValue("@company_name", companyName);
                    command.ExecuteScalar();

                    // Execute 2nd command
                    command.CommandText = "insert into employees values(@emp_id,@EmpName,@gender,@phone_number,@address,@startDate,@company_id)";
                    command.Parameters.AddWithValue("@emp_id", empID);
                    command.Parameters.AddWithValue("@EmpName", empName);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@gender", gender);
                    command.Parameters.AddWithValue("@phone_number", phonenumber);
                    command.Parameters.AddWithValue("@address", address);
                    command.ExecuteScalar();

                    // Execute 3rd command
                    command.CommandText = "insert into payrolls values(@emp_id,@Basic_Pay,@Deductions,@Taxable_pay,@Income_tax,@Net_pay)";
                    command.Parameters.AddWithValue("@Basic_Pay", basicPay);
                    command.Parameters.AddWithValue("@Deductions", deductions);
                    command.Parameters.AddWithValue("@Taxable_pay", taxablePay);
                    command.Parameters.AddWithValue("@Income_tax", tax);
                    command.Parameters.AddWithValue("@Net_pay", netPay);
                    command.ExecuteScalar();

                    // Execute 4th command
                    command.CommandText = "insert into departments values(@dept_id,@dept_name)";
                    command.Parameters.AddWithValue("@dept_id", deptId);
                    command.Parameters.AddWithValue("@dept_name", deptName);
                    command.ExecuteScalar();

                    // Execute 5th command
                    command.CommandText = "insert into employee_depts values(@emp_id,@dept_id)";
                    command.ExecuteNonQuery();

                    // Commit the transaction after all commands.
                    sqlTran.Commit();
                    Console.WriteLine("All records were added into the database.");
                }
                catch (Exception ex)
                {
                    // Handle the exception if the transaction fails to commit.
                    Console.WriteLine(ex.Message);
                    try
                    {
                        // Attempt to roll back the transaction.
                        sqlTran.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        // Throws an InvalidOperationException if the connection
                        // is closed or the transaction has already been rolled
                        // back on the server.
                        Console.WriteLine(exRollback.Message);
                    }
                }
            }
        }
        /// UC 8 : Retrieves the employee details from multiple tables after implementing E-R concept.
        public void RetrieveEmployeeDetailsFromMultipleTables()
        {
            DBConnection dbc = new DBConnection();
            connection = dbc.GetConnection();
            EmployeeModel model = new EmployeeModel();
            string query = "select e.EmpId,e.EmpName,e.StartDate,e.Gender,e.PhoneNo,e.Address,p.NetPay,d.DeptName from employees e,payrolls p, departments d,employee_depts ed where e.EmpId = p.EmpId and ed.EmpId = e.EmpId and ed.DeptId = d.DeptId";
            try
            {
                using (connection)
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            model.EmployeeID = reader.GetInt32(0);
                            model.EmployeeName = reader.GetString(1);
                            model.StartDate = reader.GetDateTime(2);
                            model.Gender = reader.GetString(3);
                            model.PhoneNumber = reader.GetInt64(4);
                            model.Address = reader.GetString(5);
                            model.NetPay = reader.GetDouble(6);
                            model.Department = reader.GetString(7);
                            Console.WriteLine($"EmpId:{model.EmployeeID}\nEmpName:{model.EmployeeName}\nStartDate:{model.StartDate}\nGender:{model.Gender}\nPhoneNo:{model.PhoneNumber}\nAddress:{model.Address}\nNetPay:{model.NetPay}\nDepartment:{model.Department}");
                            Console.WriteLine("\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found");
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State.Equals("Open"))
                    connection.Close();
            }
        }
    }
}