namespace KMAPRequest
{
    partial class Form1
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
            this.btnAdvSearch = new System.Windows.Forms.Button();
            this.txtAdvFolderID = new System.Windows.Forms.TextBox();
            this.JsonResultTxt = new System.Windows.Forms.TextBox();
            this.txtAdvKeyword = new System.Windows.Forms.TextBox();
            this.lblAdvKeyword = new System.Windows.Forms.Label();
            this.lblFolderId = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAdvSearch
            // 
            this.btnAdvSearch.Location = new System.Drawing.Point(449, 89);
            this.btnAdvSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAdvSearch.Name = "btnAdvSearch";
            this.btnAdvSearch.Size = new System.Drawing.Size(67, 19);
            this.btnAdvSearch.TabIndex = 0;
            this.btnAdvSearch.Text = "AdvSearch";
            this.btnAdvSearch.UseVisualStyleBackColor = true;
            this.btnAdvSearch.Click += new System.EventHandler(this.btnAdvSearch_Click);
            // 
            // txtAdvFolderID
            // 
            this.txtAdvFolderID.Location = new System.Drawing.Point(131, 64);
            this.txtAdvFolderID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAdvFolderID.Name = "txtAdvFolderID";
            this.txtAdvFolderID.Size = new System.Drawing.Size(153, 25);
            this.txtAdvFolderID.TabIndex = 1;
            // 
            // JsonResultTxt
            // 
            this.JsonResultTxt.Location = new System.Drawing.Point(11, 157);
            this.JsonResultTxt.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.JsonResultTxt.Multiline = true;
            this.JsonResultTxt.Name = "JsonResultTxt";
            this.JsonResultTxt.Size = new System.Drawing.Size(507, 238);
            this.JsonResultTxt.TabIndex = 2;
            // 
            // txtAdvKeyword
            // 
            this.txtAdvKeyword.Location = new System.Drawing.Point(131, 20);
            this.txtAdvKeyword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAdvKeyword.Name = "txtAdvKeyword";
            this.txtAdvKeyword.Size = new System.Drawing.Size(387, 25);
            this.txtAdvKeyword.TabIndex = 3;
            // 
            // lblAdvKeyword
            // 
            this.lblAdvKeyword.AutoSize = true;
            this.lblAdvKeyword.Location = new System.Drawing.Point(28, 29);
            this.lblAdvKeyword.Name = "lblAdvKeyword";
            this.lblAdvKeyword.Size = new System.Drawing.Size(83, 15);
            this.lblAdvKeyword.TabIndex = 4;
            this.lblAdvKeyword.Text = "AdvKeyword";
            // 
            // lblFolderId
            // 
            this.lblFolderId.AutoSize = true;
            this.lblFolderId.Location = new System.Drawing.Point(28, 74);
            this.lblFolderId.Name = "lblFolderId";
            this.lblFolderId.Size = new System.Drawing.Size(60, 15);
            this.lblFolderId.TabIndex = 5;
            this.lblFolderId.Text = "Folder Id";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 404);
            this.Controls.Add(this.lblFolderId);
            this.Controls.Add(this.lblAdvKeyword);
            this.Controls.Add(this.txtAdvKeyword);
            this.Controls.Add(this.JsonResultTxt);
            this.Controls.Add(this.txtAdvFolderID);
            this.Controls.Add(this.btnAdvSearch);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAdvSearch;
        private System.Windows.Forms.TextBox txtAdvFolderID;
        private System.Windows.Forms.TextBox JsonResultTxt;
        private System.Windows.Forms.TextBox txtAdvKeyword;
        private System.Windows.Forms.Label lblAdvKeyword;
        private System.Windows.Forms.Label lblFolderId;
    }
}

