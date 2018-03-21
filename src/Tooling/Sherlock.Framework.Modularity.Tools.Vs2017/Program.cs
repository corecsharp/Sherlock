namespace Sherlock.Framework.Modularity.Tools.Vs2017
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    class Program
    {
        private const string DestDirectoryName = "Modules";

        //modular --c xxx.csproj --d xx/publish/
        // CLI: dotnet modularity --config $(MSBuildProjectFullPath) --dest $(publishUrl)
        static int Main(string[] args)
        {

            var tuple = GetParameters(args);
            var projectFilePath = tuple.Item1;
            var dest = tuple.Item2;

            if (string.IsNullOrEmpty(projectFilePath) || string.IsNullOrEmpty(dest))
            {
                return 0;
            }

            Console.WriteLine();
            Console.WriteLine();

            var handler = new ProjectHandler(projectFilePath);
            List<ProjectFileInfo> files = handler.GetProjectFiles();
            List<KeyValuePair<ProjectFileInfo, string>> sourceFiles = GetCopyFiles(files);
            CopyFile(sourceFiles, dest);
            Console.WriteLine("Finish");

            Console.WriteLine("modularity succeeded!");
            return 0;
        }

        private static Tuple<string, string> GetParameters(string[] args)
        {
            var paramDic = new Dictionary<string, string>();
            if (args.Length < 4)
            {
                Console.WriteLine($"Sherlock modulary : parameters length less than 4.".Red().Bright());
            }

            for (int i = 0; i < args.Length; i += 2)
            {
                paramDic.Add(args[i], args[i + 1]);
            }

            string projectFilePath;
            var success = paramDic.TryGetValue("--config", out projectFilePath) || paramDic.TryGetValue("--c", out projectFilePath);


            if (string.IsNullOrEmpty(projectFilePath) || !File.Exists(projectFilePath))
            {
                Console.WriteLine($"Sherlock modulary : project file {projectFilePath} not exists.".Red().Bright());
            }

            string dest;
            success = paramDic.TryGetValue("--dest", out dest) || paramDic.TryGetValue("--d", out dest); ;


            if (string.IsNullOrEmpty(dest))
            {
                Console.WriteLine($"Sherlock modulary : dest paramter not exists.".Red().Bright());
            }
            return new Tuple<string, string>(projectFilePath, dest);
        }

        static List<KeyValuePair<ProjectFileInfo, string>> GetCopyFiles(IEnumerable<ProjectFileInfo> projectFile)
        {
            var result = projectFile
                .Select(
                    x => new
                    {
                        Files = Directory.EnumerateFiles(x.ProjectPath, "*.*", SearchOption.AllDirectories),
                        Project = x
                    });
            var files = new List<KeyValuePair<ProjectFileInfo, string>>();
            foreach (var project in result)
            {
                {
                    var projectFiles = project.Files.Where(x => IsSearchFile(project.Project.ProjectPath, x)).Select(x => new KeyValuePair<ProjectFileInfo, string>(project.Project, x));
                    files.AddRange(projectFiles);
                }
            }

            foreach (var file in files)
            {
                Console.WriteLine($"Found in Project {file.Key.ProjectName} file : {file.Value}");
            }

            return files;
        }

        static void CopyFile(List<KeyValuePair<ProjectFileInfo, string>> projectFile, string dest)
        {
            string destRoot = Path.Combine(dest, DestDirectoryName);
            if (Directory.Exists(destRoot))
            {
                Directory.Delete(destRoot, true);
            }

            try
            {
                foreach (KeyValuePair<ProjectFileInfo, string> keyValuePair in projectFile)
                {
                    var destFolder = Path.Combine(destRoot, keyValuePair.Key.ProjectName);
                    var destPath = keyValuePair.Value.Replace(keyValuePair.Key.ProjectPath, destFolder);
                    Console.WriteLine($"Sherlock modulary : deploy dependency file {destPath}");
                    if (!Directory.Exists(Path.GetDirectoryName(destPath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                    }
                    File.Copy(keyValuePair.Value, destPath, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static bool IsSearchFile(string rootFolder, string filePath)
        {
            string ext = Path.GetExtension(filePath);
            string fileName = Path.GetFileName(filePath);

            bool allowed = ext.Equals(".cshtml", StringComparison.OrdinalIgnoreCase) ||
                ext.Equals(".html", StringComparison.OrdinalIgnoreCase) ||
                ext.Equals(".js", StringComparison.OrdinalIgnoreCase) ||
                ext.Equals(".css", StringComparison.OrdinalIgnoreCase) ||
                ext.Equals(".json", StringComparison.OrdinalIgnoreCase) ||
                ext.Equals(".xml", StringComparison.OrdinalIgnoreCase);

            string objFolder = Path.Combine(rootFolder, "obj");
            string binFolder = Path.Combine(rootFolder, "bin");
            bool isExcepted = filePath.StartsWith(objFolder, StringComparison.OrdinalIgnoreCase) ||
                filePath.StartsWith(binFolder, StringComparison.OrdinalIgnoreCase) ||
                fileName.Equals("project.json", StringComparison.OrdinalIgnoreCase) ||
                fileName.Equals("project.lock.json", StringComparison.OrdinalIgnoreCase);

            return !isExcepted && allowed;
        }
    }
}