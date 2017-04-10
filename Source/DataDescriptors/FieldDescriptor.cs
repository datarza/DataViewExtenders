using System;
using System.Data;

namespace CBComponents.DataDescriptors
{
  /// <summary>
  /// Editor mode of fields on the Panel
  /// </summary>
  public enum FieldEditorMode
  {
    Auto,
    TextBox, MultilineTextBox,
    NumberTextBox,
    DateTimeTextBox,
    BitMask, BitMask64,
    CheckBox, ComboBox, ComboTextBox, ListBox,
    GuidEditor
  }

  /// <summary>
  /// DataDescriptor of fields on the Panel
  /// </summary>
  public class FieldDataDescriptor
  {
    public string CaptionText { get; set; }
    public string ColumnName { get; set; }
    public FieldEditorMode Mode { get; set; }
    public EditorDataStyle? Style { get; set; }
    public bool IsNull { get; set; }
    public object NullValue { get; set; }
    public bool IsReadOnly { get; set; }
    public int? MaxLength { get; set; }
    public decimal? Minimum { get; set; }
    public decimal? Maximum { get; set; }
    public int? SizeWidth { get; set; }
    public object DataSource { get; set; }
    public string ValueMember { get; set; }
    public string DisplayMember { get; set; }
    public GetListBoxItemsDelegate GetListBoxItemsMethod { get; set; }
    public FormatValueDelegate FormatValueMethod { get; set; }
    public FieldDataDescriptor(string CaptionText, DataColumn Column, FieldEditorMode Mode = FieldEditorMode.Auto,
                       EditorDataStyle? Style = null, int? SizeWidth = null, object NullValue = null, bool IsReadOnly = false,
                       decimal? Minimum = null, decimal? Maximum = null,
                       object DataSource = null, string ValueMember = null, string DisplayMember = null,
                       GetListBoxItemsDelegate GetListBoxItemsMethod = null, FormatValueDelegate FormatValueMethod = null)
    {
      if (Mode == FieldEditorMode.Auto)
      {
        if (DataSource != null && ValueMember != null && DisplayMember != null && GetListBoxItemsMethod != null) Mode = FieldEditorMode.ListBox;
        else if (DataSource != null && ValueMember != null && DisplayMember != null) Mode = FieldEditorMode.ComboBox;
        else if (DataSource != null) Mode = FieldEditorMode.ComboTextBox;
        else if (FormatValueMethod != null) { Mode = FieldEditorMode.TextBox; this.IsReadOnly = true; }
        else if (Column.DataType == typeof(bool)) Mode = FieldEditorMode.CheckBox;
        else if (Column.DataType == typeof(byte)) { Mode = FieldEditorMode.NumberTextBox; this.Minimum = byte.MinValue; this.Maximum = byte.MaxValue; }
        else if (Column.DataType == typeof(short)) { Mode = FieldEditorMode.NumberTextBox; this.Minimum = short.MinValue; this.Maximum = short.MaxValue; }
        else if (Column.DataType == typeof(int)) { Mode = FieldEditorMode.NumberTextBox; this.Minimum = int.MinValue; this.Maximum = int.MaxValue; }
        else if (Column.DataType == typeof(long)) { Mode = FieldEditorMode.NumberTextBox; this.Minimum = long.MinValue; this.Maximum = long.MaxValue; }
        else if (Column.DataType == typeof(ushort)) { Mode = FieldEditorMode.NumberTextBox; this.Minimum = ushort.MinValue; this.Maximum = ushort.MaxValue; }
        else if (Column.DataType == typeof(uint)) { Mode = FieldEditorMode.NumberTextBox; this.Minimum = uint.MinValue; this.Maximum = uint.MaxValue; }
        else if (Column.DataType == typeof(ulong)) { Mode = FieldEditorMode.NumberTextBox; this.Minimum = ulong.MinValue; this.Maximum = ulong.MaxValue; }
        else if (Column.DataType == typeof(DateTime)) Mode = FieldEditorMode.DateTimeTextBox;
        else if (Column.DataType == typeof(Guid)) { Mode = FieldEditorMode.TextBox; this.IsReadOnly = true; }
        else if (Column.MaxLength > 260) Mode = FieldEditorMode.MultilineTextBox;
        else Mode = FieldEditorMode.TextBox;
      }
      if (Mode == FieldEditorMode.BitMask && (Column.DataType == typeof(long) || Column.DataType == typeof(ulong)))
      {
        Mode = FieldEditorMode.BitMask64;
      }
      this.CaptionText = CaptionText; // Column.Caption;
      this.ColumnName = Column.ColumnName;
      this.Mode = Mode;
      this.Style = Style;
      this.IsNull = Column.AllowDBNull || Mode == FieldEditorMode.GuidEditor;
      this.NullValue = NullValue;
      this.IsReadOnly = this.IsReadOnly || Column.ReadOnly || IsReadOnly || Mode == FieldEditorMode.GuidEditor;
      if (this.IsReadOnly && this.Mode == FieldEditorMode.NumberTextBox) this.Mode = FieldEditorMode.TextBox;
      if (Column.MaxLength > 0) this.MaxLength = Column.MaxLength;
      if (Minimum.HasValue) this.Minimum = Minimum;
      if (Maximum.HasValue) this.Maximum = Maximum;
      this.SizeWidth = SizeWidth;
      this.DataSource = DataSource;
      this.ValueMember = ValueMember;
      this.DisplayMember = DisplayMember;
      this.GetListBoxItemsMethod = GetListBoxItemsMethod;
      this.FormatValueMethod = FormatValueMethod;
    }
    /// <summary>
    /// Resulted Control after generation 
    /// </summary>
    public System.Windows.Forms.Control GeneratedControl = null;
  }

}
