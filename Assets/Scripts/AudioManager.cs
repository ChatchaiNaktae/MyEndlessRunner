using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    private void Awake()
    {
        instance = this;
    }
    
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        StartCoroutine(PlaySFXCoroutine(clip, volume));
    }
    
    IEnumerator PlaySFXCoroutine(AudioClip clip, float volume = 1f)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        
        yield return new WaitForSeconds(audioSource.clip.length * 2);
        
        Destroy(audioSource);
    }
    
    // What: Adjusts the background music volume based on settings.
    public void SetMusicVolume(float volume)
    {
        // สมมติว่าตัวเล่นเพลงชื่อ bgmSource เปลี่ยนชื่อตามโค้ดจริงของน้องชัยได้เลยครับ
        // bgmSource.volume = volume; 
    }
    
    // What: Adjusts the sound effects volume based on settings.
    public void SetSFXVolume(float volume)
    {
        // สมมติว่าตัวเล่นเสียงเอฟเฟกต์ชื่อ sfxSource
        // sfxSource.volume = volume; 
    }
}