using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [Header("Cloud Prefabs")]
    public GameObject[] cloudPrefabs; // ไว้ใส่เมฆ A, B, C
    
    [Header("Spawn Settings")]
    public float minSpawnTime = 1.5f;
    public float maxSpawnTime = 4.0f;
    
    public float minY = 1.5f; 
    public float maxY = 4.5f; 
    
    void Start()
    {
        SpawnCloud();
    }
    
    void SpawnCloud()
    {
        GameObject selectedCloud = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];
        
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(transform.position.x, randomY, 0);
        
        GameObject newCloud = Instantiate(selectedCloud, spawnPos, Quaternion.identity);
        
        Cloud cloudScript = newCloud.GetComponent<Cloud>();
        if (cloudScript != null)
        {
            cloudScript.parallaxSpeed = Random.Range(1.0f, 2.5f); 
            float scaleMultiplier = cloudScript.parallaxSpeed / 2.5f; 
            float finalScale = Mathf.Clamp(scaleMultiplier, 0.5f, 1.2f); // คุมไม่ให้เล็กหรือใหญ่เกินไป
            newCloud.transform.localScale = new Vector3(finalScale, finalScale, 1f);
        }
        
        float randomTime = Random.Range(minSpawnTime, maxSpawnTime);
        float calculatedSpawnTime = randomTime / PlayerScript.speedMultiplier;
        
        Invoke("SpawnCloud", calculatedSpawnTime);
    }
}
