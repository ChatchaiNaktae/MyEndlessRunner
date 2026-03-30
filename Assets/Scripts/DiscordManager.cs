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
        
        FindObjectOfType<DiscordManager>().UpdateStatus(
            "At Main Menu", 
            "Preparing to Run", 
            "icon_menu",
            "Endless Runner"
        );
    }
    
    void Update()
    {
        if (discord != null)
        {
            discord.RunCallbacks();
        }
    }
    
    public void OnOpenPetShop() {
        UpdateStatus(
            "In Pet Shop", 
            "Browsing for a buddy", 
            "icon_pet", 
            "Selecting Pet"
        );
    }
    
    public void UpdateStatus(string details, string state, string largeImageKey, string largeText)
    {
        if (discord == null) return;
        
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            Details = details,
            State = state,
            Assets = {
                LargeImage = largeImageKey,
                LargeText = largeText
            },
            Instance = true
        };
        
        if (details.Contains("Running")) {
            activity.Timestamps.Start = System.DateTimeOffset.Now.ToUnixTimeSeconds();
        }
        
        activityManager.UpdateActivity(activity, (result) => {
            if (result == Discord.Result.Ok)
            {
                Debug.Log("Discord Status Updated: " + details);
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
