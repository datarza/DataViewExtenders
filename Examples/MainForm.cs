using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CBComponents;
using CBComponents.DataDescriptors;
using Examples.Database;

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

      var tbl1 = this.Database.Departments;
      this.departmentsGridView.AddColumns(this.departmentsBindingSource, 
        new ColumnDataDescriptor("Department", tbl1.DepartmentNameColumn),
        new ColumnDataDescriptor("Is closed?", tbl1.IsClosedColumn),
        new ColumnDataDescriptor("Group", tbl1.CompanyGroupColumn),
        new ColumnDataDescriptor("Remarks", tbl1.RemarksColumn, FillWeight: 100));
      this.departmentsGridView.PrepareStyleForEditingData();
      this.departmentsGridView.AddDataRowStateDrawingInRowHeaders();

      var tbl2 = this.Database.Employees;
      var _salaryGroups = daoDataSet.CreateSalaryGroupsLookupTable();
      this.employeesGridView.AddColumns(this.employeesBindingSource,
        new ColumnDataDescriptor("Employee", tbl2.EmployeeNameColumn, FillWeight: 100),
        new ColumnDataDescriptor("Department", tbl2.DepartmentIDColumn, DataSource: tbl1, ValueMember: tbl1.DepartmentIDColumn.ColumnName, DisplayMember: tbl1.DepartmentNameColumn.ColumnName),
        new ColumnDataDescriptor("Phone", tbl2.PhoneNumberColumn),
        new ColumnDataDescriptor("Date of birth", tbl2.DateBirthColumn, Style: EditorDataStyle.Date),
        new ColumnDataDescriptor("Salary", tbl2.SalaryGroupColumn, DataSource: _salaryGroups, ValueMember: _salaryGroups.Columns[0].ColumnName, DisplayMember: _salaryGroups.Columns[1].ColumnName));
      this.employeesGridView.PrepareStyleForEditingData();
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
