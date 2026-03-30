using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float parallaxSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
        float currentSpeed = parallaxSpeed * PlayerScript.speedMultiplier;
        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);
        
        if (transform.position.x <= -30f)
        {
            Destroy(gameObject);
        }
    }
}
