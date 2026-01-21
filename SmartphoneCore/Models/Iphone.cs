using SmartphoneCore.Models;
using SmartphoneCore.Services;

namespace SmartphoneCore.Models
{
    public class Iphone : Smartphone
    {
        public Iphone(string numero, string modelo, string imei, int memoria) : base(numero, modelo, imei, memoria) { }
        public override Task InstalarAplicativoAsync(App app, IStorageService storageService)
        {
            Console.WriteLine($"\n[Iphone Store] Iniciando processo de download de {app.Nome}...");

            base.InstalarApp(app, storageService);

            return Task.CompletedTask;
        }
    }
}