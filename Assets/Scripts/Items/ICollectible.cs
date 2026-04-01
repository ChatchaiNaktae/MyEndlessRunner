using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// What: Defines a contract for any item that the player can collect.
public interface ICollectible
{
    // What: The action to perform when the player collects this item.
    void Collect(PlayerScript player);
}