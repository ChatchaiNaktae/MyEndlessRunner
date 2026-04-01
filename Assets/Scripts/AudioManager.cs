using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// What: Centralized manager for all game audio (BGM and SFX).
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    [Header("Settings & Audio Sources")]
    public SettingsDatabase settingsDatabase;
    public AudioSource bgmSource;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    
    private void Start()
    {
        // What: Apply saved music volume when the game starts.
        if (settingsDatabase != null && bgmSource != null)
        {
            bgmSource.volume = settingsDatabase.GetMusicVolume();
        }
    }
    
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        float finalVolume = volume;
        
        if (settingsDatabase != null)
        {
            finalVolume = volume * settingsDatabase.GetSFXVolume(); 
        }
        
        StartCoroutine(PlaySFXCoroutine(clip, finalVolume));
    }
    
    IEnumerator PlaySFXCoroutine(AudioClip clip, float volume = 1f)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        
        yield return new WaitForSeconds(audioSource.clip.length + 0.1f);
        
        Destroy(audioSource);
    }
    
    public void SetMusicVolume(float volume)
    {
        if (bgmSource != null) bgmSource.volume = volume; 
    }
    
    public void SetSFXVolume(float volume)
    {
    }
}