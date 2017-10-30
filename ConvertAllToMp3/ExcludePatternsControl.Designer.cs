namespace ConvertAllToMp3
{
    partial class ExcludePatternsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._pattern = new System.Windows.Forms.TextBox();
            this._addButton = new System.Windows.Forms.Button();
            this._patterns = new System.Windows.Forms.ListBox();
            this._removeButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this._pattern, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._addButton, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this._patterns, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._removeButton, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(447, 226);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _pattern
            // 
            this._pattern.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pattern.Location = new System.Drawing.Point(3, 3);
            this._pattern.Name = "_pattern";
            this._pattern.Size = new System.Drawing.Size(360, 20);
            this._pattern.TabIndex = 0;
            this._pattern.TextChanged += new System.EventHandler(this._pattern_TextChanged);
            // 
            // _addButton
            // 
            this._addButton.Location = new System.Drawing.Point(369, 3);
            this._addButton.Name = "_addButton";
            this._addButton.Size = new System.Drawing.Size(75, 23);
            this._addButton.TabIndex = 1;
            this._addButton.Text = "&Add";
            this._addButton.UseVisualStyleBackColor = true;
            this._addButton.SizeChanged += new System.EventHandler(this._addButton_SizeChanged);
            this._addButton.Click += new System.EventHandler(this._addButton_Click);
            // 
            // _patterns
            // 
            this._patterns.Dock = System.Windows.Forms.DockStyle.Fill;
            this._patterns.FormattingEnabled = true;
            this._patterns.IntegralHeight = false;
            this._patterns.Location = new System.Drawing.Point(3, 32);
            this._patterns.Name = "_patterns";
            this._patterns.Size = new System.Drawing.Size(360, 191);
            this._patterns.TabIndex = 2;
            this._patterns.SelectedIndexChanged += new System.EventHandler(this._patterns_SelectedIndexChanged);
            // 
            // _removeButton
            // 
            this._removeButton.Location = new System.Drawing.Point(369, 32);
            this._removeButton.Name = "_removeButton";
            this._removeButton.Size = new System.Drawing.Size(75, 23);
            this._removeButton.TabIndex = 3;
            this._removeButton.Text = "&Remove";
            this._removeButton.UseVisualStyleBackColor = true;
            this._removeButton.Click += new System.EventHandler(this._removeButton_Click);
            // 
            // ExcludePatternsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ExcludePatternsControl";
            this.Size = new System.Drawing.Size(447, 226);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox _pattern;
        private System.Windows.Forms.Button _addButton;
        private System.Windows.Forms.ListBox _patterns;
        private System.Windows.Forms.Button _removeButton;
    }
}
