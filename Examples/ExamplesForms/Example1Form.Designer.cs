namespace Examples
{
  partial class Example1Form
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
      System.Windows.Forms.FlowLayoutPanel mainPanel;
      System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
      System.Windows.Forms.Label label1;
      System.Windows.Forms.Label label2;
      System.Windows.Forms.Label label3;
      System.Windows.Forms.Label label4;
      this.bmclBox = new CBComponents.BitMaskCheckedListBox();
      this.textBoxValue = new System.Windows.Forms.TextBox();
      this.textBoxLongValue = new System.Windows.Forms.TextBox();
      this.textBoxValues = new System.Windows.Forms.TextBox();
      mainPanel = new System.Windows.Forms.FlowLayoutPanel();
      tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
      label1 = new System.Windows.Forms.Label();
      label2 = new System.Windows.Forms.Label();
      label3 = new System.Windows.Forms.Label();
      label4 = new System.Windows.Forms.Label();
      mainPanel.SuspendLayout();
      tableLayoutPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // mainPanel
      // 
      mainPanel.AutoScroll = true;
      mainPanel.AutoSize = true;
      mainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      mainPanel.Controls.Add(tableLayoutPanel);
      mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      mainPanel.Location = new System.Drawing.Point(0, 0);
      mainPanel.Name = "mainPanel";
      mainPanel.Padding = new System.Windows.Forms.Padding(6);
      mainPanel.Size = new System.Drawing.Size(464, 362);
      mainPanel.TabIndex = 0;
      // 
      // tableLayoutPanel
      // 
      tableLayoutPanel.AutoSize = true;
      tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      tableLayoutPanel.ColumnCount = 2;
      tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      tableLayoutPanel.Controls.Add(label1, 0, 0);
      tableLayoutPanel.Controls.Add(label2, 0, 1);
      tableLayoutPanel.Controls.Add(label3, 0, 2);
      tableLayoutPanel.Controls.Add(label4, 0, 3);
      tableLayoutPanel.Controls.Add(this.bmclBox, 1, 0);
      tableLayoutPanel.Controls.Add(this.textBoxValue, 1, 1);
      tableLayoutPanel.Controls.Add(this.textBoxLongValue, 1, 2);
      tableLayoutPanel.Controls.Add(this.textBoxValues, 1, 3);
      tableLayoutPanel.Location = new System.Drawing.Point(9, 9);
      tableLayoutPanel.Name = "tableLayoutPanel";
      tableLayoutPanel.RowCount = 4;
      tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      tableLayoutPanel.Size = new System.Drawing.Size(397, 318);
      tableLayoutPanel.TabIndex = 0;
      // 
      // label1
      // 
      label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point(42, 6);
      label1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size(26, 13);
      label1.TabIndex = 1;
      label1.Text = "List:";
      label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label2
      // 
      label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point(31, 166);
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size(37, 13);
      label2.TabIndex = 2;
      label2.Text = "Value:";
      label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label3
      // 
      label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point(7, 192);
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size(61, 13);
      label3.TabIndex = 3;
      label3.Text = "LongValue:";
      label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label4
      // 
      label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      label4.AutoSize = true;
      label4.Location = new System.Drawing.Point(3, 218);
      label4.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
      label4.Name = "label4";
      label4.Size = new System.Drawing.Size(65, 13);
      label4.TabIndex = 5;
      label4.Text = "GetValues():";
      label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // bmclBox
      // 
      this.bmclBox.CheckOnClick = true;
      this.bmclBox.Location = new System.Drawing.Point(74, 3);
      this.bmclBox.LongValue = ((long)(0));
      this.bmclBox.MultiColumn = true;
      this.bmclBox.Name = "bmclBox";
      this.bmclBox.Size = new System.Drawing.Size(320, 154);
      this.bmclBox.TabIndex = 0;
      // 
      // textBoxValue
      // 
      this.textBoxValue.Location = new System.Drawing.Point(74, 163);
      this.textBoxValue.Name = "textBoxValue";
      this.textBoxValue.Size = new System.Drawing.Size(100, 20);
      this.textBoxValue.TabIndex = 1;
      // 
      // textBoxLongValue
      // 
      this.textBoxLongValue.Location = new System.Drawing.Point(74, 189);
      this.textBoxLongValue.Name = "textBoxLongValue";
      this.textBoxLongValue.Size = new System.Drawing.Size(200, 20);
      this.textBoxLongValue.TabIndex = 4;
      // 
      // textBoxValues
      // 
      this.textBoxValues.Location = new System.Drawing.Point(74, 215);
      this.textBoxValues.Multiline = true;
      this.textBoxValues.Name = "textBoxValues";
      this.textBoxValues.ReadOnly = true;
      this.textBoxValues.Size = new System.Drawing.Size(320, 100);
      this.textBoxValues.TabIndex = 6;
      // 
      // Example1Form
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.ClientSize = new System.Drawing.Size(464, 362);
      this.Controls.Add(mainPanel);
      this.MinimumSize = new System.Drawing.Size(200, 120);
      this.Name = "Example1Form";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "ExamplesForm";
      this.Load += new System.EventHandler(this.ExamplesForm_Load);
      mainPanel.ResumeLayout(false);
      mainPanel.PerformLayout();
      tableLayoutPanel.ResumeLayout(false);
      tableLayoutPanel.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private CBComponents.BitMaskCheckedListBox bmclBox;
    private System.Windows.Forms.TextBox textBoxValue;
    private System.Windows.Forms.TextBox textBoxLongValue;
    private System.Windows.Forms.TextBox textBoxValues;
  }
}