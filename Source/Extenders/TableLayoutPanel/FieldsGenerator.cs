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
    /// <summary>
    /// Fields generator that works on field data descriptions (FieldDataDescriptor)
    /// </summary>
    /// <param name="dataPanel">TableLayoutPanel</param>
    /// <param name="DataSource">Data Source to support data-binding</param>
    /// <param name="Fields">Field data descriptors</param>
    public static void GenerateFields(this TableLayoutPanel dataPanel, object DataSource, params FieldDataDescriptor[] Fields)
    {
      TableLayoutPanelExtenders.GenerateFields(dataPanel, null, DataSource, Fields);
    }

    /// <summary>
    /// Fields generator that works on field data descriptions (FieldDataDescriptor)
    /// </summary>
    /// <param name="dataPanel">TableLayoutPanel</param>
    /// <param name="toolTip">ToolTip</param>
    /// <param name="DataSource">Data Source to support data-binding</param>
    /// <param name="Fields">Field data descriptors</param>
    public static void GenerateFields(this TableLayoutPanel dataPanel, ToolTip toolTip, object DataSource, params FieldDataDescriptor[] Fields)
    {
      int tabIndex = 0;
      dataPanel.SuspendLayout();

      foreach (var column in Fields)
      {
        dataPanel.RowCount += 1;
        dataPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var _label = new Label(); // creating the label
        _label.AutoSize = true;
        _label.Margin = new Padding(12, column.Mode == FieldEditorMode.MultilineTextBox ? 3 : 0, 3, 0);
        if (column.Mode == FieldEditorMode.MultilineTextBox || column.Mode == FieldEditorMode.BitMask)
        {
          _label.Anchor = AnchorStyles.Right | AnchorStyles.Top;
          _label.TextAlign = ContentAlignment.BottomRight;
        }
        else
        {
          _label.Anchor = AnchorStyles.Right;
          _label.TextAlign = ContentAlignment.MiddleRight;
        }
        _label.TabIndex = tabIndex++;
        _label.Text = column.HeaderText;
        dataPanel.Controls.Add(_label, 0, dataPanel.RowCount - 1);

        if (column.Mode == FieldEditorMode.TextBox || column.Mode == FieldEditorMode.MultilineTextBox || column.Mode == FieldEditorMode.DateTimeTextBox)
        { // creating editor control for text
          var _textBox = new TextBox();
          column.GeneratedControl = _textBox;
          _textBox.Multiline = column.Mode == FieldEditorMode.MultilineTextBox;
          _textBox.Size = new Size(column.SizeWidth.HasValue ? column.SizeWidth.Value : (int)DataDescriptorSizeWidth.Normal, _textBox.Multiline ? 150 : 20);
          _textBox.Anchor = AnchorStyles.Left;
          if (column.MaxLength.HasValue) _textBox.MaxLength = column.MaxLength.Value;
          _textBox.TabIndex = tabIndex++;
          if (toolTip != null) toolTip.SetToolTip(_textBox, column.ColumnName);
          _label.Click += delegate { _textBox.Focus(); };
          var binding = new Binding("Text", DataSource, column.ColumnName, true, column.IsReadOnly ? DataSourceUpdateMode.Never : DataSourceUpdateMode.OnPropertyChanged);
          if (column.FormatValueMethod != null)
          {
            _textBox.Tag = column.FormatValueMethod;
            binding.FormattingEnabled = true;
            binding.Format += new ConvertEventHandler(BindingFormat);
          }
          if (column.IsNull) binding.DataSourceNullValue = DBNull.Value;
          if (column.NullValue != null) binding.NullValue = column.NullValue;
          if (column.Style.HasValue)
          {
            TableLayoutPanelExtenders.SetBindingStyle(binding, column.Style.Value);
            _textBox.TextAlign = column.Style == EditorDataStyle.DateTime || column.Style == EditorDataStyle.Date ? HorizontalAlignment.Center : HorizontalAlignment.Right;
          }
          _textBox.DataBindings.Add(binding);
          _textBox.ReadOnly = column.IsReadOnly;
          if (column.DataSource != null && column.DataSource is string[])
          {
            _textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            _textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            _textBox.AutoCompleteCustomSource = new AutoCompleteStringCollection();
            _textBox.AutoCompleteCustomSource.AddRange((string[])column.DataSource);
          }
          dataPanel.Controls.Add(_textBox, 1, dataPanel.RowCount - 1);
        }
        else if (column.Mode == FieldEditorMode.NumberTextBox)
        { // creating editor control for numbers
          var _textBox = new NumericUpDown();
          column.GeneratedControl = _textBox;
          _textBox.Minimum = column.Minimum.HasValue ? column.Minimum.Value : decimal.MinValue;
          _textBox.Maximum = column.Maximum.HasValue ? column.Maximum.Value : decimal.MaxValue;
          _textBox.DecimalPlaces = 0;
          _textBox.ThousandsSeparator = true;
          _textBox.Size = new Size(column.SizeWidth.HasValue ? column.SizeWidth.Value : (int)DataDescriptorSizeWidth.Normal, 20);
          _textBox.Anchor = AnchorStyles.Left;
          _textBox.TabIndex = tabIndex++;
          if (toolTip != null) toolTip.SetToolTip(_textBox, column.ColumnName);
          _label.Click += delegate { _textBox.Focus(); };
          var binding = new Binding("Text", DataSource, column.ColumnName, true, column.IsReadOnly ? DataSourceUpdateMode.Never : DataSourceUpdateMode.OnPropertyChanged);
          if (column.FormatValueMethod != null)
          {
            _textBox.Tag = column.FormatValueMethod;
            binding.FormattingEnabled = true;
            binding.Format += new ConvertEventHandler(BindingFormat);
          }
          if (column.IsNull) binding.DataSourceNullValue = DBNull.Value;
          if (column.NullValue != null) binding.NullValue = column.NullValue;
          if (column.Style.HasValue)
          {
            TableLayoutPanelExtenders.SetBindingStyle(binding, column.Style.Value);
            _textBox.TextAlign = HorizontalAlignment.Right;
          }
          _textBox.DataBindings.Add(binding);
          _textBox.ReadOnly = column.IsReadOnly;
          dataPanel.Controls.Add(_textBox, 1, dataPanel.RowCount - 1);
        }
        else if (column.Mode == FieldEditorMode.CheckBox)
        { // creating editor control for booleans
          var _checkBox = new CheckBox();
          column.GeneratedControl = _checkBox;
          _checkBox.Anchor = AnchorStyles.Left;
          _checkBox.AutoSize = false;
          _checkBox.Size = new Size(20, 20);
          _checkBox.TabIndex = tabIndex++;
          if (toolTip != null) toolTip.SetToolTip(_checkBox, column.ColumnName);
          _label.Click += delegate { _checkBox.Checked = !_checkBox.Checked; };
          if (column.IsNull)
          {
            _checkBox.DataBindings.Add(new Binding("CheckState", DataSource, column.ColumnName, true, column.IsReadOnly ? DataSourceUpdateMode.Never : DataSourceUpdateMode.OnPropertyChanged, CheckState.Indeterminate));
            _checkBox.ThreeState = true;
          }
          else _checkBox.DataBindings.Add(new Binding("Checked", DataSource, column.ColumnName, false, column.IsReadOnly ? DataSourceUpdateMode.Never : DataSourceUpdateMode.OnPropertyChanged, false));
          _checkBox.Enabled = !column.IsReadOnly;
          dataPanel.Controls.Add(_checkBox, 1, dataPanel.RowCount - 1);
        }
        else if (column.Mode == FieldEditorMode.ListBox)
        { // creating editor control for lists with dialog
          var _comboBox = new ComboBox();
          column.GeneratedControl = _comboBox;
          _comboBox.Size = new Size(column.SizeWidth.HasValue ? column.SizeWidth.Value : (int)DataDescriptorSizeWidth.Normal, 21);
          _comboBox.Anchor = AnchorStyles.Left;
          if (column.MaxLength.HasValue) _comboBox.MaxLength = column.MaxLength.Value;
          _comboBox.TabIndex = tabIndex++;
          _comboBox.DropDownStyle = ComboBoxStyle.Simple;
          if (toolTip != null) toolTip.SetToolTip(_comboBox, column.ColumnName);
          _label.Click += delegate { _comboBox.Focus(); };
          if (column.FormatValueMethod != null)
          {
            var binding = new Binding("Text", DataSource, column.ColumnName, true, DataSourceUpdateMode.Never);
            _comboBox.Tag = column.FormatValueMethod;
            binding.FormattingEnabled = true;
            binding.Format += new ConvertEventHandler(BindingFormat);
            if (column.IsNull) binding.DataSourceNullValue = DBNull.Value;
            _comboBox.DataBindings.Add(binding);
          }
          else
          {
            var binding = new Binding("SelectedValue", DataSource, column.ColumnName, true, DataSourceUpdateMode.Never);
            if (column.IsNull) binding.DataSourceNullValue = DBNull.Value;
            _comboBox.DataBindings.Add(binding);
          }
          _comboBox.DataSource = column.DataSource;
          if (!string.IsNullOrWhiteSpace(column.ValueMember)) _comboBox.ValueMember = column.ValueMember;
          if (!string.IsNullOrWhiteSpace(column.DisplayMember)) _comboBox.DisplayMember = column.DisplayMember;
          _comboBox.Enabled = false;
          if (column.IsReadOnly) dataPanel.Controls.Add(_comboBox, 1, dataPanel.RowCount - 1);
          else
          {
            var _pnl = new TableLayoutPanel();
            _pnl.AutoSize = true;
            _pnl.RowCount = 1;
            _pnl.ColumnCount = 2;
            _pnl.ColumnStyles.Add(new ColumnStyle());
            _pnl.ColumnStyles.Add(new ColumnStyle());
            _pnl.Controls.Add(_comboBox, 0, 0);
            var _btnSelect = new Button();
            _btnSelect.Anchor = AnchorStyles.Left;
            _btnSelect.Size = new Size(20, 20);
            _btnSelect.FlatStyle = FlatStyle.Flat;
            _btnSelect.FlatAppearance.BorderSize = 0;
            _btnSelect.FlatAppearance.MouseOverBackColor = SystemColors.Control;
            _btnSelect.FlatAppearance.MouseDownBackColor = SystemColors.ControlLight;
            _btnSelect.Image = FormServices.Images.TableSelectImage;
            _btnSelect.ImageAlign = ContentAlignment.MiddleCenter;
            _btnSelect.TabIndex = tabIndex++;
            _btnSelect.TabStop = false;
            _btnSelect.Tag = Tuple.Create(column.DataSource, column.ValueMember, column.DisplayMember, column.ColumnName, column.GetListBoxItemsMethod);
            if (toolTip != null) toolTip.SetToolTip(_btnSelect, string.Format("Select a value for {0}", column.ColumnName));
            _btnSelect.Click += new EventHandler(TableLayoutPanelExtenders.SelectFieldClick);
            _pnl.Controls.Add(_btnSelect, 1, 0);
            _pnl.Margin = new Padding(0, _pnl.Margin.Top, 0, _pnl.Margin.Bottom);
            _comboBox.Size = new Size((column.SizeWidth.HasValue ? column.SizeWidth.Value : (int)DataDescriptorSizeWidth.Normal) - _btnSelect.Width - _btnSelect.Margin.Left - _btnSelect.Margin.Right - _comboBox.Margin.Right, _comboBox.Height);
            dataPanel.Controls.Add(_pnl, 1, dataPanel.RowCount - 1);
          }
        }
        else if (column.Mode == FieldEditorMode.ComboBox || column.Mode == FieldEditorMode.ComboTextBox)
        { // creating editor control for drop down lists
          var _comboBox = new ComboBox();
          column.GeneratedControl = _comboBox;
          _comboBox.Size = new Size(column.SizeWidth.HasValue ? column.SizeWidth.Value : (int)DataDescriptorSizeWidth.Normal, 20);
          _comboBox.Anchor = AnchorStyles.Left;
          if (column.MaxLength.HasValue) _comboBox.MaxLength = column.MaxLength.Value;
          _comboBox.TabIndex = tabIndex++;
          _comboBox.DropDownStyle = column.Mode == FieldEditorMode.ComboBox ? ComboBoxStyle.DropDownList : ComboBoxStyle.DropDown;
          if (toolTip != null) toolTip.SetToolTip(_comboBox, column.ColumnName);
          _label.Click += delegate { _comboBox.Focus(); if (_comboBox.Enabled) _comboBox.DroppedDown = true; };
          var binding = new Binding(column.Mode == FieldEditorMode.ComboBox ? "SelectedValue" : "Text", DataSource, column.ColumnName, true, column.IsReadOnly ? DataSourceUpdateMode.Never : DataSourceUpdateMode.OnPropertyChanged);
          if (column.IsNull) binding.DataSourceNullValue = DBNull.Value;
          _comboBox.DataBindings.Add(binding);
          _comboBox.DataSource = column.DataSource;
          if (!string.IsNullOrWhiteSpace(column.ValueMember)) _comboBox.ValueMember = column.ValueMember;
          if (!string.IsNullOrWhiteSpace(column.DisplayMember)) _comboBox.DisplayMember = column.DisplayMember;
          _comboBox.Enabled = !column.IsReadOnly;
          if (column.Mode == FieldEditorMode.ComboTextBox)
          {
            _comboBox.AutoCompleteMode = AutoCompleteMode.Append;
            _comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
          }
          dataPanel.Controls.Add(_comboBox, 1, dataPanel.RowCount - 1);
        }
        else if (column.Mode == FieldEditorMode.GuidEditor)
        { // creating editor control for Guid
          var _textBox = new TextBox();
          column.GeneratedControl = _textBox;
          _textBox.Size = new Size((int)DataDescriptorSizeWidth.Normal, 20);
          _textBox.Anchor = AnchorStyles.Left;
          _textBox.TabIndex = tabIndex++;
          if (toolTip != null) toolTip.SetToolTip(_textBox, column.ColumnName);
          _label.Click += delegate { _textBox.Focus(); };
          if (DataSource != null) _textBox.DataBindings.Add(new Binding("Text", DataSource, column.ColumnName, true, DataSourceUpdateMode.Never, "(null)"));
          _textBox.ReadOnly = true;
          dataPanel.Controls.Add(_textBox, 1, dataPanel.RowCount - 1);
        }
        else if (column.Mode == FieldEditorMode.BitMask)
        { // creating editor control for mask (bits)
          var _listBox = new BitMaskCheckedListBox();
          column.GeneratedControl = _listBox;
          _listBox.Size = new Size(column.SizeWidth.HasValue ? column.SizeWidth.Value : (int)DataDescriptorSizeWidth.Normal, 150);
          _listBox.Anchor = AnchorStyles.Left;
          _listBox.TabIndex = tabIndex++;
          if (toolTip != null) toolTip.SetToolTip(_listBox, column.ColumnName);
          _label.Click += delegate { _listBox.Focus(); };
          var binding = new Binding("Value", DataSource, column.ColumnName, true, column.IsReadOnly ? DataSourceUpdateMode.Never : DataSourceUpdateMode.OnPropertyChanged);
          if (column.IsNull) binding.DataSourceNullValue = DBNull.Value;
          if (column.NullValue != null) binding.NullValue = column.NullValue;
          if (column.Style.HasValue) TableLayoutPanelExtenders.SetBindingStyle(binding, column.Style.Value);
          _listBox.DataBindings.Add(binding);
          _listBox.Enabled = !column.IsReadOnly;
          if (column.DataSource != null)
          {
            if (column.DataSource is string[])
            {
              _listBox.Items.AddRange((string[])column.DataSource);
              if (_listBox.Items.Count > 0)
              {
                var _nHeight = _listBox.Items.Count * (_listBox.GetItemHeight(0) + 4) + 1;
                if (_nHeight < _listBox.Size.Height) _listBox.Size = new Size(_listBox.Size.Width, _nHeight);
              }
            }
            else
            {
              _listBox.DataSource = column.DataSource;
              _listBox.DisplayMember = column.DisplayMember;
            }
          }
          dataPanel.Controls.Add(_listBox, 1, dataPanel.RowCount - 1);
        }
        if (column.IsNull && !column.IsReadOnly && column.Mode != FieldEditorMode.CheckBox || column.Mode == FieldEditorMode.GuidEditor)
        { // button for clear value
          if (dataPanel.ColumnCount == 2)
          {
            dataPanel.ColumnCount = 3;
            dataPanel.ColumnStyles.Add(new ColumnStyle());
          }
          var _btnClear = new Button();
          _btnClear.Anchor = AnchorStyles.Left;
          if (column.Mode == FieldEditorMode.MultilineTextBox) _btnClear.Anchor |= AnchorStyles.Top;
          _btnClear.Size = new Size(20, 20);
          _btnClear.FlatStyle = FlatStyle.Flat;
          _btnClear.FlatAppearance.BorderSize = 0;
          _btnClear.FlatAppearance.MouseOverBackColor = SystemColors.Control;
          _btnClear.FlatAppearance.MouseDownBackColor = SystemColors.ControlLight;
          _btnClear.Image = FormServices.Images.TableClearImage;
          _btnClear.ImageAlign = ContentAlignment.MiddleCenter;
          _btnClear.TabIndex = tabIndex++;
          _btnClear.TabStop = false;
          _btnClear.Click += new EventHandler(TableLayoutPanelExtenders.ClearFieldClick);
          _btnClear.Tag = column.ColumnName;
          if (toolTip != null) toolTip.SetToolTip(_btnClear, string.Format("Clear value from {0}", column.ColumnName));
          dataPanel.Controls.Add(_btnClear, 2, dataPanel.RowCount - 1);
        }
      }

      dataPanel.ResumeLayout();

    }

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

    private static void ClearFieldClick(object sender, EventArgs e)
    {
      // TODO: should be realized - var cm = base.BindingContext[this.DataSource] as CurrencyManager;
      CurrencyManager cm = null;
      if (cm != null)
        try
        {
          var col = cm.GetItemProperties().Find((string)((Control)sender).Tag, false);
          if (col != null && cm.Count > 0) col.SetValue(cm.Current, DBNull.Value);
          cm.EndCurrentEdit();
          // TODO: should be realized to select previous control - this.ProcessTabKey(false);
        }
        catch (Exception ex)
        {
          FormServices.ShowError(ex);
          cm.CancelCurrentEdit();
        }
    }

    private static void SelectFieldClick(object sender, EventArgs e)
    {
      // TODO: should be realized - var cm = base.BindingContext[this.DataSource] as CurrencyManager;
      CurrencyManager cm = null;
      if (cm != null)
      {
        var _sender = (Control)sender;
        var _data = _sender.Tag as Tuple<object, string, string, string, GetListBoxItemsDelegate>;
        if (_data != null)
          try
          {
            var col = cm.GetItemProperties().Find(_data.Item4, false);
            if (col != null)
            {
              object items = _data.Item1;
              bool agc = false;
              if (_data.Item5 != null)
              {
                items = _data.Item5.Invoke();
                if (items == null)
                {
                  FormServices.ShowError("No data", true);
                  return;
                }
                agc = true;
              }
              var row = SelectItemForm.GetSelectedRow(items, _data.Item2, _data.Item3, col.GetValue(cm.Current), agc);
              if (row != null)
              {
                col.SetValue(cm.Current, row[_data.Item2]);
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
