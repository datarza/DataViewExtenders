using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CBComponents;

namespace Examples
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
      this.InitializeGrid();

      // bottom buttons
      this.btnLoadData.Image = Properties.Resources.database_refresh;
      this.btnLoadData.Click += delegate { this.Database.LoadFromDatabase(); };
      this.btnSaveData.Image = Properties.Resources.database_save;
      this.btnSaveData.Click += delegate { this.Database.SaveToDatabase(); };
      this.btnExit.Image = Properties.Resources.door;
      this.btnExit.Click += delegate { this.Close(); };
    }

    private void InitializeGrid()
    {
      this.Database.LoadFromDatabase();

      this.departmentsGridView.AutoGenerateColumns = false;
      this.departmentsGridView.Columns.Clear();
      this.departmentsGridView.AddTextColumn("DepartmentID", "ID", "DepartmentID").Visible = false;
      this.departmentsGridView.AddTextColumn("DepartmentName", "Department", "DepartmentName").SetAutoSizeAllCellsStyle();
      this.departmentsGridView.AddCheckColumn("IsClosed", "Closed?", "IsClosed").SetAutoSizeAllCellsStyle();
      this.departmentsGridView.AddTextColumn("CompanyGroup", "Group", "CompanyGroup").SetAutoSizeAllCellsStyle();
      this.departmentsGridView.AddTextColumn("Remarks", "Remarks", "Remarks").SetAutoSizeFillStyle(50);
      this.departmentsGridView.AddDataRowStateDrawingInRowHeaders();

      this.employeesGridView.AutoGenerateColumns = false;
      this.employeesGridView.Columns.Clear();
      this.employeesGridView.AddTextColumn("EmployeeID", "ID", "EmployeeID").Visible = false;
      this.employeesGridView.AddTextColumn("EmployeeName", "Employee", "EmployeeName").SetAutoSizeFillStyle(100);
      this.employeesGridView.AddTextColumn("DepartmentID", "Department", "DepartmentID").SetAutoSizeAllCellsStyle();
      this.employeesGridView.AddTextColumn("PhoneNumber", "Phone", "PhoneNumber").SetAutoSizeAllCellsStyle();
      this.employeesGridView.AddTextColumn("DateBirth", "Date of birth", "DateBirth").SetAutoSizeAllCellsStyle();
      this.employeesGridView.AddTextColumn("SalaryGroup", "Group of salary", "SalaryGroup").SetAutoSizeAllCellsStyle();
      this.employeesGridView.AddDataRowStateDrawingInRowHeaders();

      // Binding data
      this.departmentsNavigator.BindingSource = this.departmentsBindingSource;
      this.departmentsGridView.DataSource = this.departmentsBindingSource;
      this.employeesNavigator.BindingSource = this.employeesBindingSource;
      this.employeesGridView.DataSource = this.employeesBindingSource;

    }

    private void MainForm_Load(object sender, EventArgs e)
    {

    }
  }
}
