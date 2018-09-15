namespace DirLinker.Views
{
    partial class DirLinkerView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirLinkerView));
            this.LinkPointEdit = new System.Windows.Forms.TextBox();
            this.BrowsePoint = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.LinkFrom = new System.Windows.Forms.TextBox();
            this.BrowseTarget = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkTargetFileOverwrite = new System.Windows.Forms.CheckBox();
            this.CopyToTarget = new System.Windows.Forms.RadioButton();
            this.DeleteIt = new System.Windows.Forms.RadioButton();
            this.Go = new System.Windows.Forms.Button();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.BrowseFilePoint = new System.Windows.Forms.Button();
            this.BrowseFileTarget = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // LinkPointEdit
            // 
            this.LinkPointEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LinkPointEdit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.LinkPointEdit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.LinkPointEdit.Cursor = System.Windows.Forms.Cursors.Default;
            this.LinkPointEdit.Location = new System.Drawing.Point(92, 15);
            this.LinkPointEdit.Name = "LinkPointEdit";
            this.LinkPointEdit.Size = new System.Drawing.Size(337, 20);
            this.LinkPointEdit.TabIndex = 0;
            // 
            // BrowsePoint
            // 
            this.BrowsePoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowsePoint.BackgroundImage = global::DirLinker.Properties.Resources._1272101353_folder_horizontal_open;
            this.BrowsePoint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BrowsePoint.Location = new System.Drawing.Point(447, 12);
            this.BrowsePoint.Name = "BrowsePoint";
            this.BrowsePoint.Size = new System.Drawing.Size(30, 23);
            this.BrowsePoint.TabIndex = 2;
            this.BrowsePoint.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Link Location:";
            // 
            // LinkFrom
            // 
            this.LinkFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LinkFrom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.LinkFrom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.LinkFrom.Location = new System.Drawing.Point(92, 42);
            this.LinkFrom.Name = "LinkFrom";
            this.LinkFrom.Size = new System.Drawing.Size(337, 20);
            this.LinkFrom.TabIndex = 1;
            // 
            // BrowseTarget
            // 
            this.BrowseTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseTarget.BackgroundImage = global::DirLinker.Properties.Resources._1272101353_folder_horizontal_open;
            this.BrowseTarget.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BrowseTarget.Location = new System.Drawing.Point(447, 39);
            this.BrowseTarget.Name = "BrowseTarget";
            this.BrowseTarget.Size = new System.Drawing.Size(30, 23);
            this.BrowseTarget.TabIndex = 4;
            this.BrowseTarget.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Link To:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkTargetFileOverwrite);
            this.groupBox1.Controls.Add(this.CopyToTarget);
            this.groupBox1.Controls.Add(this.DeleteIt);
            this.groupBox1.Location = new System.Drawing.Point(15, 68);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(505, 147);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "If the link location exists";
            // 
            // chkTargetFileOverwrite
            // 
            this.chkTargetFileOverwrite.AutoSize = true;
            this.chkTargetFileOverwrite.Location = new System.Drawing.Point(28, 65);
            this.chkTargetFileOverwrite.Name = "chkTargetFileOverwrite";
            this.chkTargetFileOverwrite.Size = new System.Drawing.Size(129, 17);
            this.chkTargetFileOverwrite.TabIndex = 2;
            this.chkTargetFileOverwrite.Text = "Overwrite Target Files";
            this.chkTargetFileOverwrite.UseVisualStyleBackColor = true;
            // 
            // CopyToTarget
            // 
            this.CopyToTarget.AutoSize = true;
            this.CopyToTarget.Checked = true;
            this.CopyToTarget.Location = new System.Drawing.Point(7, 42);
            this.CopyToTarget.Name = "CopyToTarget";
            this.CopyToTarget.Size = new System.Drawing.Size(199, 17);
            this.CopyToTarget.TabIndex = 1;
            this.CopyToTarget.TabStop = true;
            this.CopyToTarget.Text = "Copy contents to target then delete it";
            this.CopyToTarget.UseVisualStyleBackColor = true;
            // 
            // DeleteIt
            // 
            this.DeleteIt.AutoSize = true;
            this.DeleteIt.Location = new System.Drawing.Point(7, 19);
            this.DeleteIt.Name = "DeleteIt";
            this.DeleteIt.Size = new System.Drawing.Size(64, 17);
            this.DeleteIt.TabIndex = 0;
            this.DeleteIt.Text = "Delete it";
            this.DeleteIt.UseVisualStyleBackColor = true;
            // 
            // Go
            // 
            this.Go.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Go.Location = new System.Drawing.Point(361, 221);
            this.Go.Name = "Go";
            this.Go.Size = new System.Drawing.Size(75, 23);
            this.Go.TabIndex = 7;
            this.Go.Text = "Go!";
            this.Go.UseVisualStyleBackColor = true;
            // 
            // CloseBtn
            // 
            this.CloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseBtn.Location = new System.Drawing.Point(442, 221);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(75, 23);
            this.CloseBtn.TabIndex = 8;
            this.CloseBtn.Text = "Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            // 
            // ErrorProvider
            // 
            this.ErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.ErrorProvider.ContainerControl = this;
            // 
            // BrowseFilePoint
            // 
            this.BrowseFilePoint.BackgroundImage = global::DirLinker.Properties.Resources._1272101321_old_edit_find;
            this.BrowseFilePoint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BrowseFilePoint.Location = new System.Drawing.Point(483, 13);
            this.BrowseFilePoint.Name = "BrowseFilePoint";
            this.BrowseFilePoint.Size = new System.Drawing.Size(33, 23);
            this.BrowseFilePoint.TabIndex = 3;
            this.BrowseFilePoint.UseVisualStyleBackColor = true;
            // 
            // BrowseFileTarget
            // 
            this.BrowseFileTarget.BackgroundImage = global::DirLinker.Properties.Resources._1272101321_old_edit_find;
            this.BrowseFileTarget.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BrowseFileTarget.Location = new System.Drawing.Point(483, 39);
            this.BrowseFileTarget.Name = "BrowseFileTarget";
            this.BrowseFileTarget.Size = new System.Drawing.Size(33, 23);
            this.BrowseFileTarget.TabIndex = 5;
            this.BrowseFileTarget.UseVisualStyleBackColor = true;
            // 
            // DirLinkerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 252);
            this.Controls.Add(this.BrowseFileTarget);
            this.Controls.Add(this.BrowseFilePoint);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.Go);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BrowseTarget);
            this.Controls.Add(this.LinkFrom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BrowsePoint);
            this.Controls.Add(this.LinkPointEdit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(546, 263);
            this.Name = "DirLinkerView";
            this.Text = "Directory Linker";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox LinkPointEdit;
        private System.Windows.Forms.Button BrowsePoint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox LinkFrom;
        private System.Windows.Forms.Button BrowseTarget;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton CopyToTarget;
        private System.Windows.Forms.RadioButton DeleteIt;
        private System.Windows.Forms.Button Go;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.CheckBox chkTargetFileOverwrite;
        private System.Windows.Forms.ErrorProvider ErrorProvider;
        private System.Windows.Forms.Button BrowseFileTarget;
        private System.Windows.Forms.Button BrowseFilePoint;
    }
}