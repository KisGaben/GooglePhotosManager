using System.Threading.Tasks;
using Core.Data;

namespace Core.Services.GoogleDrive.Interfaces
{
    public interface IGoogleDriveService
    {
        Task<GoogleUserInfo> GetUserInfoAsync(string userId);
    }
}
