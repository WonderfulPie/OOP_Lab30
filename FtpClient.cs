using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.IO;
using System.Net;

namespace Lab_30_Danylko
{
    public class FtpClient
    {
        private string ftpServer;
        private string userName;
        private string password;

        public FtpClient(string ftpServer, string userName, string password)
        {
            this.ftpServer = ftpServer;
            this.userName = userName;
            this.password = password;

        }

        private void LogMessage(string message)
        {
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ftp_log.txt");
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }

        public void AppendFile(string filePath, byte[] fileContents)
        {
            try
            {
                string uri = ftpServer + filePath;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.AppendFile;
                request.Credentials = new NetworkCredential(userName, password);

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    LogMessage($"Appended file: {filePath} - Status: {response.StatusDescription}");
                }
            }
            catch (WebException ex)
            {
                LogMessage($"Error appending file: {filePath} - {ex.Message}");
                throw;
            }
        }

        public void DeleteFile(string filePath)
        {
            try
            {
                string uri = ftpServer + filePath;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(userName, password);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    LogMessage($"Deleted file: {filePath} - Status: {response.StatusDescription}");
                }
            }
            catch (WebException ex)
            {
                LogMessage($"Error deleting file: {filePath} - {ex.Message}");
                throw;
            }
        }

        public byte[] RetrieveFile(string filePath)
        {
            try
            {
                string uri = ftpServer + filePath;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(userName, password);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    responseStream.CopyTo(memoryStream);
                    LogMessage($"Retrieved file: {filePath} - Status: {response.StatusDescription}");
                    return memoryStream.ToArray();
                }
            }
            catch (WebException ex)
            {
                LogMessage($"Error retrieving file: {filePath} - {ex.Message}");
                throw;
            }
        }

        public DateTime GetModifiedTime(string filePath)
        {
            try
            {
                string uri = ftpServer + filePath;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                request.Credentials = new NetworkCredential(userName, password);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    LogMessage($"Retrieved modified time for file: {filePath} - Status: {response.StatusDescription}");
                    return response.LastModified;
                }
            }
            catch (WebException ex)
            {
                LogMessage($"Error retrieving modified time for file: {filePath} - {ex.Message}");
                throw;
            }
        }

        public long GetFileSize(string filePath)
        {
            try
            {
                string uri = ftpServer + filePath;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.GetFileSize;
                request.Credentials = new NetworkCredential(userName, password);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    LogMessage($"Retrieved file size for file: {filePath} - Status: {response.StatusDescription}");
                    return response.ContentLength;
                }
            }
            catch (WebException ex)
            {
                LogMessage($"Error retrieving file size for file: {filePath} - {ex.Message}");
                throw;
            }
        }

        public string[] ListDirectory(string directoryPath)
        {
            try
            {
                string uri = ftpServer + directoryPath;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(userName, password);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string line;
                    var result = new System.Collections.Generic.List<string>();
                    while ((line = reader.ReadLine()) != null)
                    {
                        result.Add(line);
                    }
                    LogMessage($"Listed directory: {directoryPath} - Status: {response.StatusDescription}");
                    return result.ToArray();
                }
            }
            catch (WebException ex)
            {
                LogMessage($"Error listing directory: {directoryPath} - {ex.Message}");
                throw;
            }
        }

        public string[] ListDirectoryDetails(string directoryPath)
        {
            try
            {
                string uri = ftpServer + directoryPath;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.Credentials = new NetworkCredential(userName, password);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string line;
                    var result = new System.Collections.Generic.List<string>();
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Логування кожного рядка для діагностики проблем
                        System.Diagnostics.Debug.WriteLine($"Received line: {line}");
                        LogMessage($"Received line: {line}");
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            result.Add(line);
                        }
                    }
                    LogMessage($"Listed directory details: {directoryPath} - Status: {response.StatusDescription}");
                    return result.ToArray();
                }
            }
            catch (WebException ex)
            {
                LogMessage($"Error listing directory details: {directoryPath} - {ex.Message}");
                throw;
            }
        }

        public void MakeDirectory(string directoryPath)
        {
            try
            {
                string uri = ftpServer + directoryPath;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(userName, password);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    LogMessage($"Made directory: {directoryPath} - Status: {response.StatusDescription}");
                }
            }
            catch (WebException ex)
            {
                LogMessage($"Error making directory: {directoryPath} - {ex.Message}");
                throw;
            }
        }

        public void RemoveDirectory(string directoryPath)
        {
            try
            {
                string uri = ftpServer + directoryPath;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.RemoveDirectory;
                request.Credentials = new NetworkCredential(userName, password);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    LogMessage($"Removed directory: {directoryPath} - Status: {response.StatusDescription}");
                }
            }
            catch (WebException ex)
            {
                LogMessage($"Error removing directory: {directoryPath} - {ex.Message}");
                throw;
            }
        }

        public void RenameFile(string currentFilePath, string newFilePath)
        {

                string uri = ftpServer + currentFilePath;
                string newFileName = Path.GetFileName(newFilePath); // Отримуємо нове ім'я файлу/директорії
                string directoryPath = Path.GetDirectoryName(currentFilePath)?.Replace("\\", "/");

                if (directoryPath == null)
                {
                    throw new ArgumentNullException(nameof(directoryPath));
                }

                if (!directoryPath.EndsWith("/"))
                {
                    directoryPath += "/";
                }

                string newUri = ftpServer + directoryPath + newFileName;

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.Rename;
                request.RenameTo = newFileName; // Встановлюємо нове ім'я для перейменування
                request.Credentials = new NetworkCredential(userName, password);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    LogMessage($"Renamed file from {currentFilePath} to {newFileName} - Status: {response.StatusDescription}");
                }
          
        }

        public void StoreFile(string filePath, byte[] fileContents)
        {
            try
            {
                string uri = ftpServer + filePath;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(userName, password);

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    LogMessage($"Stored file: {filePath} - Status: {response.StatusDescription}");
                }
            }
            catch (WebException ex)
            {
                LogMessage($"Error storing file: {filePath} - {ex.Message}");
                throw;
            }
        }

        public void StoreUniqueFile(byte[] fileContents)
        {
            try
            {
                string uri = ftpServer + "/";
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.UploadFileWithUniqueName;
                request.Credentials = new NetworkCredential(userName, password);

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    LogMessage($"Stored unique file - Status: {response.StatusDescription}");
                }
            }
            catch (WebException ex)
            {
                LogMessage($"Error storing unique file - {ex.Message}");
                throw;
            }
        }
    }
}