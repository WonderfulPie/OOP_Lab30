using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Microsoft.VisualBasic; // Додано для використання Interaction.InputBox

namespace Lab_30_Danylko
{
    public partial class MainForm : Form
    {
        private string ftpServer;
        private string userName;
        private string password;
        private FtpClient ftpClient;

        public MainForm()
        {
            InitializeComponent();
        }

        // Підключається до FTP сервера та завантажує кореневий каталог у TreeView
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            ftpServer = serverTextBox.Text;
            userName = userTextBox.Text;
            password = passwordTextBox.Text;

            // Переконайтеся, що ftpServer має правильний формат
            if (!ftpServer.StartsWith("ftp://"))
            {
                ftpServer = "ftp://" + ftpServer;
            }

            try
            {
                ftpClient = new FtpClient(ftpServer, userName, password);

                // Завантаження структури каталогу у TreeView
                var rootNode = new TreeNode("/");
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(rootNode);

                LoadDirectory(rootNode);
                LogMessage("Connected to FTP server successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to FTP server: " + ex.Message);
                LogMessage("Error connecting to FTP server: " + ex.Message);
            }
        }

        // Завантажує структуру каталогу у TreeView
        private void LoadDirectory(TreeNode node)
        {
            try
            {
                string path = node.FullPath.Replace("\\", "/");
                if (!path.StartsWith("/"))
                {
                    path = "/" + path;
                }

                string[] entries = ftpClient.ListDirectoryDetails(path);
                node.Nodes.Clear(); // Очистка текущих узлов перед добавлением новых

                foreach (string entry in entries)
                {
                    // Пример строки: drwxr-xr-x 2 user group 4096 Jan 1 12:34 foldername
                    // Разделяем строку на части, чтобы получить все элементы
                    string[] tokens = entry.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tokens.Length >= 9)
                    {
                        string name = null;
                        if (tokens.Length == 9)
                        {
                            name = tokens[8]; // Если нет пробелов в имени файла/папки
                        }
                        else
                        {
                            // Объединяем все элементы после атрибутов в имя файла/папки
                            name = string.Join(" ", tokens, 8, tokens.Length - 8);
                        }

                        // Пропускаем элементы "." и ".."
                        if (name == "." || name == "..")
                        {
                            continue;
                        }

                        string attributes = tokens[0];

                        var childNode = new TreeNode(name);
                        if (attributes.StartsWith("d"))
                        {
                            // Это папка
                            childNode.Nodes.Add(new TreeNode()); // Добавляем пустой узел для возможности раскрытия
                        }
                        node.Nodes.Add(childNode);
                    }
                    else
                    {
                        // Логирование для диагностики проблем
                        System.Diagnostics.Debug.WriteLine($"Invalid entry format: {entry}");
                        LogMessage($"Invalid entry format: {entry}");
                    }
                }
                node.Expand();
                LogMessage("Loaded directory successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading directory: " + ex.Message);
                LogMessage("Error loading directory: " + ex.Message);
            }
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            // Не реалізовано
            MessageBox.Show("Settings button clicked.");
        }

        private void UploadFilesButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true; // Дозволити вибір кількох файлів
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    byte[] fileContents = File.ReadAllBytes(filePath);
                    string fileName = Path.GetFileName(filePath);
                    try
                    {
                        string remoteDirectory = treeView1.SelectedNode.FullPath.Replace("\\", "/");
                        if (!remoteDirectory.StartsWith("/"))
                        {
                            remoteDirectory = "/" + remoteDirectory;
                        }
                        if (!remoteDirectory.EndsWith("/"))
                        {
                            remoteDirectory += "/";
                        }
                        string remoteFilePath = remoteDirectory + fileName;

                        LogMessage($"Uploading file: {fileName} to {remoteFilePath}");
                        ftpClient.StoreFile(remoteFilePath, fileContents);
                        LogMessage($"File uploaded successfully: {fileName}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error uploading file: {fileName} - {ex.Message}");
                        LogMessage($"Error uploading file: {fileName} - {ex.Message}");
                    }
                }

                // Оновлення вмісту після завантаження файлів
                LoadDirectory(treeView1.SelectedNode);
            }
        }



        private void UploadFolderButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string folderPath = folderBrowserDialog.SelectedPath;

                UploadFolder(folderPath, treeView1.SelectedNode.FullPath.Replace("\\", "/"));
            }
        }

        private void UploadFolder(string folderPath, string remotePath)
        {
            // Переконайтеся, що віддалений шлях має правильний формат
            if (!remotePath.StartsWith("/"))
            {
                remotePath = "/" + remotePath;
            }
            if (!remotePath.EndsWith("/"))
            {
                remotePath += "/";
            }

            // Завантаження всіх файлів у поточній папці
            foreach (string filePath in Directory.GetFiles(folderPath))
            {
                byte[] fileContents = File.ReadAllBytes(filePath);
                string fileName = Path.GetFileName(filePath);
                string remoteFilePath = remotePath + fileName;

                try
                {
                    LogMessage($"Uploading file: {fileName} to {remoteFilePath}");
                    ftpClient.StoreFile(remoteFilePath, fileContents);
                    LogMessage($"File uploaded successfully: {fileName}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error uploading file: {fileName} - {ex.Message}");
                    LogMessage($"Error uploading file: {fileName} - {ex.Message}");
                }
            }

            // Рекурсивне завантаження всіх підкаталогів
            foreach (string subFolderPath in Directory.GetDirectories(folderPath))
            {
                string folderName = Path.GetFileName(subFolderPath);
                string remoteSubFolderPath = remotePath + folderName;

                try
                {
                    LogMessage($"Creating directory: {remoteSubFolderPath}");
                    ftpClient.MakeDirectory(remoteSubFolderPath);
                    LogMessage($"Directory created successfully: {remoteSubFolderPath}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating directory: {remoteSubFolderPath} - {ex.Message}");
                    LogMessage($"Error creating directory: {remoteSubFolderPath} - {ex.Message}");
                }

                UploadFolder(subFolderPath, remoteSubFolderPath);
            }

            // Оновлення вмісту після завантаження папки
            LoadDirectory(treeView1.SelectedNode);
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "")
            {
                e.Node.Nodes.Clear();
                LoadDirectory(e.Node);
            }
        }

        // Видаляє вибраний файл або каталог
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                string filePath = treeView1.SelectedNode.FullPath.Replace("\\", "/");
                try
                {
                    ftpClient.DeleteFile(filePath);
                    treeView1.SelectedNode.Remove();
                    MessageBox.Show("File deleted successfully.");
                    LogMessage($"File deleted successfully: {filePath}");

                    // Обновление содержимого после удаления файла
                    LoadDirectory(treeView1.SelectedNode.Parent);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting file: " + ex.Message);
                    LogMessage("Error deleting file: " + ex.Message);
                }
            }
        }

        // Завантажує файл з локальної машини на FTP сервер
        private void UploadFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] fileContents = File.ReadAllBytes(openFileDialog.FileName);
                string fileName = Path.GetFileName(openFileDialog.FileName);
                try
                {
                    string remoteDirectory = treeView1.SelectedNode.FullPath.Replace("\\", "/");
                    if (!remoteDirectory.StartsWith("/"))
                    {
                        remoteDirectory = "/" + remoteDirectory;
                    }
                    if (!remoteDirectory.EndsWith("/"))
                    {
                        remoteDirectory += "/";
                    }
                    string remoteFilePath = remoteDirectory + fileName;

                    LogMessage($"Uploading file: {fileName} to {remoteFilePath}");
                    ftpClient.StoreFile(remoteFilePath, fileContents); // Виправлений виклик методу
                    MessageBox.Show("File uploaded successfully.");
                    LogMessage($"File uploaded successfully: {fileName}");

                    // Оновлення вмісту після завантаження файлу
                    LoadDirectory(treeView1.SelectedNode);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error uploading file: {fileName} - {ex.Message}");
                    LogMessage($"Error uploading file: {fileName} - {ex.Message}");
                }
            }
        }

        // Створює новий каталог у вибраному каталозі
        private void MakeDirButton_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                string dirName = Interaction.InputBox("Enter directory name:", "Make Directory");
                if (!string.IsNullOrEmpty(dirName))
                {
                    string dirPath = treeView1.SelectedNode.FullPath.Replace("\\", "/") + "/" + dirName;
                    try
                    {
                        LogMessage($"Creating directory: {dirPath}");
                        ftpClient.MakeDirectory(dirPath);
                        treeView1.SelectedNode.Nodes.Add(new TreeNode(dirName));
                        MessageBox.Show("Directory created successfully.");
                        LogMessage($"Directory created successfully: {dirPath}");

                        // Обновление содержимого после создания директории
                        LoadDirectory(treeView1.SelectedNode);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error creating directory: " + ex.Message);
                        LogMessage("Error creating directory: " + ex.Message);
                    }
                }
            }
        }

        // Видаляє вибраний каталог
        private void RemoveDirButton_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                string dirPath = treeView1.SelectedNode.FullPath.Replace("\\", "/");
                try
                {
                    LogMessage($"Removing directory: {dirPath}");
                    ftpClient.RemoveDirectory(dirPath);
                    treeView1.SelectedNode.Remove();
                    MessageBox.Show("Directory removed successfully.");
                    LogMessage($"Directory removed successfully: {dirPath}");

                    // Обновление содержимого после удаления директории
                    LoadDirectory(treeView1.SelectedNode.Parent);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error removing directory: " + ex.Message);
                    LogMessage("Error removing directory: " + ex.Message);
                }
            }
        }

        // Перейменовує вибраний файл або каталог
        private void RenameButton_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                string newName = Interaction.InputBox("Enter new name:", "Rename");
                if (!string.IsNullOrEmpty(newName))
                {
                    string currentPath = treeView1.SelectedNode.FullPath.Replace("\\", "/");
                    string directoryPath = Path.GetDirectoryName(currentPath)?.Replace("\\", "/");

                    if (string.IsNullOrEmpty(directoryPath))
                    {
                        directoryPath = "/";
                    }

                    if (!directoryPath.EndsWith("/"))
                    {
                        directoryPath += "/";
                    }

                    string newPath = directoryPath + newName;

                    // Проверка на пустой путь
                    if (string.IsNullOrEmpty(currentPath) || string.IsNullOrEmpty(newPath))
                    {
                        MessageBox.Show("Invalid path for renaming.");
                        return;
                    }

                    try
                    {
                        LogMessage($"Renaming file from {currentPath} to {newPath}");
                        ftpClient.RenameFile(currentPath, newPath);
                        treeView1.SelectedNode.Text = newName;
                        MessageBox.Show("File renamed successfully.");
                        LogMessage($"File renamed from {currentPath} to {newPath}");

                        // Обновление содержимого после переименования
                        LoadDirectory(treeView1.SelectedNode.Parent ?? treeView1.SelectedNode);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error renaming file: " + ex.Message);
                        LogMessage("Error renaming file: " + ex.Message);
                    }
                }
            }
        }

        // Завантажує вибраний файл з FTP сервера на локальну машину
        private void DownloadButton_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = treeView1.SelectedNode.Text;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = treeView1.SelectedNode.FullPath;
                    try
                    {
                        LogMessage($"Downloading file: {filePath}");
                        byte[] fileContents = ftpClient.RetrieveFile(filePath);
                        File.WriteAllBytes(saveFileDialog.FileName, fileContents);
                        MessageBox.Show("File downloaded successfully.");
                        LogMessage($"File downloaded successfully: {filePath}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error downloading file: {filePath} - {ex.Message}");
                        LogMessage($"Error downloading file: {filePath} - {ex.Message}");
                    }
                }
            }
        }

        private void LogMessage(string message)
        {
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ftp_log.txt");
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
    }
}