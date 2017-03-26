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
      this.Database.LoadFromDatabase();

      // bottom buttons
      this.btnLoadData.Image = Properties.Resources.database_refresh;
      this.btnLoadData.Click += delegate { this.Database.LoadFromDatabase(); };
      this.btnSaveData.Image = Properties.Resources.database_save;
      this.btnSaveData.Click += delegate { this.Database.SaveToDatabase(); };
      this.btnExit.Image = Properties.Resources.door;
      this.btnExit.Click += delegate { this.Close(); };
    }

  }
}
