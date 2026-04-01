using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour, ICollectible
{
    public void Collect(PlayerScript player)
    {
        player.TriggerMagnet();
        AudioManager.instance.PlaySFX(player.coinSound);
        Destroy(this.gameObject);
    }
}