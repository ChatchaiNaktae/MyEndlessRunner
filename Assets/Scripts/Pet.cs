using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;
    public float smoothTime = 0.3f;
    public Vector3 offset = new Vector3(-1.5f, 1f, 0f);
    
    private Vector3 velocity = Vector3.zero;
    
    [Header("Animation Settings")]
    public float bobSpeed = 3f;
    public float bobHeight = 0.2f;
    
    [Header("Skill: Auto Heal")]
    public float healInterval = 10f;
    public float healAmount = 5f;
    private float healTimer = 0f;
    
    private PlayerScript playerScript;
    
    // Start is called before the first frame update
    void Start()
    {
        if (target != null)
        {
            playerScript = target.GetComponent<PlayerScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || PlayerScript.speedMultiplier == 0f) return;
        
        FollowAndBob();
        UseSkill();
    }
    
    void FollowAndBob()
    {
        Vector3 targetPosition = target.position + offset;
        
        float bobbing = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        targetPosition.y += bobbing;
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
    
    void UseSkill()
    {
        if (playerScript != null)
        {
            healTimer += Time.deltaTime;
            if (healTimer >= healInterval)
            {
                playerScript.currentEnergy += healAmount;
                
                if (playerScript.currentEnergy > playerScript.maxEnergy)
                {
                    playerScript.currentEnergy = playerScript.maxEnergy;
                }
                
                if (playerScript.energyBar != null)
                {
                    playerScript.energyBar.value = playerScript.currentEnergy;
                }
                
                healTimer = 0f;
                
                // (ถ้ามีเสียงฮีล สามารถสั่ง PlaySFX ตรงนี้ได้เลยครับ)
            }
        }
    }
}
