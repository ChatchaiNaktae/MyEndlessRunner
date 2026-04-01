using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectible
{
    public int scoreValue = 5;
    public float magnetSpeed = 15f; 
    
    private Transform playerTransform;
    
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (PlayerScript.isMagnetActive && playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance < 10f)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, magnetSpeed * Time.deltaTime);
            }
        }
    }
    
    // What: Implements the Collect method required by ICollectible interface.
    public void Collect(PlayerScript player)
    {
        // Add score using the helper method
        player.AddScore(scoreValue);
        
        // Play sound (assuming coinSound is public or you can access it)
        AudioManager.instance.PlaySFX(player.coinSound);
        
        // Destroy the coin object
        Destroy(this.gameObject);
    }
}