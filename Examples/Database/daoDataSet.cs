using System;
using System.IO;

namespace Examples.Database
{
  partial class daoDataSet
  {
    private const string fileName = "data.xml";

    public void LoadFromDatabase()
    {
      if (!File.Exists(fileName)) daoDataSet.PrepareDatabase();
      this.BeginInit();
      if (this.Employees.Count > 0) this.Employees.Clear();
      if (this.Departments.Count > 0) this.Departments.Clear();
      this.ReadXml(fileName);
      this.EndInit();
    }

    public void SaveToDatabase()
    {
      this.WriteXml(fileName);
    }

    public static void PrepareDatabase()
    {
      // TODO: this method should be corrected before release
      return;
      daoDataSet ds = new daoDataSet();
      // Departments
      ds.Departments.AddDepartmentsRow(Guid.NewGuid(), "Services", false, "MM", null);
      ds.Departments.AddDepartmentsRow(Guid.NewGuid(), "Marketing", false, "MM", null);
      ds.Departments.AddDepartmentsRow(Guid.NewGuid(), "Human Resources", false, "HR", null);
      ds.Departments.AddDepartmentsRow(Guid.NewGuid(), "Financial", false, "AM", null);
      ds.Departments.AddDepartmentsRow(Guid.NewGuid(), "Purchasing", false, "SM", null);
      ds.Departments.AddDepartmentsRow(Guid.NewGuid(), "Sales", false, "MM", null);
      ds.Departments.AddDepartmentsRow(Guid.NewGuid(), "IT", false, "KT", null);
      ds.Departments.AddDepartmentsRow(Guid.NewGuid(), "Inventory", false, null, null);
      ds.Departments.AddDepartmentsRow(Guid.NewGuid(), "Paper printing", true, null, null);
      ds.Departments.AddDepartmentsRow(Guid.NewGuid(), "Quality Asurance", false, "KT", null);
      ds.Departments.AddDepartmentsRow(Guid.NewGuid(), "Insurance", false, "AM", null);
      ds.Departments.AddDepartmentsRow(Guid.NewGuid(), "Licenses", true, "AM", null);
      ds.Departments.AddDepartmentsRow(Guid.NewGuid(), "Operational", false, "AM", null);
      ds.Departments.AddDepartmentsRow(Guid.NewGuid(), "Staff", false, "HR", null);
      ds.Departments.AddDepartmentsRow(Guid.NewGuid(), "Customer Service", false, "MM", null);
      ds.Departments.AddDepartmentsRow(Guid.NewGuid(), "Organizational", false, "MN", null);
      // Employees
      var row = ds.Employees.NewEmployeesRow(); // Peter Walt
      row.EmployeeID = Guid.NewGuid();
      row.EmployeeName = "Peter Walt";
      row.DepartmentsRow = ds.Departments[1];
      row.SalaryGroup = 0;
      ds.Employees.AddEmployeesRow(row);
      row = ds.Employees.NewEmployeesRow(); // Olesea Rozhkova
      row.EmployeeID = Guid.NewGuid();
      row.EmployeeName = "Olesea Rozhkova";
      row.DepartmentsRow = ds.Departments[2];
      row.PhoneNumber = 12345678901;
      row.DateBirth = new DateTime(1977, 03, 21);
      row.SalaryGroup = 1;
      ds.Employees.AddEmployeesRow(row);
      row = ds.Employees.NewEmployeesRow(); // Sima Ludlow
      row.EmployeeID = Guid.NewGuid();
      row.EmployeeName = "Sima Ludlow";
      row.DepartmentsRow = ds.Departments[3];
      row.PhoneNumber = 12345678901;
      row.DateBirth = new DateTime(1973, 12, 1);
      row.SalaryGroup = 2;
      ds.Employees.AddEmployeesRow(row);
      row = ds.Employees.NewEmployeesRow(); // Kaur Rajvinder
      row.EmployeeID = Guid.NewGuid();
      row.EmployeeName = "Kaur Rajvinder";
      row.DepartmentsRow = ds.Departments[4];
      row.PhoneNumber = 12345678901;
      row.DateBirth = new DateTime(1963, 11, 9);
      row.SalaryGroup = 2;
      ds.Employees.AddEmployeesRow(row);
      // Save created rows
      ds.SaveToDatabase();
    }
  }
}
