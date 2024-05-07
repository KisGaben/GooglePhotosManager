using Core.Credentials;
using Core.Services;
using Microsoft.UI.Xaml;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GooglePhotosManager
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public ObservableCollection<User> Users { get; set; }
        private IGoogleDriveService _driveService;
        private ISettings _settings;

        public MainWindow()
        {
            Users = new ObservableCollection<User>();

            this.InitializeComponent();
            RootPanel.DataContext = this;

            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Properties\googleDriveSecrets.json");
            var credentials = new GoogleDriveApiFileCredentials(file);
            _driveService = new GoogleDrive(credentials);

            _settings = new LocalSettings();

            InitUsersFromSettings();

        }

        private async void InitUsersFromSettings()
        {
            foreach(var user in _settings.Settings.UserSettings)
            {
                Users.Add(await LoadUserAsync(user.Value.Id));
            }
        }

        private async void AddNewUserAsync()
        {
            var id = Guid.NewGuid().ToString();

            var user = await LoadUserAsync(id);

            Users.Add(user);

            _settings.Settings.UserSettings.Add(user.Email, new UserSettings
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
            });

            _settings.Save();
        }

        private async Task<User> LoadUserAsync(string userId)
        {
            var userInfo = await _driveService.GetUserInfoAsync(userId);

            return new User
            {
                Id = userId,
                Name = userInfo.Name,
                Email = userInfo.Email,
                Storage = userInfo.Storage,
            };
        }

        private void newUserButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewUserAsync();
        }
    }

    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Storage { get; set; }
    }
}
