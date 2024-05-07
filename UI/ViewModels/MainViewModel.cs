using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Services.GoogleDrive.Interfaces;
using Core.Settings.Data;
using Core.Settings.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UI.Data;

namespace UI.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        #region Services
        private IGoogleDriveService _driveService;
        private ISettings _settings;
        #endregion

        public ObservableCollection<UserViewModel> Users { get; set; }

        [ObservableProperty]
        private string _title = "Google Storage Manager";

        public MainViewModel(IGoogleDriveService driveService, ISettings settings)
        {
            _driveService = driveService;
            _settings = settings;

            Users = new ObservableCollection<UserViewModel>();
            InitUsersFromSettings();
        }


        #region Commands
        [RelayCommand]
        public async Task AddNewUserAsync()
        {
            var id = Guid.NewGuid().ToString();

            var user = await LoadUserAsync(id);

            Users.Add(GetUserViewModel(user));

            _settings.Settings.UserSettings.Add(user.Id, new UserSettings
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
            });

            _settings.Save();
        }
        #endregion

        #region Methods
        private async void InitUsersFromSettings()
        {
            foreach (var userSetting in _settings.Settings.UserSettings)
            {
                var user = await LoadUserAsync(userSetting.Value.Id);
                Users.Add(GetUserViewModel(user));
            }
        }

        private async Task<User> LoadUserAsync(string userId)
        {
            var userInfo = await _driveService.GetUserInfoAsync(userId);

            return new User(userInfo);
        }

        private UserViewModel GetUserViewModel(User user)
        {
            var vm = new UserViewModel(user);
            vm.UserRemoving += OnUserRemoving;
            vm.UserRefreshing += OnUserRefreshingAsync;
            return vm;
        }

        private void OnUserRemoving(object sender, string e)
        {
            var userVm = sender as UserViewModel;
            Users.Remove(userVm);
            _settings.Settings.UserSettings.Remove(e);
            _settings.Save();
        }

        private async void OnUserRefreshingAsync(object sender, string e)
        {
            var userVm = sender as UserViewModel;
            var user = await LoadUserAsync(e);
            userVm.User = user;

            _settings.Settings.UserSettings[e].Name = user.Name;
            _settings.Settings.UserSettings[e].Email = user.Email;
            _settings.Save();
        }
        #endregion


    }
}
