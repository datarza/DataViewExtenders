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
      dataPanel.SuspendLayout();
      dataPanel.Controls.Clear();
      if (DataSource == null || Fields == null || Fields.Length == 0) return;
      dataPanel.ColumnStyles.Clear();
      dataPanel.ColumnCount = 2;
      dataPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
      dataPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
      dataPanel.RowStyles.Clear();
      dataPanel.RowCount = 0;
      dataPanel.AutoSize = true;
      dataPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      dataPanel.Margin = new Padding(3, 3, 9, 3);

      int tabIndex = 0;
      foreach (var field in Fields)
      {
        dataPanel.RowCount += 1;
        dataPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        #region creating the label

        var leadLabel = new Label();
        leadLabel.Text = field.CaptionText; // TODO: add option for generating label in format: column.CaptionText + ":"
        leadLabel.TextAlign = ContentAlignment.MiddleRight;
        leadLabel.Padding = new Padding(0, 0, 0, 0);
        if (field.Mode == FieldEditorMode.MultilineTextBox || field.Mode == FieldEditorMode.BitMask)
        {
          leadLabel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
          leadLabel.Margin = new Padding(12, 6, 0, 6);
        }
        else
        {
          leadLabel.Anchor = AnchorStyles.Right;
          leadLabel.Margin = new Padding(12, 1, 0, 1);
        }
        leadLabel.TabIndex = tabIndex++;
        leadLabel.AutoSize = true;
        dataPanel.Controls.Add(leadLabel, 0, dataPanel.RowCount - 1);

        #endregion

        Binding binding = null;
        if (field.Mode == FieldEditorMode.TextBox || field.Mode == FieldEditorMode.MultilineTextBox || field.Mode == FieldEditorMode.DateTimeTextBox)
        {
          #region creating editor control for text
          var _textBox = new TextBox();
          field.GeneratedControl = _textBox;
          _textBox.Multiline = field.Mode == FieldEditorMode.MultilineTextBox;
          _textBox.Size = new Size(field.SizeWidth.HasValue ? field.SizeWidth.Value : (int)DataDescriptorSizeWidth.Normal, _textBox.Multiline ? 125 : 20);
          _textBox.Anchor = AnchorStyles.Left;
          if (field.MaxLength.HasValue) _textBox.MaxLength = field.MaxLength.Value;
          _textBox.TabIndex = tabIndex++;
          if (toolTip != null) toolTip.SetToolTip(_textBox, field.ColumnName);
          leadLabel.Click += delegate { _textBox.Focus(); };
          binding = new Binding("Text", DataSource, field.ColumnName, true, field.IsReadOnly ? DataSourceUpdateMode.Never : DataSourceUpdateMode.OnPropertyChanged);
          if (field.FormatValueMethod != null)
          {
            _textBox.Tag = field.FormatValueMethod;
            binding.FormattingEnabled = true;
            binding.Format += TableLayoutPanelExtenders.BindingFormat;
          }
          if (field.IsNull) binding.DataSourceNullValue = DBNull.Value;
          if (field.NullValue != null) binding.NullValue = field.NullValue;
          if (field.Style.HasValue)
          {
            TableLayoutPanelExtenders.SetBindingStyle(binding, field.Style.Value);
            _textBox.TextAlign = field.Style == EditorDataStyle.DateTime || field.Style == EditorDataStyle.Date ? HorizontalAlignment.Center : HorizontalAlignment.Right;
          }
          _textBox.DataBindings.Add(binding);
          _textBox.ReadOnly = field.IsReadOnly;
          if (field.DataSource != null && field.DataSource is string[])
          {
            _textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            _textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            _textBox.AutoCompleteCustomSource = new AutoCompleteStringCollection();
            _textBox.AutoCompleteCustomSource.AddRange((string[])field.DataSource);
          }
          dataPanel.Controls.Add(_textBox, 1, dataPanel.RowCount - 1);
          #endregion
        }
        else if (field.Mode == FieldEditorMode.NumberTextBox)
        {
          #region creating editor control for numbers
          var _textBox = new NumericUpDown();
          field.GeneratedControl = _textBox;
          _textBox.Minimum = field.Minimum.HasValue ? field.Minimum.Value : decimal.MinValue;
          _textBox.Maximum = field.Maximum.HasValue ? field.Maximum.Value : decimal.MaxValue;
          _textBox.DecimalPlaces = 0;
          _textBox.ThousandsSeparator = true;
          _textBox.Size = new Size(field.SizeWidth.HasValue ? field.SizeWidth.Value : (int)DataDescriptorSizeWidth.Normal, 20);
          _textBox.Anchor = AnchorStyles.Left;
          _textBox.TabIndex = tabIndex++;
          if (toolTip != null) toolTip.SetToolTip(_textBox, field.ColumnName);
          leadLabel.Click += delegate { _textBox.Focus(); };
          binding = new Binding("Text", DataSource, field.ColumnName, true, field.IsReadOnly ? DataSourceUpdateMode.Never : DataSourceUpdateMode.OnPropertyChanged);
          if (field.FormatValueMethod != null)
          {
            _textBox.Tag = field.FormatValueMethod;
            binding.FormattingEnabled = true;
            binding.Format += TableLayoutPanelExtenders.BindingFormat;
          }
          if (field.IsNull) binding.DataSourceNullValue = DBNull.Value;
          if (field.NullValue != null) binding.NullValue = field.NullValue;
          if (field.Style.HasValue)
          {
            TableLayoutPanelExtenders.SetBindingStyle(binding, field.Style.Value);
            _textBox.TextAlign = HorizontalAlignment.Right;
          }
          _textBox.DataBindings.Add(binding);
          _textBox.ReadOnly = field.IsReadOnly;
          dataPanel.Controls.Add(_textBox, 1, dataPanel.RowCount - 1);
          #endregion
        }
        else if (field.Mode == FieldEditorMode.CheckBox)
        {
          #region creating editor control for booleans
          var _checkBox = new CheckBox();
          field.GeneratedControl = _checkBox;
          _checkBox.Anchor = AnchorStyles.Left;
          _checkBox.AutoSize = false;
          _checkBox.Size = new Size(20, 20);
          _checkBox.TabIndex = tabIndex++;
          if (toolTip != null) toolTip.SetToolTip(_checkBox, field.ColumnName);
          leadLabel.Click += delegate { _checkBox.Checked = !_checkBox.Checked; };
          if (field.IsNull)
          {
            _checkBox.DataBindings.Add(new Binding("CheckState", DataSource, field.ColumnName, true, field.IsReadOnly ? DataSourceUpdateMode.Never : DataSourceUpdateMode.OnPropertyChanged, CheckState.Indeterminate));
            _checkBox.ThreeState = true;
          }
          else _checkBox.DataBindings.Add(new Binding("Checked", DataSource, field.ColumnName, false, field.IsReadOnly ? DataSourceUpdateMode.Never : DataSourceUpdateMode.OnPropertyChanged, false));
          _checkBox.Enabled = !field.IsReadOnly;
          dataPanel.Controls.Add(_checkBox, 1, dataPanel.RowCount - 1);
          #endregion
        }
        else if (field.Mode == FieldEditorMode.ListBox)
        {
          #region creating editor control for lists with dialog
          var _comboBox = new ComboBox();
          field.GeneratedControl = _comboBox;
          _comboBox.Size = new Size(field.SizeWidth.HasValue ? field.SizeWidth.Value : (int)DataDescriptorSizeWidth.Normal, 21);
          _comboBox.Anchor = AnchorStyles.Left;
          if (field.MaxLength.HasValue) _comboBox.MaxLength = field.MaxLength.Value;
          _comboBox.TabIndex = tabIndex++;
          _comboBox.DropDownStyle = ComboBoxStyle.Simple;
          if (toolTip != null) toolTip.SetToolTip(_comboBox, field.ColumnName);
          leadLabel.Click += delegate { _comboBox.Focus(); };
          if (field.FormatValueMethod != null)
          {
            binding = new Binding("Text", DataSource, field.ColumnName, true, DataSourceUpdateMode.Never);
            _comboBox.Tag = field.FormatValueMethod;
            binding.FormattingEnabled = true;
            binding.Format += TableLayoutPanelExtenders.BindingFormat;
            if (field.IsNull) binding.DataSourceNullValue = DBNull.Value;
            _comboBox.DataBindings.Add(binding);
          }
          else
          {
            binding = new Binding("SelectedValue", DataSource, field.ColumnName, true, DataSourceUpdateMode.Never);
            if (field.IsNull) binding.DataSourceNullValue = DBNull.Value;
            _comboBox.DataBindings.Add(binding);
          }
          _comboBox.DataSource = field.DataSource;
          if (!string.IsNullOrWhiteSpace(field.ValueMember)) _comboBox.ValueMember = field.ValueMember;
          if (!string.IsNullOrWhiteSpace(field.DisplayMember)) _comboBox.DisplayMember = field.DisplayMember;
          _comboBox.Enabled = false;
          if (field.IsReadOnly) dataPanel.Controls.Add(_comboBox, 1, dataPanel.RowCount - 1);
          else
          {
            var _pnl = new TableLayoutPanel();
            _pnl.AutoSize = true;
            _pnl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _pnl.Margin = new Padding(0);
            _pnl.Padding = new Padding(0);
            _pnl.RowCount = 1;
            _pnl.ColumnCount = 2;
            _pnl.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _pnl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _pnl.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _pnl.Controls.Add(_comboBox, 0, 0);
            var _btnSelect = new Button();
            _btnSelect.Anchor = AnchorStyles.Left;
            _btnSelect.MaximumSize = new Size(128, 128);
            _btnSelect.Margin = new Padding(1, 0, 1, 0);
            _btnSelect.Padding = new Padding(0);
            _btnSelect.AutoSize = true;
            _btnSelect.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _btnSelect.FlatStyle = FlatStyle.Flat;
            _btnSelect.FlatAppearance.BorderSize = 0;
            _btnSelect.FlatAppearance.MouseOverBackColor = SystemColors.GradientActiveCaption;
            _btnSelect.FlatAppearance.MouseDownBackColor = SystemColors.ControlLight;
            _btnSelect.Image = FormServices.Images.TableSelectImage;
            _btnSelect.ImageAlign = ContentAlignment.MiddleCenter;
            _btnSelect.TextAlign = ContentAlignment.MiddleCenter;
            _btnSelect.TabIndex = tabIndex++;
            _btnSelect.TabStop = false;
            if (toolTip != null) toolTip.SetToolTip(_btnSelect, string.Format("Select a value for {0}", field.ColumnName));
            _pnl.Controls.Add(_btnSelect, 1, 0);
            _comboBox.Size = new Size((field.SizeWidth.HasValue ? field.SizeWidth.Value : (int)DataDescriptorSizeWidth.Normal) - _btnSelect.Width - _btnSelect.Margin.Left - _btnSelect.Margin.Right - _comboBox.Margin.Right, _comboBox.Height);
            _btnSelect.Click += delegate (object sender, EventArgs e)
            {
              var cm = binding.BindingManagerBase;
              if (cm != null)
              {
                try
                {
                  var col = cm.GetItemProperties().Find(field.ColumnName, false);
                  if (col != null)
                  {
                    object items = field.DataSource;
                    bool agc = false;
                    if (field.GetListBoxItemsMethod != null)
                    {
                      items = field.GetListBoxItemsMethod.Invoke();
                      if (items == null)
                      {
                        FormServices.ShowError("No data exists", true);
                        return;
                      }
                      agc = true;
                    }
                    var row = SelectItemForm.GetSelectedRow(items, field.ValueMember, field.DisplayMember, col.GetValue(cm.Current), agc);
                    if (row != null)
                    {
                      col.SetValue(cm.Current, row[field.ValueMember]);
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
            };
            dataPanel.Controls.Add(_pnl, 1, dataPanel.RowCount - 1);
          }
          #endregion
        }
        else if (field.Mode == FieldEditorMode.ComboBox || field.Mode == FieldEditorMode.ComboTextBox)
        {
          #region creating editor control for drop down lists
          var _comboBox = new ComboBox();
          field.GeneratedControl = _comboBox;
          _comboBox.Size = new Size(field.SizeWidth.HasValue ? field.SizeWidth.Value : (int)DataDescriptorSizeWidth.Normal, 20);
          _comboBox.Anchor = AnchorStyles.Left;
          if (field.MaxLength.HasValue) _comboBox.MaxLength = field.MaxLength.Value;
          _comboBox.TabIndex = tabIndex++;
          _comboBox.DropDownStyle = field.Mode == FieldEditorMode.ComboBox ? ComboBoxStyle.DropDownList : ComboBoxStyle.DropDown;
          if (toolTip != null) toolTip.SetToolTip(_comboBox, field.ColumnName);
          leadLabel.Click += delegate { _comboBox.Focus(); if (_comboBox.Enabled) _comboBox.DroppedDown = true; };
          binding = new Binding(field.Mode == FieldEditorMode.ComboBox ? "SelectedValue" : "Text", DataSource, field.ColumnName, true, field.IsReadOnly ? DataSourceUpdateMode.Never : DataSourceUpdateMode.OnPropertyChanged);
          if (field.IsNull) binding.DataSourceNullValue = DBNull.Value;
          _comboBox.DataBindings.Add(binding);
          _comboBox.DataSource = field.DataSource;
          if (!string.IsNullOrWhiteSpace(field.ValueMember)) _comboBox.ValueMember = field.ValueMember;
          if (!string.IsNullOrWhiteSpace(field.DisplayMember)) _comboBox.DisplayMember = field.DisplayMember;
          _comboBox.Enabled = !field.IsReadOnly;
          if (field.Mode == FieldEditorMode.ComboTextBox)
          {
            _comboBox.AutoCompleteMode = AutoCompleteMode.Append;
            _comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
          }
          dataPanel.Controls.Add(_comboBox, 1, dataPanel.RowCount - 1);
          #endregion
        }
        else if (field.Mode == FieldEditorMode.GuidEditor)
        {
          #region creating editor control for Guid
          var _textBox = new TextBox();
          field.GeneratedControl = _textBox;
          _textBox.Size = new Size((int)DataDescriptorSizeWidth.Normal, 20);
          _textBox.Anchor = AnchorStyles.Left;
          _textBox.TabIndex = tabIndex++;
          if (toolTip != null) toolTip.SetToolTip(_textBox, field.ColumnName);
          leadLabel.Click += delegate { _textBox.Focus(); };
          if (DataSource != null) _textBox.DataBindings.Add(new Binding("Text", DataSource, field.ColumnName, true, DataSourceUpdateMode.Never, "(null)"));
          _textBox.ReadOnly = true;
          dataPanel.Controls.Add(_textBox, 1, dataPanel.RowCount - 1);
          #endregion
        }
        else if (field.Mode == FieldEditorMode.BitMask)
        {
          #region creating editor control for mask (bits)
          var _listBox = new BitMaskCheckedListBox();
          field.GeneratedControl = _listBox;
          _listBox.Size = new Size(field.SizeWidth.HasValue ? field.SizeWidth.Value : (int)DataDescriptorSizeWidth.Normal, 150);
          _listBox.Anchor = AnchorStyles.Left;
          _listBox.TabIndex = tabIndex++;
          if (toolTip != null) toolTip.SetToolTip(_listBox, field.ColumnName);
          leadLabel.Click += delegate { _listBox.Focus(); };
          binding = new Binding("Value", DataSource, field.ColumnName, true, field.IsReadOnly ? DataSourceUpdateMode.Never : DataSourceUpdateMode.OnPropertyChanged);
          if (field.IsNull) binding.DataSourceNullValue = DBNull.Value;
          if (field.NullValue != null) binding.NullValue = field.NullValue;
          if (field.Style.HasValue) TableLayoutPanelExtenders.SetBindingStyle(binding, field.Style.Value);
          _listBox.DataBindings.Add(binding);
          _listBox.Enabled = !field.IsReadOnly;
          if (field.DataSource != null)
          {
            if (field.DataSource is string[])
            {
              _listBox.Items.AddRange((string[])field.DataSource);
              if (_listBox.Items.Count > 0)
              {
                var _nHeight = _listBox.Items.Count * (_listBox.GetItemHeight(0) + 4) + 1;
                if (_nHeight < _listBox.Size.Height) _listBox.Size = new Size(_listBox.Size.Width, _nHeight);
              }
            }
            else
            {
              _listBox.DataSource = field.DataSource;
              _listBox.DisplayMember = field.DisplayMember;
            }
          }
          dataPanel.Controls.Add(_listBox, 1, dataPanel.RowCount - 1);
          #endregion
        }
        if (field.IsNull && !field.IsReadOnly && field.Mode != FieldEditorMode.CheckBox || field.Mode == FieldEditorMode.GuidEditor)
        {
          #region button for clear value
          if (dataPanel.ColumnCount == 2)
          {
            dataPanel.ColumnCount = 3;
            dataPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
          }
          var _btnClear = new Button();
          _btnClear.Anchor = AnchorStyles.Left;
          if (field.Mode == FieldEditorMode.MultilineTextBox) _btnClear.Anchor |= AnchorStyles.Top;
          _btnClear.MaximumSize = new Size(128, 128);
          _btnClear.Margin = new Padding(1, 0, 1, 0);
          _btnClear.Padding = new Padding(0);
          _btnClear.AutoSize = true;
          _btnClear.AutoSizeMode = AutoSizeMode.GrowAndShrink;
          _btnClear.FlatStyle = FlatStyle.Flat;
          _btnClear.FlatAppearance.BorderSize = 0;
          _btnClear.FlatAppearance.MouseOverBackColor = SystemColors.GradientActiveCaption;
          _btnClear.FlatAppearance.MouseDownBackColor = SystemColors.ControlLight;
          _btnClear.Image = FormServices.Images.TableClearImage;
          _btnClear.ImageAlign = ContentAlignment.MiddleCenter;
          _btnClear.TextAlign = ContentAlignment.MiddleCenter;
          _btnClear.TabIndex = tabIndex++;
          _btnClear.TabStop = false;
          if (toolTip != null) toolTip.SetToolTip(_btnClear, string.Format("Clear value from {0}", field.ColumnName));
          _btnClear.Click += delegate
          {
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
          };
          dataPanel.Controls.Add(_btnClear, 2, dataPanel.RowCount - 1);
          #endregion
        }
      }
      dataPanel.ResumeLayout(false);
    }

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
