using System;
using System.IO;
using System.Reflection;

namespace ArchDoorLibrary.Services
{
    public static class ProjectPaths
    {
        public static string FindProjectRoot()
        {
            string start = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var current = new DirectoryInfo(start);
            while (current != null)
            {
                if (Directory.Exists(Path.Combine(current.FullName, "frontend")) &&
                    Directory.Exists(Path.Combine(current.FullName, "data")))
                    return current.FullName;
                current = current.Parent;
            }

            // Optional override for deployments where the project folder is elsewhere.
            string configured = Environment.GetEnvironmentVariable("ARCHDOORLIBRARY_HOME");
            if (!string.IsNullOrWhiteSpace(configured) && Directory.Exists(configured))
                return configured;

            throw new DirectoryNotFoundException(
                "Không tìm thấy thư mục project có frontend và data. " +
                "Hãy giữ cấu trúc thư mục khi triển khai hoặc đặt ARCHDOORLIBRARY_HOME.");
        }

        public static string DataDirectory
        {
            get { return Path.Combine(FindProjectRoot(), "data"); }
        }
    }
}
