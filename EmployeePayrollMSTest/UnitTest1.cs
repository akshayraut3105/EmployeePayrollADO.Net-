using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day26_EmployeePayrollADO.Net;
namespace EmployeePayrollMSTest
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// UC 4 : Given the update salary value check if the database got updated.
        /// </summary>
        [TestMethod]
        public void GivenUpdateSalaryValue_CheckIfTheDatabaseGotUpdated()
        {
            //Arrange
            string empName = "Teressa";
            double basicPay = 30000;
            EmployeeRepository empRepository = new EmployeeRepository();
            EmployeeModel empModel = new EmployeeModel();
            //Act
            empRepository.UpdateSalaryIntoDatabase(empName, basicPay);
            double expectedPay = empRepository.ReadUpdatedSalaryFromDatabase(empName);
            //Assert
            Assert.AreEqual(basicPay, expectedPay);
        }
    }
}