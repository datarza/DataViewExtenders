using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CBComponents
{
  using CBComponents.DataDescriptors;
  
  public static partial class DataGridViewExtenders
  {
    /// <summary>
    /// Columns generator that works on column data descriptions (Column Data Descriptor)
    /// </summary>
    /// <param name="viewGrid">this DataGridView</param>
    /// <param name="DataSource">Data Source to support data-binding</param>
    /// <param name="Columns">Column data descriptors</param>
    public static void AddColumns(this DataGridView viewGrid, object DataSource, params ColumnDataDescriptor[] Columns)
    {

    }

    /// <summary>
    /// Adding a drawing of the changes tracking for changed or inserted rows
    /// </summary>
    /// <param name="viewGrid">this DataGridView</param>
    /// <param name="cColor">Color for changed rows or null for using the default yellow color</param>
    /// <param name="iColor">Color for inserted rows or null for using the default red color</param>
    public static void AddCellPainting(this DataGridView viewGrid, Color? cColor = null, Color? iColor = null, bool IsGradient = true)
    {
      if (cColor == null) cColor = Color.FromArgb(0x99, 0xFF, 0xCC, 0x33);
      if (iColor == null) iColor = Color.FromArgb(0x99, 0xFF, 0x33, 0x33);
      viewGrid.CellPainting += delegate (object sender, DataGridViewCellPaintingEventArgs e)
      { 
        if (e.ColumnIndex == -1 && e.RowIndex >= 0)
        {
          var dbItem = viewGrid.Rows[e.RowIndex].DataBoundItem as DataRowView;
          if (dbItem != null)
          {
            var rowState = dbItem.Row.RowState;
            if (rowState == DataRowState.Added || rowState == DataRowState.Modified)
            {
              Color _color = rowState == DataRowState.Modified ? cColor.Value : iColor.Value;
              Brush _brush;
              if (IsGradient) _brush = new LinearGradientBrush(e.CellBounds, Color.Transparent, _color, LinearGradientMode.Horizontal);
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
