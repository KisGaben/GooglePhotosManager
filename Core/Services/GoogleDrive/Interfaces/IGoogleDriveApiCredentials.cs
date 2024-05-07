namespace Core.Services.GoogleDrive.Interfaces
{
    public interface IGoogleDriveApiCredentials
    {
        string ClientId { get; }
        string ClientSecret { get; }
    }
}
