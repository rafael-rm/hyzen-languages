
using System.ComponentModel.DataAnnotations;

namespace HyzenLanguages.SDK;

public class Settings
{
    [Required]
    public string LocalesPath { get; set; }
    
    [Required]
    public string DefaultLanguage { get; set; }

    public bool PreLoadAllLocales { get; set; }
    public bool AutoDetectLanguage { get; set; }
    public bool DebugMode { get; set; }
}