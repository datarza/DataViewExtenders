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
    /// Adding a drawing in the RowHeaders for the changed or inserted rows
    /// based on yellow colors with gradient 
    /// </summary>
    /// <param name="viewGrid">DataGridView</param>
    public static void AddDataRowStateDrawingInRowHeaders(this DataGridView viewGrid)
    {
      DataGridViewExtenders.AddDataRowStateDrawingInRowHeaders(viewGrid,
        Color.FromArgb(0x99, 0xFF, 0xCC, 0x33),
        Color.FromArgb(0x99, 0xFF, 0x99, 0x00), 
        true);
    }

    /// <summary>
    /// Adding a drawing in the RowHeaders for the changed or inserted rows
    /// based on prefered color with gradient 
    /// </summary>
    /// <param name="viewGrid">DataGridView</param>
    /// <param name="preferedColor">Color for inserted and changed rows</param>
    public static void AddDataRowStateDrawingInRowHeaders(this DataGridView viewGrid, Color preferedColor)
    {
      DataGridViewExtenders.AddDataRowStateDrawingInRowHeaders(viewGrid,
        preferedColor,
        preferedColor,
        true);
    }

    /// <summary>
    /// Adding a drawing in the RowHeaders for the changed or inserted rows
    /// </summary>
    /// <param name="viewGrid">DataGridView</param>
    /// <param name="chgColor">Color for changed rows or null for using the default yellow color</param>
    /// <param name="insColor">Color for inserted rows or null for using the default yellow-red color</param>
    public static void AddDataRowStateDrawingInRowHeaders(this DataGridView viewGrid, Color chgColor, Color insColor, bool IsGradient)
    {
      // drawing the RowHeader
      viewGrid.CellPainting += delegate (object sender, DataGridViewCellPaintingEventArgs e)
      {
        if (viewGrid.RowHeadersVisible)
          if (e.ColumnIndex == -1 && e.RowIndex >= 0)
          {
            var dbItem = viewGrid.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (dbItem != null)
            {
              var rowState = dbItem.Row.RowState;
              if (rowState == DataRowState.Added || rowState == DataRowState.Modified)
              {
                Color _color = rowState == DataRowState.Modified ? chgColor : insColor;
                Brush _brush;
                if (IsGradient) _brush = new System.Drawing.Drawing2D.LinearGradientBrush(e.CellBounds, Color.Transparent, _color, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
                else _brush = new SolidBrush(_color);
                e.Paint(e.ClipBounds, DataGridViewPaintParts.Background);
                e.Graphics.FillRectangle(_brush, e.CellBounds);
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Background);
                e.Handled = true;
              }
            }
          }
      };
    }

  }
}
