namespace Examples
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
      System.Windows.Forms.FlowLayoutPanel buttonsLayoutPanel;
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.button3 = new System.Windows.Forms.Button();
      mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
      buttonsLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
      mainLayoutPanel.SuspendLayout();
      buttonsLayoutPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // mainLayoutPanel
      // 
      mainLayoutPanel.ColumnCount = 2;
      mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      mainLayoutPanel.Controls.Add(buttonsLayoutPanel, 0, 0);
      mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      mainLayoutPanel.Location = new System.Drawing.Point(0, 0);
      mainLayoutPanel.Name = "mainLayoutPanel";
      mainLayoutPanel.RowCount = 1;
      mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      mainLayoutPanel.Size = new System.Drawing.Size(624, 442);
      mainLayoutPanel.TabIndex = 0;
      // 
      // buttonsLayoutPanel
      // 
      buttonsLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.Top;
      buttonsLayoutPanel.AutoSize = true;
      buttonsLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      buttonsLayoutPanel.Controls.Add(this.button1);
      buttonsLayoutPanel.Controls.Add(this.button2);
      buttonsLayoutPanel.Controls.Add(this.button3);
      buttonsLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
      buttonsLayoutPanel.Location = new System.Drawing.Point(3, 3);
      buttonsLayoutPanel.Name = "buttonsLayoutPanel";
      buttonsLayoutPanel.Padding = new System.Windows.Forms.Padding(0, 12, 0, 12);
      buttonsLayoutPanel.Size = new System.Drawing.Size(156, 138);
      buttonsLayoutPanel.TabIndex = 0;
      // 
      // button1
      // 
      this.button1.AutoSize = true;
      this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.button1.Location = new System.Drawing.Point(3, 15);
      this.button1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 12);
      this.button1.MinimumSize = new System.Drawing.Size(150, 25);
      this.button1.Name = "button1";
      this.button1.Padding = new System.Windows.Forms.Padding(3);
      this.button1.Size = new System.Drawing.Size(150, 29);
      this.button1.TabIndex = 0;
      this.button1.Text = "Overview";
      this.button1.UseVisualStyleBackColor = true;
      // 
      // button2
      // 
      this.button2.AutoSize = true;
      this.button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.button2.Location = new System.Drawing.Point(3, 59);
      this.button2.MinimumSize = new System.Drawing.Size(150, 25);
      this.button2.Name = "button2";
      this.button2.Padding = new System.Windows.Forms.Padding(3);
      this.button2.Size = new System.Drawing.Size(150, 29);
      this.button2.TabIndex = 1;
      this.button2.Text = "Departments";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // button3
      // 
      this.button3.AutoSize = true;
      this.button3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.button3.Location = new System.Drawing.Point(3, 94);
      this.button3.MinimumSize = new System.Drawing.Size(150, 25);
      this.button3.Name = "button3";
      this.button3.Padding = new System.Windows.Forms.Padding(3);
      this.button3.Size = new System.Drawing.Size(150, 29);
      this.button3.TabIndex = 2;
      this.button3.Text = "Employees";
      this.button3.UseVisualStyleBackColor = true;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(624, 442);
      this.Controls.Add(mainLayoutPanel);
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "DataGridViewExtenders examples";
      mainLayoutPanel.ResumeLayout(false);
      mainLayoutPanel.PerformLayout();
      buttonsLayoutPanel.ResumeLayout(false);
      buttonsLayoutPanel.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button3;
  }
}

