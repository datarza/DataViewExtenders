using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Examples
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
      this.departmentsGridView.AutoGenerateColumns = false;
      this.employeesGridView.AutoGenerateColumns = false;
    }

    private void btnLoadData_Click(object sender, EventArgs e)
    {
      this.Database.LoadFromDatabase();
    }

    private void btnSaveData_Click(object sender, EventArgs e)
    {
      this.Database.SaveToDatabase();
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}
