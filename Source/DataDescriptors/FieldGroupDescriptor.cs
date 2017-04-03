using System;
using System.Data;

namespace CBComponents.DataDescriptors
{
  /// <summary>
  /// Width size of column or field
  /// </summary>
  public enum DataDescriptorSizeWidth : int
  {
    Smaller = 45,
    Small = 90,
    Medium = 135,
    Normal = 225,
    Large = 320,
    Larger = 360 
  }

  /// <summary>
  /// DataDescriptor of groups on the Panel
  /// </summary>
  public class GroupDataDescriptor
  {
    public string CaptionText { get; set; }

    public FieldDataDescriptor[] Fields { get; set; }

    private const int _defaultSizeWidth = (int)DataDescriptorSizeWidth.Smaller / 2;

    public GroupDataDescriptor(string CaptionText, int SizeWidth, params FieldDataDescriptor[] Fields)
    {
      this.CaptionText = CaptionText;
      if (SizeWidth > _defaultSizeWidth)
        foreach (var field in Fields)
          if (!field.SizeWidth.HasValue)
            field.SizeWidth = SizeWidth;
      this.Fields = Fields;
    }

    public GroupDataDescriptor(string CaptionText, DataDescriptorSizeWidth SizeWidth, params FieldDataDescriptor[] Fields)
      : this(CaptionText, (int)SizeWidth, Fields) { }

    public GroupDataDescriptor(string CaptionText, params FieldDataDescriptor[] Fields)
      : this(CaptionText, _defaultSizeWidth, Fields) { }

    /// <summary>
    /// Resulted Panel after generation 
    /// </summary>
    public System.Windows.Forms.Panel GeneratedPanel = null;

  }

}
