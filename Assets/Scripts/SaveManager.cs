using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public int totalCoins;
    public int highScore;
    public int totalDeaths;
    public int selectedPetIndex;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // What: Saves all necessary player data to PlayerPrefs.
    public void SaveGame()
    {
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.SetInt("TotalDeaths", totalDeaths);
        PlayerPrefs.SetInt("SelectedPetIndex", selectedPetIndex);
        
        PlayerPrefs.Save(); // บังคับให้เขียนลงดิสก์ทันที
        Debug.Log("Game Saved Successfully!");
    }
    
    // What: Loads player data from PlayerPrefs.
    public void LoadGame()
    {
        // Parameter ที่สองคือค่า Default ถ้าหาข้อมูลไม่เจอ
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        totalDeaths = PlayerPrefs.GetInt("TotalDeaths", 0);
        selectedPetIndex = PlayerPrefs.GetInt("SelectedPetIndex", 0);
        
        Debug.Log("Game Loaded Successfully!");
    }
    
    // Optional: Reset data (useful for debugging)
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        LoadGame(); // โหลดค่า default กลับมา
        Debug.Log("All Data Reset!");
    }
    
    // What: Checks if a specific pet is unlocked.
    public bool IsPetUnlocked(int petIndex, bool isUnlockedByDefault)
    {
        if (isUnlockedByDefault) return true;
        
        // Return true (1) if unlocked, false (0) if locked.
        return PlayerPrefs.GetInt("PetUnlocked_" + petIndex, 0) == 1;
    }
    
    // What: Unlocks a pet and saves the game immediately.
    public void UnlockPet(int petIndex)
    {
        PlayerPrefs.SetInt("PetUnlocked_" + petIndex, 1);
        SaveGame(); // บังคับเซฟทันทีเพื่อป้องกันเงินหาย
    }
}