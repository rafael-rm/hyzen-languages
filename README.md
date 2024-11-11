# HyzenLang SDK

HyzenLang é uma biblioteca de tradução leve para .NET 8, desenvolvida para facilitar o gerenciamento de diferentes idiomas em aplicações. A biblioteca carrega arquivos de tradução (JSON) e permite alternância de idiomas, substituição de variáveis e mais.

## Recursos

- **Carregamento de idiomas**: Carrega automaticamente arquivos JSON de tradução.
- **Detecção de idioma automática**: Identifica o idioma do sistema e tenta usá-lo se estiver disponível.
- **Fallback de idioma**: Se uma tradução não for encontrada, a biblioteca utiliza o idioma padrão.
- **Suporte a placeholders**: Permite a substituição de variáveis na tradução.
- **Modo de depuração**: Exibe mensagens detalhadas sobre o carregamento e uso dos idiomas.

## Instalação

Adicione o pacote via NuGet:

```bash
dotnet add package HyzenLang
```

## Configuração Básica

1. Crie os arquivos JSON de tradução e os armazene em uma pasta.
2. Adicione as chaves e traduções em cada arquivo JSON, usando o código do idioma como nome do arquivo (ex.: `en.json`, `pt-BR.json`).
3. Inicialize a biblioteca no seu código:

```cs
using HyzenLanguages.SDK;

var settings = new Settings
{
    LocalesPath = "caminho/para/locales",
    DefaultLanguage = "en",
    PreLoadAllLocales = true,
    AutoDetectLanguage = true,
    DebugMode = true
};

HyzenLang.Initialize(settings);
```


## Uso

Para buscar uma tradução:

```cs
string texto1 = HyzenLang.Get().GetText("chave_da_mensagem");
string texto2 = HyzenLang.Get().GetText("chave_da_mensagem", new { Nome = "Usuário" });
string texto3 = HyzenLang.Get().GetText("chave_da_mensagem", new { Nome = "Rafael", Idade = 21, Username = "rafael-rm" }));
```


### Alternar Idioma

Para mudar o idioma ativo:

```cs
HyzenLang.Get().ChangeLanguage("pt-BR");
```


## Estrutura do Arquivo JSON

Cada arquivo de tradução JSON deve seguir a estrutura de chave-valor, onde a chave é o identificador único da mensagem e o valor é a mensagem traduzida.

Exemplo (`en.json`):

```json
{
    "welcome_message": "Welcome, {{Nome}}!",
    "farewell_message": "Goodbye!"
}
```


## Configurações

As configurações do SDK são passadas pela classe `Settings`, com as seguintes propriedades:

- **LocalesPath**: Caminho da pasta contendo os arquivos de tradução.
- **DefaultLanguage**: Idioma padrão a ser utilizado. Este também é o idioma de fallback, ou seja, se uma tradução não for encontrada no idioma ativo, a biblioteca usará o idioma padrão.
- **PreLoadAllLocales**: Se `true`, todos os idiomas serão carregados na inicialização.
- **AutoDetectLanguage**: Se `true`, a biblioteca detectará automaticamente o idioma do sistema.
- **DebugMode**: Habilita mensagens detalhadas para ajudar na depuração.

## Contribuição

Sinta-se à vontade para contribuir com sugestões, melhorias e correções!
