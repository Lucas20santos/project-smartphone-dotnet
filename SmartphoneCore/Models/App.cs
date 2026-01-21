using SmartphoneCore.Models;
using SmartphoneCore.Services;

namespace SmartphoneCore.Models
{
    public class App
    {
        public string Nome { get; }
        public string Versao { get; }
        public double TamanhoEmMb { get; } 
        public App(string nome, string versao, int tamanho)
        {
            if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome não pode ser vazio.");
            if (string.IsNullOrWhiteSpace(versao)) throw new ArgumentException("A versao do App não pode ser vazio.");
            if (tamanho <= 0) throw new ArgumentOutOfRangeException("O tamanho do aplicativo não pode ser negativo.");

            Nome = nome;
            Versao = versao;
            TamanhoEmMb = tamanho;
        }
    }
}