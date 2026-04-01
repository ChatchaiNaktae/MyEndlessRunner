using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// What: Handles the UI and logic for a single mission item.
public class MissionUIItem : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI descriptionText;
    public Button claimButton;
    public TextMeshProUGUI buttonText;
    
    private int missionIndex;
    private int targetAmount;
    private int rewardCoins;
    private MissionMenu menuController;
    
    // What: Initializes the mission UI with data from the database.
    public void Setup(MissionMenu controller, string name, int target, int reward, int index)
    {
        menuController = controller;
        missionIndex = index;
        targetAmount = target;
        rewardCoins = reward;
        
        // Clear old clicks and set up the claim function
        claimButton.onClick.RemoveAllListeners();
        claimButton.onClick.AddListener(ClaimReward);
        
        RefreshUI(); // Update text immediately
    }
    
    // What: Updates the button and text based on player progress.
    public void RefreshUI()
    {
        if (SaveManager.instance == null) return;
        
        // Check if this specific mission index is already claimed
        bool isClaimed = PlayerPrefs.GetInt("MissionClaimed_" + missionIndex, 0) == 1;
        int currentProgress = SaveManager.instance.lifetimeCoins; 
        
        if (isClaimed)
        {
            descriptionText.text = $"Collect {targetAmount} Coins (COMPLETED)";
            claimButton.interactable = false;
            buttonText.text = "CLAIMED";
        }
        else if (currentProgress >= targetAmount)
        {
            descriptionText.text = $"Collect {targetAmount} Coins ({currentProgress}/{targetAmount})";
            claimButton.interactable = true;
            buttonText.text = "CLAIM REWARD";
        }
        else
        {
            descriptionText.text = $"Collect {targetAmount} Coins ({currentProgress}/{targetAmount})";
            claimButton.interactable = false;
            buttonText.text = "IN PROGRESS";
        }
    }
    
    // What: Gives the reward to the player and marks the mission as claimed.
    private void ClaimReward()
    {
        PlayerPrefs.SetInt("MissionClaimed_" + missionIndex, 1);
        
        SaveManager.instance.totalCoins += rewardCoins;
        SaveManager.instance.SaveGame();
        
        RefreshUI(); // Update this button
        
        if (menuController != null) menuController.RefreshAllMissions();
    }
}