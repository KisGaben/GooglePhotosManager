using CommunityToolkit.Mvvm.Input;
using Core.Services;
using Microsoft.UI.Xaml;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

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



        public MainWindow()
        {
            Users = new ObservableCollection<User>();

            this.InitializeComponent();
            RootPanel.DataContext = this;

        }

        private async void newUserButton_Click(object sender, RoutedEventArgs e)
        {
            var id = Guid.NewGuid().ToString();

            var driveApi = new DriveApi();
            var storage = await driveApi.getUserStorageAsync(id);

            var user = new User
            {
                Id = id,
                Name = storage.User.DisplayName,
                Storage = storage.StorageQuota.Usage.ToString(),
            };

            Users.Add(user);
        }


    }

    public class User : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Storage { get; set; }

        public ICommand RefreshCommand { get; set; }

        public User()
        {
            RefreshCommand = new RelayCommand(() => RefreshUserAsync());
        }

        private async Task RefreshUserAsync()
        {
            var driveApi = new DriveApi();
            var storage = await driveApi.getUserStorageAsync(Id);
            Storage = storage.StorageQuota.Usage.ToString();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Storage)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
