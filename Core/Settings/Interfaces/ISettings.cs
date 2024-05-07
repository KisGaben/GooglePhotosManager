using Core.Settings.Data;

namespace Core.Settings.Interfaces
{
    public interface ISettings
    {
        SettingsData Settings { get; }

        void Save();
    }
}
