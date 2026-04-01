using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// What: Automatically updates TextMeshPro text based on the selected language in SettingsDatabase.
public class LocalizedText : MonoBehaviour
{
    public enum TextKey { Play, Shop, Settings, Quit, Music, SFX, Graphics, Language }
    public TextKey key;
    
    private TextMeshProUGUI textMesh;
    private SettingsMenu settingsMenu;
    
    void Awake() {
        textMesh = GetComponent<TextMeshProUGUI>();
        settingsMenu = FindObjectOfType<SettingsMenu>(true); // หาตัวคุม Settings
    }
    
    void OnEnable() { OnLanguageChanged(); }
    
    // What: Updates the text string when a language change event is received.
    public void OnLanguageChanged() {
        if (settingsMenu == null || settingsMenu.settingsDatabase == null) return;
        
        int langIndex = (int)settingsMenu.settingsDatabase.GetLanguage();
        LanguageData data = settingsMenu.languages[langIndex];
        
        // เลือกคำแปลตาม Key ที่เราตั้งไว้ใน Inspector
        textMesh.text = key switch {
            TextKey.Play => data.playBtn,
            TextKey.Shop => data.shopBtn,
            TextKey.Settings => data.settingsBtn,
            TextKey.Quit => data.quitBtn,
            TextKey.Music => data.musicLabel,
            TextKey.SFX => data.sfxLabel,
            TextKey.Graphics => data.graphicsLabel,
            TextKey.Language => data.languageLabel,
            _ => textMesh.text
        };
    }
}