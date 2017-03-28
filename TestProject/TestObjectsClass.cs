using System;

namespace TestProject
{
  /// <summary>
  /// Form and Panel for testing
  /// </summary>
  internal class TestObjects : IDisposable
  {
    public System.Windows.Forms.Form Form { get; private set; }
    public System.Windows.Forms.DataGridView DataGrid { get; private set; }

    public TestObjects(bool PrepareEditControls = false)
    {
      this.Form = new System.Windows.Forms.Form();
      this.Form.SuspendLayout();
      this.Form.SetDesktopBounds(0, 0, 640, 480);

      this.DataGrid = new System.Windows.Forms.DataGridView();
      this.Form.Controls.Add(this.DataGrid);
      this.DataGrid.SetBounds(0, 0, 320, 240);

      if (PrepareEditControls) this.PrepareEditControls();

      this.Form.ResumeLayout(false);
      this.Form.PerformLayout();
    }

    public void SuspendLayout()
    {
      this.Form.SuspendLayout();
    }

    public void PerformLayout()
    {
      this.Form.ResumeLayout(false);
      this.Form.PerformLayout();
    }

    private void PrepareEditControls()
    {
      if (this.Form == null || this.DataGrid == null || this.DataGrid.Parent != this.Form) return;
      
      this.DataGrid.AutoSize = true;
      this.DataGrid.ColumnCount = 2;
      this.DataGrid.RowCount = 3;
      
    }

    private bool disposedValue = false; 
    public void Dispose()
    {
      if (!disposedValue)
      {
        this.Form.Dispose();
        this.Form = null;
        this.DataGrid = null;
        disposedValue = true;
      }
    }

  }
}
