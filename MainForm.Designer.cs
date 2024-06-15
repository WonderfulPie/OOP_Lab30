namespace Lab_30_Danylko
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox serverTextBox;
        private System.Windows.Forms.TextBox userTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button uploadFileButton;
        private System.Windows.Forms.Button uploadFilesButton; // Нова кнопка для завантаження групи файлів
        private System.Windows.Forms.Button settingsButton; // Кнопка налаштувань
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button makeDirButton;
        private System.Windows.Forms.Button removeDirButton;
        private System.Windows.Forms.Button renameButton;
        private System.Windows.Forms.Button downloadButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.serverTextBox = new System.Windows.Forms.TextBox();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.uploadFileButton = new System.Windows.Forms.Button();
            this.uploadFilesButton = new System.Windows.Forms.Button();
            this.settingsButton = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.makeDirButton = new System.Windows.Forms.Button();
            this.removeDirButton = new System.Windows.Forms.Button();
            this.renameButton = new System.Windows.Forms.Button();
            this.downloadButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // serverTextBox
            // 
            this.serverTextBox.Location = new System.Drawing.Point(111, 12);
            this.serverTextBox.Name = "serverTextBox";
            this.serverTextBox.Size = new System.Drawing.Size(300, 22);
            this.serverTextBox.TabIndex = 0;
            // 
            // userTextBox
            // 
            this.userTextBox.Location = new System.Drawing.Point(111, 38);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(300, 22);
            this.userTextBox.TabIndex = 1;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(111, 64);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(300, 22);
            this.passwordTextBox.TabIndex = 2;
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(12, 90);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 3;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(174, 119);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 4;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // uploadFileButton
            // 
            this.uploadFileButton.Location = new System.Drawing.Point(93, 90);
            this.uploadFileButton.Name = "uploadFileButton";
            this.uploadFileButton.Size = new System.Drawing.Size(75, 23);
            this.uploadFileButton.TabIndex = 5;
            this.uploadFileButton.Text = "Upload File";
            this.uploadFileButton.UseVisualStyleBackColor = true;
            this.uploadFileButton.Click += new System.EventHandler(this.UploadFileButton_Click);
            // 
            // uploadFilesButton
            // 
            this.uploadFilesButton.Location = new System.Drawing.Point(93, 119);
            this.uploadFilesButton.Name = "uploadFilesButton";
            this.uploadFilesButton.Size = new System.Drawing.Size(75, 23);
            this.uploadFilesButton.TabIndex = 11;
            this.uploadFilesButton.Text = "Upload Files";
            this.uploadFilesButton.UseVisualStyleBackColor = true;
            this.uploadFilesButton.Click += new System.EventHandler(this.UploadFilesButton_Click);
            // 
            // settingsButton
            // 
            this.settingsButton.Location = new System.Drawing.Point(337, 90);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(75, 23);
            this.settingsButton.TabIndex = 13;
            this.settingsButton.Text = "Settings";
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 148);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(399, 395);
            this.treeView1.TabIndex = 6;
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
            // 
            // makeDirButton
            // 
            this.makeDirButton.Location = new System.Drawing.Point(255, 90);
            this.makeDirButton.Name = "makeDirButton";
            this.makeDirButton.Size = new System.Drawing.Size(75, 23);
            this.makeDirButton.TabIndex = 7;
            this.makeDirButton.Text = "Make Dir";
            this.makeDirButton.UseVisualStyleBackColor = true;
            this.makeDirButton.Click += new System.EventHandler(this.MakeDirButton_Click);
            // 
            // removeDirButton
            // 
            this.removeDirButton.Location = new System.Drawing.Point(255, 119);
            this.removeDirButton.Name = "removeDirButton";
            this.removeDirButton.Size = new System.Drawing.Size(75, 23);
            this.removeDirButton.TabIndex = 8;
            this.removeDirButton.Text = "Remove Dir";
            this.removeDirButton.UseVisualStyleBackColor = true;
            this.removeDirButton.Click += new System.EventHandler(this.RemoveDirButton_Click);
            // 
            // renameButton
            // 
            this.renameButton.Location = new System.Drawing.Point(12, 119);
            this.renameButton.Name = "renameButton";
            this.renameButton.Size = new System.Drawing.Size(75, 23);
            this.renameButton.TabIndex = 9;
            this.renameButton.Text = "Rename";
            this.renameButton.UseVisualStyleBackColor = true;
            this.renameButton.Click += new System.EventHandler(this.RenameButton_Click);
            // 
            // downloadButton
            // 
            this.downloadButton.Location = new System.Drawing.Point(174, 90);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(75, 23);
            this.downloadButton.TabIndex = 10;
            this.downloadButton.Text = "Download";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 16);
            this.label1.TabIndex = 14;
            this.label1.Text = "Хост";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 16);
            this.label2.TabIndex = 15;
            this.label2.Text = "Юзернейм";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 16;
            this.label3.Text = "Пароль";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(424, 555);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.downloadButton);
            this.Controls.Add(this.renameButton);
            this.Controls.Add(this.removeDirButton);
            this.Controls.Add(this.makeDirButton);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.uploadFileButton);
            this.Controls.Add(this.uploadFilesButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.userTextBox);
            this.Controls.Add(this.serverTextBox);
            this.Controls.Add(this.settingsButton);
            this.Name = "MainForm";
            this.Text = "FTP Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}