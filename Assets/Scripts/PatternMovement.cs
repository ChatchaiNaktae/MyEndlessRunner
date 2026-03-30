using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternMovement : MonoBehaviour
{
    public float speed = 5f;
    
    void Start()
    {
        Invoke("DestroyAfterTime", 10f);
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