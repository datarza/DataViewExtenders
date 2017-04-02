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
    // Called when formatting value is needed
    private static void BindingFormat(object sender, ConvertEventArgs e)
    {
      var _sender = sender as Binding;
      if (_sender != null && _sender.Control.Tag is FormatValueDelegate)
      {
        var _data = (FormatValueDelegate)_sender.Control.Tag;
        var _value = _data.Invoke(_sender.BindingManagerBase.Current, _sender.BindingMemberInfo.BindingField);
        if (_value != null) e.Value = _value.ToString();
      }
    }

    // Called when formatting value is needed for ComboBox
    private static void ComboBoxFormat(object sender, ListControlConvertEventArgs e)
    {
      var _sender = sender as ComboBox;
      if (_sender != null && _sender.Tag is FormatValueDelegate && _sender.DataBindings != null && _sender.DataBindings.Count == 1)
      {
        var _binding = _sender.DataBindings[0];
        if (_binding.BindingManagerBase != null && _binding.BindingManagerBase.Count > 0 && _binding.BindingManagerBase.Position >= 0 && _binding.BindingMemberInfo != null && _binding.BindingMemberInfo.BindingField != null)
        {
          var _data = (FormatValueDelegate)_sender.Tag;
          var _value = _data.Invoke(_binding.BindingManagerBase.Current, _binding.BindingMemberInfo.BindingField);
          if (_value != null) e.Value = _value.ToString();
        }
      }
    }

    // Called by the button (located on the right from the editor control) to set the DBNull.Value
    private static void ClearFieldClick(object sender, EventArgs e)
    {
      var _sender = (Control)sender;
      var binding = _sender.Tag as Binding;
      var cm = binding.BindingManagerBase;
      if (cm != null)
        try
        {
          var col = cm.GetItemProperties().Find(binding.BindingMemberInfo.BindingMember, false);
          if (col != null && cm.Count > 0) col.SetValue(cm.Current, DBNull.Value);
          cm.EndCurrentEdit();
          binding.Control.Focus();
        }
        catch (Exception ex)
        {
          FormServices.ShowError(ex);
          cm.CancelCurrentEdit();
        }
    }

    // Called by the button (located on the right from the editor control) to select the parent row
    private static void SelectFieldClick(object sender, EventArgs e)
    {
      var _sender = (Control)sender;
      var _data = _sender.Tag as Tuple<Binding, object, string, string, string, GetListBoxItemsDelegate>;
      var binding = _data.Item1;
      var cm = binding.BindingManagerBase;
      if (cm != null)
      {
        if (_data != null)
          try
          {
            var col = cm.GetItemProperties().Find(_data.Item5, false);
            if (col != null)
            {
              object items = _data.Item2;
              bool agc = false;
              if (_data.Item6 != null)
              {
                items = _data.Item6.Invoke();
                if (items == null)
                {
                  FormServices.ShowError("No data exists", true);
                  return;
                }
                agc = true;
              }
              var row = SelectItemForm.GetSelectedRow(items, _data.Item3, _data.Item4, col.GetValue(cm.Current), agc);
              if (row != null)
              {
                col.SetValue(cm.Current, row[_data.Item3]);
                cm.EndCurrentEdit();
              }
            }
          }
          catch (Exception ex)
          {
            FormServices.ShowError(ex);
            cm.CancelCurrentEdit();
          }
      }
    }

  }
}
