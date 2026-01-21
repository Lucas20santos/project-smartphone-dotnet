using SmartphoneCore.Models;
using SmartphoneCore.Services;

namespace SmartphoneCore.Models
{
    public abstract class Smartphone
    {
        public string Numero { get; protected set; }
        public string Modelo { get; protected set; }
        public
        string IMEI
        { get; protected set; }
        public int Memoria { get; protected set; }
        private readonly List<App> _aplicativosInstalados;
        public IReadOnlyList<App> AplicativosInstalados => _aplicativosInstalados;
        public Smartphone(string numero, string modelo, string imei, int memoria)
        {
            string textException = "Por favor não passar um argumento nulo ou vazio.";
            string textExceptionNumber = "Por favor não passar um valor fora do intervalo";

            if (string.IsNullOrWhiteSpace(numero)) throw new ArgumentException(textException);
            if (string.IsNullOrWhiteSpace(modelo)) throw new ArgumentException(textException);
            if (string.IsNullOrWhiteSpace(imei)) throw new ArgumentException(textException);
            if (memoria <= 0) throw new ArgumentOutOfRangeException(textExceptionNumber);

            _aplicativosInstalados = new List<App>();
            Numero = numero;
            Modelo = modelo;
            IMEI = imei;
            Memoria = memoria;
        }
        public void Ligar()
        {
            Console.WriteLine($"{Modelo} está ligando.");
        }
        public void ReceberLigacao(string numero)
        {
            Console.WriteLine($"O {Modelo} está recebendo ligação do número {numero}.");
        }
        public void ListarAplicativos()
        {
            if (_aplicativosInstalados.Count == 0)
            {
                Console.WriteLine("Nenhum app instalado");
                return;
            }
            foreach (var app in _aplicativosInstalados)
            {
                Console.WriteLine($"{app.Nome} - {app.Versao} - {app.TamanhoEmMb}");
            }
        }
        protected virtual bool InstalarApp(App app, IStorageService storageService)
        {
            if (storageService.ChecarEspaco(this, app.TamanhoEmMb))
            {
                _aplicativosInstalados.Add(app);
                Console.WriteLine($"✅ {Modelo}: {app.Nome} instalado com sucesso.");
                return true;
            }
            else
            {
                Console.WriteLine($"🛑 {Modelo}: Falha ao instalar {app.Nome}. Memória insuficiente.");
                return false;
            }
        }
        public abstract Task InstalarAplicativoAsync(App app, IStorageService storageService);
    }
}