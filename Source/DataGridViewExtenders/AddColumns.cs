using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

//
// DataViewExtenders
//
// Extenders for WinForms controls, such as DataGridView, 
// BindingSource, BindingNavigator and so on 
//
// Author: Radu Martin (CanadianBeaver)
// Email: radu.martin@hotmail.com
// GitHub: https://github.com/CanadianBeaver/DataViewExtenders
// 

namespace CBComponents
{
  using CBComponents.DataDescriptors;
  using CBComponents.Forms;

  public static partial class DataGridViewExtenders
  {
    /// <summary>
    /// Columns generator that works on column data descriptions (Column Data Descriptor)
    /// </summary>
    /// <param name="viewGrid">DataGridView</param>
    /// <param name="DataSource">Data Source to support data-binding</param>
    /// <param name="Columns">Column data descriptors</param>
    public static void AddColumns(this DataGridView viewGrid, object DataSource, params ColumnDataDescriptor[] Columns)
    {
      viewGrid.AutoGenerateColumns = false;
      viewGrid.Columns.Clear();
      foreach (var column in Columns)
      {
        if (column.Mode == ColumnEditorMode.TextBox)
        { 
          var newColumn = new DataGridViewTextBoxColumn();
          column.GeneratedDataGridViewColumn = newColumn;
          newColumn.SortMode = DataGridViewColumnSortMode.Automatic;
          newColumn.HeaderText = column.HeaderText;
          if (column.MaxLength.HasValue) newColumn.MaxInputLength = column.MaxLength.Value;
          if (newColumn.MaxInputLength > 260) newColumn.DefaultCellStyle = new DataGridViewCellStyle() { WrapMode = DataGridViewTriState.True };
          if (column.FillWeight.HasValue)
          {
            newColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            newColumn.FillWeight = column.FillWeight.Value;
            newColumn.MinimumWidth = column.HeaderText.Length * 10;
            if (newColumn.MinimumWidth < 50) newColumn.MinimumWidth = 50;
          }
          newColumn.ToolTipText = column.ColumnName;
          newColumn.DataPropertyName = column.ColumnName;
          if (column.NullValue != null) { if (newColumn.DefaultCellStyle != null) newColumn.DefaultCellStyle.NullValue = column.NullValue; else newColumn.DefaultCellStyle = new DataGridViewCellStyle() { NullValue = column.NullValue }; }
          newColumn.ReadOnly = column.IsReadOnly;
          if (column.Style.HasValue) DataGridViewExtenders.SetEditorDataStyle(newColumn, column.Style.Value);
          viewGrid.Columns.Add(newColumn);
          if (column.FormatValueMethod != null)
          {
            newColumn.Tag = column.FormatValueMethod;
            viewGrid.CellFormatting += delegate (object sender, DataGridViewCellFormattingEventArgs e)
            {
              var _data = newColumn.Tag as FormatValueDelegate;
              if (_data != null && e.ColumnIndex == newColumn.Index && e.RowIndex >= 0)
              {
                var _value = _data.Invoke(viewGrid.Rows[e.RowIndex].DataBoundItem, newColumn.DataPropertyName);
                if (_value != null)
                {
                  e.Value = _value;
                  e.FormattingApplied = _value is string;
                }
              }
            };
          }
        }
        else if (column.Mode == ColumnEditorMode.CheckBox)
        { 
          var newColumn = new DataGridViewCheckBoxColumn();
          column.GeneratedDataGridViewColumn = newColumn;
          newColumn.SortMode = DataGridViewColumnSortMode.Automatic;
          newColumn.HeaderText = column.HeaderText;
          if (column.FillWeight.HasValue)
          {
            newColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            newColumn.FillWeight = column.FillWeight.Value;
            newColumn.MinimumWidth = column.HeaderText.Length * 10;
            if (newColumn.MinimumWidth < 50) newColumn.MinimumWidth = 50;
          }
          newColumn.ToolTipText = column.ColumnName;
          newColumn.ThreeState = column.IsNull;
          newColumn.DataPropertyName = column.ColumnName;
          newColumn.ReadOnly = column.IsReadOnly;
          viewGrid.Columns.Add(newColumn);
        }
        else if (column.Mode == ColumnEditorMode.ListBox)
        { 
          DataGridViewColumn newColumn;
          if (column.IsReadOnly)
          {
            var _newColumn = new DataGridViewTextBoxColumn();
            newColumn = _newColumn;
          }
          else
          {
            var _newColumn = new DataGridViewLinkColumn();
            _newColumn.TrackVisitedState = false;
            _newColumn.LinkBehavior = LinkBehavior.HoverUnderline;
            newColumn = _newColumn;
          }
          column.GeneratedDataGridViewColumn = newColumn;
          newColumn.SortMode = DataGridViewColumnSortMode.Automatic;
          newColumn.HeaderText = column.HeaderText;
          if (column.FillWeight.HasValue)
          {
            newColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            newColumn.FillWeight = column.FillWeight.Value;
            newColumn.MinimumWidth = column.HeaderText.Length * 10;
            if (newColumn.MinimumWidth < 50) newColumn.MinimumWidth = 50;
          }
          newColumn.ToolTipText = column.ColumnName;
          newColumn.DataPropertyName = column.ColumnName;
          newColumn.ReadOnly = true;
          if (column.Style.HasValue) DataGridViewExtenders.SetEditorDataStyle(newColumn, column.Style.Value);
          newColumn.Tag = Tuple.Create(column.DataSource, column.ValueMember, column.DisplayMember, column.GetListBoxItemsMethod, column.FormatValueMethod);
          viewGrid.Columns.Add(newColumn);
          viewGrid.CellFormatting += delegate (object sender, DataGridViewCellFormattingEventArgs e)
          {
            var _data = newColumn.Tag as Tuple<object, string, string, GetListBoxItemsDelegate, FormatValueDelegate>;
            if (_data != null && e.ColumnIndex == newColumn.Index && e.RowIndex >= 0)
            {
              if (_data.Item5 != null)
              {
                var _value = _data.Item5.Invoke(viewGrid.Rows[e.RowIndex].DataBoundItem, newColumn.DataPropertyName);
                if (_value != null)
                {
                  e.Value = _value;
                  e.FormattingApplied = _value is string;
                }
              }
              else if (viewGrid.Rows[e.RowIndex].DataBoundItem is DataRowView && (_data.Item1 is DataView || _data.Item1 is DataTable))
              {
                var dataBoundItem = (DataRowView)viewGrid.Rows[e.RowIndex].DataBoundItem;
                var _data1 = _data.Item1 is DataView ? ((DataView)_data.Item1).Table : (DataTable)_data.Item1;
                var _row = _data1.Rows.Find(dataBoundItem[newColumn.DataPropertyName]);
                e.Value = _row != null ? _row[_data.Item3].ToString() : string.Empty;
                e.FormattingApplied = true;
              }
            }
          };
          if (!column.IsReadOnly)
            viewGrid.CellClick += delegate (object sender, DataGridViewCellEventArgs e)
            {
              var _data = newColumn.Tag as Tuple<object, string, string, GetListBoxItemsDelegate, FormatValueDelegate>;
              if (_data != null && e.ColumnIndex == newColumn.Index && e.RowIndex >= 0 && viewGrid.Rows[e.RowIndex].DataBoundItem is DataRowView)
              {
                var dataBoundItem = (DataRowView)viewGrid.Rows[e.RowIndex].DataBoundItem;
                object items = _data.Item1;
                bool agc = false;
                if (_data.Item4 != null)
                {
                  items = _data.Item4.Invoke();
                  if (items == null)
                  {
                    //MessageBox.Show("No data exists");
                    //return;
                    throw new ArgumentNullException();
                  }
                  agc = true;
                }
                var row = SelectItemForm.GetSelectedRow(items, _data.Item2, _data.Item3, dataBoundItem[newColumn.DataPropertyName], agc);
                if (row != null)
                {
                  dataBoundItem.BeginEdit();
                  dataBoundItem[newColumn.DataPropertyName] = row[_data.Item2];
                  dataBoundItem.EndEdit();
                }
              }
            };
        }
        else if (column.Mode == ColumnEditorMode.ComboBox)
        { 
          var newColumn = new DataGridViewComboBoxColumn();
          column.GeneratedDataGridViewColumn = newColumn;
          newColumn.DisplayStyleForCurrentCellOnly = true;
          newColumn.DisplayStyle = column.IsReadOnly ? DataGridViewComboBoxDisplayStyle.Nothing : DataGridViewComboBoxDisplayStyle.DropDownButton;
          newColumn.AutoComplete = true;
          if (column.IsNull) newColumn.DefaultCellStyle = new DataGridViewCellStyle() { DataSourceNullValue = DBNull.Value };
          newColumn.SortMode = DataGridViewColumnSortMode.Automatic;
          newColumn.HeaderText = column.HeaderText;
          if (column.FillWeight.HasValue)
          {
            newColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            newColumn.FillWeight = column.FillWeight.Value;
            newColumn.MinimumWidth = column.HeaderText.Length * 10;
            if (newColumn.MinimumWidth < 50) newColumn.MinimumWidth = 50;
          }
          newColumn.ToolTipText = column.ColumnName;
          newColumn.DataPropertyName = column.ColumnName;
          newColumn.DataSource = column.DataSource;
          if (!string.IsNullOrWhiteSpace(column.ValueMember)) newColumn.ValueMember = column.ValueMember;
          if (!string.IsNullOrWhiteSpace(column.DisplayMember)) newColumn.DisplayMember = column.DisplayMember;
          newColumn.ReadOnly = column.IsReadOnly;
          viewGrid.Columns.Add(newColumn);
          if (column.FormatValueMethod != null)
          {
            newColumn.Tag = column.FormatValueMethod;
            viewGrid.CellFormatting += delegate (object sender, DataGridViewCellFormattingEventArgs e)
            {
              var _data = newColumn.Tag as FormatValueDelegate;
              if (_data != null && e.ColumnIndex == newColumn.Index && e.RowIndex >= 0)
              {
                var _value = _data.Invoke(viewGrid.Rows[e.RowIndex].DataBoundItem, newColumn.DataPropertyName);
                if (_value != null)
                {
                  e.Value = _value;
                  e.FormattingApplied = _value is string;
                }
              }
            };
          }
        }
      }
      viewGrid.DataSource = DataSource;
    }
    
    #region Preparation for visualizations

    /// <summary>
    /// Preparing DataGridView for showing data
    /// </summary>
    /// <param name="viewGrid">DataGridView</param>
    /// <param name="IsReadOnly"></param>
    public static void PrepareStyleForShowingData(this DataGridView viewGrid, bool IsReadOnly = true)
    {
      viewGrid.AllowUserToAddRows = false;
      viewGrid.AllowUserToDeleteRows = false;
      viewGrid.AllowUserToResizeRows = false;
      viewGrid.ReadOnly = IsReadOnly;
      viewGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
      viewGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
      viewGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
      viewGrid.EnableHeadersVisualStyles = true;
      viewGrid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
      viewGrid.ColumnHeadersVisible = true;
      viewGrid.RowHeadersVisible = false;
      viewGrid.RowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = SystemColors.Control, ForeColor = SystemColors.ControlText, SelectionBackColor = SystemColors.Highlight, SelectionForeColor = SystemColors.HighlightText };
      viewGrid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = SystemColors.ControlLightLight, ForeColor = SystemColors.ControlText, SelectionBackColor = SystemColors.Highlight, SelectionForeColor = SystemColors.HighlightText };
      viewGrid.BackgroundColor = SystemColors.AppWorkspace;

      viewGrid.MultiSelect = false;
      viewGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      viewGrid.StandardTab = true;

      typeof(DataGridView).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty, null, viewGrid, new object[] { true });
      viewGrid.DataError += new DataGridViewDataErrorEventHandler(viewGrid_DataError);
      if (viewGrid.DataSource is BindingSource) ((BindingSource)viewGrid.DataSource).DataError += new BindingManagerDataErrorEventHandler(bindingSource_DataError);
    }

    /// <summary>
    /// Preparing DataGridView for editing data
    /// </summary>
    /// <param name="viewGrid">DataGridView</param>
    public static void PrepareStyleForEditingData(this DataGridView viewGrid)
    {
      viewGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      viewGrid.AllowUserToResizeColumns = false;
      viewGrid.AllowUserToResizeRows = false;
      //viewGrid.AllowUserToAddRows = false;
      viewGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
      viewGrid.BorderStyle = BorderStyle.None;
      viewGrid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
      viewGrid.ColumnHeadersVisible = true;
      //viewGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
      //viewGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
      viewGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      viewGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
      viewGrid.RowHeadersVisible = true;
      viewGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      //viewGrid.RowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = SystemColors.Control, ForeColor = SystemColors.ControlText, SelectionBackColor = SystemColors.Highlight, SelectionForeColor = SystemColors.HighlightText };
      //viewGrid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = SystemColors.ControlLightLight, ForeColor = SystemColors.ControlText, SelectionBackColor = SystemColors.Highlight, SelectionForeColor = SystemColors.HighlightText };
      viewGrid.BackgroundColor = SystemColors.AppWorkspace;

      viewGrid.MultiSelect = false;
      viewGrid.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
      viewGrid.StandardTab = false;

      typeof(DataGridView).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty, null, viewGrid, new object[] { true });
      viewGrid.DataError += new DataGridViewDataErrorEventHandler(viewGrid_DataError);
      if (viewGrid.DataSource is BindingSource) ((BindingSource)viewGrid.DataSource).DataError += new BindingManagerDataErrorEventHandler(bindingSource_DataError);
      viewGrid.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(viewGrid_RowHeaderMouseDoubleClick);

    }
    
    /// <summary>
    /// Preparing DataGridView for report data 
    /// </summary>
    /// <param name="viewGrid">DataGridView</param>
    /// <param name="bindingSource"></param>
    public static void PrepareStyleForShowingReportData(this DataGridView viewGrid, BindingSource bindingSource)
    {
      viewGrid.AutoGenerateColumns = false;
      viewGrid.DataSource = bindingSource;
      DataGridViewExtenders.PrepareStyleForEditingData(viewGrid);
      viewGrid.ReadOnly = true;
      viewGrid.ColumnHeadersVisible = false; 
      viewGrid.RowHeadersVisible = false;
      viewGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      viewGrid.StandardTab = true;
    }

    private static void bindingSource_DataError(object sender, BindingManagerDataErrorEventArgs e)
    {
      // TODO: Log message - e.Exception;
    }

    private static void viewGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      // TODO: Log message - e.Exception;
      e.ThrowException = false;
      e.Cancel = true;
    }

    private static void viewGrid_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (sender is DataGridView)
      {
        var viewGrid = (DataGridView)sender;
        var dataBoundItem = viewGrid.Rows[e.RowIndex].DataBoundItem;
        if (dataBoundItem != null)
          SelectItemForm.ShowData(dataBoundItem, "Row inspector");
      }
    }

    #endregion

    /// <summary>
    /// Creating new text column and adding it to the DataGridView
    /// </summary>
    /// <param name="viewGrid">DataGridView</param>
    /// <param name="DataPropertyName"></param>
    /// <param name="HeaderText"></param>
    /// <param name="ToolTipText"></param>
    /// <returns>DataGridViewTextBoxColumn</returns>
    public static DataGridViewTextBoxColumn AddTextColumn(this DataGridView viewGrid, string DataPropertyName, string HeaderText = null, string ToolTipText = null)
    {
      DataGridViewTextBoxColumn result = new DataGridViewTextBoxColumn();
      result.DataPropertyName = DataPropertyName;
      if (!string.IsNullOrEmpty(HeaderText)) result.HeaderText = HeaderText;
      if (!string.IsNullOrWhiteSpace(ToolTipText)) result.ToolTipText = ToolTipText;
      result.ReadOnly = true;
      result.SortMode = DataGridViewColumnSortMode.Automatic;
      viewGrid.Columns.Add(result);
      return result;
    }
    
    /// <summary>
    /// Creating new check column and adding it to the DataGridView
    /// </summary>
    /// <param name="viewGrid">DataGridView</param>
    /// <param name="DataPropertyName"></param>
    /// <param name="HeaderText"></param>
    /// <param name="ToolTipText"></param>
    /// <returns>DataGridViewCheckBoxColumn</returns>
    public static DataGridViewCheckBoxColumn AddCheckColumn(this DataGridView viewGrid, string DataPropertyName, string HeaderText = null, string ToolTipText = null)
    {
      DataGridViewCheckBoxColumn result = new DataGridViewCheckBoxColumn();
      result.DataPropertyName = DataPropertyName;
      if (!string.IsNullOrEmpty(HeaderText)) result.HeaderText = HeaderText;
      if (!string.IsNullOrWhiteSpace(ToolTipText)) result.ToolTipText = ToolTipText;
      result.ReadOnly = true;
      result.SortMode = DataGridViewColumnSortMode.Automatic;
      viewGrid.Columns.Add(result);
      return result;
    }

    /// <summary>
    /// Set the visualization style for the DataGridViewColumn based on prefered EditorDataStyle
    /// </summary>
    /// <param name="column">DataGridViewColumn</param>
    /// <param name="Style">Data visualization style</param>
    private static void SetEditorDataStyle(DataGridViewColumn column, EditorDataStyle Style)
    {
      switch (Style)
      {
        case EditorDataStyle.Quantity: column.SetNumberStyle(); break;
        case EditorDataStyle.Price: column.SetMoneyStyle(); break;
        case EditorDataStyle.Percent: column.SetPercentStyle(); break;
        case EditorDataStyle.DateTime: column.SetDateTimeStyle(); break;
        case EditorDataStyle.Date: column.SetDateStyle(); break;
        case EditorDataStyle.Weight: column.SetWeightStyle(); break;
        case EditorDataStyle.Volume: column.SetVolumeStyle(); break;
      }
    }

  }
}
