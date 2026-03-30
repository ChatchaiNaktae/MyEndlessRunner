using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : MonoBehaviour
{
    public float speed;

    private void Start()
    {
        Invoke("DestroyAfterTime", 10);
    }

    void Update()
    {
        float currentSpeed = speed * PlayerScript.speedMultiplier;
        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);
    }

    void DestroyAfterTime()
    {
        Destroy(gameObject);
    }
}