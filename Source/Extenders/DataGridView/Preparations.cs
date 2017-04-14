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
    /// Preparing DataGridView for showing data
    /// </summary>
    /// <param name="dataGrid">DataGridView</param>
    /// <param name="IsReadOnly"></param>
    public static void PrepareStyleForShowingData(this DataGridView dataGrid, bool IsReadOnly = true)
    {
      dataGrid.AllowUserToAddRows = false;
      dataGrid.AllowUserToDeleteRows = false;
      dataGrid.AllowUserToResizeRows = false;
      dataGrid.ReadOnly = IsReadOnly;
      dataGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
      dataGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
      dataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
      dataGrid.EnableHeadersVisualStyles = true;
      dataGrid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
      dataGrid.ColumnHeadersVisible = true;
      dataGrid.RowHeadersVisible = false;
      dataGrid.RowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = SystemColors.Control, ForeColor = SystemColors.ControlText, SelectionBackColor = SystemColors.Highlight, SelectionForeColor = SystemColors.HighlightText };
      dataGrid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = SystemColors.ControlLightLight, ForeColor = SystemColors.ControlText, SelectionBackColor = SystemColors.Highlight, SelectionForeColor = SystemColors.HighlightText };
      dataGrid.BackgroundColor = SystemColors.AppWorkspace;

      dataGrid.MultiSelect = false;
      dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      dataGrid.StandardTab = true;

      typeof(DataGridView).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty, null, dataGrid, new object[] { true });
      dataGrid.DataError += new DataGridViewDataErrorEventHandler(dataGrid_DataError);
      if (dataGrid.DataSource is BindingSource) ((BindingSource)dataGrid.DataSource).DataError += new BindingManagerDataErrorEventHandler(bindingSource_DataError);
    }

    /// <summary>
    /// Preparing DataGridView for editing data
    /// </summary>
    /// <param name="dataGrid">DataGridView</param>
    public static void PrepareStyleForEditingData(this DataGridView dataGrid)
    {
      dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      dataGrid.AllowUserToResizeColumns = false;
      dataGrid.AllowUserToResizeRows = false;
      //dataGrid.AllowUserToAddRows = false;
      dataGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
      dataGrid.BorderStyle = BorderStyle.None;
      dataGrid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
      dataGrid.ColumnHeadersVisible = true;
      //dataGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
      //dataGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
      dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
      dataGrid.RowHeadersVisible = true;
      dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      //dataGrid.RowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = SystemColors.Control, ForeColor = SystemColors.ControlText, SelectionBackColor = SystemColors.Highlight, SelectionForeColor = SystemColors.HighlightText };
      //dataGrid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = SystemColors.ControlLightLight, ForeColor = SystemColors.ControlText, SelectionBackColor = SystemColors.Highlight, SelectionForeColor = SystemColors.HighlightText };
      dataGrid.BackgroundColor = SystemColors.AppWorkspace;

      dataGrid.MultiSelect = false;
      dataGrid.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
      dataGrid.StandardTab = false;

      typeof(DataGridView).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty, null, dataGrid, new object[] { true });
      dataGrid.DataError += new DataGridViewDataErrorEventHandler(dataGrid_DataError);
      if (dataGrid.DataSource is BindingSource) ((BindingSource)dataGrid.DataSource).DataError += new BindingManagerDataErrorEventHandler(bindingSource_DataError);
      dataGrid.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(dataGrid_RowHeaderMouseDoubleClick);

    }
    
    /// <summary>
    /// Preparing DataGridView for report data 
    /// </summary>
    /// <param name="dataGrid">DataGridView</param>
    /// <param name="bindingSource"></param>
    public static void PrepareStyleForShowingReportData(this DataGridView dataGrid, BindingSource bindingSource)
    {
      dataGrid.AutoGenerateColumns = false;
      dataGrid.DataSource = bindingSource;
      DataGridViewExtenders.PrepareStyleForEditingData(dataGrid);
      dataGrid.ReadOnly = true;
      dataGrid.ColumnHeadersVisible = false; 
      dataGrid.RowHeadersVisible = false;
      dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      dataGrid.StandardTab = true;
    }

    private static void bindingSource_DataError(object sender, BindingManagerDataErrorEventArgs e)
    {
      // TODO: Log message - e.Exception;
    }

    private static void dataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      FormServices.ShowError(e.Exception);
      e.ThrowException = false;
      e.Cancel = true;
    }

    private static void dataGrid_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (sender is DataGridView)
      {
        var dataGrid = (DataGridView)sender;
        var dataBoundItem = dataGrid.Rows[e.RowIndex].DataBoundItem;
        if (dataBoundItem != null)
          SelectItemForm.ShowData(dataBoundItem, "Row inspector");
      }
    }

  }
}
