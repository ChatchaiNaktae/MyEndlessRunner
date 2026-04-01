using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// What: A database for game settings, handling defaults and PlayerPrefs saving automatically.
[CreateAssetMenu(fileName = "MainSettingsDatabase", menuName = "EndlessRunner/SettingsDatabase")]
public class SettingsDatabase : ScriptableObject
{
    public enum GraphicQuality { Basic, Normal, High }
    // เพิ่ม Enum ภาษาเข้ามาด้วยเลยครับ
    public enum GameLanguage { English, Thai, Japanese }
    
    [Header("Default Settings")]
    public float defaultMusicVolume = 1f;
    public float defaultSfxVolume = 1f;
    public GraphicQuality defaultQuality = GraphicQuality.Normal;
    public GameLanguage defaultLanguage = GameLanguage.English;
    
    // --- Audio ---
    public float GetMusicVolume() => PlayerPrefs.GetFloat("MusicVolume", defaultMusicVolume);
    public void SetMusicVolume(float v) { PlayerPrefs.SetFloat("MusicVolume", v); PlayerPrefs.Save(); }
    
    public float GetSFXVolume() => PlayerPrefs.GetFloat("SFXVolume", defaultSfxVolume);
    public void SetSFXVolume(float v) { PlayerPrefs.SetFloat("SFXVolume", v); PlayerPrefs.Save(); }
    
    // --- Graphics ---
    public GraphicQuality GetGraphicQuality() => (GraphicQuality)PlayerPrefs.GetInt("GraphicQuality", (int)defaultQuality);
    public void SetGraphicQuality(GraphicQuality q) { PlayerPrefs.SetInt("GraphicQuality", (int)q); PlayerPrefs.Save(); }
    
    // --- Language ---
    public GameLanguage GetLanguage() => (GameLanguage)PlayerPrefs.GetInt("GameLanguage", (int)defaultLanguage);
    public void SetLanguage(GameLanguage l) { PlayerPrefs.SetInt("GameLanguage", (int)l); PlayerPrefs.Save(); }
}