using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CBComponents.Forms
{
  using CBComponents;

  /// <summary>
  /// This form is using for selecting row from Collections or DataTable
  /// </summary>
  public sealed class SelectItemForm : Form
  {
    public class ColumnDefinition
    {
      public string HeaderText;
      public string DataPropertyName;
      public DataGridViewContentAlignment ContentAlignment;
      public float? FillWeight;
      public SqlDbType DataPropertyType;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="HeaderText"></param>
    /// <param name="DataPropertyName"></param>
    /// <param name="ContentAlignment"></param>
    /// <param name="FillWeight"></param>
    /// <param name="DataPropertyType"></param>
    /// <returns></returns>
    public static ColumnDefinition CreateColumnDefinition(string HeaderText, string DataPropertyName, DataGridViewContentAlignment ContentAlignment = DataGridViewContentAlignment.NotSet, float? FillWeight = null, SqlDbType DataPropertyType = SqlDbType.NVarChar)
    {
      var result = new ColumnDefinition();
      result.HeaderText = HeaderText;
      result.DataPropertyName = DataPropertyName;
      result.ContentAlignment = ContentAlignment;
      result.FillWeight = FillWeight;
      result.DataPropertyType = DataPropertyType;
      return result;
    }

    public static T GetSelectedRow<T>(IEnumerable<T> DataSource, params SelectItemForm.ColumnDefinition[] Columns)
    {
      return SelectItemForm.GetSelectedRow<T>(null, null, DataSource, default(T), Columns);
    }

    public static T GetSelectedRow<T>(IEnumerable<T> DataSource, T SelectedRow, params SelectItemForm.ColumnDefinition[] Columns)
    {
      return SelectItemForm.GetSelectedRow<T>(null, null, DataSource, SelectedRow, Columns);
    }

    public static T GetSelectedRow<T>(string Text, IEnumerable<T> DataSource, T SelectedRow, params SelectItemForm.ColumnDefinition[] Columns)
    {
      return SelectItemForm.GetSelectedRow<T>(Text, null, DataSource, SelectedRow, Columns);
    }

    public static T GetSelectedRow<T>(string Text, string HeaderText, IEnumerable<T> DataSource, T SelectedRow, params SelectItemForm.ColumnDefinition[] Columns)
    {
      if (DataSource == null || DataSource.Count() == 0) return default(T);
      if (DataSource.Count() == 1) return DataSource.First();
      using (var form = new SelectItemForm(new SortableBindingList<T>(DataSource), SelectedRow == null ? DataSource.First() : SelectedRow, Columns))
      {
        if (!string.IsNullOrWhiteSpace(Text)) form.Text = Text;
        if (!string.IsNullOrWhiteSpace(HeaderText)) form.SetHeaderText(HeaderText);

        if (FormServices.ShowFormDialog(form) == DialogResult.OK && form.bindingSource.Current is T)
          return (T)form.bindingSource.Current;
        else return default(T);
      }
    }

    public static T GetSelectedRow<T>(DataTable DataSource, params SelectItemForm.ColumnDefinition[] Columns) 
      where T : DataRow
    {
      return SelectItemForm.GetSelectedRow<T>(DataSource, null, Columns);
    }

    public static T GetSelectedRow<T>(DataTable DataSource, T SelectedRow, params SelectItemForm.ColumnDefinition[] Columns) 
      where T : DataRow
    {
      if (DataSource == null || DataSource.Rows.Count == 0) return null;
      if (DataSource.Rows.Count == 1) return DataSource.Rows[0] as T;
      using (var form = new SelectItemForm(DataSource, SelectedRow ?? DataSource.Rows[0], Columns))
        if (FormServices.ShowFormDialog(form) == DialogResult.OK)
        {
          var drv = form.bindingSource.Current as DataRowView;
          if (drv != null) return drv.Row as T;
          else return form.bindingSource.Current as T;
        }
        else return null;
    }

    internal static DataRow GetSelectedRow(object DataSource, string ValueMember = null, string DisplayMember = null, object SelectedValue = null, bool AutoGenerateColumns = false)
    {
      SelectItemForm.ColumnDefinition[] Columns = null;
      if (!AutoGenerateColumns && !string.IsNullOrWhiteSpace(ValueMember) && !string.IsNullOrWhiteSpace(DisplayMember))
      {
        Columns = new SelectItemForm.ColumnDefinition[] {
          SelectItemForm.CreateColumnDefinition("Value", ValueMember),
          SelectItemForm.CreateColumnDefinition("Name", DisplayMember, FillWeight: 100),
        };
      }
      using (var form = new SelectItemForm(DataSource, SelectedValue, Columns))
      {
        try
        {
          var pos = form.bindingSource.Find(ValueMember, SelectedValue);
          if (pos >= 0) form.bindingSource.Position = pos;
        }
        catch { }
        if (FormServices.ShowFormDialog(form) == DialogResult.OK)
        {
          var drv = form.bindingSource.Current as DataRowView;
          if (drv != null) return drv.Row;
          else return form.bindingSource.Current as DataRow;
        }
        else return null;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="DataSource"></param>
    /// <param name="Text"></param>
    /// <param name="Columns"></param>
    /// <returns></returns>
    public static void ShowData(object DataSource, string Text, params SelectItemForm.ColumnDefinition[] Columns)
    {
      SelectItemForm.ShowData(DataSource, Text, null, Columns);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="DataSource"></param>
    /// <param name="Text"></param>
    /// <param name="HeaderText"></param>
    /// <param name="Columns"></param>
    /// <returns></returns>
    public static void ShowData(object DataSource, string Text, string HeaderText, params SelectItemForm.ColumnDefinition[] Columns)
    {
      using (var form = new SelectItemForm(DataSource, null, Columns))
      {
        if (!string.IsNullOrWhiteSpace(Text)) form.Text = Text;
        if (!string.IsNullOrWhiteSpace(HeaderText)) form.SetHeaderText(HeaderText);
        FormServices.ShowFormDialog(form);
      }
    }

    #region Form creation

    private BindingSource bindingSource;
    public readonly DataGridView dataGrid;
    private PromptedTextBox searchTextBox;
    private Label headerLabel;
    private Label footerLabel;
    private Button btnOk;
    private Button btnClose;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="DataSource"></param>
    /// <param name="Text"></param>
    /// <param name="HeaderText"></param>
    /// <returns></returns>
    public static SelectItemForm CreateFormWithoutColumns(object DataSource, string Text = null, string HeaderText = null)
    {
      var form = new SelectItemForm(DataSource, null, new SelectItemForm.ColumnDefinition[] { null });
      if (!string.IsNullOrWhiteSpace(Text)) form.Text = Text;
      if (!string.IsNullOrWhiteSpace(HeaderText)) form.SetHeaderText(HeaderText);
      return form;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="DataSource"></param>
    /// <param name="SelectedItem"></param>
    /// <param name="Columns"></param>
    private SelectItemForm(object DataSource, object SelectedItem, params SelectItemForm.ColumnDefinition[] Columns)
    {
      // setting up the bindingSource
      this.bindingSource = new BindingSource();
      this.bindingSource.DataSource = DataSource;
      if (SelectedItem != null)
      {
        this.bindingSource.MoveLast();
        while (this.bindingSource.Position > 0)
        {
          if (this.bindingSource.Current == SelectedItem) break;
          else
          {
            var drv = this.bindingSource.Current as DataRowView;
            if (drv != null && drv.Row == SelectedItem) break;
            if (drv != null && SelectedItem is DataRow && drv.Row.ItemArray.SequenceEqual(((DataRow)SelectedItem).ItemArray)) break;
          }
          this.bindingSource.MovePrevious();
        }
      }

      // Setting up the controls
      this.dataGrid = new DataGridView();
      if (this.bindingSource.SupportsFiltering) this.searchTextBox = new PromptedTextBox();
      this.headerLabel = new Label();
      this.footerLabel = new Label();
      this.btnClose = new Button();
      if (SelectedItem != null) this.btnOk = new Button();
      var mainPanel = new TableLayoutPanel();

      mainPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // mainPanel
      // 
      mainPanel.ColumnCount = 4;
      mainPanel.ColumnStyles.Add(new ColumnStyle());
      mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
      mainPanel.ColumnStyles.Add(new ColumnStyle());
      mainPanel.ColumnStyles.Add(new ColumnStyle());
      if (this.bindingSource.SupportsFiltering) mainPanel.Controls.Add(this.searchTextBox, 2, 0);
      mainPanel.Controls.Add(this.dataGrid, 0, 1);
      mainPanel.Controls.Add(this.headerLabel, 0, 0);
      mainPanel.Controls.Add(this.footerLabel, 0, 2);
      mainPanel.Controls.Add(this.btnClose, 3, 2);
      if (SelectedItem != null) mainPanel.Controls.Add(this.btnOk, 2, 2);
      mainPanel.Dock = DockStyle.Fill;
      mainPanel.RowCount = 3;
      mainPanel.RowStyles.Add(new RowStyle());
      mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
      mainPanel.RowStyles.Add(new RowStyle());
      mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
      mainPanel.Padding = new Padding(6);
      // 
      // searchTextBox
      // 
      if (this.bindingSource.SupportsFiltering)
      {
        this.searchTextBox.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        mainPanel.SetColumnSpan(this.searchTextBox, 2);
        this.searchTextBox.FocusSelect = true;
        this.searchTextBox.PromptForeColor = SystemColors.ControlDark;
        this.searchTextBox.PromptText = "searching...";
        this.searchTextBox.Size = new Size(150, 20);
        this.searchTextBox.TabIndex = 1;
        this.searchTextBox.TextChanged += delegate
        {
          string _txt = this.searchTextBox.Text.Trim();
          if (string.IsNullOrEmpty(_txt)) this.bindingSource.RemoveFilter();
          else this.bindingSource.Filter = RowFilterBuilder.BuildColumnFilter(_txt, this.dataGrid).Trim();
          foreach (DataGridViewRow row in this.dataGrid.SelectedRows) row.Selected = false;
          if (this.dataGrid.Rows.Count > 0) this.dataGrid.Rows[0].Selected = true;
        };
      }
      // 
      // dataGrid
      // 
      mainPanel.SetColumnSpan(this.dataGrid, 4);
      this.dataGrid.Dock = DockStyle.Fill;
      this.dataGrid.TabIndex = 0;
      this.dataGrid.PrepareStyleForShowingData();
      this.dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
      if (Columns == null || Columns.Length == 0) this.dataGrid.AutoGenerateColumns = true;
      else
      {
        this.dataGrid.AutoGenerateColumns = false;
        if (Columns[0] != null)
          foreach (var _column in Columns)
          {
            DataGridViewColumn _c;
            if (_column.DataPropertyType == SqlDbType.Bit) _c = this.dataGrid.AddCheckColumn(_column.DataPropertyName, _column.HeaderText);
            else if (_column.DataPropertyType == SqlDbType.Date) _c = this.dataGrid.AddTextColumn(_column.DataPropertyName, _column.HeaderText).SetDateStyle();
            else if (_column.DataPropertyType == SqlDbType.DateTime) _c = this.dataGrid.AddTextColumn(_column.DataPropertyName, _column.HeaderText).SetDateTimeWithSecondsStyle();
            else if (_column.DataPropertyType == SqlDbType.Int || _column.DataPropertyType == SqlDbType.TinyInt) _c = this.dataGrid.AddTextColumn(_column.DataPropertyName, _column.HeaderText).SetNumberStyle();
            else if (_column.DataPropertyType == SqlDbType.Decimal || _column.DataPropertyType == SqlDbType.Float) _c = this.dataGrid.AddTextColumn(_column.DataPropertyName, _column.HeaderText).SetDecimalStyle();
            else if (_column.DataPropertyType == SqlDbType.Money) _c = this.dataGrid.AddTextColumn(_column.DataPropertyName, _column.HeaderText).SetMoneyStyle();
            else _c = this.dataGrid.AddTextColumn(_column.DataPropertyName, _column.HeaderText);
            if (_column.ContentAlignment != DataGridViewContentAlignment.NotSet) _c.SetStyles(50, _column.ContentAlignment);
            if (_column.FillWeight.HasValue) _c.SetAutoSizeFillStyle(100, _column.FillWeight.Value);
          }
      }
      if (SelectedItem == null)
        this.dataGrid.DataBindingComplete += delegate
        {
          foreach (DataGridViewRow row in this.dataGrid.SelectedRows)
            row.Selected = false;
        };
      else
      {
        this.dataGrid.DoubleClick += delegate { this.AcceptButton.PerformClick(); };
        this.dataGrid.KeyDown += delegate (object sender, KeyEventArgs e) { if (e.KeyData == Keys.Enter || e.KeyData == Keys.Return) { e.Handled = true; this.AcceptButton.PerformClick(); } };
      }
      this.dataGrid.DataSource = this.bindingSource;
      // 
      // headerLabel
      // 
      this.headerLabel.Anchor = AnchorStyles.Left;
      mainPanel.SetColumnSpan(this.headerLabel, 2);
      this.headerLabel.AutoSize = true;
      this.headerLabel.ForeColor = SystemColors.ControlDarkDark;
      this.headerLabel.TextAlign = ContentAlignment.MiddleLeft;
      this.headerLabel.Visible = false;
      // 
      // footerLabel
      // 
      this.footerLabel.Anchor = AnchorStyles.Left;
      mainPanel.SetColumnSpan(this.footerLabel, 2);
      this.footerLabel.AutoSize = true;
      this.footerLabel.ForeColor = SystemColors.ControlDarkDark;
      this.headerLabel.TextAlign = ContentAlignment.MiddleLeft;
      this.footerLabel.Visible = false;
      // 
      // btnOk
      // 
      if (SelectedItem != null)
      {
        this.btnOk.Anchor = AnchorStyles.Right;
        this.btnOk.DialogResult = DialogResult.OK;
        this.btnOk.Size = new Size(80, 25);
        this.btnOk.TabIndex = 3;
        this.btnOk.Text = "Select";
        this.btnOk.Margin = new Padding(3, 9, 3, 6);
      }
      // 
      // btnClose
      // 
      this.btnClose.Anchor = AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Size = new Size(80, 25);
      this.btnClose.TabIndex = 4;
      this.btnClose.Text = SelectedItem != null ? "Cancel" : "Close";
      this.btnClose.Margin = new Padding(3, 9, 3, 6);
      // 
      // SelectItemForm
      // 
      this.AutoScaleDimensions = new SizeF(6F, 13F);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AcceptButton = SelectedItem != null ? this.btnOk : this.btnClose;
      this.CancelButton = this.btnClose;
      this.ClientSize = new Size(624, 362);
      this.Controls.Add(mainPanel);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new Size(320, 200);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = SelectedItem != null ? "Select a row" : "Data";
      mainPanel.ResumeLayout(false);
      mainPanel.PerformLayout();
      this.ResumeLayout(false);
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if (this.searchTextBox != null && !this.searchTextBox.Focused && this.searchTextBox.Visible && (keyData >= Keys.A && keyData <= Keys.Z || keyData >= Keys.D0 && keyData <= Keys.D9))
      {
        this.searchTextBox.Select();
        return true;
      }
      else return base.ProcessCmdKey(ref msg, keyData);
    }

    protected override void OnLoad(EventArgs e)
    {
      // width of form without scroll
      var nWidth = this.dataGrid.PreferredSize.Width + 2 * this.dataGrid.Left + 1;
      var screenWidth = 2 * Screen.PrimaryScreen.WorkingArea.Width / 3; // 2/3 is maximun
      if (screenWidth < nWidth) nWidth = screenWidth;
      if (this.MinimumSize.Width > nWidth) nWidth = this.MinimumSize.Width; // if (480 > nWidth) nWidth = 480;
      this.Left -= (nWidth - this.Width) / 2;
      this.Width = nWidth;
      base.OnLoad(e);
      // Hiding the identification columns
      if (this.dataGrid.AutoGenerateColumns && this.dataGrid.Columns.Count > 0 && this.dataGrid.Columns[0].DataPropertyName.EndsWith("ID"))
        this.dataGrid.Columns[0].Visible = false;
    }

    protected override void OnShown(EventArgs e)
    {
      base.OnShown(e);
      if (!this.footerLabel.Visible && this.bindingSource.Count > 0)
      {
        this.footerLabel.Text = string.Format("Rows: {0}", this.bindingSource.Count);
        this.footerLabel.Visible = true;
      }
    }

    #endregion

    /// <summary>
    /// Setting thext in the header
    /// </summary>
    /// <param name="Text">Prefered text</param>
    public void SetHeaderText(string Text)
    {
      if (!string.IsNullOrWhiteSpace(Text))
      {
        this.headerLabel.Text = Text;
        this.headerLabel.Visible = true;
      }
    }
    
    /// <summary>
    /// Setting thext in the footer
    /// </summary>
    /// <param name="Text">Prefered text</param>
    public void SetFooterText(string Text)
    {
      if (!string.IsNullOrWhiteSpace(Text))
      {
        this.footerLabel.Text = Text;
        this.footerLabel.Visible = true;
      }
    }

  }
}