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
    public static void AddFields(this FlowLayoutPanel dataPanel, object DataSource, params GroupDataDescriptor[] Groups)
    {
      FlowLayoutPanelExtenders.AddGroups(dataPanel, null, DataSource, Groups);
    }

    /// <summary>
    /// Group generator that works on group and field data descriptions (GroupDataDescriptor & FieldDataDescriptor)
    /// </summary>
    /// <param name="dataPanel">FlowLayoutPanel</param>
    /// <param name="toolTip">ToolTip</param>
    /// <param name="DataSource">Data Source to support data-binding</param>
    /// <param name="Groups">Group data descriptors and Field data descriptors</param>
    public static void AddGroups(this FlowLayoutPanel dataPanel, ToolTip toolTip, object DataSource, params GroupDataDescriptor[] Groups)
    {
      dataPanel.SuspendLayout();
      dataPanel.Controls.Clear();
      if (DataSource == null || Groups == null || Groups.Length == 0) return;
      foreach (var group in Groups)
      {
        var htlPanel = new EditorTableLayoutPanel();
        group.GeneratedPanel = htlPanel;
        htlPanel.AutoSize = true;
        htlPanel.Margin = new Padding(3, 3, 12, 3);
        htlPanel.CaptionText = group.CaptionText;
        htlPanel.ColumnCount = 2;
        htlPanel.ColumnStyles.Add(new ColumnStyle());
        htlPanel.ColumnStyles.Add(new ColumnStyle());
        htlPanel.AddFields(toolTip, DataSource, group.Fields);
        dataPanel.Controls.Add(htlPanel);
      }
      dataPanel.ResumeLayout();
      dataPanel.PerformLayout();
    }
  }
}
