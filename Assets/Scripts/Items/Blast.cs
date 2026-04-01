using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour, ICollectible
{
    public void Collect(PlayerScript player)
    {
        player.TriggerBlast();
        AudioManager.instance.PlaySFX(player.coinSound);
        Destroy(this.gameObject);
    }
}