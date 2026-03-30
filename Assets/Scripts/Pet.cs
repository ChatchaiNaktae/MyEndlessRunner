using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    public enum PetType { Flying, Ground } 
    
    [Header("Pet Settings")]
    public PetType petType = PetType.Ground;
    public Transform target;
    public float smoothTime = 0.3f;
    public Vector3 offset = new Vector3(-1.5f, -0.5f, 0f);
    
    private Vector3 velocity = Vector3.zero;
    
    [Header("Animation Settings")]
    public float bobSpeed = 3f;
    public float bobHeight = 0.2f;
    private Animator animator;
    
    [Header("Skill: Auto Heal")]
    public float healInterval = 10f;
    public float healAmount = 5f;
    private float healTimer = 0f;
    
    private PlayerScript playerScript;
    
    void Start()
    {
        if (target != null)
        {
            playerScript = target.GetComponent<PlayerScript>();
        }
        
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (target == null) return;
        
        if (PlayerScript.speedMultiplier == 0f)
        {
            if (animator != null) 
            {
                animator.Play("Shark_Pet_Idle");
                animator.speed = 1f;
            }
            return;
        }
        else
        {
            if (animator != null) 
            {
                animator.Play("Shark_Pet_Run");
                animator.speed = PlayerScript.speedMultiplier / 1.5f; 
            }
        }
        
        FollowTarget();
        
        UseSkill();
    }
    
    void FollowTarget()
    {
        Vector3 targetPosition = target.position + offset;
        
        if (petType == PetType.Flying)
        {
            float bobbing = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            targetPosition.y += bobbing;
        }
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
    
    void UseSkill()
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
        }
    }
}
