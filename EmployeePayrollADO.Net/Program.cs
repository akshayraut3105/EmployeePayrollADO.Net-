using System;

namespace Day26_EmployeePayrollADO.Net
{
    class Program
    {
        static void Main(string[] args)
        {
            EmployeeRepository repository = new EmployeeRepository();

            //UC 8 
            repository.RetrieveEmployeeDetailsFromMultipleTables();
        }

        /// Adds the new employee into the database.
        public static void AddNewEmployee()
        {
            EmployeeRepository repository = new EmployeeRepository();
            EmployeeModel model = new EmployeeModel();
            model.EmployeeName = "sam";
            model.Address = "chennai";
            model.BasicPay = 50000;
            model.Deductions = 4000;
            model.Department = "Sales";
            model.Gender = "M";
            model.PhoneNumber = 983798;
            model.NetPay = 66000;
            model.Income_Tax = 3200;
            model.StartDate = DateTime.Now;
            model.TaxablePay = 3200;
            Console.WriteLine(repository.AddEmployee(model) ? "Record inserted successfully " : "Failed");
        }
    }
}