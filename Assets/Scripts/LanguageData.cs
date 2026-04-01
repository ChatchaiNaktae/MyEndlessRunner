using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// What: Holds translated strings for a specific language.
[CreateAssetMenu(fileName = "NewLanguage", menuName = "EndlessRunner/LanguageData")]
public class LanguageData : ScriptableObject
{
    public string languageName;
    
    [Header("Main Menu")]
    public string playBtn;
    public string shopBtn;
    public string settingsBtn;
    public string quitBtn;
    
    [Header("Settings")]
    public string musicLabel;
    public string sfxLabel;
    public string graphicsLabel;
    public string languageLabel;
}