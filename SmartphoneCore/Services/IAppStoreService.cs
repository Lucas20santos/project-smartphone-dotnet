using SmartphoneCore.Models;
using SmartphoneCore.Services;

namespace SmartphoneCore.Services
{
    public interface IAppStoreService
    {
        Task DownloadAppAsync(Smartphone smartphone, string nomeApp, IStorageService storageService);
    }
}