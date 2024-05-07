using System.Collections.Generic;

namespace Core.Settings.Data
{
    public class SettingsData
    {
        public Dictionary<string, UserSettings> UserSettings { get; set; } = new Dictionary<string, UserSettings>();
    }

    public class UserSettings
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
