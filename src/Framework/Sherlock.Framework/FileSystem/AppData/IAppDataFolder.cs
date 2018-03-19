using System;
using System.Collections.Generic;
using System.IO;

namespace Sherlock.Framework.FileSystem.AppData
{
    /// <summary>
    /// ʵ�ִ˽ӿ��Գ���Ӧ�ó���Ŀ¼ "~/App_Data" �� 
    /// </summary>
    public interface IAppDataFolder
    {
        IEnumerable<string> ListFiles(string path);
        IEnumerable<string> ListDirectories(string path);

        string Combine(params string[] paths);

        bool FileExists(string path);
        void CreateFile(string path, string content);
        Stream CreateFile(string path);
        string ReadFile(string path);
        Stream OpenFile(string path);
        void StoreFile(string sourceFileName, string destinationPath);
        void DeleteFile(string path);

        DateTime GetFileLastWriteTimeUtc(string path);

        void CreateDirectory(string path);
        bool DirectoryExists(string path);

        string MapPath(string path);
        string GetVirtualPath(string path);
    }
}