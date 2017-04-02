using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

//
// HeaderTableLayoutPanel
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
  [ToolboxBitmap(typeof(TableLayoutPanel))]
  public class HeaderTableLayoutPanel : System.Windows.Forms.TableLayoutPanel
  {
    /// <summary>
    /// Header text
    /// </summary>
    [Browsable(true), DefaultValue(null), Category("Header"), Description("Header text")]
    public string CaptionText
    {
      get { return this.captionText; }
      set
      {
        if (this.captionText != value)
        {
          this.captionText = value;
          this.CalculateCaptionParams();
          Invalidate();
        }
      }
    }
    private string captionText = null;

    /// <summary>
    /// Drawing styles for Header
    /// </summary>
    public enum HighlightCaptionStyle
    {
      ForeColor, HighlightColor, ForeStyle, HighlightStyle, NavisionAxaptaStyle, GroupBoxStyle
    }

    /// <summary>
    /// Drawing header style
    /// </summary>
    [Browsable(true), DefaultValue(HighlightCaptionStyle.ForeColor), Category("Header"), Description("Drawing header style")]
    public HighlightCaptionStyle CaptionStyle
    {
      get { return this.captionStyle; }
      set
      {
        if (this.captionStyle != value)
        {
          this.captionStyle = value;
          this.CalculateCaptionParams();
          Invalidate();
        }
      }
    }
    private HighlightCaptionStyle captionStyle = HighlightCaptionStyle.ForeColor;

    /// <summary>
    /// Width of the header line
    /// </summary>
    [Browsable(true), DefaultValue((byte)2), Category("Header"), Description("Width of the header line")]
    public byte CaptionLineWidth
    {
      get { return this.captionLineWidth; }
      set
      {
        if (this.captionLineWidth != value)
        {
          this.captionLineWidth = value;
          this.CalculateCaptionParams();
          Invalidate();
        }
      }
    }
    private byte captionLineWidth = 2;

    protected override void OnForeColorChanged(EventArgs e)
    {
      base.OnForeColorChanged(e);
      this.CalculateCaptionParams();
      Invalidate();
    }

    protected override void OnBackColorChanged(EventArgs e)
    {
      base.OnBackColorChanged(e);
      this.CalculateCaptionParams();
      Invalidate();
    }

    protected override void OnFontChanged(EventArgs e)
    {
      base.OnFontChanged(e);
      this.CalculateCaptionParams();
      Invalidate();
    }

    // calculating and storing params for drawing
    private int captionTextWidth;
    private int captionTextHeight;
    private Color captionTextColor;
    private Color captionLineBeginColor;
    private Color captionLineEndColor;
    private void CalculateCaptionParams()
    {
      if (!string.IsNullOrEmpty(this.captionText))
        using (var g = this.CreateGraphics())
        {
          var _size = g.MeasureString(this.captionText + "I", this.Font).ToSize();
          this.captionTextWidth = _size.Width;
          this.captionTextHeight = _size.Height;
        }
      else
      {
        this.captionTextWidth = 0;
        this.captionTextHeight = 0;
      }
      if (this.captionStyle == HighlightCaptionStyle.ForeColor)
      {
        this.captionTextColor = this.ForeColor;
        this.captionLineBeginColor = this.ForeColor;
        this.captionLineEndColor = this.BackColor;
      }
      else if (this.captionStyle == HighlightCaptionStyle.ForeStyle)
      {
        this.captionTextColor = this.BackColor;
        this.captionLineBeginColor = this.ForeColor;
        this.captionLineEndColor = this.BackColor;
      }
      else
      {
        this.captionTextColor = this.captionStyle == HighlightCaptionStyle.HighlightStyle ? SystemColors.HighlightText : SystemColors.Highlight;
        this.captionLineBeginColor = SystemColors.MenuHighlight;
        this.captionLineEndColor = this.BackColor;
      }
    }

    // changing Rectangle according CaptionText and CaptionStyle
    public override Rectangle DisplayRectangle
    {
      get
      {
        var result = base.DisplayRectangle;
        int resize = 0;
        if (this.captionTextHeight > 0)
        {
          resize = this.captionTextHeight;
          if (this.captionStyle == HighlightCaptionStyle.NavisionAxaptaStyle) resize += 1;
          else if (this.captionStyle == HighlightCaptionStyle.ForeStyle || this.captionStyle == HighlightCaptionStyle.HighlightStyle) resize += 1;
          else if (this.captionStyle != HighlightCaptionStyle.GroupBoxStyle) resize += this.captionLineWidth > 0 ? 2 : 1;
        }
        else if (this.captionStyle == HighlightCaptionStyle.GroupBoxStyle) resize += 10;
        if (this.captionStyle == HighlightCaptionStyle.ForeStyle || this.captionStyle == HighlightCaptionStyle.HighlightStyle) resize += this.captionLineWidth * 2;
        else if (this.captionStyle == HighlightCaptionStyle.ForeColor || this.captionStyle == HighlightCaptionStyle.HighlightColor) resize += this.captionLineWidth;
        result.Height -= resize;
        result.Offset(0, resize);
        return result;
      }
    }

    // changing Size according CaptionText and CaptionStyle
    protected override Size SizeFromClientSize(Size clientSize)
    {
      var result = base.SizeFromClientSize(clientSize);
      int resize = 0;
      if (this.captionTextHeight > 0)
      {
        resize = this.captionTextHeight;
        if (this.captionStyle == HighlightCaptionStyle.NavisionAxaptaStyle) resize += 1;
        else if (this.captionStyle == HighlightCaptionStyle.ForeStyle || this.captionStyle == HighlightCaptionStyle.HighlightStyle) resize += 1;
        else if (this.captionStyle != HighlightCaptionStyle.GroupBoxStyle) resize += this.captionLineWidth > 0 ? 2 : 1;
      }
      else if (this.captionStyle == HighlightCaptionStyle.GroupBoxStyle) resize += 10;
      if (this.captionStyle == HighlightCaptionStyle.ForeStyle || this.captionStyle == HighlightCaptionStyle.HighlightStyle) resize += this.captionLineWidth * 2;
      else if (this.captionStyle == HighlightCaptionStyle.ForeColor || this.captionStyle == HighlightCaptionStyle.HighlightColor) resize += this.captionLineWidth;
      result.Height += resize;
      return result;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      // draw gradient
      if (this.captionStyle == HighlightCaptionStyle.ForeStyle || this.captionStyle == HighlightCaptionStyle.HighlightStyle)
      { // HighlightCaptionStyle.HighlightStyle allways draw
        float _wPen = this.captionLineWidth * 2 + this.captionTextHeight;
        if (_wPen > 0)
          using (Brush _gBrush = new LinearGradientBrush(new Point(0, 0), new Point(this.Width, 0), this.captionLineBeginColor, this.captionLineEndColor))
          using (Pen _gPen = new Pen(_gBrush, _wPen))
            e.Graphics.DrawLine(_gPen, 0, _wPen / 2, this.Width, _wPen / 2);
      }
      else if (this.captionStyle == HighlightCaptionStyle.GroupBoxStyle)
      { // HighlightCaptionStyle.GroupBox draw GroupBox canvas
        string _capText = this.captionText;
        if (!string.IsNullOrEmpty(_capText))
        {
          _capText = _capText.Trim();
          if (!string.IsNullOrEmpty(_capText)) _capText = string.Format(" {0} ", _capText);
        }
        GroupBoxRenderer.DrawGroupBox(e.Graphics, this.ClientRectangle, _capText, this.Font, this.captionTextColor, this.Enabled ? System.Windows.Forms.VisualStyles.GroupBoxState.Normal : System.Windows.Forms.VisualStyles.GroupBoxState.Disabled);
      }
      else if (this.captionLineWidth > 0)
        if (this.captionStyle != HighlightCaptionStyle.NavisionAxaptaStyle)
        { // HighlightCaptionMode.ForeColor | HighlightCaptionMode.HighlightColor
          using (Brush _gradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(this.Width, 0), this.captionLineBeginColor, this.captionLineEndColor))
          using (Pen _gradientPen = new Pen(_gradientBrush, this.captionLineWidth))
            e.Graphics.DrawLine(_gradientPen, 0, this.captionTextHeight + this.captionLineWidth / 2, this.Width, this.captionTextHeight + this.captionLineWidth / 2);
        }
        else if (this.captionTextWidth + 1 < this.Width)
        { // HighlightCaptionMode.NavisionAxapta
          using (Brush _gradientBrush = new LinearGradientBrush(new Point(this.captionTextWidth, 0), new Point(this.Width, 0), this.captionLineBeginColor, this.captionLineEndColor))
          using (Pen _gradientPen = new Pen(_gradientBrush, this.captionLineWidth > this.captionTextHeight ? this.captionTextHeight : this.captionLineWidth))
            e.Graphics.DrawLine(_gradientPen, this.captionTextWidth, this.captionTextHeight / 2 + 1, this.Width, this.captionTextHeight / 2 + 1);
        }
      // draw Text
      if (this.captionTextHeight > 0 && this.captionStyle != HighlightCaptionStyle.GroupBoxStyle)
        using (Brush _textBrush = new SolidBrush(this.captionTextColor))
          e.Graphics.DrawString(this.captionText, this.Font, _textBrush, 0, this.captionStyle == HighlightCaptionStyle.HighlightStyle || this.captionStyle == HighlightCaptionStyle.ForeStyle ? this.CaptionLineWidth : 0);
    }

  }
}
