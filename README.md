# EscaMaker

EscaMaker é uma aplicação Blazor WebAssembly robusta projetada para otimizar a criação e o gerenciamento de escalas de serviço. A ferramenta oferece aos usuários funcionalidades avançadas para geração, persistência, importação e exportação de escalas, além de recursos para pré-visualização em PDF e consultas detalhadas por indivíduo ou período.

## Visão Geral do Projeto

O EscaMaker é desenvolvido sobre o framework Blazor WebAssembly no ecossistema .NET 10 (ou superior, C# 14), garantindo uma experiência de usuário rica e responsiva diretamente no navegador. Utiliza a biblioteca MudBlazor para uma interface de usuário moderna e acessível, Blazored.LocalStorage para persistência de dados no lado do cliente e aprimora a gestão de dados com uma integração API para autenticação e persistência remota utilizando iText para geração programática de documentos PDF.

### Principais Funcionalidades:

*   **Autenticação e Persistência Remota**: Sistema de autenticação de usuário e capacidade de salvar/carregar dados de escalas de forma persistente em um backend através de uma API RESTful.
*   **Geração Dinâmica de Escalas**: Crie escalas flexíveis baseadas no ano e mês selecionados, com a opção de gerar rapidamente a escala para o mês atual.
*   **Gerenciamento de Dados Local**: Salve e recupere dados da escala de serviço diretamente do armazenamento local do navegador utilizando `Blazored.LocalStorage`.
*   **Interoperabilidade JSON**: Importe e exporte dados de escala em formato JSON, facilitando o compartilhamento e backup entre sistemas.
*   **Geração e Visualização de PDF**: Capacidade de gerar e pré-visualizar escalas completas ou individuais em formato PDF, com suporte para download.
*   **Consulta Avançada de Escalas**: Filtre e visualize escalas por pessoa específica ou por um período definido, otimizando a análise e o acompanhamento.
*   **Reinicialização de Escalas**: Funcionalidade para limpar todos os dados de escala atualmente carregados na interface, redefinindo os campos de entrada.

## Estrutura do Projeto

A solução é organizada em projetos distintos para promover a separação de responsabilidades e facilitar a manutenção:

*   **`EscaMaker`**: O projeto principal Blazor WebAssembly, contendo a lógica da interface do usuário (UI), serviços do lado do cliente e utilitários.
    *   `Layout`: Componentes Razor que definem a estrutura da aplicação para uma experiência de usuário consistente, como `MainLayout.razor` e `NavMenuMain.razor` (que agora inclui um link para a página de __Login__).
    *   `Pages`: Componentes Razor de nível superior que representam as principais rotas da aplicação (ex: `Home.razor`, `EscalaGenerator.razor` para criação de escalas, e `Login.razor` para autenticação).
    *   `Pages/Components`: Componentes Razor reutilizáveis, encapsulando funcionalidades específicas da UI para promover a modularidade (ex: `EscalaTipo`, `PreviewGP` para a visualização detalhada de escalas por pessoa, `EscalaPeriodoSelect`).
    *   `Services`: Implementações de lógica de negócios e abstrações para interações externas (ex: `PDFEscala` para geração de PDF da escala completa, `EscalaPessoaPdf` para PDFs de escalas individuais, `JsonExport` para manipulação de JSON, e `APIService` para interações com o backend).
    *   `Utils`: Classes utilitárias diversas para operações comuns (ex: `DateTimeUtil` para manipulação de datas, `GeneratePDF` para processos de criação de documentos PDF).
    *   `View`: Classes de modelo de dados (DTOs - Data Transfer Objects) para a representação de dados na UI e para a comunicação com a API (ex: `EscalaTipoView`, `EscalaInfoPDF`, e classes relacionadas a DTOs de administração de API como `Login`, `LoginResponse`, `EscaMakerInfo`, `ApiResponse`, `DeleteResult`, `GetAllNamesResponseDTO`).
*   **`EscaMakerTestProj`**: Projeto de testes de unidade dedicado à validação da lógica de negócios e das funções utilitárias da aplicação.

## Como Usar

1.  **Configuração e Execução**: Abra a solução no Visual Studio 2022+ e inicie o projeto `EscaMaker`. A aplicação será executada no navegador.
2.  **Autenticação (Opcional)**: Navegue até a página __Login__ (`/login`) para autenticar-se e acessar funcionalidades de persistência remota.
3.  **Gerar Escalas**: Acesse a página "__Criar Escala__" (`/escala-create`), selecione o ano e o mês desejados e clique em "__Gerar Escala__" ou "__Gerar Mês Atual__" para inicializar a tabela de escala.
4.  **Preenchimento da Escala**: Insira os nomes das pessoas nas células correspondentes da tabela de escala, atribuindo as funções aos dias.
5.  **Persistência de Dados**:
    *   **Localmente**: Utilize os botões "__Salvar Local__" e "__Carregar Local__" para persistir o estado da escala no armazenamento local do navegador.
    *   **Remotamente (API)**: Após o login, utilize as opções para salvar ou carregar escalas do serviço de backend.
6.  **Importação/Exportação JSON**: Use as funcionalidades "__Exportar Escalas__" e "__Importar Escalas__" para manipular dados de escala em formato JSON.
7.  **Geração e Visualização PDF**: Clique em "__Download PDF__" para baixar a escala completa ou utilize a opção "__Pré-visualizar__" para visualizar a escala no navegador. Para escalas individuais, a pré-visualização e download em PDF são acessíveis através do componente `PreviewGP`.
8.  **Consultas Específicas**: Em "Escalas por Pessoa" ou "Escalas por Período", realize consultas detalhadas para análise.
9.  **Limpar Escalas**: O botão "__Limpar Escalas__" remove todos os dados da escala da interface, redefinindo os campos de ano e mês para os valores padrão e exibindo uma mensagem de instrução.

## Testes

O projeto `EscaMakerTestProj` é fundamental para garantir a integridade e correção da aplicação, focando em testes de unidade.

### `Test1.cs`

A classe `Test1` contém métodos para verificar a precisão das utilidades de manipulação de datas em `DateTimeUtil`, essenciais para a lógica de agendamento de escalas.

*   **`FirstDayFromWeekDay(int mes, int ano, DayOfWeek dayOfWeek)`**: Esta função utilitária determina o primeiro dia do mês (como número do dia) em que um específico `DayOfWeek` ocorre.
*   **`Days(int mes, int ano, DayOfWeek dayOfWeek)`**: Retorna uma lista (`IEnumerable<byte>`) de todos os dias do mês (números do dia) que correspondem a um determinado `DayOfWeek`.
*   **`TestMethod1()`**: Este método de teste de unidade abrangente valida a exatidão das funções `Days` e `FirstDayFromWeekDay`. Ele emprega agosto de 2025 como caso de teste e verifica a correspondência dos dias calculados para sábados, domingos e quartas-feiras com conjuntos de valores esperados.

    Exemplo de Asserções `TestMethod1` para Agosto de 2025:
    *   **Sábados esperados**: 2, 9, 16, 23, 30.
    *   **Domingos esperados**: 3, 10, 17, 24, 31.
    *   **Quartas-feiras esperadas**: 6, 13, 20, 27.

Estes testes garantem que a lógica subjacente para a determinação dos dias de serviço na escala está funcionando conforme o esperado, o que é crucial para todo o sistema de agendamento de escalas.