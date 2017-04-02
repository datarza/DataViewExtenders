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

  public static partial class TableLayoutPanelExtenders
  {
    // 
    private static void SetBindingStyle(Binding binding, EditorDataStyle Style)
    {
      switch (Style)
      { // TODO: should be configured according local culture (pondus, kg) (foot, m3)
        case EditorDataStyle.Quantity: binding.FormatString = "D"; break;
        case EditorDataStyle.Price: binding.FormatString = "#,0.##' CAD.'"; break;
        case EditorDataStyle.Percent: binding.FormatString = "P"; break;
        case EditorDataStyle.DateTime: binding.FormatString = "f"; break;
        case EditorDataStyle.Date: binding.FormatString = "D"; break;
        case EditorDataStyle.Weight: binding.FormatString = "#,0.##' kg'"; break;
        case EditorDataStyle.Volume: binding.FormatString = "N3"; break;
      }
    }
  }
}
