using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPetDatabase", menuName = "EndlessRunner/PetDatabase")]
public class PetDatabase : ScriptableObject
{
    [System.Serializable]
    public class PetData
    {
        public string petName;
        public Sprite petSprite;
        public GameObject petPrefab;
    }
    
    public List<PetData> allPets;
}