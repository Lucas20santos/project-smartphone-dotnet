# Projeto Smartphone

-----

## 🚀 Guia de Desenvolvimento Passo a Passo (Roadmap Incremental)

### Etapa 1: Preparação do Ambiente e Estrutura Inicial (Foundation)

Antes de tudo, precisamos organizar o projeto seguindo a arquitetura sugerida para manter o código limpo e testável.

## Sua Tarefa (1.1): Criar a Solução e Projetos

1. Crie uma pasta principal para o projeto (ex: `SmartphoneProject`).
1. Dentro dela, crie a **Solution (.sln)** para agrupar todos os projetos:

```bash
    dotnet new sln -n SmartphoneSolution 
```

1. Crie os três projetos essenciais:
      * **SmartphoneCore:** Uma biblioteca de classes (`classlib`) para a lógica principal.
      * **SmartphoneApp:** Um projeto de console (`console`) para a demonstração e execução.
      * **SmartphoneCore.Tests:** Um projeto de testes (`xunit`).
    <!-- end list -->
    ```bash
    dotnet new classlib -n SmartphoneCore
    dotnet new console -n SmartphoneApp
    dotnet new xunit -n SmartphoneCore.Tests
    ```

1. Adicione os projetos à Solution:

```bash
    dotnet sln add SmartphoneCore/SmartphoneCore.csproj
    dotnet sln add SmartphoneApp/SmartphoneApp.csproj
    dotnet sln add SmartphoneCore.Tests/SmartphoneCore.Tests.csproj
```

1. Adicione as referências necessárias (o App e os Testes precisam "enxergar" o Core):

```bash
    dotnet add SmartphoneApp/SmartphoneApp.csproj reference SmartphoneCore/SmartphoneCore.csproj
    dotnet add SmartphoneCore.Tests/SmartphoneCore.Tests.csproj reference SmartphoneCore/SmartphoneCore.csproj
```

1. Crie a estrutura de pastas dentro de `SmartphoneCore` conforme o guia: `Models/`, `Services/`, `Persistence/`, `Events/`, `Utils/`.

**Resultado Esperado:** Um ambiente configurado com a separação de responsabilidades (lógica, execução, testes).

-----

### Etapa 2: MVP - Modelos Essenciais (O Domínio)

Vamos construir as classes de domínio que são o coração do projeto.

**Sua Tarefa (2.1): A Classe `App` (Models/App.cs)**

1. Crie a classe `App` em `Models/App.cs`.
2. Defina as propriedades: `Nome`, `Versao` (string), e `TamanhoEmMb` (int ou double).
3. Implemente um construtor para inicializar essas propriedades.

**Sua Tarefa (2.2): A Classe Abstrata `Smartphone` (Models/Smartphone.cs)**

Este é o ponto mais importante para a herança e polimorfismo.

1. Crie a classe `Smartphone` em `Models/Smartphone.cs` e declare-a como **`abstract`**.
2. Defina as propriedades: `Numero`, `Modelo`, `IMEI` (string) e `Memoria` (int, em MB). *Dica: Use `protected set` para propriedades que só devem ser alteradas no construtor ou na própria classe.*
3. Crie uma propriedade para a lista de aplicativos instalados. *Dica: Use `List<App>` internamente e exponha-a como `IReadOnlyList<App>` (Boas Práticas).*
4. Crie o **construtor** que recebe e inicializa todas as propriedades.
5. **Validações:** No construtor, implemente as regras:
      * `Numero` e `IMEI` não podem ser nulos ou vazios (`ArgumentException`).
      * `Memoria` deve ser um valor positivo (`ArgumentOutOfRangeException`).
6. Defina os métodos concretos:
      * `Ligar()`: Simplesmente imprime uma mensagem.
      * `ReceberLigacao(string numero)`: Simplesmente imprime uma mensagem com o número.
      * `ListarAplicativos()`: Imprime os nomes dos apps instalados.
7. Defina o método abstrato que será o foco do polimorfismo:
      * `public abstract Task InstalarAplicativoAsync(App app);` (Vamos usar `Task` agora para preparar para o assincronismo na Etapa 5).

**Resultado Esperado:** Um modelo base seguro e validado, que define o contrato para os smartphones concretos.

-----

### Etapa 3: Implementações Concretas (Polimorfismo em Ação)

Agora, vamos herdar a classe base e implementar a lógica específica de cada marca.

**Sua Tarefa (3.1): Implementar `Nokia` e `iPhone`**

1. Crie as classes `Nokia` e `Iphone` em `Models/Nokia.cs` e `Models/Iphone.cs`, respectivamente.
1. Ambas devem herdar de `Smartphone` (`: Smartphone`).
1. Crie o construtor nas subclasses e use a palavra-chave **`base`** para chamar o construtor da classe `Smartphone`.

```csharp
    public Nokia(string numero, string modelo, string imei, int memoria)
        : base(numero, modelo, imei, memoria)
    {
        // ... implementação específica, se houver
    }
```

1. Implemente o método **`InstalarAplicativoAsync(App app)`** em cada uma, usando a palavra-chave **`override`**.
1. **Comportamento Polimórfico:**
      * Em **`Nokia`**, o método deve imprimir: *"Instalando o aplicativo \[Nome do App] no Nokia..."*
      * Em **`Iphone`**, o método deve imprimir: *"Instalando o aplicativo \[Nome do App] na App Store do iPhone..."*
1. Por enquanto, apenas imprima a mensagem. A lógica de adição real à lista de apps será feita na próxima etapa.

**Resultado Esperado:** Duas classes concretas que herdam e implementam um comportamento obrigatório de forma distinta (Polimorfismo).

-----

### Etapa 4: Serviços de Suporte - Storage e AppStore (Injeção de Dependência e Assincronismo)

Para seguir as boas práticas e facilitar o teste, vamos criar Serviços que cuidam de funções específicas, desacoplando a lógica do Smartphone.

**Sua Tarefa (4.1): Criar o Serviço de Armazenamento (`StorageService`)**

A checagem de espaço é um requisito do projeto.

1. Crie a interface `Services/IStorageService.cs` com o seguinte contrato:
      * `bool ChecarEspaco(Smartphone smartphone, double tamanhoEmMb);`
2. Crie a implementação `Services/StorageService.cs` para essa interface.
3. O método `ChecarEspaco` deve comparar o espaço disponível com o tamanho do app. *Dica: Você precisará somar o tamanho dos apps já instalados e comparar com a `Memoria` total do smartphone.*

**Sua Tarefa (4.2): Refatorar a Instalação no `Smartphone`**

Agora que temos o `StorageService`, vamos usá-lo na classe `Smartphone`.

1. Retorne à classe **`Smartphone`**.
2. Altere o método abstrato `InstalarAplicativoAsync` para receber uma instância de um serviço (Princípio da Injeção de Dependência):

```csharp
    public abstract Task InstalarAplicativoAsync(App app, IStorageService storageService);
```

1. No construtor, adicione a lógica para inicializar a lista de aplicativos.
1. Implemente a lógica de instalação, de forma **reutilizável** em uma classe auxiliar (ou no próprio `Smartphone`):
      * 1. Chame `storageService.ChecarEspaco`.
      * 1. Se houver espaço, adicione o `App` à lista de instalados e imprima o sucesso.
      * 1. Se não houver espaço, imprima uma mensagem de erro.

**Sua Tarefa (4.3): Criar o Serviço de Loja de Aplicativos (`AppStoreService`)**

Vamos simular o download de um aplicativo, introduzindo `async/await`.

1. Crie a interface `Services/IAppStoreService.cs` com o método:
      * `Task DownloadAppAsync(Smartphone smartphone, string nomeApp, IStorageService storageService);`
1. Crie a implementação `Services/AppStoreService.cs`.
1. No método `DownloadAppAsync`:
      * 1. Simule a obtenção dos dados do App (crie uma instância de `App` com dados fixos, por exemplo).
      * 1. Use **`await Task.Delay(2000);`** para simular um tempo de download de 2 segundos.
      * 1. Chame o método `smartphone.InstalarAplicativoAsync(app, storageService)` e use **`await`** nessa chamada.

**Resultado Esperado:** O coração da lógica (instalação) está pronto, usa polimorfismo (`Nokia`/`iPhone`) e depende de um serviço externo (`IStorageService`) para verificar as regras. O `AppStoreService` simula o fluxo completo de download e instalação de forma assíncrona.

-----

### Etapa 5: Demonstração e Teste Inicial (Program.cs)

É hora de ver o trabalho em ação.

#### Sua Tarefa (5.1): Criar a Demonstração (SmartphoneApp/Program.cs)

1. No `Program.cs`, crie uma instância do `StorageService` e do `AppStoreService`.
1. Crie as instâncias de `Nokia` e `Iphone`. *Dica: Dê ao iPhone menos memória (ex: 64 MB) para testar a validação de espaço.*
1. Execute o cenário de sucesso (app pequeno, memória suficiente) usando o `AppStoreService`:

```csharp
    var storage = new StorageService();
    var appStore = new AppStoreService();
    var iphone = new Iphone("123", "X", "IMEI1", 64);
    Console.WriteLine("--- Teste iPhone (Sucesso) ---");
    await appStore.DownloadAppAsync(iphone, "Instagram", storage);
    iphone.ListarAplicativos();
```

1. Execute o cenário de falha (tentar instalar um app grande demais ou vários apps até encher a memória):

```csharp
    // Crie um AppStoreService que simule o download de um app de 100MB
    Console.WriteLine("\n--- Teste iPhone (Sem Espaço) ---");
    await appStore.DownloadAppAsync(iphone, "Jogo Pesado", storage);
    iphone.ListarAplicativos(); // Não deve aparecer o Jogo Pesado
```

### Sua Tarefa (5.2): Adicionar o primeiro Teste Unitário (SmartphoneCore.Tests)

1. No projeto `SmartphoneCore.Tests`, renomeie o arquivo de teste para algo como `SmartphoneTests.cs`.
2. Crie um teste para a regra de validação do construtor: **Se a memória for zero ou negativa, deve lançar uma exceção.**
      * *Dica: Use `Assert.Throws<ArgumentOutOfRangeException>(() => new Nokia(...));`.*
3. Crie um teste para o método `Ligar()`. *Dica: Você precisará capturar a saída do console usando técnicas de teste.*

**Resultado Esperado:** Uma aplicação de console funcional que demonstra o fluxo de instalação com sucesso e falha, e o primeiro teste unitário garantindo a integridade dos dados de entrada.

-----

### Próximos Passos (Avançado)

Após a Etapa 5, o seu MVP estará completo\! Você já terá implementado Herança, Polimorfismo, Encapsulamento, Validações, Injeção de Dependência e Assincronismo.

Seu próximo foco deve ser:

1. **Eventos (`Events/`)**: Implementar a notificação de eventos (`ReceberLigacao`, `InstalacaoConcluida`) usando `EventHandler` e disparar esses eventos nos métodos relevantes.
2. **Persistência (`Persistence/`)**: Implementar o `JsonPersistence` para salvar e carregar o estado do smartphone (incluindo a lista de apps) em um arquivo `.json` usando a biblioteca `System.Text.Json`.

**Continue o bom trabalho\! Qual das tarefas da Etapa 2 você gostaria de começar primeiro?**
