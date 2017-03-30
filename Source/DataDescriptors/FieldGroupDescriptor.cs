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

    public HeaderTableLayoutPanel.HighlightCaptionStyle CaptionStyle { get; set; } = HeaderTableLayoutPanel.HighlightCaptionStyle.HighlightColor;

    private const HeaderTableLayoutPanel.HighlightCaptionStyle _defaultHighlightCaptionStyle = HeaderTableLayoutPanel.HighlightCaptionStyle.HighlightColor;
    private const int _defaultSizeWidth = (int)DataDescriptorSizeWidth.Smaller / 2;

    public GroupDataDescriptor(string CaptionText, HeaderTableLayoutPanel.HighlightCaptionStyle CaptionStyle, int SizeWidth, params FieldDataDescriptor[] Fields)
    {
      this.CaptionText = CaptionText;
      this.CaptionStyle = CaptionStyle;
      if (SizeWidth > _defaultSizeWidth)
        foreach (var field in Fields)
          if (!field.SizeWidth.HasValue)
            field.SizeWidth = SizeWidth;
      this.Fields = Fields;
    }

    public GroupDataDescriptor(string CaptionText, HeaderTableLayoutPanel.HighlightCaptionStyle CaptionStyle, DataDescriptorSizeWidth SizeWidth, params FieldDataDescriptor[] Fields)
      : this(CaptionText, CaptionStyle, (int)SizeWidth, Fields)
    {
    }

    public GroupDataDescriptor(string CaptionText, params FieldDataDescriptor[] Fields)
      : this(CaptionText, _defaultHighlightCaptionStyle, _defaultSizeWidth, Fields)
    {
    }

    public GroupDataDescriptor(string CaptionText, HeaderTableLayoutPanel.HighlightCaptionStyle CaptionStyle, params FieldDataDescriptor[] Fields)
      : this(CaptionText, CaptionStyle, _defaultSizeWidth, Fields)
    {
    }

    public GroupDataDescriptor(string CaptionText, int SizeWidth, params FieldDataDescriptor[] Fields)
      : this(CaptionText, _defaultHighlightCaptionStyle, SizeWidth, Fields)
    {
    }

    public GroupDataDescriptor(string CaptionText, DataDescriptorSizeWidth SizeWidth, params FieldDataDescriptor[] Fields)
      : this(CaptionText, _defaultHighlightCaptionStyle, (int)SizeWidth, Fields)
    {
    }

    /// <summary>
    /// Resulted Panel after generation 
    /// </summary>
    public System.Windows.Forms.Panel GeneratedPanel = null;

  }

}
