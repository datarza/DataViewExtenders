using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Examples
{
  public partial class Example1Form : Form
  {
    public const string TextName = "BitMaskCheckedListBox example";

    public Example1Form()
    {
      InitializeComponent();
      this.Text = Example1Form.TextName;

      long j = 1;
      for (int i = 0; i < 201; i++)
      {
        if (i<32) this.bmclBox.Items.Add(string.Format("{0,2:D2} - {1}", i + 1, j));
        else if (i < 64) this.bmclBox.Items.Add(string.Format("{0,2:D2}", i + 1));
        else this.bmclBox.Items.Add(string.Format("+{0,2:D3}", i + 1));
        j = j * 2;
      }

      this.textBoxValue.DataBindings.Add("Text", this.bmclBox, "Value", false, DataSourceUpdateMode.OnPropertyChanged);
      this.textBoxLongValue.DataBindings.Add("Text", this.bmclBox, "LongValue", false, DataSourceUpdateMode.OnPropertyChanged);
      this.bmclBox.SelectedValueChanged += BmclBox_SelectedValueChanged;

      this.bmclBox.Value = 54613;
      this.BmclBox_SelectedValueChanged(this, EventArgs.Empty);
    }

    private void BmclBox_SelectedValueChanged(object sender, EventArgs e)
    {
      var items = this.bmclBox.GetValues();
      string result = string.Empty;
      for (int i = 0; i < items.Length; i++)
        if (items[i])
          result += string.Format("{0,2:D2}, ", i + 1);
      if (!string.IsNullOrEmpty(result)) result = result.Remove(result.Length - 2);
      this.textBoxValues.Text = result;      
    }

    private void ExamplesForm_Load(object sender, EventArgs e)
    {

    }
  }
}
