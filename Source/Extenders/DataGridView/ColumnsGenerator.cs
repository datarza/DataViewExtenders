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
    /// Columns generator that works on column data descriptions (ColumnDataDescriptor)
    /// </summary>
    /// <param name="dataGrid">DataGridView</param>
    /// <param name="DataSource">Data Source to support data-binding</param>
    /// <param name="Columns">Column data descriptors</param>
    public static void GenerateColumns(this DataGridView dataGrid, object DataSource, params ColumnDataDescriptor[] Columns)
    {
      dataGrid.SuspendLayout();
      dataGrid.AutoGenerateColumns = false;
      dataGrid.Columns.Clear();
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
          dataGrid.Columns.Add(newColumn);
          if (column.FormatValueMethod != null)
          {
            newColumn.Tag = column.FormatValueMethod;
            dataGrid.CellFormatting += delegate (object sender, DataGridViewCellFormattingEventArgs e)
            {
              var _data = newColumn.Tag as FormatValueDelegate;
              if (_data != null && e.ColumnIndex == newColumn.Index && e.RowIndex >= 0)
              {
                var _value = _data.Invoke(dataGrid.Rows[e.RowIndex].DataBoundItem, newColumn.DataPropertyName);
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
          dataGrid.Columns.Add(newColumn);
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
          dataGrid.Columns.Add(newColumn);
          dataGrid.CellFormatting += delegate (object sender, DataGridViewCellFormattingEventArgs e)
          {
            var _data = newColumn.Tag as Tuple<object, string, string, GetListBoxItemsDelegate, FormatValueDelegate>;
            if (_data != null && e.ColumnIndex == newColumn.Index && e.RowIndex >= 0)
            {
              if (_data.Item5 != null)
              {
                var _value = _data.Item5.Invoke(dataGrid.Rows[e.RowIndex].DataBoundItem, newColumn.DataPropertyName);
                if (_value != null)
                {
                  e.Value = _value;
                  e.FormattingApplied = _value is string;
                }
              }
              else if (dataGrid.Rows[e.RowIndex].DataBoundItem is DataRowView && (_data.Item1 is DataView || _data.Item1 is DataTable))
              {
                var dataBoundItem = (DataRowView)dataGrid.Rows[e.RowIndex].DataBoundItem;
                var _data1 = _data.Item1 is DataView ? ((DataView)_data.Item1).Table : (DataTable)_data.Item1;
                var _row = _data1.Rows.Find(dataBoundItem[newColumn.DataPropertyName]);
                e.Value = _row != null ? _row[_data.Item3].ToString() : string.Empty;
                e.FormattingApplied = true;
              }
            }
          };
          if (!column.IsReadOnly)
            dataGrid.CellClick += delegate (object sender, DataGridViewCellEventArgs e)
            {
              var _data = newColumn.Tag as Tuple<object, string, string, GetListBoxItemsDelegate, FormatValueDelegate>;
              if (_data != null && e.ColumnIndex == newColumn.Index && e.RowIndex >= 0 && dataGrid.Rows[e.RowIndex].DataBoundItem is DataRowView)
              {
                var dataBoundItem = (DataRowView)dataGrid.Rows[e.RowIndex].DataBoundItem;
                object items = _data.Item1;
                bool agc = false;
                if (_data.Item4 != null)
                {
                  items = _data.Item4.Invoke();
                  if (items == null)
                  {
                    FormServices.ShowError("No data exists", true);
                    return;
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
          dataGrid.Columns.Add(newColumn);
          if (column.FormatValueMethod != null)
          {
            newColumn.Tag = column.FormatValueMethod;
            dataGrid.CellFormatting += delegate (object sender, DataGridViewCellFormattingEventArgs e)
            {
              var _data = newColumn.Tag as FormatValueDelegate;
              if (_data != null && e.ColumnIndex == newColumn.Index && e.RowIndex >= 0)
              {
                var _value = _data.Invoke(dataGrid.Rows[e.RowIndex].DataBoundItem, newColumn.DataPropertyName);
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
      dataGrid.DataSource = DataSource;
      dataGrid.ResumeLayout(false);
    }
    
    #region Add Text and Check Columns

    /// <summary>
    /// Creating new TextBox column and adding it to the DataGridView
    /// </summary>
    /// <param name="dataGrid">DataGridView</param>
    /// <param name="DataPropertyName"></param>
    /// <param name="HeaderText"></param>
    /// <param name="ToolTipText"></param>
    /// <returns>DataGridViewTextBoxColumn</returns>
    public static DataGridViewTextBoxColumn AddTextColumn(this DataGridView dataGrid, string DataPropertyName, string HeaderText = null, string ToolTipText = null)
    {
      DataGridViewTextBoxColumn result = new DataGridViewTextBoxColumn();
      result.DataPropertyName = DataPropertyName;
      if (!string.IsNullOrEmpty(HeaderText)) result.HeaderText = HeaderText;
      if (!string.IsNullOrWhiteSpace(ToolTipText)) result.ToolTipText = ToolTipText;
      result.ReadOnly = true;
      result.SortMode = DataGridViewColumnSortMode.Automatic;
      dataGrid.Columns.Add(result);
      return result;
    }

    /// <summary>
    /// Creating new CheckBox column and adding it to the DataGridView
    /// </summary>
    /// <param name="dataGrid">DataGridView</param>
    /// <param name="DataPropertyName"></param>
    /// <param name="HeaderText"></param>
    /// <param name="ToolTipText"></param>
    /// <returns>DataGridViewCheckBoxColumn</returns>
    public static DataGridViewCheckBoxColumn AddCheckColumn(this DataGridView dataGrid, string DataPropertyName, string HeaderText = null, string ToolTipText = null)
    {
      DataGridViewCheckBoxColumn result = new DataGridViewCheckBoxColumn();
      result.DataPropertyName = DataPropertyName;
      if (!string.IsNullOrEmpty(HeaderText)) result.HeaderText = HeaderText;
      if (!string.IsNullOrWhiteSpace(ToolTipText)) result.ToolTipText = ToolTipText;
      result.ReadOnly = true;
      result.SortMode = DataGridViewColumnSortMode.Automatic;
      dataGrid.Columns.Add(result);
      return result;
    }

    #endregion

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
