using SmartphoneCore.Models;
using SmartphoneCore.Services;

namespace SmartphoneCore.Services
{
    public interface IStorageService
    {
        bool ChecarEspaco(Smartphone smartphone, double tamanhoEmMb);
    }
}