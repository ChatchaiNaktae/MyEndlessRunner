using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// What: Controls the pause menu UI, game time scaling, and scene transitions.
public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuUI; 
    
    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu"; 
    
    // ตัวแปรสำหรับเชื่อมต่อกับผู้เล่น (กฎข้อ 1: อ่านแล้วเข้าใจง่าย)
    private PlayerScript player;
    
    private void Start()
    {
        pauseMenuUI.SetActive(false);
        
        // What: Automatically find the player in the scene when the game starts.
        player = FindObjectOfType<PlayerScript>();
    }
    
    private void Update()
    {
        // What: Block the pause menu if the player is dead (Game Over screen is active).
        // (กฎข้อ 3: ไม่ซับซ้อน แค่เพิ่มบรรทัดนี้บรรทัดเดียว จบปิ๊ง!)
        if (player != null && player.isDead) return;
        
        // What: Toggle the pause state when the Escape key is pressed.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuUI.activeSelf)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }
    
    // What: Pauses the game by showing the UI and stopping time.
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        
        if (DiscordManager.instance != null)
        {
            DiscordManager.instance.UpdateStatus("Game Paused", "Taking a break", "icon_menu", "Paused");
        }
    }
    
    // What: Resumes the game by hiding the UI and restoring time.
    public void Unpause()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        
        if (DiscordManager.instance != null)
        {
            DiscordManager.instance.UpdateStatus("Running for Life!", "Back in action", "icon_game", "In-Game");
        }
    }
    
    // What: Restarts the current active scene.
    public void ReStart()
    {
        Unpause(); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // What: Loads the main menu scene.
    public void BackToMenu()
    {
        Unpause();
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    // What: Open Setting.
    public void OpenSettings()
    {
        
    }
}
