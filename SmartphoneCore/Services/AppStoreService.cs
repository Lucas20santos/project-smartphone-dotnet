using SmartphoneCore.Models;
using SmartphoneCore.Services;

namespace SmartphoneCore.Services
{
    public class AppStoreService : IAppStoreService
    {
        public async Task DownloadAppAsync(Smartphone smartphone, string nomeApp, IStorageService storageService)
        {
            Console.WriteLine($"\n[AppStore] Iniciando download de {nomeApp}...");

            await Task.Delay(2500);

            var appParaInstalar = new App(
                nome: nomeApp,
                versao: "1.0.0",
                tamanho:
                    (nomeApp == "Jogo Pesado") ? 150 : 30
            );

            Console.WriteLine($"[AppStore] Download de {appParaInstalar.Nome} ({appParaInstalar.TamanhoEmMb} MB) concluído.");

            await smartphone.InstalarAplicativoAsync(appParaInstalar, storageService);
        }
    }
}
