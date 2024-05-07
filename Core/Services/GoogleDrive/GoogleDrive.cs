using Core.Data;
using Core.Services.GoogleDrive.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.GoogleDrive
{
    public class GoogleDrive : IGoogleDriveService
    {
        private readonly ClientSecrets _credentials;

        public GoogleDrive(IGoogleDriveApiCredentials credentials)
        {
            _credentials = new ClientSecrets
            {
                ClientId = credentials.ClientId,
                ClientSecret = credentials.ClientSecret
            };
        }

        private Task<UserCredential> GetCredentialsForUserAsync(string userId)
        {
            return GoogleWebAuthorizationBroker.AuthorizeAsync(
                    _credentials,
                    new[] { DriveService.Scope.DriveMetadataReadonly },
                    userId,
                    CancellationToken.None
                );
        }

        private async Task<DriveService> GetDriveServiceAsync(string userId)
        {
            return new DriveService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = await GetCredentialsForUserAsync(userId),
            });
        }


        public async Task<GoogleUserInfo> GetUserInfoAsync(string userId)
        {
            var service = await GetDriveServiceAsync(userId);
            var about = service.About.Get();
            about.Fields = "*";
            var result = await about.ExecuteAsync();

            return new GoogleUserInfo
            {
                ID = userId,
                Name = result.User.DisplayName,
                Email = result.User.EmailAddress,
                Avatar = result.User.PhotoLink,
                UsedStorage = result.StorageQuota.Usage ?? 0,
                TotalStorage = result.StorageQuota.Limit ?? 0
            };
        }
    }
}
