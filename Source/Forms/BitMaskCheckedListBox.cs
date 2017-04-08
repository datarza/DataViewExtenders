using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

//
// BitMaskCheckedListBox
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
  [ToolboxBitmap(typeof(CheckedListBox))]
  public class BitMaskCheckedListBox : CheckedListBox
  {
    public BitMaskCheckedListBox()
    {
      this.CheckOnClick = true;
      /*this.ItemCheck += (sender, e) => { this.ValueChanged?.Invoke(sender, EventArgs.Empty); };*/
      this.SelectedValueChanged += (sender, e) => { this.ValueChanged?.Invoke(sender, EventArgs.Empty); };
    }
    
    [BrowsableAttribute(false)]
    public event EventHandler ValueChanged;

    [DefaultValue(0)]
    [Bindable(true), Browsable(true)]
    public long Value
    {
      get
      {
        long result = 0;
        for (int i = 0; i < this.Items.Count; i++)
          if (this.GetItemChecked(i)) result |= (long)1 << i;
        return result;
      }
      set
      {
        this.BeginUpdate();
        for (int i = 0; i < this.Items.Count; i++)
        {
          long j = (long)1 << i;
          bool nch = (value & j) == j;
          bool och = this.GetItemChecked(i);
          if (nch != och) this.SetItemChecked(i, nch);
        }
        this.EndUpdate();
      }
    }

  }
}
