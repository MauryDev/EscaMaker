# EscaMaker

EscaMaker é uma aplicação Blazor WebAssembly projetada para simplificar a criação e o gerenciamento de escalas de serviço. A ferramenta permite aos usuários gerar, salvar, carregar, importar e exportar escalas de forma eficiente, além de oferecer funcionalidades para pré-visualização em PDF e consulta de escalas por pessoa ou período.

## Visão Geral do Projeto

O EscaMaker é construído com Blazor WebAssembly, utilizando MudBlazor para a interface do usuário, Blazored.LocalStorage para persistência de dados localmente e iText para geração de documentos PDF.

### Principais Funcionalidades:

*   **Geração de Escalas**: Crie escalas baseadas no ano e mês selecionados, com suporte para gerar escalas para o mês atual.
*   **Gerenciamento Local**: Salve e carregue dados da escala diretamente no armazenamento local do navegador.
*   **Importação/Exportação JSON**: Importe e exporte dados de escala em formato JSON para fácil compartilhamento e backup.
*   **Visualização de PDF**: Gere e pré-visualize as escalas em formato PDF, com a opção de download.
*   **Consulta Avançada**: Visualize escalas por pessoa específica ou por um período selecionado.
*   **Limpar Escalas**: Opção para limpar todos os dados de escala gerados na interface atual.

## Estrutura do Projeto

*   **`EscaMaker`**: O projeto principal Blazor WebAssembly que contém a lógica da UI, serviços e utilitários.
    *   `Pages`: Componentes Razor que representam as páginas da aplicação, como `EscalaGenerator.razor` (a página principal de criação de escalas) e `Home.razor`.
    *   `Pages/Components`: Componentes Razor reutilizáveis (e.g., `EscalaTipo`, `PreviewGP`, `EscalaPeriodoSelect`).
    *   `Services`: Classes que encapsulam a lógica de negócios, como `PDFEscala` para manipulação de PDF e `JsonExport` para operações JSON.
    *   `Utils`: Classes utilitárias, incluindo `DateTimeUtil` para manipulação de datas e `GeneratePDF` para criação de documentos PDF.
    *   `View`: Classes de modelo de dados para visualização, como `EscalaTipoView` e `EscalaInfoPDF`.
*   **`EscaMakerTestProj`**: Projeto de testes de unidade para validar a lógica de negócios e as funções utilitárias.

## Como Usar

1.  **Executar o Projeto**: Abra a solução no Visual Studio 2022+ e inicie o projeto `EscaMaker`.
2.  **Gerar Escalas**: Na página "__Criar Escala__" (`/escala-create`), selecione o ano e o mês e clique em "__Gerar Escala__" ou "__Gerar Mês Atual__".
3.  **Preencher as Escalas**: Insira os nomes das pessoas nas respectivas células da tabela de escalas.
4.  **Salvar/Carregar Localmente**: Use os botões "__Salvar Local__" e "__Carregar Local__" para persistir o estado da escala no seu navegador.
5.  **Exportar/Importar JSON**: Utilize "__Exportar Escalas__" e "__Importar Escalas__" para trabalhar com arquivos JSON.
6.  **Gerar PDF**: Clique em "__Download PDF__" para baixar a escala ou "__Pré-visualizar__" para visualizar no navegador.
7.  **Consultar Escalas**: Use "__Escalas por Pessoa__" ou "__Escalas por Período__" para análises específicas.
8.  **Limpar Escalas**: O botão "__Limpar Escalas__" removerá todos os nomes e redefinirá os campos de ano e mês para os valores padrão, exibindo a mensagem "Selecione um ano e mês para gerar a escala.".

## Testes

O projeto `EscaMakerTestProj` contém testes de unidade para garantir a correção das funcionalidades principais.

### `Test1.cs`

A classe `Test1` inclui métodos para testar as utilidades de manipulação de datas, especificamente focando na identificação dos dias de semana dentro de um determinado mês e ano.

*   **`FirstDayFromWeekDay(int mes, int ano, DayOfWeek dayOfWeek)`**: Esta função utilitária calcula o primeiro dia de ocorrência de um `DayOfWeek` específico em um dado mês e ano.
*   **`Days(int mes, int ano, DayOfWeek dayOfWeek)`**: Esta função retorna uma lista de todos os dias (como números do dia do mês) em que um determinado `DayOfWeek` ocorre dentro do mês e ano especificados.
*   **`TestMethod1()`**: Este é um teste de unidade que verifica a correção das funções `Days` e `FirstDayFromWeekDay`. Ele usa o mês de agosto de 2025 como exemplo e espera conjuntos específicos de dias para sábados, domingos e quartas-feiras. O teste compara as listas geradas com listas de valores esperados para garantir que os cálculos estejam corretos.

    Exemplo: Para Agosto de 2025, `TestMethod1` verifica se:
    *   Os sábados são: 2, 9, 16, 23, 30.
    *   Os domingos são: 3, 10, 17, 24, 31.
    *   As quartas-feiras são: 6, 13, 20, 27.

Estes testes garantem que a lógica subjacente para a determinação dos dias de serviço na escala está funcionando conforme o esperado, o que é crucial para a integridade do agendamento de escalas.