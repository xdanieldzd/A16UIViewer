namespace A16UIViewer
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dgvImages = new System.Windows.Forms.DataGridView();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpLayout = new System.Windows.Forms.TabPage();
            this.lrcLayout = new A16UIViewer.Controls.LayoutRenderControl();
            this.tvLayout = new System.Windows.Forms.TreeView();
            this.tpTextures = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.dgvImages)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tpLayout.SuspendLayout();
            this.tpTextures.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvImages
            // 
            this.dgvImages.AllowUserToAddRows = false;
            this.dgvImages.AllowUserToDeleteRows = false;
            this.dgvImages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvImages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvImages.Location = new System.Drawing.Point(6, 3);
            this.dgvImages.Name = "dgvImages";
            this.dgvImages.ReadOnly = true;
            this.dgvImages.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvImages.Size = new System.Drawing.Size(1390, 753);
            this.dgvImages.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tpLayout);
            this.tabControl.Controls.Add(this.tpTextures);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1510, 788);
            this.tabControl.TabIndex = 1;
            // 
            // tpLayout
            // 
            this.tpLayout.Controls.Add(this.lrcLayout);
            this.tpLayout.Controls.Add(this.tvLayout);
            this.tpLayout.Location = new System.Drawing.Point(4, 22);
            this.tpLayout.Name = "tpLayout";
            this.tpLayout.Padding = new System.Windows.Forms.Padding(3);
            this.tpLayout.Size = new System.Drawing.Size(1502, 762);
            this.tpLayout.TabIndex = 1;
            this.tpLayout.Text = "Layout";
            this.tpLayout.UseVisualStyleBackColor = true;
            // 
            // lrcLayout
            // 
            this.lrcLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lrcLayout.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("lrcLayout.BackgroundImage")));
            this.lrcLayout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lrcLayout.Location = new System.Drawing.Point(212, 6);
            this.lrcLayout.Name = "lrcLayout";
            this.lrcLayout.Size = new System.Drawing.Size(1284, 750);
            this.lrcLayout.TabIndex = 2;
            // 
            // tvLayout
            // 
            this.tvLayout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tvLayout.Location = new System.Drawing.Point(6, 6);
            this.tvLayout.Name = "tvLayout";
            this.tvLayout.Size = new System.Drawing.Size(200, 750);
            this.tvLayout.TabIndex = 1;
            // 
            // tpTextures
            // 
            this.tpTextures.Controls.Add(this.dgvImages);
            this.tpTextures.Location = new System.Drawing.Point(4, 22);
            this.tpTextures.Name = "tpTextures";
            this.tpTextures.Padding = new System.Windows.Forms.Padding(3);
            this.tpTextures.Size = new System.Drawing.Size(1502, 762);
            this.tpTextures.TabIndex = 0;
            this.tpTextures.Text = "Textures";
            this.tpTextures.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1534, 812);
            this.Controls.Add(this.tabControl);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.dgvImages)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tpLayout.ResumeLayout(false);
            this.tpTextures.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvImages;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpTextures;
        private System.Windows.Forms.TabPage tpLayout;
        private System.Windows.Forms.TreeView tvLayout;
        private Controls.LayoutRenderControl lrcLayout;
    }
}

