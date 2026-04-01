using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giant : MonoBehaviour, ICollectible
{
    public void Collect(PlayerScript player)
    {
        player.TriggerGiant();
        AudioManager.instance.PlaySFX(player.coinSound);
        Destroy(this.gameObject);
    }
}