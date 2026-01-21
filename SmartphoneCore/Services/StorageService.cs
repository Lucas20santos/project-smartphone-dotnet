using SmartphoneCore.Models;
using SmartphoneCore.Services;

namespace SmartphoneCore.Services
{
    public class StorageService : IStorageService
    {
        public bool ChecarEspaco(Smartphone smartphone, double tamanhoEmMb)
        {
            double espacoOcupado = smartphone.AplicativosInstalados.Sum(app => app.TamanhoEmMb);
            double espacoNecessarioTotal = espacoOcupado + tamanhoEmMb;
            
            return espacoNecessarioTotal <= smartphone.Memoria;
        }
    }
}