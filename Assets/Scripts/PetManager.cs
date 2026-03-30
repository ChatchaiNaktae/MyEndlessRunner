using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{
    [Header("Available Pets")]
    public GameObject[] petList;
    
    void Start()
    {
        int selectedPetIndex = PlayerPrefs.GetInt("SelectedPet", 0);
        
        for (int i = 0; i < petList.Length; i++)
        {
            if (petList[i] != null)
            {
                petList[i].SetActive(false);
            }
        }
        
        if (selectedPetIndex >= 0 && selectedPetIndex < petList.Length)
        {
            if (petList[selectedPetIndex] != null)
            {
                petList[selectedPetIndex].SetActive(true);
            }
        }
    }
}
