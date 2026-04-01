using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// What: A database to store all missions, making it easy to add or edit missions in the Unity Inspector.
[CreateAssetMenu(fileName = "NewMissionDatabase", menuName = "EndlessRunner/MissionDatabase")]
public class MissionDatabase : ScriptableObject
{
    [System.Serializable]
    public class MissionData
    {
        public string missionName;    // Name or title of the mission
        public int targetAmount;      // How many coins the player needs to collect
        public int rewardCoins;       // How many coins the player gets as a reward
    }
    
    public List<MissionData> allMissions;
}