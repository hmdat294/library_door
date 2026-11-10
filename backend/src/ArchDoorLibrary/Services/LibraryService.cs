using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace ArchDoorLibrary.Services
{
    public static class LibraryService
    {
        private static readonly object Sync = new object();

        public static JObject ReadLibrary()
        {
            string path = Path.Combine(ProjectPaths.DataDirectory, "library.json");
            lock (Sync)
            {
                if (!File.Exists(path))
                    return new JObject { ["schemaVersion"] = 1, ["doors"] = new JArray() };
                return JObject.Parse(File.ReadAllText(path));
            }
        }

        public static void SaveDoor(JObject door)
        {
            string id = (string)door["id"];
            if (string.IsNullOrWhiteSpace(id))
                throw new InvalidOperationException("door.id không được để trống.");

            string doorsDir = Path.Combine(ProjectPaths.DataDirectory, "doors");
            string recordDir = Path.Combine(doorsDir, id);
            Directory.CreateDirectory(recordDir);
            string doorPath = Path.Combine(recordDir, "door.json");

            lock (Sync)
            {
                File.WriteAllText(doorPath, door.ToString(Formatting.Indented));
                JObject library = ReadLibrary();
                JArray doors = library["doors"] as JArray ?? new JArray();
                bool found = false;
                foreach (JObject item in doors)
                {
                    if (string.Equals((string)item["id"], id, StringComparison.OrdinalIgnoreCase))
                    {
                        item["name"] = door["name"];
                        item["path"] = "doors/" + id + "/door.json";
                        found = true;
                        break;
                    }
                }
                if (!found)
                    doors.Add(new JObject {
                        ["id"] = id,
                        ["name"] = door["name"],
                        ["path"] = "doors/" + id + "/door.json"
                    });
                library["schemaVersion"] = 1;
                library["doors"] = doors;
                File.WriteAllText(Path.Combine(ProjectPaths.DataDirectory, "library.json"),
                    library.ToString(Formatting.Indented));
            }
        }
    }
}
