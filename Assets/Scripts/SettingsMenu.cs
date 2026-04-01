using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI References")]
    public Slider musicSlider;
    public Slider sfxSlider;
    
    void Start()
    {
        // What: Load saved volume settings into the sliders when the menu opens.
        if (SaveManager.instance != null)
        {
            if (musicSlider != null) musicSlider.value = SaveManager.instance.musicVolume;
            if (sfxSlider != null) sfxSlider.value = SaveManager.instance.sfxVolume;
        }
        
        // What: Listen for slider changes in real-time.
        if (musicSlider != null) musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
    }
    
    // What: Updates music volume, saves it, and applies it to the AudioListener.
    public void UpdateMusicVolume(float value)
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.musicVolume = value;
            SaveManager.instance.SaveGame();
        }
        
        // Apply volume directly (Requires AudioManager to have a specific function, or handle it there)
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMusicVolume(value);
        }
    }
    
    // What: Updates SFX volume, saves it, and applies it globally.
    public void UpdateSFXVolume(float value)
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.sfxVolume = value;
            SaveManager.instance.SaveGame();
        }
        
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetSFXVolume(value);
        }
    }
}
