using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [System.Serializable]
    public class PetInfo
    {
        public string petName;
        public Sprite petSprite;
    }
    
    [Header("Panels")]
    public GameObject shopPanel;
    
    [Header("Pet Shop UI")]
    public PetDatabase petDatabase;
    public GameObject petButtonPrefab;
    public Transform shopContentContainer;
    public List<PetInfo> availablePets;
    
    void Start()
    {
        if (!PlayerPrefs.HasKey("SelectedPet"))
        {
            PlayerPrefs.SetInt("SelectedPet", 0);
            PlayerPrefs.Save();
        }
        
        if (shopPanel != null)
        {
            shopPanel.SetActive(false); 
        }

        GeneratePetShopUI();
    }
    
    void GeneratePetShopUI()
    {
        foreach (Transform child in shopContentContainer) Destroy(child.gameObject);
        
        for (int i = 0; i < petDatabase.allPets.Count; i++)
        {
            GameObject newBtnObj = Instantiate(petButtonPrefab, shopContentContainer);
            PetButton petBtnScript = newBtnObj.GetComponent<PetButton>();
            
            if (petBtnScript != null)
            {
                petBtnScript.Setup(this, petDatabase.allPets[i].petName, petDatabase.allPets[i].petSprite, i);
            }
        }
    }
    
    public void PlayGame() { SceneManager.LoadScene("GameScene"); }
    public void QuitGame() { Application.Quit(); Debug.Log("Game Closed"); }
    public void OpenShop() { if (shopPanel != null) shopPanel.SetActive(true); }
    public void CloseShop() { if (shopPanel != null) shopPanel.SetActive(false); }
    
    public void SelectPet(int petIndex)
    {
        PlayerPrefs.SetInt("SelectedPet", petIndex);
        PlayerPrefs.Save(); 
        Debug.Log("คุณชัยเลือกสัตว์เลี้ยง: " + petDatabase.allPets[petIndex].petName);
    }
}