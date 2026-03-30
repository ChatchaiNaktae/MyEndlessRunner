using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{
    [Header("Database & Target")]
    public PetDatabase petDatabase;
    public Transform playerTransform;
    
    void Start()
    {
        int selectedPetIndex = PlayerPrefs.GetInt("SelectedPet", 0);
        
        if (petDatabase != null && selectedPetIndex >= 0 && selectedPetIndex < petDatabase.allPets.Count)
        {
            GameObject petPrefab = petDatabase.allPets[selectedPetIndex].petPrefab;
            
            if (petPrefab != null && playerTransform != null)
            {
                GameObject spawnedPet = Instantiate(petPrefab, playerTransform.position, Quaternion.identity);
                
                Pet petScript = spawnedPet.GetComponent<Pet>();
                if (petScript != null)
                {
                    petScript.target = playerTransform;
                }
            }
        }
    }
}
