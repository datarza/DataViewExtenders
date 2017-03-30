using System;
using System.Data;

namespace CBComponents.DataDescriptors
{
  /// <summary>
  /// Editor mode of columns on the GridView
  /// </summary>
  public enum ColumnEditorMode
  {
    Auto,
    TextBox,
    CheckBox,
    ComboBox, ListBox
  }

  /// <summary>
  /// DataDescriptor of columns on the GridView
  /// </summary>
  public class ColumnDataDescriptor
  {
    public string HeaderText { get; set; }
    public string ColumnName { get; set; }
    public ColumnEditorMode Mode { get; set; }
    public EditorDataStyle? Style { get; set; }
    public bool IsNull { get; set; }
    public object NullValue { get; set; }
    public bool IsReadOnly { get; set; }
    public int? MaxLength { get; set; }
    public float? FillWeight { get; set; }
    public object DataSource { get; set; }
    public string ValueMember { get; set; }
    public string DisplayMember { get; set; }
    public GetListBoxItemsDelegate GetListBoxItemsMethod { get; set; }
    public FormatValueDelegate FormatValueMethod { get; set; }
    public ColumnDataDescriptor(string HeaderText, DataColumn Column, ColumnEditorMode Mode = ColumnEditorMode.Auto,
                        EditorDataStyle? Style = null, float? FillWeight = null, object NullValue = null, bool IsReadOnly = false,
                        object DataSource = null, string ValueMember = null, string DisplayMember = null,
                        GetListBoxItemsDelegate GetListBoxItemsMethod = null,
                        FormatValueDelegate FormatValueMethod = null)
    {
      if (Mode == ColumnEditorMode.Auto)
      {
        if (DataSource != null && ValueMember != null && DisplayMember != null && GetListBoxItemsMethod != null) Mode = ColumnEditorMode.ListBox;
        else if (ValueMember != null && GetListBoxItemsMethod != null && FormatValueMethod != null) Mode = ColumnEditorMode.ListBox;
        else if (DataSource != null && ValueMember != null && DisplayMember != null) Mode = ColumnEditorMode.ComboBox;
        else if (FormatValueMethod != null) { Mode = ColumnEditorMode.TextBox; this.IsReadOnly = true; }
        else if (Column.DataType == typeof(bool)) Mode = ColumnEditorMode.CheckBox;
        else if (Column.DataType == typeof(Guid)) { Mode = ColumnEditorMode.TextBox; this.IsReadOnly = true; }
        else Mode = ColumnEditorMode.TextBox;
      }
      this.HeaderText = HeaderText; // Column.Caption;
      this.ColumnName = Column.ColumnName;
      this.Mode = Mode;
      this.Style = Style;
      this.IsNull = Column.AllowDBNull;
      this.NullValue = NullValue;
      this.IsReadOnly = this.IsReadOnly || Column.ReadOnly || IsReadOnly;
      if (Column.MaxLength > 0) this.MaxLength = Column.MaxLength;
      this.FillWeight = FillWeight;
      this.DataSource = DataSource;
      this.ValueMember = ValueMember;
      this.DisplayMember = DisplayMember;
      this.GetListBoxItemsMethod = GetListBoxItemsMethod;
      this.FormatValueMethod = FormatValueMethod;
    }
    /// <summary>
    /// Resulted Column after generation 
    /// </summary>
    public System.Windows.Forms.DataGridViewColumn GeneratedDataGridViewColumn = null;
  }
  
}
