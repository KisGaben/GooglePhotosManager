using Core.Services.GoogleDrive.Interfaces;

namespace Core.Services.GoogleDrive.Credentials
{
    public class GoogleDriveApiSimpleCredentials : IGoogleDriveApiCredentials
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
