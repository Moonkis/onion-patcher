namespace OnionPatcher
{
    partial class Patcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Patcher));
            this.LocationGroupBox = new System.Windows.Forms.GroupBox();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.PathBrowseButton = new System.Windows.Forms.Button();
            this.PathBox = new System.Windows.Forms.TextBox();
            this.PatchButton = new System.Windows.Forms.Button();
            this.RestoreButton = new System.Windows.Forms.Button();
            this.LocationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // LocationGroupBox
            // 
            this.LocationGroupBox.Controls.Add(this.StatusLabel);
            this.LocationGroupBox.Controls.Add(this.PathBrowseButton);
            this.LocationGroupBox.Controls.Add(this.PathBox);
            this.LocationGroupBox.Location = new System.Drawing.Point(12, 12);
            this.LocationGroupBox.Name = "LocationGroupBox";
            this.LocationGroupBox.Size = new System.Drawing.Size(387, 76);
            this.LocationGroupBox.TabIndex = 0;
            this.LocationGroupBox.TabStop = false;
            this.LocationGroupBox.Text = "ONI Root Directory";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.StatusLabel.Location = new System.Drawing.Point(87, 51);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(84, 14);
            this.StatusLabel.TabIndex = 3;
            this.StatusLabel.Text = "Status: N/A";
            // 
            // PathBrowseButton
            // 
            this.PathBrowseButton.Location = new System.Drawing.Point(6, 47);
            this.PathBrowseButton.Name = "PathBrowseButton";
            this.PathBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.PathBrowseButton.TabIndex = 1;
            this.PathBrowseButton.Text = "Browse";
            this.PathBrowseButton.UseVisualStyleBackColor = true;
            this.PathBrowseButton.Click += new System.EventHandler(this.PathBrowseButton_Click);
            // 
            // PathBox
            // 
            this.PathBox.Enabled = false;
            this.PathBox.Location = new System.Drawing.Point(6, 19);
            this.PathBox.Name = "PathBox";
            this.PathBox.Size = new System.Drawing.Size(375, 22);
            this.PathBox.TabIndex = 0;
            // 
            // PatchButton
            // 
            this.PatchButton.Enabled = false;
            this.PatchButton.Location = new System.Drawing.Point(18, 145);
            this.PatchButton.Name = "PatchButton";
            this.PatchButton.Size = new System.Drawing.Size(75, 23);
            this.PatchButton.TabIndex = 1;
            this.PatchButton.Text = "Patch";
            this.PatchButton.UseVisualStyleBackColor = true;
            this.PatchButton.Click += new System.EventHandler(this.PatchButton_Click);
            // 
            // RestoreButton
            // 
            this.RestoreButton.Enabled = false;
            this.RestoreButton.Location = new System.Drawing.Point(275, 145);
            this.RestoreButton.Name = "RestoreButton";
            this.RestoreButton.Size = new System.Drawing.Size(124, 23);
            this.RestoreButton.TabIndex = 2;
            this.RestoreButton.Text = "Restore Backup";
            this.RestoreButton.UseVisualStyleBackColor = true;
            this.RestoreButton.Click += new System.EventHandler(this.RestoreButton_Click);
            // 
            // Patcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 180);
            this.Controls.Add(this.RestoreButton);
            this.Controls.Add(this.PatchButton);
            this.Controls.Add(this.LocationGroupBox);
            this.Font = new System.Drawing.Font("Monaco", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Patcher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OnionPatcher";
            this.LocationGroupBox.ResumeLayout(false);
            this.LocationGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox LocationGroupBox;
        private System.Windows.Forms.Button PathBrowseButton;
        private System.Windows.Forms.TextBox PathBox;
        private System.Windows.Forms.Button PatchButton;
        private System.Windows.Forms.Button RestoreButton;
        private System.Windows.Forms.Label StatusLabel;
    }
}

