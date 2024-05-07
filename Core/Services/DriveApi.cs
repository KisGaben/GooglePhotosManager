using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services
{
    public class DriveApi
    {
        public async Task<About> getUserStorageAsync(string userId)
        {
            UserCredential credentials;

            using (var stream = new FileStream("clientSecrets.json", FileMode.Open, FileAccess.Read))
            {
                credentials = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new[] { DriveService.Scope.DriveMetadataReadonly },
                    userId,
                    CancellationToken.None
                );
            }

            var service = new DriveService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials
            });

            var about = service.About.Get();
            about.Fields = "*";
            var result = await about.ExecuteAsync();

            return result;
        }
    }
}
