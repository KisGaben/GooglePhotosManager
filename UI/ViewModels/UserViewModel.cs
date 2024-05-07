using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Services.GoogleDrive.Interfaces;
using Core.Settings.Data;
using Core.Settings.Interfaces;
using System;
using System.Threading.Tasks;
using UI.Data;

namespace UI.ViewModels
{
    public partial class UserViewModel : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        private User _user;

        public double UsagePercentage => (double)User.UsedStorage / User.TotalStorage * 100;
        public string TotalStorageFormatted => FormatBytes(User.TotalStorage, "0");
        public string UsedStorageFormated => FormatBytes(User.UsedStorage);
        public string UsageFormatted => $"{UsedStorageFormated} / {TotalStorageFormatted}";

        public event EventHandler<string> UserRemoving;
        public event EventHandler<string> UserRefreshing;
        #endregion

        public UserViewModel(User user)
        {
            User = user;
            User.PropertyChanged += (s, e) => OnDependentPropertyChanged(e.PropertyName);
            PropertyChanged += (s, e) => OnDependentPropertyChanged(e.PropertyName);
        }

        #region Commands
        [RelayCommand]
        public void RefreshUser()
        {
            UserRefreshing?.Invoke(this, User.Id);
        }

        [RelayCommand]
        public void DeleteUser()
        {
            UserRemoving?.Invoke(this, User.Id);
        }

        #endregion

        #region Methods
        private string FormatBytes(double bytes, string format = "0.00")
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };

            for (int i = 0; i < sizes.Length; i++)
            {
                if (bytes < 1024 || i == sizes.Length - 1)
                    return $"{string.Format($"{{0:{format}}}", bytes)} {sizes[i]}";

                bytes /= 1024;
            }

            throw new Exception("This should never happen");
        }

        private void OnDependentPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(User) || propertyName == nameof(User.UsedStorage) || propertyName == nameof(User.TotalStorage))
            {
                OnPropertyChanged(nameof(UsagePercentage));
                OnPropertyChanged(nameof(UsageFormatted));
                OnPropertyChanged(nameof(TotalStorageFormatted));
                OnPropertyChanged(nameof(UsedStorageFormated));
            }
        }
        #endregion
    }
}
