﻿using System;
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

      // Binding data
      this.departmentsNavigator.BindingSource = this.departmentsBindingSource;
      this.employeesNavigator.BindingSource = this.employeesBindingSource;
      //this.departmentsGridView.AutoGenerateColumns = false;
      this.departmentsGridView.DataSource = this.departmentsBindingSource;
      //this.employeesGridView.AutoGenerateColumns = false;
      this.employeesGridView.DataSource = this.employeesBindingSource;

      this.Database.LoadFromDatabase();

      // bottom buttons
      this.btnLoadData.Image = Properties.Resources.database_refresh;
      this.btnLoadData.Click += delegate { this.Database.LoadFromDatabase(); };
      this.btnSaveData.Image = Properties.Resources.database_save;
      this.btnSaveData.Click += delegate { this.Database.SaveToDatabase(); };
      this.btnExit.Image = Properties.Resources.door;
      this.btnExit.Click += delegate { this.Close(); };
    }
    
    private void MainForm_Load(object sender, EventArgs e)
    {
      // 
      this.departmentsGridView.AddDataRowStateDrawingInRowHeaders();
      this.employeesGridView.AddDataRowStateDrawingInRowHeaders();
      //
      this.departmentsGridView.Columns[1].SetAutoSizeAllCellsStyle();
      //
      this.employeesGridView.Columns[4].SetDateTimeWithSecondsStyle();
      this.employeesGridView.Columns[5].SetWeightStyle();
    }
  }
}
