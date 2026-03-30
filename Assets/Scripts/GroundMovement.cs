using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GroundMovement : MonoBehaviour
{
    public GameObject groundPrefeb;
    public float speed;
    private bool hasSpawnedGround = false;
    
    [Header("Pitfall Settings")]
    public float gapChance = 0.2f;
    public float gapSize = 4f;

    private void Update()
    {
        float currentSpeed = speed * PlayerScript.speedMultiplier;
        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);
        
        if (transform.position.x <= 0 && !hasSpawnedGround)
        {
            float extraGap = 0;
            
            if (Random.value <= gapChance)
            {
                extraGap = gapSize;
            }
            
            float spawnX = transform.position.x + 26.5f + extraGap;
            Instantiate(groundPrefeb, new Vector3(spawnX, 0, 0), Quaternion.identity);
            
            hasSpawnedGround = true;
        }
        
        if (transform.position.x <= -27f)
        {
            Destroy(gameObject);
        }
    }
}