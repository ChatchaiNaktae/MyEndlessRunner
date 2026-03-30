using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusSpawner : MonoBehaviour
{
    [Header("Cactus Settings")]
    public GameObject cactusA;
    public GameObject cactusB;
    public GameObject cactusC;
    public float minTimeBetweenSpawns = 1.2f;
    public float maxTimeBetweenSpawns = 2.5f;
    public float absoluteMinSpawnTime = 0.75f;
    
    [Header("General Coin Settings")]
    public GameObject[] coinPrefabs;
    public float coinSpacing = 1.2f; 
    public float moveSpeed = 5f;     
    
    [Header("Arc Pattern Settings")]
    public float arcHeight = 3.5f;   
    public float arcOffsetY = 0.8f;  
    
    [Header("Line Pattern Settings")]
    public float lineGroundY = 0.8f; 
    public float lineAirY = 3.0f;    
    
    [Header("Platform Settings")]
    public GameObject platformPrefab;
    public float platformY = 2.8f;
    public float platformCoinOffsetY = 1.0f;

    [Header("Spawn Chances (Total must be 1.0 or 100%)")]
    public float cactusChance = 0.5f;
    public float lineChance = 0.25f;
    public float platformChance = 0.25f;
    
    [Header("Ground Check")]
    public LayerMask groundLayer;
    
    void Start()
    {
        SpawnNextObject(); 
    }
    
    void SpawnNextObject()
    {
        float randomTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
        float roll = Random.value;
        
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, 5f, groundLayer);
        
        if (roll <= platformChance) 
        {
            GeneratePlatformPattern();
            randomTime += 1.5f;
        }
        else if (roll <= platformChance + lineChance) 
        {
            GenerateLinePattern();
            randomTime += 1.0f; 
        }
        else 
        {
            if (groundHit.collider != null)
            {
                Vector3 spawnPos = transform.position;
                Instantiate(ChooseRandomCactus(), spawnPos, Quaternion.identity);
            
                if (Random.value <= 0.7f)
                {
                    GenerateArcOverCactus(spawnPos);
                }
            }
            else
            {
                Debug.Log("Skipped Cactus: Pitfall detected!");
            }
        }
        
        float calculatedSpawnTime = randomTime / PlayerScript.speedMultiplier;
        float finalSpawnTime = Mathf.Max(calculatedSpawnTime, absoluteMinSpawnTime);
        Invoke("SpawnNextObject", finalSpawnTime);
    }
    
    GameObject ChooseRandomCactus()
    {
        int rnd = Random.Range(0, 3);
        switch (rnd)
        {
            case 0: return cactusA;
            case 1: return cactusB;
            case 2: return cactusC;
        }
        return cactusA;
    }
    
    void GenerateArcOverCactus(Vector3 centerPos)
    {
        GameObject patternContainer = new GameObject("CoinArcPattern");
        patternContainer.transform.position = centerPos;
        PatternMovement movementScript = patternContainer.AddComponent<PatternMovement>();
        movementScript.speed = moveSpeed;
        
        int coinAmount = 5; 
        GameObject selectedCoin = coinPrefabs[Random.Range(0, coinPrefabs.Length)];
        float halfWidth = (coinAmount - 1) * coinSpacing / 2f;
        
        for (int i = 0; i < coinAmount; i++)
        {
            float progress = (float)i / (coinAmount - 1); 
            float yPos = Mathf.Sin(progress * Mathf.PI) * arcHeight;
            Vector3 localPos = new Vector3((i * coinSpacing) - halfWidth, yPos + arcOffsetY, 0);
            Vector3 finalPos = patternContainer.transform.position + localPos;
            
            GameObject newCoin = Instantiate(selectedCoin, finalPos, Quaternion.identity);
            newCoin.transform.SetParent(patternContainer.transform);
        }
    }
    
    void GenerateLinePattern()
    {
        GameObject patternContainer = new GameObject("CoinLinePattern");
        patternContainer.transform.position = transform.position;
        PatternMovement movementScript = patternContainer.AddComponent<PatternMovement>();
        movementScript.speed = moveSpeed;
        
        int coinAmount = Random.Range(4, 8); 
        GameObject selectedCoin = coinPrefabs[Random.Range(0, coinPrefabs.Length)];
        float heightY = (Random.value > 0.5f) ? lineGroundY : lineAirY;
        
        for (int i = 0; i < coinAmount; i++)
        {
            Vector3 localPos = new Vector3(i * coinSpacing, heightY, 0);
            Vector3 finalPos = patternContainer.transform.position + localPos;
            GameObject newCoin = Instantiate(selectedCoin, finalPos, Quaternion.identity);
            newCoin.transform.SetParent(patternContainer.transform);
        }
    }
    
    void GeneratePlatformPattern()
    {
        Vector3 platformPos = new Vector3(transform.position.x, platformY, 0);
        Instantiate(platformPrefab, platformPos, Quaternion.identity);
        
        GameObject patternContainer = new GameObject("CoinPlatformPattern");
        patternContainer.transform.position = platformPos; 
        PatternMovement movementScript = patternContainer.AddComponent<PatternMovement>();
        movementScript.speed = moveSpeed;
        
        int coinAmount = 4;
        GameObject selectedCoin = coinPrefabs[Random.Range(0, coinPrefabs.Length)];
        float halfWidth = (coinAmount - 1) * coinSpacing / 2f;
        
        for (int i = 0; i < coinAmount; i++)
        {
            Vector3 localPos = new Vector3((i * coinSpacing) - halfWidth, platformCoinOffsetY, 0);
            Vector3 finalPos = patternContainer.transform.position + localPos;
            
            GameObject newCoin = Instantiate(selectedCoin, finalPos, Quaternion.identity);
            newCoin.transform.SetParent(patternContainer.transform);
        }
    }
}