using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
    /// <param name="viewGrid">this DataGridView</param>
    /// <param name="DataSource">Data Source to support data-binding</param>
    /// <param name="Columns">Column data descriptors</param>
    public static void AddColumns(this DataGridView viewGrid, object DataSource, params ColumnDataDescriptor[] Columns)
    {

    }

  }
}
