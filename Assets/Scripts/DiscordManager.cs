using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

public class DiscordManager : MonoBehaviour
{
    [Header("Discord Settings")]
    public long applicationID; 
    
    private Discord.Discord discord;
    
    public static DiscordManager instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    void Start()
    {
        discord = new Discord.Discord(applicationID, (System.UInt64)Discord.CreateFlags.Default);
        
        UpdateDiscordStatus("In Main Menu", "Preparing to run");
    }
    
    void Update()
    {
        if (discord != null)
        {
            discord.RunCallbacks();
        }
    }
    
    public void UpdateDiscordStatus(string stateText, string detailsText)
    {
        if (discord == null) return;
        
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            State = stateText,
            Details = detailsText,
            Assets = {
                LargeImage = "logo",
                LargeText = "My Endless Runner"
            }
        };
        
        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res == Discord.Result.Ok)
            {
                Debug.Log("Discord status updated successfully!");
            }
            else
            {
                Debug.LogWarning("Failed to update Discord status.");
            }
        });
    }
    
    void OnApplicationQuit()
    {
        if (discord != null)
        {
            discord.Dispose();
        }
    }
}
