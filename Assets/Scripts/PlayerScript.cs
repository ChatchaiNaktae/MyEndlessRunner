using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [Header("Basic Movement")]
    public float jumpForce;
    public LayerMask groundLayer;
    public float rayLength = 0.1f;
    public bool canDoubleJump;
    
    [Header("UI & Scoring")]
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI highScoreUI;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    private int score;
    private int highScore;
    public static float speedMultiplier = 1f;
    public float speedIncreaseRate = 0.02f;
    public float maxSpeedMultiplier = 3f;
    
    [Header("Energy System")]
    public Slider energyBar;
    public float maxEnergy = 100f;  
    public float currentEnergy;
    public float energyDrainRate = 5f;
    public float obstacleDamage = 20f;
    public float potionHeal = 20f;
    private bool isDead = false;
    
    [Header("Magnet Power-up")]
    public static bool isMagnetActive = false;
    public float magnetDuration = 5f;
    
    [Header("Audio")]
    public AudioClip jumpSound;
    public AudioClip landingSound;
    public AudioClip deathSound;
    public AudioClip coinSound;
    
    [Header("Invincibility Settings")]
    public float invincibilityDuration = 5f;
    public float blinkInterval = 0.2f;
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;
    
    [Header("Power-ups Settings")]
    public float giantDuration = 5f;
    public float giantScale = 2.5f;
    private bool isGiant = false;
    
    public float blastDuration = 3f;
    public float blastSpeedMultiplier = 2.5f;
    private bool isBlast = false;
    private float normalSpeedIncreaseRate;
    
    [Header("Sliding Settings")]
    public float slideColliderHeight = 0.5f;
    private float originalColliderHeight;
    private Vector2 originalColliderOffset;
    private bool isSliding = false;
    private BoxCollider2D playerCollider;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;
    private Coroutine blastCoroutine;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
        
        originalColliderHeight = playerCollider.size.y;
        originalColliderOffset = playerCollider.offset;
        
        normalSpeedIncreaseRate = speedIncreaseRate;
        
        int playerLayer = LayerMask.NameToLayer("Player");
        int obstacleLayer = LayerMask.NameToLayer("Obstacles");
        Physics2D.IgnoreLayerCollision(playerLayer, obstacleLayer, false);
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        
        speedMultiplier = 1f;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (highScoreUI != null) highScoreUI.text = "High Score: " + highScore.ToString();
        
        currentEnergy = maxEnergy;
        if (energyBar != null)
        {
            energyBar.maxValue = maxEnergy;
            energyBar.value = currentEnergy;
        }
        
        isMagnetActive = false;
    }
    
    void Update()
    {
        if (isDead) return;
        
        currentEnergy -= energyDrainRate * Time.deltaTime;
        
        if (!isBlast && speedMultiplier < maxSpeedMultiplier)
        {
            speedMultiplier += speedIncreaseRate * Time.deltaTime;
        }
        
        if (energyBar != null) energyBar.value = currentEnergy;
        if (currentEnergy <= 0) { PlayerDie(); return; }
        
        float currentRayLength = rayLength * transform.localScale.y;
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, currentRayLength, groundLayer);
        
        HandleInput();
        UpdateAnimations();
    }
    
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isSliding)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            AudioManager.instance.PlaySFX(jumpSound);
            canDoubleJump = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump && !isSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); 
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            AudioManager.instance.PlaySFX(jumpSound);
            canDoubleJump = false;
        }
        
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && isGrounded)
        {
            if (!isSliding) StartSliding();
        }
        else
        {
            if (isSliding) StopSliding();
        }
    }
    
    void StartSliding()
    {
        isSliding = true;
        playerCollider.size = new Vector2(playerCollider.size.x, slideColliderHeight);
        playerCollider.offset = new Vector2(playerCollider.offset.x, originalColliderOffset.y - (originalColliderHeight - slideColliderHeight) / 2);
    }
    
    void StopSliding()
    {
        isSliding = false;
        playerCollider.size = new Vector2(playerCollider.size.x, originalColliderHeight);
        playerCollider.offset = originalColliderOffset;
    }
    
    void UpdateAnimations()
    {
        if (isSliding) animator.Play("Player_Slide");
        else if (isGrounded) animator.Play("Player_Run");
        else animator.Play("Player_Jump");
    }
    
    private void OnCollisionEnter2D(Collision2D hit)
    {
        if (isDead) return;
        
        string hitTag = hit.gameObject.tag;
        string parentTag = hit.transform.parent != null ? hit.transform.parent.tag : "";
        
        bool isObstacle = hitTag == "CanHit" || parentTag == "CanHit";
        bool isPlatform = hitTag == "Platform" || parentTag == "Platform";
        
        if (isObstacle || isPlatform)
        {
            if (isGiant || isBlast)
            {
                SmashObject(hit.gameObject);
                return;
            }
            
            if (isObstacle && !isInvincible)
            {
                currentEnergy -= obstacleDamage;
                if (energyBar != null) energyBar.value = currentEnergy;
                if (currentEnergy <= 0) { PlayerDie(); return; }
                StartCoroutine(ActivateTemporaryInvincibilityRoutine());
            }
        }
        else
        {
            AudioManager.instance.PlaySFX(landingSound);
        }
    }
    
    private void OnCollisionStay2D(Collision2D hit)
    {
        if (isDead) return;

        if (isGiant || isBlast)
        {
            string hitTag = hit.gameObject.tag;
            string parentTag = hit.transform.parent != null ? hit.transform.parent.tag : "";
            
            if (hitTag == "CanHit" || parentTag == "CanHit" || hitTag == "Platform" || parentTag == "Platform")
            {
                SmashObject(hit.gameObject);
            }
        }
    }
    
    void SmashObject(GameObject obj)
    {
        if (obj.transform.parent != null)
        {
            Destroy(obj.transform.parent.gameObject);
        }
        else
        {
            Destroy(obj);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (isDead) return;
        
        if (hit.gameObject.tag == "Coin")
        {
            Coin jellyScript = hit.gameObject.GetComponent<Coin>();
            if (jellyScript != null) score += jellyScript.scoreValue;
            else score += 5;

            scoreUI.text = score.ToString();
            CheckHighScore();
            
            AudioManager.instance.PlaySFX(coinSound);
            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.tag == "Potion")
        {
            currentEnergy += potionHeal;
            if (currentEnergy > maxEnergy) currentEnergy = maxEnergy;
            
            AudioManager.instance.PlaySFX(coinSound);
            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.tag == "Magnet")
        {
            StartCoroutine(ActivateMagnetRoutine());
            AudioManager.instance.PlaySFX(coinSound);
            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.tag == "Giant")
        {
            StartCoroutine(GiantRoutine());
            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.tag == "Blast")
        {
            if (blastCoroutine != null) StopCoroutine(blastCoroutine);
            
            blastCoroutine = StartCoroutine(BlastRoutine());
            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.tag == "DeathZone")
        {
            PlayerDie();
        }
    }
    
    IEnumerator GiantRoutine()
    {
        isGiant = true;
        transform.localScale = new Vector3(giantScale, giantScale, 1f);
        yield return new WaitForSeconds(giantDuration);
        transform.localScale = new Vector3(1f, 1f, 1f);
        isGiant = false;
    }
    
    IEnumerator BlastRoutine()
    {
        float baseMultiplier = isBlast ? (speedMultiplier / blastSpeedMultiplier) : speedMultiplier;
        
        isBlast = true;
        speedMultiplier = baseMultiplier * blastSpeedMultiplier;
        isMagnetActive = true;
        
        yield return new WaitForSeconds(blastDuration);
        
        speedMultiplier = baseMultiplier;
        isBlast = false;
        isMagnetActive = false;
        blastCoroutine = null;
    }
    
    void CheckHighScore()
    {
        if (score > highScore)
        {
            highScore = score; 
            if (highScoreUI != null) highScoreUI.text = "High Score: " + highScore.ToString();
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save(); 
        }
    }
    
    void PlayerDie()
    {
        isDead = true;
        currentEnergy = 0;
        if (energyBar != null) energyBar.value = 0;
        
        AudioManager.instance.PlaySFX(deathSound);
        animator.Play("Player_Die");
        
        rb.velocity = Vector2.zero;
        
        Invoke("ShowGameOverPanel", deathSound.length);
    }
    
    void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (finalScoreText != null)
            {
                finalScoreText.text = "SCORE: " + score.ToString();
            }
        }
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    IEnumerator ActivateMagnetRoutine()
    {
        isMagnetActive = true;
        yield return new WaitForSeconds(magnetDuration);
        isMagnetActive = false;
    }
    
    IEnumerator ActivateTemporaryInvincibilityRoutine()
    {
        isInvincible = true;
        
        int playerLayer = LayerMask.NameToLayer("Player");
        int obstacleLayer = LayerMask.NameToLayer("Obstacles");
        
        Physics2D.IgnoreLayerCollision(playerLayer, obstacleLayer, true);
        
        float timer = 0f;
        while (timer < invincibilityDuration)
        {
            Color oldColor = spriteRenderer.color;
            spriteRenderer.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0.2f);
            
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
            if (timer >= invincibilityDuration) break;
            
            spriteRenderer.color = new Color(oldColor.r, oldColor.g, oldColor.b, 1f);
            
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }
        
        Color finalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(finalColor.r, finalColor.g, finalColor.b, 1f);
        
        Physics2D.IgnoreLayerCollision(playerLayer, obstacleLayer, false);
        
        isInvincible = false;
    }
    
    void WaitForSceneLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}