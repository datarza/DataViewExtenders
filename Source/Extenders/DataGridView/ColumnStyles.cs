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
    /// 
    /// </summary>
    /// <param name="column">DataGridViewColumn</param>
    /// <param name="Width"></param>
    /// <param name="Alignment"></param>
    /// <param name="Format"></param>
    /// <returns>DataGridViewColumn</returns>
    public static DataGridViewColumn SetStyles(this DataGridViewColumn column, int Width, DataGridViewContentAlignment Alignment = DataGridViewContentAlignment.MiddleLeft, string Format = null)
    {
      column.Width = Width;
      column.DefaultCellStyle.Alignment = Alignment;
      if (!string.IsNullOrEmpty(Format)) column.DefaultCellStyle.Format = Format;
      return column;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="column">DataGridViewColumn</param>
    /// <param name="MinimumWidth"></param>
    /// <param name="FillWeight"></param>
    /// <returns>DataGridViewColumn</returns>
    public static DataGridViewColumn SetAutoSizeFillStyle(this DataGridViewColumn column, int MinimumWidth, float FillWeight = 100)
    {
      column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      column.MinimumWidth = MinimumWidth;
      column.FillWeight = FillWeight;
      return column;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="column">DataGridViewColumn</param>
    /// <returns>DataGridViewColumn</returns>
    public static DataGridViewColumn SetAutoSizeAllCellsStyle(this DataGridViewColumn column)
    {
      column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      return column;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="column">DataGridViewColumn</param>
    /// <param name="Width"></param>
    /// <returns>DataGridViewColumn</returns>
    public static DataGridViewColumn SetNumberStyle(this DataGridViewColumn column, int Width = 80)
    {
      return column.SetStyles(Width, DataGridViewContentAlignment.MiddleRight, "N0");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="column">DataGridViewColumn</param>
    /// <param name="Width"></param>
    /// <returns>DataGridViewColumn</returns>
    public static DataGridViewColumn SetDecimalStyle(this DataGridViewColumn column, int Width = 80)
    {
      return column.SetStyles(Width, DataGridViewContentAlignment.MiddleRight, "N");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="column">DataGridViewColumn</param>
    /// <param name="Width"></param>
    /// <returns>DataGridViewColumn</returns>
    public static DataGridViewColumn SetMoneyStyle(this DataGridViewColumn column, int Width = 80)
    {
      // TODO: should be configured according local culture
      return column.SetStyles(Width, DataGridViewContentAlignment.MiddleRight, "C");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="column">DataGridViewColumn</param>
    /// <param name="Width"></param>
    /// <returns>DataGridViewColumn</returns>
    public static DataGridViewColumn SetPercentStyle(this DataGridViewColumn column, int Width = 80)
    {
      return column.SetStyles(Width, DataGridViewContentAlignment.MiddleCenter, "P");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="column">DataGridViewColumn</param>
    /// <param name="Width"></param>
    /// <returns>DataGridViewColumn</returns>
    public static DataGridViewColumn SetDateStyle(this DataGridViewColumn column, int Width = 110)
    {
      return column.SetStyles(Width, DataGridViewContentAlignment.MiddleCenter, "d");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="column">DataGridViewColumn</param>
    /// <param name="Width"></param>
    /// <returns>DataGridViewColumn</returns>
    public static DataGridViewColumn SetDateTimeStyle(this DataGridViewColumn column, int Width = 160)
    {
      return column.SetStyles(Width, DataGridViewContentAlignment.MiddleRight, "g");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="column">DataGridViewColumn</param>
    /// <param name="Width"></param>
    /// <returns>DataGridViewColumn</returns>
    public static DataGridViewColumn SetDateTimeWithSecondsStyle(this DataGridViewColumn column, int Width = 160)
    {
      return column.SetStyles(Width, DataGridViewContentAlignment.MiddleRight, "G");
    }
        
  }
}
