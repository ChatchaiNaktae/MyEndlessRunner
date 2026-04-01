using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject container;
    private PlayerScript player;

    private void Start()
    {
        player  = FindObjectOfType<PlayerScript>();
    }
    
    private void Update()
    {
        if (player == null || player.isDead) 
        {
            return; 
        }
        
        // 1. Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 2. If the pause menu is currently active (visible), resume the game
            if (container.activeSelf)
            {
                ResumeGame();
            }
            // 3. If the pause menu is NOT active, pause the game
            else
            {
                PauseGame();
            }
        }
    }
    
    // Custom function to handle pausing logic
    private void PauseGame()
    {
        container.SetActive(true);
        Time.timeScale = 0f; // Freeze the game
    }
    
    // This function is called when pressing Esc again, or clicking the "Resume" button
    public void ResumeGame()
    {
        container.SetActive(false);
        Time.timeScale = 1f; // Unfreeze the game
    }
    
    public void MainMenuGame()
    {
        // IMPORTANT: Must unfreeze time before loading the Main Menu, 
        // otherwise the Main Menu animations/functions might be frozen too!
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu");
    }
    
    public void QuitGame()
    {
        // This prints a message in the Unity console to show it works
        Debug.Log("Quit Game!");
        
        // This closes the game (Only works when you build the final game, not in Editor)
        Application.Quit();
    }
}
