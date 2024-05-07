using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Core.Settings.Interfaces;
using Core.Settings.Data;

namespace Core.Settings
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

        public SettingsData Settings { get; private set; }

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
                Settings = JsonSerializer.Deserialize<SettingsData>(stream);
            }
        }

        private void InitSettings()
        {
            Settings = new SettingsData();
            Save();
        }
    }
}
