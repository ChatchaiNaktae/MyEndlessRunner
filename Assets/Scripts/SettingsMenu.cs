using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("Database")]
    public SettingsDatabase settingsDatabase;
    public List<LanguageData> languages;
    
    [Header("UI Elements")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown languageDropdown;
    
    [Header("Panels")]
    public GameObject settingPanel;
    
    void Start()
    {
        if (settingsDatabase == null) return;
        
        // 1. โหลดค่าเก่ามาตั้งให้ UI (สำคัญมาก! เพื่อให้ Slider ไม่กลับไปค่าเริ่มต้น)
        musicSlider.value = settingsDatabase.GetMusicVolume();
        sfxSlider.value = settingsDatabase.GetSFXVolume();
        qualityDropdown.value = (int)settingsDatabase.GetGraphicQuality();
        languageDropdown.value = (int)settingsDatabase.GetLanguage();
        
        // 2. ผูกฟังก์ชันการเปลี่ยนค่า
        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
        qualityDropdown.onValueChanged.AddListener(UpdateGraphicQuality);
        languageDropdown.onValueChanged.AddListener(UpdateLanguage);
        
        // 3. สั่งรันภาษาปัจจุบันทันที
        UpdateLanguage(languageDropdown.value);
    }
    
    public void UpdateMusicVolume(float value) {
        settingsDatabase.SetMusicVolume(value);
        if (AudioManager.instance != null) AudioManager.instance.SetMusicVolume(value);
    }
    
    public void UpdateSFXVolume(float value) {
        settingsDatabase.SetSFXVolume(value);
    }
    
    public void UpdateGraphicQuality(int index) {
        settingsDatabase.SetGraphicQuality((SettingsDatabase.GraphicQuality)index);
        QualitySettings.SetQualityLevel(index, true);
    }
    
    public void UpdateLanguage(int index) {
        settingsDatabase.SetLanguage((SettingsDatabase.GameLanguage)index);
        // สั่งให้ UI ทั้งเกมเปลี่ยนภาษา (เดี๋ยวเราจะทำสคริปต์ช่วยในขั้นถัดไป)
        BroadcastMessage("OnLanguageChanged", SendMessageOptions.DontRequireReceiver);
    }
    
    public void OpenSettings() { if (settingPanel != null) settingPanel.SetActive(true); if (DiscordManager.instance != null) DiscordManager.instance.OnOpenSettings(); }
    public void CloseSettings() { if (settingPanel != null) settingPanel.SetActive(false); }
}
