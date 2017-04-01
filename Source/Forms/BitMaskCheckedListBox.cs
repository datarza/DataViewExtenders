using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CBComponents.Forms
{
  [ToolboxBitmap(typeof(CheckedListBox))]
  internal sealed class BitMaskCheckedListBox : CheckedListBox
  {
    public BitMaskCheckedListBox()
    {
      this.CheckOnClick = true;
      this.ItemCheck += (sender, e) => { if (this.ValueChanged != null) this.ValueChanged(sender, EventArgs.Empty); };
    }

    [BrowsableAttribute(false)]
    public event EventHandler ValueChanged;

    [DefaultValue(0)]
    [Bindable(true), Browsable(true)]
    public long Value
    {
      get
      {
        int result = 0;
        for (int i = 0; i < this.Items.Count; i++)
          if (this.GetItemChecked(i)) result |= 1 << i;
        return result;
      }
      set
      {
        this.BeginUpdate();
        for (int i = 0; i < this.Items.Count; i++)
        {
          int j = 1 << i;
          bool nch = (value & j) == j;
          bool och = this.GetItemChecked(i);
          if (nch != och) this.SetItemChecked(i, nch);
        }
        this.EndUpdate();
      }
    }

  }
}
