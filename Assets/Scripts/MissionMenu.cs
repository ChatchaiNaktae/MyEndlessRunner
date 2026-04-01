using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// What: Generates and manages the list of missions using the MissionDatabase.
public class MissionMenu : MonoBehaviour
{
    [Header("Database & UI")]
    public MissionDatabase missionDatabase;
    public GameObject missionPrefab; // Prefab of a single mission item
    public Transform missionContentContainer; // The layout group to spawn items into
    
    private List<MissionUIItem> spawnedMissions = new List<MissionUIItem>();
    
    [Header("Panels")]
    public GameObject missionPanel;
    
    void Start()
    {
        GenerateMissionUI();
    }
    
    // What: Clears old UI and creates new mission items based on the database.
    private void GenerateMissionUI()
    {
        foreach (Transform child in missionContentContainer) Destroy(child.gameObject);
        spawnedMissions.Clear();
        
        for (int i = 0; i < missionDatabase.allMissions.Count; i++)
        {
            GameObject newObj = Instantiate(missionPrefab, missionContentContainer);
            MissionUIItem missionScript = newObj.GetComponent<MissionUIItem>();
            
            if (missionScript != null)
            {
                var data = missionDatabase.allMissions[i];
                missionScript.Setup(this, data.missionName, data.targetAmount, data.rewardCoins, i);
                spawnedMissions.Add(missionScript);
            }
        }
    }
    
    public void OpenMission() { if (missionPanel != null) missionPanel.SetActive(true); if (DiscordManager.instance != null) DiscordManager.instance.OnOpenMission(); }
    public void CloseMission() { if (missionPanel != null) missionPanel.SetActive(false); }
    
    // What: Tells all spawned missions to update their UI states.
    public void RefreshAllMissions()
    {
        foreach (var mission in spawnedMissions)
        {
            if (mission != null) mission.RefreshUI();
        }
        
        // Update total coins in the main menu shop UI (if it exists)
        var mainMenu = FindObjectOfType<MainMenuController>();
        if (mainMenu != null) mainMenu.RefreshShopUI();
    }
}