using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour, ICollectible
{
    public float healAmount = 20f;
    
    public void Collect(PlayerScript player)
    {
        player.HealEnergy(healAmount);
        AudioManager.instance.PlaySFX(player.coinSound);
        Destroy(this.gameObject);
    }
}
