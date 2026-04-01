using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
    public TextMeshProUGUI totalCoinText; // <--- อ้างอิงตัวหนังสือ "YOUR COIN: 554"
    private List<PetButton> spawnedButtons = new List<PetButton>(); // <--- เก็บรายชื่อปุ่มไว้สั่งเปลี่ยนข้อความ
    
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
        spawnedButtons.Clear();
        
        for (int i = 0; i < petDatabase.allPets.Count; i++)
        {
            GameObject newBtnObj = Instantiate(petButtonPrefab, shopContentContainer);
            PetButton petBtnScript = newBtnObj.GetComponent<PetButton>();
            
            if (petBtnScript != null)
            {
                var currentPet = petDatabase.allPets[i];
                petBtnScript.Setup(this, currentPet.petName, currentPet.petSprite, i, currentPet.price, currentPet.isUnlockedByDefault);
                spawnedButtons.Add(petBtnScript);
            }
        }
        
        RefreshShopUI();
    }
    
    // What: Updates the total coins UI and tells all pet buttons to update their state.
    public void RefreshShopUI()
    {
        // 1. อัปเดตตัวเลขเงินที่มุมซ้ายล่าง
        if (SaveManager.instance != null && totalCoinText != null)
        {
            totalCoinText.text = "YOUR COIN: " + SaveManager.instance.totalCoins.ToString();
        }
        
        // 2. สั่งให้ปุ่มทุกปุ่มอัปเดตคำว่า BUY / SELECT / SELECTED
        foreach (var btn in spawnedButtons)
        {
            if (btn != null) btn.RefreshUI();
        }
    }
    
    public void PlayGame() { SceneManager.LoadScene("GameScene"); }
    public void QuitGame() { Application.Quit(); Debug.Log("Game Closed"); }
    public void OpenShop() { if (shopPanel != null) shopPanel.SetActive(true); if (DiscordManager.instance != null) DiscordManager.instance.OnOpenPetShop(); }
    public void CloseShop() { if (shopPanel != null) shopPanel.SetActive(false); }
    
    public void SelectPet(int petIndex)
    {
        PlayerPrefs.SetInt("SelectedPet", petIndex);
        PlayerPrefs.Save(); 
        Debug.Log("คุณชัยเลือกสัตว์เลี้ยง: " + petDatabase.allPets[petIndex].petName);
    }
}