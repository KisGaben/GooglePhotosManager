using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Credentials
{
    public class GoogleDriveApiFileCredentials : IGoogleDriveApiCredentials
    {
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }
        public string FilePath { get; private set; }

        public GoogleDriveApiFileCredentials(string filePath)
        {
            FilePath = filePath;
            LoadCredentials();
        }

        private void LoadCredentials()
        {
            using (var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                var credentials = JsonSerializer.Deserialize<GoogleDriveSecrets>(stream);
                ClientId = credentials.Installed.ClientId;
                ClientSecret = credentials.Installed.ClientSecret;
            }
        }

        private class GoogleDriveSecrets
        {
            [JsonPropertyName("installed")]
            public GoogleDriveCredentials Installed { get; set; }
        }

        private class GoogleDriveCredentials
        {
            [JsonPropertyName("client_id")]
            public string ClientId { get; set; }
            [JsonPropertyName("client_secret")]
            public string ClientSecret { get; set; }
        }

    }

    public class GoogleDriveApiSimpleCredentials : IGoogleDriveApiCredentials
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public interface IGoogleDriveApiCredentials
    {
        string ClientId { get; }
        string ClientSecret { get; }
    }
}
