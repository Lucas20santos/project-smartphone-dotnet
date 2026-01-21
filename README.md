# 📱 Sistema de Gerenciamento e Instalação de Aplicativos Mobile (POO Avançado)

## 🤖 Construído com Assistência de IA (Gemini)

Este projeto foi concebido, estruturado e desenvolvido passo a passo com a ajuda da IA Gemini. A ferramenta atuou como um mentor técnico, fornecendo as sugestões arquiteturais, validando o código, corrigindo erros de compilação, e garantindo a aplicação correta dos padrões de C\# e Programação Orientada a Objetos.

O *log* de conversas com a IA é o principal registro da metodologia de construção e das decisões de design tomadas.

-----

## 🎯 Objetivo do Projeto

O objetivo principal deste projeto é consolidar e demonstrar o domínio dos pilares avançados da **Programação Orientada a Objetos (POO)** e conceitos de design de software em C\#/.NET, simulando um ambiente simplificado de gerenciamento de *smartphones* e instalação de aplicativos.

## 🧱 Conceitos de POO Aplicados

A arquitetura do projeto foi desenhada para aplicar os seguintes princípios:

| Conceito | Aplicação no Projeto |
| :--- | :--- |
| **Herança e Polimorfismo** | A classe base `Smartphone` (abstrata) define o contrato. As classes `Nokia` e `Iphone` herdam e implementam de forma polimórfica o método `InstalarAplicativoAsync()`, adicionando seu comportamento específico. |
| **Encapsulamento** | A lista de aplicativos instalados é mantida como um campo `private readonly List<App>` e é exposta ao mundo externo apenas através da interface de leitura **`IReadOnlyList<App>`**, protegendo o estado interno do objeto. |
| **Abstração (Interfaces)** | O projeto utiliza interfaces (`IStorageService`, `IAppStoreService`) para desacoplar a definição do contrato de sua implementação, permitindo a **Injeção de Dependência (DI)**. |
| **Assincronismo** | Utilização de `Task`, `async` e `await` no `AppStoreService` e nos métodos de instalação para simular operações demoradas (como *download*) sem bloquear a *thread* principal. |
| **Separação de Preocupações** | A lógica é dividida em *namespaces* e pastas claras: `Models` (Entidades), `Services` (Lógica de Negócio e DI), e `Core` (Domínio principal). |

-----

## 📁 Estrutura do Projeto Core

O projeto principal (`SmartphoneCore`) segue a seguinte organização lógica, conforme sugerido e validado pela IA:

```cs
SmartphoneCore/
├── Models/
│   ├── App.cs           <- Entidade de Dados (Com validação básica no construtor).
│   ├── Smartphone.cs    <- Classe Abstrata (Lógica base de instalação e validações).
│   ├── Nokia.cs         <- Implementação (Comportamento polimórfico).
│   └── Iphone.cs        <- Implementação (Comportamento polimórfico).
├── Services/
│   ├── IStorageService.cs     <- Interface para checagem de espaço.
│   ├── StorageService.cs      <- Implementação da lógica de checagem.
│   ├── IAppStoreService.cs    <- Interface para orquestração de download/instalação.
│   └── AppStoreService.cs     <- Implementação com simulação de 'await Task.Delay'.
└── SmartphoneCore.csproj
```

### Decisões Cruciais de Design (Diálogo com a IA)

| Tópico Discutido | Resultado / Decisão de Design |
| :--- | :--- |
| **Dependências** | Definiu-se que `App` e `Tests` precisavam "enxergar" o `Core`, mas o `Core` não depende de ninguém, garantindo a natureza de **biblioteca de domínio** independente. |
| **Estrutura de Pastas** | Foi estabelecida a separação clara entre `Models`, `Services`, `Persistence` e `Utils` para seguir o princípio de Separação de Preocupações. |
| **Encapsulamento** | A IA validou o uso do par `private readonly List<App>` e `public IReadOnlyList<App>` para garantir que a lista de apps seja somente leitura externamente. |
| **Centralização da Lógica** | O método `protected virtual InstalarApp()` foi criado na classe `Smartphone` para centralizar a lógica de `ChecarEspaco` e `_aplicativosInstalados.Add()` na classe base, permitindo que as subclasses apenas adicionassem mensagens polimórficas (Reuso de Código). |
| **Assinatura Assíncrona** | O tipo de retorno `Task` foi introduzido para preparar o projeto para o assincronismo, simulando operações reais de I/O em métodos como `InstalarAplicativoAsync`. |

-----

## 🚀 Como Rodar o Projeto

### Pré-requisitos

* [.NET SDK 8.0](https://dotnet.microsoft.com/download) ou superior.
* Editor de Código (VS Code, Visual Studio, etc.).

### Execução

1. **Clone o Repositório:**

```bash
    git clone [SEU_LINK_DO_REPOSITORIO]
    cd NomeDoDiretorioDoProjeto/SmartphoneProject
```

1. **Execute o Projeto de Demonstração:**

Rode o projeto `SmartphoneApp`, que contém a lógica de demonstração no `Program.cs`.

```bash
    dotnet run --project SmartphoneApp
```

### Exemplo de Saída Esperada

A saída deverá demonstrar o polimorfismo, o assincronismo (com o delay de 2.5s) e a checagem de espaço:

```cs
    --- Cenário 1: Nokia (Instalação de Sucesso) ---
    Nokia Tijolão está ligando.

    [AppStore] Iniciando download de WhatsApp...
    // ... (Aguarde 2.5 segundos) ...
    [AppStore] Download de WhatsApp (30 MB) concluído.

    [Nokia Store] Iniciando processo de download de WhatsApp...
    ✅ Nokia Tijolão: WhatsApp instalado com sucesso.

    --- Aplicativos Instalados (Nokia Tijolão) ---
    - WhatsApp - 1.0.0 - 30 MB

    --- Cenário 2: iPhone (Memória Insuficiente) ---
    O iPhone 4S está recebendo ligação do número 119999-0000.

    [AppStore] Iniciando download de Jogo Pesado...
    // ... (Aguarde 2.5 segundos) ...
    [AppStore] Download de Jogo Pesado (150 MB) concluído.

    [Iphone Store] Iniciando processo de download de Jogo Pesado...
    🛑 iPhone 4S: Falha ao instalar Jogo Pesado. Memória insuficiente.

    --- Aplicativos Instalados (iPhone 4S) ---
    Nenhum app instalado.
```

-----

## 🛠️ Tecnologias

* **Linguagem:** C\#
* **Plataforma:** .NET 8.0
* **Metodologia:** POO Avançado (Abstração, DI, Assincronismo)
* **Assistência:** Gemini (Google)

-----

Feito com 💙 por [Seu Nome/GitHub User]
