using CommunityToolkit.Mvvm.ComponentModel;
using Core.Data;

namespace UI.Data
{
    public partial class User : ObservableObject
    {
        [ObservableProperty]
        private string _id;
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private string _email;
        [ObservableProperty]
        private string _avatar;
        [ObservableProperty]
        private long _totalStorage;
        [ObservableProperty]
        private long _usedStorage;

        public User(GoogleUserInfo googleUserInfo)
        {
            Id = googleUserInfo.ID;
            Name = googleUserInfo.Name;
            Email = googleUserInfo.Email;
            Avatar = googleUserInfo.Avatar;
            TotalStorage = googleUserInfo.TotalStorage;
            UsedStorage = googleUserInfo.UsedStorage;
        }
    }
}
