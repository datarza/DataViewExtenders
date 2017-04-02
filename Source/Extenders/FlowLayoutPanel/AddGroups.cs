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

  public static partial class FlowLayoutPanelExtenders
  {
    /// <summary>
    /// Group generator that works on group and field data descriptions (GroupDataDescriptor & FieldDataDescriptor)
    /// </summary>
    /// <param name="dataPanel">FlowLayoutPanel</param>
    /// <param name="DataSource">Data Source to support data-binding</param>
    /// <param name="Fields">Field data descriptors</param>
    public static void GenerateFields(this FlowLayoutPanel dataPanel, object DataSource, params GroupDataDescriptor[] Groups)
    {
      FlowLayoutPanelExtenders.GenerateGroups(dataPanel, null, DataSource, Groups);
    }

    /// <summary>
    /// Group generator that works on group and field data descriptions (GroupDataDescriptor & FieldDataDescriptor)
    /// </summary>
    /// <param name="groupPanel">FlowLayoutPanel</param>
    /// <param name="toolTip">ToolTip</param>
    /// <param name="DataSource">Data Source to support data-binding</param>
    /// <param name="Groups">Group data descriptors and Field data descriptors</param>
    public static void GenerateGroups(this FlowLayoutPanel groupPanel, ToolTip toolTip, object DataSource, params GroupDataDescriptor[] Groups)
    {
      groupPanel.SuspendLayout();
      groupPanel.Controls.Clear();
      if (DataSource == null || Groups == null || Groups.Length == 0) return;
      foreach (var group in Groups)
      {
        var dataPanel = new EditorTableLayoutPanel();
        group.GeneratedPanel = dataPanel;
        dataPanel.SuspendLayout();
        dataPanel.AutoSize = true;
        dataPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        dataPanel.Margin = new Padding(3, 3, 9, 3);
        dataPanel.CaptionText = group.CaptionText;
        dataPanel.CaptionLineWidth = 2;
        dataPanel.CaptionStyle = EditorTableLayoutPanel.HighlightCaptionStyle.NavisionAxaptaStyle;
        dataPanel.ColumnCount = 2;
        dataPanel.ColumnStyles.Add(new ColumnStyle());
        dataPanel.ColumnStyles.Add(new ColumnStyle());
        dataPanel.GenerateFields(toolTip, DataSource, group.Fields);
        groupPanel.Controls.Add(dataPanel);
        dataPanel.ResumeLayout();
      }
      groupPanel.ResumeLayout();
      groupPanel.PerformLayout();
    }
  }
}
