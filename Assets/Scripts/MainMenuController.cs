using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    void Start()
    {
        if (!PlayerPrefs.HasKey("SelectedPet"))
        {
            PlayerPrefs.SetInt("SelectedPet", 0);
            PlayerPrefs.Save();
        }
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); 
    }
    
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }
    
    public void SelectPet(int petIndex)
    {
        PlayerPrefs.SetInt("SelectedPet", petIndex);
        PlayerPrefs.Save(); 
        
        Debug.Log("คุณชัยได้เลือกสัตว์เลี้ยงหมายเลข: " + petIndex);
    }
}
