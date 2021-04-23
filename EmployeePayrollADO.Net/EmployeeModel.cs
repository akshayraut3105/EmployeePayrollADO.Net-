using System;
using System.Collections.Generic;
using System.Text;

namespace Day26_EmployeePayrollADO.Net
{
    public class EmployeeModel
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public double PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public string Gender { get; set; }
        public double BasicPay { get; set; }
        public double Deductions { get; set; }
        public double TaxablePay { get; set; }
        public double Income_Tax { get; set; }
        public double NetPay { get; set; }
        public DateTime StartDate { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int DeptID { get; set; }
        public string DeptName { get; set; }
    }
}