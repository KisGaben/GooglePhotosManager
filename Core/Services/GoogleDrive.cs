using Core.Credentials;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services
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
                UserId = userId,
                Email = result.User.EmailAddress,
                Name = result.User.DisplayName,
                Storage = result.StorageQuota.Usage.ToString(),
            };
        }
    }

    public interface IGoogleDriveService
    {
        Task<GoogleUserInfo> GetUserInfoAsync(string userId);
    }

    public class GoogleUserInfo
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Storage { get; set; }
    }
}
