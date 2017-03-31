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
