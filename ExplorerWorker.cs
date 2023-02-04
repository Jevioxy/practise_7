using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Explorer
{
    static class ExplorerWorker
    {
        static public Dictionary<string, string> GetDrives()
        {
            // Метод, испольующийся для получения словаря с Дисками.
            DriveInfo[] dis = DriveInfo.GetDrives();
            Dictionary<string, string> rows = new Dictionary<string, string>();

            foreach (DriveInfo di in dis)
            {
                double totalSpaceGB = Math.Round(di.TotalSize / Math.Pow(2, 30), 2);
                rows[di.Name] = $"Диск {di.Name} (Объем: {totalSpaceGB} GB)";
            }
            return rows;
        }

        static public Dictionary<string, string> GetFilesAndDirs(string path)
        {
            // Метод, испольующийся для получения словаря с папками и файлами.
            var directory = new DirectoryInfo(path);
            var dirs = directory.GetDirectories();
            var files = directory.GetFiles();

            Dictionary<string, string> rows = new Dictionary<string, string>();

            foreach ( var dir in dirs)
                rows[dir.FullName] = $"{dir.Name} \t(Дата последнего изменения: {dir.LastWriteTime})";

            foreach (var file in files)
                rows[file.FullName] = $"{file.Name} \t(Дата последнего изменения: {file.LastWriteTime})";
            
            return rows;
        }

        static public bool OpenFile(string path)
        {
            // Метод, испольующийся для открытия файла.
            if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
            {
                try
                {
                    Process.Start(path);
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть данный тип файла, так как для него не задана ассоциация!", "Ошибка");
                }
                return true;
            }
            return false;
        }
    }
}
