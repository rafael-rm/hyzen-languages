
using HyzenLanguages.SDK;

namespace HyzenLanguages.Console;

internal abstract class Program
{
    static async Task Main()
    {
        HyzenLang.Initialize(new Settings
        {
            LocalesPath = "D:\\Repositories\\Hyzen\\hyzen-languages\\HyzenLanguages.Console\\Locales",
            DefaultLanguage = "pt-BR",
            PreLoadAllLocales = true,
            AutoDetectLanguage = true,
            DebugMode = true
        });
        
        System.Console.WriteLine(HyzenLang.Get().GetText("Welcome", new { name = "Rafael" }));
        System.Console.WriteLine(HyzenLang.Get().GetText("Welcome", new { displayName = "Rafael" }));
        
        
        if (HyzenLang.Get().ChangeLanguage("en-US") == false)
        {
            throw new Exception("Failed to change language.");
        }
        
        
        System.Console.WriteLine(HyzenLang.Get().GetText("Welcome", new { name = "Rafael", age = 25 }));
        System.Console.WriteLine(HyzenLang.Get().GetText("Test", new { name = "Rafael", age = 25, displayName = "Rafael" }));

        System.Console.Clear();
        
        while (true)
        {
            await Task.Delay(1000);
        }
    }
}