using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace Core.Services
{
    public class LocalSettings : ISettings
    {
        private string StorageFile { get; }

        public LocalSettings(string filePath = "localSettings.json")
        {
            StorageFile = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, filePath);
            Debug.WriteLine(StorageFile);
            LoadSettings();
        }

        public Settings Settings { get; private set; }

        public void Save()
        {
            using (var stream = new FileStream(StorageFile, FileMode.Create, FileAccess.Write))
            {
                JsonSerializer.Serialize(stream, Settings);
            }
        }

        private void LoadSettings()
        {
            if (!File.Exists(StorageFile))
            {
                InitSettings();
                return;
            }

            using (var stream = new FileStream(StorageFile, FileMode.Open, FileAccess.Read))
            {
                Settings = JsonSerializer.Deserialize<Settings>(stream);
            }
        }

        private void InitSettings()
        {
            Settings = new Settings();
            Save();
        }
    }

    public class Settings
    {
        public Dictionary<string, UserSettings> UserSettings { get; set; } = new Dictionary<string, UserSettings>();
    }

    public class UserSettings
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public interface ISettings
    {
        Settings Settings { get; }

        void Save();
    }
}
