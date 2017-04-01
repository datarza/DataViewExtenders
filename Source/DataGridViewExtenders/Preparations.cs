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

  }
}
