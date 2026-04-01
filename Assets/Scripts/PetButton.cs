using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PetButton : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI nameText;
    public Image petImage;
    public Button button;
    public TextMeshProUGUI buttonText;
    
    private MainMenuController menuController;
    private int petIndex;
    private int petPrice;
    private bool isUnlockedByDefault;
    
    // What: Initializes the button with pet data.
    public void Setup(MainMenuController controller, string petName, Sprite sprite, int index, int price, bool unlockedDefault)
    {
        menuController = controller;
        petIndex = index;
        petPrice = price;
        isUnlockedByDefault = unlockedDefault;
        
        nameText.text = petName;
        petImage.sprite = sprite;
        
        // Ensure no duplicate listeners, then attach our buying logic
        button.onClick.RemoveAllListeners(); 
        button.onClick.AddListener(OnButtonClicked);
        
        RefreshUI();
    }
    
    // What: Updates the button text based on the player's save data.
    public void RefreshUI()
    {
        if (SaveManager.instance == null) return;
        
        bool isUnlocked = SaveManager.instance.IsPetUnlocked(petIndex, isUnlockedByDefault);
        bool isSelected = SaveManager.instance.selectedPetIndex == petIndex;
        
        if (isSelected)
        {
            buttonText.text = "SELECTED";
            button.interactable = false; // กดซ้ำไม่ได้ถ้าเลือกอยู่แล้ว (ป้องกันบั๊ก)
        }
        else if (isUnlocked)
        {
            buttonText.text = "SELECT";
            button.interactable = true;
        }
        else
        {
            buttonText.text = "BUY " + petPrice; // โชว์ราคาที่ต้องจ่าย
            button.interactable = true;
        }
    }
    
    // What: Logic executed when the player clicks this pet's button.
    void OnButtonClicked()
    {
        // Safety check in case SaveManager is missing
        if (SaveManager.instance == null) return;

        bool isUnlocked = SaveManager.instance.IsPetUnlocked(petIndex, isUnlockedByDefault);

        if (isUnlocked)
        {
            // Already unlocked -> Just select it
            SaveManager.instance.selectedPetIndex = petIndex;
            SaveManager.instance.SaveGame();
            Debug.Log("Successfully Selected Pet ID: " + petIndex);
            
            // Tell the controller to update the UI (if needed)
            if (menuController != null) menuController.SelectPet(petIndex);
        }
        else
        {
            // Not unlocked -> Try to buy
            if (SaveManager.instance.totalCoins >= petPrice)
            {
                // Deduct coins exactly
                SaveManager.instance.totalCoins -= petPrice;
                
                // Unlock and select
                SaveManager.instance.UnlockPet(petIndex);
                SaveManager.instance.selectedPetIndex = petIndex;
                SaveManager.instance.SaveGame();
                
                Debug.Log("Successfully Bought and Selected Pet ID: " + petIndex);
                
                if (menuController != null) menuController.SelectPet(petIndex);
            }
            else
            {
                // Not enough coins
                int missingCoins = petPrice - SaveManager.instance.totalCoins;
                Debug.Log("Not enough coins! You need " + missingCoins + " more coins.");
            }
        }
    }
}