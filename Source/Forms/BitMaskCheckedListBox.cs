using System;
using System.ComponentModel;
using System.Drawing;
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
      /*this.ItemCheck += (sender, e) =>
      {
        this.ValueChanged?.Invoke(sender, EventArgs.Empty);
        this.LongValueChanged?.Invoke(sender, EventArgs.Empty);
      };*/
      this.SelectedValueChanged += (sender, e) =>
      {
        this.ValueChanged?.Invoke(sender, EventArgs.Empty);
        this.LongValueChanged?.Invoke(sender, EventArgs.Empty);
      };
    }
    
    [BrowsableAttribute(false)]
    public event EventHandler ValueChanged;

    [BrowsableAttribute(false)]
    public event EventHandler LongValueChanged;

    [DefaultValue(0)]
    [Bindable(true), Browsable(true)]
    public int Value
    {
      get
      {
        int itemsCount = this.Items.Count;
        if (itemsCount > 32) itemsCount = 32;
        int result = 0;
        for (int i = 0; i < itemsCount; i++)
          if (this.GetItemChecked(i)) result |= 1 << i;
        return result;
      }
      set
      {
        int itemsCount = this.Items.Count;
        if (itemsCount > 32) itemsCount = 32;
        this.BeginUpdate();
        for (int i = 0; i < itemsCount; i++)
        {
          int j = 1 << i;
          bool nch = (value & j) == j;
          bool och = this.GetItemChecked(i);
          if (nch != och) this.SetItemChecked(i, nch);
        }
        this.EndUpdate();
        this.LongValueChanged?.Invoke(this, EventArgs.Empty);
      }
    }

    [DefaultValue(0)]
    [Bindable(true), Browsable(true)]
    public long LongValue
    {
      get
      {
        int itemsCount = this.Items.Count;
        if (itemsCount > 64) itemsCount = 64;
        long result = 0;
        for (int i = 0; i < itemsCount; i++)
          if (this.GetItemChecked(i)) result |= 1L << i;
        return result;
      }
      set
      {
        int itemsCount = this.Items.Count;
        if (itemsCount > 64) itemsCount = 64;
        this.BeginUpdate();
        for (int i = 0; i < itemsCount; i++)
        {
          long j = 1L << i;
          bool nch = (value & j) == j;
          bool och = this.GetItemChecked(i);
          if (nch != och) this.SetItemChecked(i, nch);
        }
        this.EndUpdate();
        this.ValueChanged?.Invoke(this, EventArgs.Empty);
      }
    }

    public bool[] GetValues()
    {
      int itemsCount = this.Items.Count;
      bool[] result = new bool[itemsCount];
      for (int i = 0; i < itemsCount; i++)
        result[i] = this.GetItemChecked(i);
      return result;
    }

    public void SetValues(bool[] Values)
    {
      int itemsCount = this.Items.Count;
      if (itemsCount > Values.Length) itemsCount = Values.Length;
      this.BeginUpdate();
      for (int i = 0; i < itemsCount; i++)
      {
        bool nch = Values[i];
        bool och = this.GetItemChecked(i);
        if (nch != och) this.SetItemChecked(i, nch);
      }
      this.EndUpdate();
      this.ValueChanged?.Invoke(this, EventArgs.Empty);
      this.LongValueChanged?.Invoke(this, EventArgs.Empty);
    }

  }
}
