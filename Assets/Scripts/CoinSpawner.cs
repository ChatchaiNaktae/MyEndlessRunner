using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    // Array to hold individual single coin prefabs (Bronze, Silver, Gold)
    public GameObject[] coinPrefabs;
    
    public float minTimeBetweenSpawns = 2f;
    public float maxTimeBetweenSpawns = 5f;
    public float absoluteMinSpawnTime = 1.5f;
    
    // Pattern Generation Settings
    public float coinSpacing = 1f; // Distance between each coin on the X axis
    public float arcHeight = 2f;   // Maximum height for the Arc pattern
    public float moveSpeed = 5f;   // Speed of the generated pattern (Must match Cactus speed)

    void Start()
    {
        SpawnCoinPattern();
    }

    void SpawnCoinPattern()
    {
        GenerateProceduralPattern();

        // Calculate precise spawn time based on game speed
        float randomTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
        float calculatedSpawnTime = randomTime / PlayerScript.speedMultiplier;
        float finalSpawnTime = Mathf.Max(calculatedSpawnTime, absoluteMinSpawnTime);
        
        Invoke("SpawnCoinPattern", finalSpawnTime);
    }

    void GenerateProceduralPattern()
    {
        // 1. Create an empty container object in the scene to hold the generated coins
        GameObject patternContainer = new GameObject("GeneratedCoinPattern");
        patternContainer.transform.position = transform.position;

        // 2. Attach the movement script so the whole group moves to the left
        PatternMovement movementScript = patternContainer.AddComponent<PatternMovement>();
        movementScript.speed = moveSpeed;

        // 3. Randomize pattern type (0 = Line, 1 = Arc) and amount of coins
        int patternType = Random.Range(0, 2); 
        int coinAmount = Random.Range(3, 8); // Generate between 3 to 7 coins
        
        // Randomly pick one type of coin (e.g., Bronze, Silver, or Gold) for this entire pattern
        GameObject selectedCoinPrefab = coinPrefabs[Random.Range(0, coinPrefabs.Length)];

        // 4. Calculate exact position for each coin using math loops
        for (int i = 0; i < coinAmount; i++)
        {
            Vector3 localSpawnPosition = Vector3.zero;

            if (patternType == 0) 
            {
                // LINE PATTERN: Just increase the X position evenly
                localSpawnPosition = new Vector3(i * coinSpacing, 0, 0);
            }
            else if (patternType == 1) 
            {
                // ARC PATTERN: Use Sine wave formula for mathematically precise curve
                // Avoid division by zero if there's only 1 coin
                float progress = (coinAmount > 1) ? ((float)i / (coinAmount - 1)) : 0; 
                float yPos = Mathf.Sin(progress * Mathf.PI) * arcHeight;
                
                localSpawnPosition = new Vector3(i * coinSpacing, yPos, 0);
            }

            // 5. Instantiate the single coin at the calculated position
            Vector3 finalWorldPosition = patternContainer.transform.position + localSpawnPosition;
            GameObject newCoin = Instantiate(selectedCoinPrefab, finalWorldPosition, Quaternion.identity);
            
            // 6. Put the coin inside the container so they move together
            newCoin.transform.SetParent(patternContainer.transform);
        }
    }
}