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
    
    private MainMenuController menuController;
    private int petIndex;
    
    public void Setup(MainMenuController controller, string petName, Sprite sprite, int index)
    {
        menuController = controller;
        petIndex = index;
        
        nameText.text = petName;
        petImage.sprite = sprite;
        
        button.onClick.RemoveAllListeners(); 
        button.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        menuController.SelectPet(petIndex);
    }
}