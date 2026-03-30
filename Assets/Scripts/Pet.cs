using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    public enum PetType { Flying, Ground }
    public enum PetSkill { HealEnergy, SpawnItem, MagnetPulse } 
    
    [Header("Pet Settings")]
    public string petName = "Shark";
    public PetType petType = PetType.Ground;
    public PetSkill petSkill = PetSkill.HealEnergy;
    public Transform target;
    public float smoothTime = 0.3f;
    public Vector3 offset = new Vector3(-1.5f, -0.5f, 0f);
    
    private Vector3 velocity = Vector3.zero;
    
    [Header("Animation Settings")]
    public float bobSpeed = 3f;
    public float bobHeight = 0.2f;
    private Animator animator;
    
    [Header("Skill Shared Settings")]
    public float skillInterval = 10f;
    private float skillTimer = 0f;
    
    [Header("Skill 1: Heal Energy")]
    public float healAmount = 5f;
    
    [Header("Skill 2: Spawn Item")]
    public GameObject itemToSpawn; 
    public float spawnOffsetY = 0.5f;
    public float spawnDistanceX = 12f;
    
    [Header("Skill 3: Magnet Pulse")]
    public float magnetDuration = 3f;
    
    [Header("Ground Check (For Ground Pet)")]
    public LayerMask groundLayer;
    
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
                animator.Play(petName + "_Pet_Idle"); 
                animator.speed = 1f;
            }
            return; 
        }
        else
        {
            if (animator != null) 
            {
                animator.Play(petName + "_Pet_Run"); 
                animator.speed = PlayerScript.speedMultiplier / 1.5f; 
            }
        }
        
        FollowTarget();
        UseSkill();
    }
    
    void FollowTarget()
    {
        Vector3 targetPosition = target.position + offset;
        
        if (petType == PetType.Ground)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, target.position.y + 2f), Vector2.down, 5f, groundLayer);
            
            if (hit.collider != null)
            {
                targetPosition.y = hit.point.y + 0.5f; 
            }
            else
            {
                targetPosition.y = transform.position.y;
            }
        }
        else if (petType == PetType.Flying)
        {
            float bobbing = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            targetPosition.y += bobbing;
        }
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
    
    void UseSkill()
    {
        skillTimer += Time.deltaTime;
        if (skillTimer >= skillInterval)
        {
            ActivateSkill();
            skillTimer = 0f;
        }
    }
    
    void ActivateSkill()
    {
        if (playerScript == null) return;
        
        switch (petSkill)
        {
            case PetSkill.HealEnergy:
                playerScript.currentEnergy += healAmount;
                if (playerScript.currentEnergy > playerScript.maxEnergy)
                    playerScript.currentEnergy = playerScript.maxEnergy;
                if (playerScript.energyBar != null)
                    playerScript.energyBar.value = playerScript.currentEnergy;
                break;
            
            case PetSkill.SpawnItem:
                if (itemToSpawn != null)
                {
                    Vector3 spawnPos = new Vector3(target.position.x + spawnDistanceX, spawnOffsetY, 0f);
                    GameObject newObj = Instantiate(itemToSpawn, spawnPos, Quaternion.identity);
                    
                    if (newObj.GetComponent<PatternMovement>() == null)
                    {
                        PatternMovement pm = newObj.AddComponent<PatternMovement>();
                        pm.speed = 5f;
                    }
                }
                break;
            
            case PetSkill.MagnetPulse:
                StartCoroutine(MagnetPulseRoutine());
                break;
        }
    }
    
    IEnumerator MagnetPulseRoutine()
    {
        PlayerScript.isMagnetActive = true;
        yield return new WaitForSeconds(magnetDuration);
        PlayerScript.isMagnetActive = false;
    }
}
