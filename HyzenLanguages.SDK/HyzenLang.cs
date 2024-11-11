using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace HyzenLanguages.SDK;

public partial class HyzenLang
{
    private static Lazy<HyzenLang> Instance { get; set; } = new(() => new HyzenLang());
    private static bool IsInitialized { get; set; }
    private string LocalesPath { get; set; }
    private string DefaultLanguage { get; set; }
    private string CurrentLanguage { get; set; }
    private bool DebugMode { get; set; }
    private Dictionary<string, ImmutableDictionary<string, string>> LoadedLocales { get; set; } = [];
    
    [GeneratedRegex(@"\{\{[^\}]+\}\}")]
    private static partial Regex PlaceholderRegex();

    private HyzenLang() { }

    public static HyzenLang Get()
    {
        if (!IsInitialized)
        {
            throw new Exception("HyzenLanguage is not initialized, call HyzenLanguage.Initialize() first.");
        }

        return Instance.Value;
    }
    
    public static void Initialize(Settings settings)
    {
        if (IsInitialized)
        {
            throw new Exception("HyzenLanguage is already initialized.");
        }
        
        Instance.Value.SetSettings(settings);
        IsInitialized = true;
    }
    
    private void SetSettings(Settings settings)
    {
        DebugMode = settings.DebugMode;
        LocalesPath = settings.LocalesPath;
        
        if (!Directory.Exists(LocalesPath))
        {
            throw new DirectoryNotFoundException($"Locales path '{LocalesPath}' not found.");
        }
        
        if (!LocaleExists(settings.DefaultLanguage))
        {
            throw new FileNotFoundException($"Default locale '{settings.DefaultLanguage}' not found.");
        }
        
        DefaultLanguage = settings.DefaultLanguage;
        CurrentLanguage = settings.DefaultLanguage;

        if (settings.PreLoadAllLocales)
        {
            LoadAllLocales();
        }
        
        if (settings.AutoDetectLanguage)
        {
            AutoDetectLanguage();
        }
    }
    
    private void AutoDetectLanguage()
    {
        var detectedLanguage = CultureInfo.CurrentCulture.Name;
    
        Debug.WriteLine($"[HyzenLanguage] Auto detected language '{detectedLanguage}'.");

        if (LocaleExists(detectedLanguage))
        {
            CurrentLanguage = detectedLanguage;
        }
        else
        {
            Debug.WriteLine($"[HyzenLanguage] Locale '{detectedLanguage}' not found, falling back to default language.");
        }
    }

    
    private bool LocaleExists(string locale)
    {
        return File.Exists(Path.Combine(LocalesPath, $"{locale}.json"));
    }
    
    private void LoadLocale(string locale)
    {
        if (LoadedLocales.ContainsKey(locale))
        {
            return;
        }
        
        if (!LocaleExists(locale))
        {
            throw new FileNotFoundException($"Locale '{locale}' not found.");
        }
        
        var localeContent = File.ReadAllText(Path.Combine(LocalesPath, $"{locale}.json"));
        LoadedLocales.Add(locale, JsonConvert.DeserializeObject<ImmutableDictionary<string, string>>(localeContent));
        
        DebugMessage($"Loaded locale '{locale}' with {LoadedLocales[locale].Count} keys.");
    }
    
    private void UnloadLocale(string locale)
    {
        if (!LoadedLocales.ContainsKey(locale))
        {
            DebugMessage($"Locale '{locale}' not loaded.");
            return;
        }
        
        LoadedLocales.Remove(locale);
        DebugMessage($"Unloaded locale '{locale}'.");
    }
    
    private void LoadAllLocales()
    {
        var locales = Directory.GetFiles(LocalesPath, "*.json").ToList();
        
        foreach(var locale in locales)
        {
            LoadLocale(Path.GetFileNameWithoutExtension(locale));
        }
    }
    
    public string GetText(string key, object variables = null)
    {
        LoadLocale(CurrentLanguage);
        
        if (!LoadedLocales[CurrentLanguage].TryGetValue(key, out var text))
        {
            DebugMessage($"Key '{key}' not found in locale '{CurrentLanguage}', falling back key to default language '{DefaultLanguage}'.");
        }
        
        if (text == null)
        {
            LoadLocale(DefaultLanguage);
            
            if (!LoadedLocales[DefaultLanguage].TryGetValue(key, out text))
            {
                DebugMessage($"Key '{key}' not found in default language '{DefaultLanguage}'.");
                return string.Empty;
            }
        }
        
        if (variables != null)
        {
            foreach (var property in variables.GetType().GetProperties())
            {
                if (DebugMode && !text.Contains($"{{{{{property.Name}}}}}"))
                {
                    DebugMessage($"Key '{key}' has unused variable '{property.Name}' in locale '{CurrentLanguage}'.");
                }
                
                text = text.Replace($"{{{{{property.Name}}}}}", property.GetValue(variables)?.ToString());
            }
        }
        
        if (PlaceholderRegex().IsMatch(text))
        {
            DebugMessage($"Key '{key}' has missing variables in locale '{CurrentLanguage}'.");
        }
        
        return text;
    }
    
    public bool ChangeLanguage(string locale)
    {
        if (!LocaleExists(locale))
        {
            DebugMessage($"Locale '{locale}' not found.");
            return false;
        }
        
        LoadLocale(locale);
        CurrentLanguage = locale;
        DebugMessage($"Changed language to '{locale}'.");
        return true;
    }
    
    private void DebugMessage(string message)
    {
        if (!DebugMode)
            return;
        
        Console.WriteLine($"[HyzenLangDebug] {message}");
    }
}