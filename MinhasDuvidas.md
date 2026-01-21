# Minhas Duvidas

## Pergunta

Nessa parte aqui: "Adicione as referências necessárias (o App e os Testes precisam "enxergar" o Core)" os outros projetos não precisariam "enxergar" o Core também ? Se não, porque não ?

## Resposta

Essa é uma ótima pergunta e demonstra que você está pensando corretamente sobre a arquitetura do projeto e o fluxo de dependências!

Você está no caminho certo. Na verdade, a regra é: **todo projeto que utiliza classes definidas em outro projeto precisa ter uma referência a ele.**

No seu cenário, as referências são estabelecidas da seguinte forma:

1. **`SmartphoneApp` precisa "enxergar" `SmartphoneCore`:**
    * O `SmartphoneApp` (seu projeto de console) é onde você cria instâncias das classes (`Nokia`, `Iphone`, `AppStoreService`, etc.) que estão definidas dentro do `SmartphoneCore`.
    * **Portanto, a referência é obrigatória:** `SmartphoneApp` $\rightarrow$ `SmartphoneCore`.

2. **`SmartphoneCore.Tests` precisa "enxergar" `SmartphoneCore`:**
    * O projeto de testes precisa acessar as classes que ele está testando (como `Smartphone`, `Nokia`, `StorageService`, etc.). Todas elas estão no `SmartphoneCore`.
    * **Portanto, a referência é obrigatória:** `SmartphoneCore.Tests` $\rightarrow$ `SmartphoneCore`.

### Por que o `SmartphoneCore` não precisa enxergar os outros?

O projeto `SmartphoneCore` é a **biblioteca de classes** (DLL) que contém a lógica de negócios, as entidades (Models) e os serviços. Ele é o coração do seu sistema e deve ser o mais independente possível.

* O `SmartphoneCore` **não precisa** de nenhuma referência para `SmartphoneApp` ou `SmartphoneCore.Tests`.
  * Ele não sabe nem precisa saber como a lógica será executada (`SmartphoneApp`).
  * Ele não sabe nem precisa saber como a lógica será testada (`SmartphoneCore.Tests`).

### Por que os outros não precisam se referenciar?

* O **`SmartphoneApp`** não precisa de referência para o **`SmartphoneCore.Tests`**, pois a aplicação de demonstração não se preocupa com os testes.
* O **`SmartphoneCore.Tests`** não precisa de referência para o **`SmartphoneApp`**, pois os testes unitários focam na lógica do Core.

### Resumo das Dependências (a seta indica "depende de")

| Projeto | Depende de | Razão |
| :--- | :--- | :--- |
| **`SmartphoneCore`** | **(Nenhum)** | É o domínio. Deve ser independente. |
| **`SmartphoneApp`** | `SmartphoneCore` | Precisa instanciar as classes de domínio (`Nokia`, `AppStore`, etc.). |
| **`SmartphoneCore.Tests`** | `SmartphoneCore` | Precisa acessar as classes que estão sendo testadas. |

Sua dúvida é muito perspicaz! As referências que sugeri (`dotnet add`) são justamente aquelas necessárias para que a lógica central (`Core`) seja utilizada (e testada) pelos projetos externos.

## Pergunta 2

Na criação das pastas do projeto dentro de SmartphoneCore para que serve cada uma delas ? Models, Services, Persistence e Utils.

## Resposta 2

Essa é uma ótima pergunta sobre a organização de projetos em .NET, o que é fundamental para a manutenção e escalabilidade do seu código!

No contexto do seu projeto **`SmartphoneCore`** (que é o núcleo da lógica de negócios), a divisão em pastas como `Models`, `Services`, `Persistence` e `Utils` segue o padrão de **Separação de Preocupações (Separation of Concerns)**, um princípio básico de engenharia de software.

Aqui está para que serve cada uma delas no seu projeto:

### 1. 📦 Models (Modelos/Entidades)

**Propósito:** Contém as classes que representam as **entidades de domínio** (a "matéria" do seu sistema). Elas definem o que o sistema é.

* **No seu projeto:**
  * `Smartphone.cs` (a classe abstrata).
  * `Nokia.cs` e `Iphone.cs` (as implementações concretas).
  * `App.cs` (o aplicativo que será instalado).
* **Responsabilidade:** Manter os dados, as propriedades e as validações intrínsecas à própria entidade (ex: A memória deve ser positiva, o IMEI não pode ser nulo).

### 2. ⚙️ Services (Serviços/Lógica de Negócios)

**Propósito:** Contém as classes (e suas interfaces) que encapsulam a **lógica de negócios** e as operações. Elas definem o que o sistema **faz**.

* **No seu projeto:**
  * `IStorageService.cs` e `StorageService.cs` (Lógica para checar se há espaço na memória).
  * `IAppStoreService.cs` e `AppStoreService.cs` (Lógica para simular o download de um aplicativo e orquestrar a instalação).
* **Responsabilidade:** Receber requisições e coordenar a execução de tarefas complexas, aplicando as regras de negócio. Essa separação permite que você substitua a implementação de um serviço (trocar um `StorageService` simples por um mais complexo) sem alterar o código do `Smartphone` (Princípio da Injeção de Dependência).

### 3. 💾 Persistence (Persistência de Dados)

**Propósito:** Contém as classes (e suas interfaces) responsáveis pela **leitura e gravação de dados** em alguma fonte (banco de dados, arquivos JSON/XML, etc.).

* **No seu projeto:**
  * `IStatePersistence.cs` (interface para o contrato de salvar/carregar).
  * `JsonPersistence.cs` (a implementação que salva o estado do `Smartphone` em um arquivo JSON).
* **Responsabilidade:** Isolar a forma como os dados são armazenados e recuperados do restante do sistema. O `Smartphone` ou o `AppStoreService` não precisam saber *como* o JSON é escrito, apenas que existe um serviço de persistência que faz isso.

### 4. 🧹 Utils (Utilitários)

**Propósito:** Contém pequenas classes ou métodos estáticos que oferecem funcionalidades de suporte e são **reutilizáveis** em várias partes do projeto, mas que não se encaixam na lógica de negócios central.

* **No seu projeto:**
  * `ValidationHelpers.cs` (Métodos estáticos auxiliares para validações comuns, como checar se uma *string* é nula, vazia ou apenas espaços em branco).
  * `LoggerSimple.cs` (Uma classe para logar mensagens no console ou em um arquivo, usada para diagnóstico).
* **Responsabilidade:** Fornecer ferramentas e código de suporte de forma limpa, evitando duplicação de código.

---

**Em resumo, a estrutura cria um fluxo lógico:**

$$\text{Models (O QUÊ)} \longleftarrow \text{Services (O QUE FAZ)} \longleftarrow \text{Persistence (COMO SALVA)} \longleftarrow \text{Utils (CÓDIGO DE APOIO)}$$

Essa organização ajuda você, como novo desenvolvedor .NET, a criar projetos limpos, organizados e fáceis de testar. Por exemplo, você só precisa testar o `StorageService` para ter certeza de que a regra de "espaço insuficiente" funciona, sem precisar rodar o sistema inteiro.

Sobre a pasta **`Events`** no seu projeto de Smartphone:

A afirmação **FALSA** sobre a pasta `Events` é:

> **"Ela deve conter a lógica para decidir se um aplicativo será instalado com sucesso ou não."**

**Explicação:**

1. **O que a pasta `Events` FAZ (Verdadeiro):**
    * Armazena classes de argumentos de evento (ex: `InstalacaoConcluidaEventArgs`).
    * Define delegados de evento (`EventHandler`).
    * **Propósito:** Oferecer um mecanismo de notificação para que outras partes do código (o seu `Program.cs`, por exemplo) saibam quando algo significativo aconteceu (ex: "A ligação começou", "A instalação terminou").

1. **O que a pasta `Events` NÃO FAZ (Falso):**

    * **Decisão Lógica:** A decisão sobre o sucesso ou falha da instalação (a checagem de espaço, o polimorfismo do método de instalação) é responsabilidade dos **`Services`** (como o `StorageService`) e das classes **`Models`** (`Nokia` / `Iphone`). A pasta `Events` apenas **notifica** o resultado dessa decisão.

Em resumo, a pasta `Events` lida com a **comunicação de estado**, não com a **execução da lógica de negócios** que altera esse estado.
