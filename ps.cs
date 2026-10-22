using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;      
    [SerializeField] private float turnSpeed = 150f;    
    [SerializeField] private float doubleJumpForce = 4f;
    [SerializeField] private float minJumpForce = 5f;      
    [SerializeField] private float maxJumpForce = 10f;     
    [SerializeField] private float maxHoldTime = 3f;      
    [SerializeField] private float runMultiplier = 2f; 
    [Header("Ground Check Settings")]
    [SerializeField] private float groundDistance = 0.5f; 
    [SerializeField] private LayerMask groundMask;

//Goes on player
    private Rigidbody rb;               
    private Vector3 moveDirection;      
    private float turnInput;            
    private bool isGrounded;            
    private bool jumpRequested = false; 
    private bool doubleJumpRequested = false; 
    private bool hasDoubleJumped = false;     
    private float currentJumpForce = 0f; 
    private float jumpHoldTime = 0f; 
    private bool isChargingJump; 

    public bool IsGrounded => isGrounded;

    public float SpeedMultiplier { get; set; } = 1f;
    public bool CanDoubleJump {get; set;} = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        //Ground Check
        isGrounded = Physics.Raycast(transform.position + transform.up*groundDistance/2, -transform.up, groundDistance, groundMask);

        if (isGrounded)
        {
            hasDoubleJumped = false;
        }


        // 2. Read Inputs
        float moveX = Input.GetAxisRaw("Horizontal"); 
        float moveZ = Input.GetAxisRaw("Vertical"); 


        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        moveDirection = cameraForward * moveZ + cameraRight * moveX;

        if (moveDirection.magnitude > 0.0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, 
                targetRotation, 
                turnSpeed * Time.deltaTime);
        }

        //Jump Input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isChargingJump = true;
            jumpHoldTime = 0f;
        }

        if (isChargingJump)
        {
            if (!isGrounded)
            {
                isChargingJump = false;
            }
            else
            {
                jumpHoldTime += Time.deltaTime;
                bool realesed = Input.GetKeyUp(KeyCode.Space);
                bool timeIsUp = jumpHoldTime >= maxHoldTime;
                

                if (realesed || timeIsUp)
                {
                    float clampedHoldTime = Mathf.Clamp01(jumpHoldTime / maxHoldTime);
                    currentJumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, clampedHoldTime);
                    jumpRequested = true;
                    isChargingJump = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
 
        if (jumpRequested)
        {
            Jump(currentJumpForce);
            currentJumpForce = 0f;
            jumpRequested = false;
        }
 
        if (doubleJumpRequested)
        {
            Jump(doubleJumpForce);
            doubleJumpRequested = false;
        }
    }
 
    private void Jump(float force)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f,
                                        rb.linearVelocity.z);
        rb.AddForce(Vector3.up * force, ForceMode.VelocityChange);
    }


    private void MovePlayer()
    {
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            SpeedMultiplier = 2f;
        }
        else
        {
            SpeedMultiplier = 1f;
        }

        float runFactor = Input.GetKey(KeyCode.LeftShift) ? runMultiplier : 1f;

        Vector3 targetVelocity = moveDirection * moveSpeed * SpeedMultiplier * runFactor;

        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }

    public bool TryDoubleJump()
    {
        if (!CanDoubleJump) return false;

        if (isGrounded || hasDoubleJumped) return false;
 
        doubleJumpRequested = true;
        hasDoubleJumped = true;
        return true;
    }

}

using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text finalScoreText;
 //Goes on EndScreen
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        finalScoreText.text = "Final Score: " + GameManager.FinalScore;
    }
}

using UnityEngine;

public class FallZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance.RestartLevel();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
//goes on GameManager
    public static int FinalScore { get; private set; }

    public int Score { get; private set; }

    [SerializeField] private string firstLevelScene = "Level1";

    private int scoreAtLevelStart;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    public void AddScore(int amount)
    {
        Score += amount;
    }

    public void SaveFinalScore()
    {
        FinalScore = Score;
    }

    public void StartNewGame()
    {
        Score = 0;
        scoreAtLevelStart = 0;
        SceneManager.LoadScene(firstLevelScene);
    }

    public void LoadNextLevel()
    {
        scoreAtLevelStart = Score;   
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        Score = scoreAtLevelStart;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

using UnityEngine;
using TMPro;
 
public class HUDController : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;

    [Header("Health")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private PlayerHealth playerHealth;
//goes on HUD
 
    private void Update()
    {
        UpdateScore();
        UpdateHealth();
    }
 
    private void UpdateScore()
    {
        // The Score property from Lab 4: anyone can read it
        scoreText.text = "Score: " + GameManager.Instance.Score;
    }

    private void UpdateHealth()
    {
        healthText.text = "Health: " + playerHealth.CurrentHealth;
    }
 
}

using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance.SaveFinalScore();
        GameManager.Instance.LoadNextLevel();
    }
}

using UnityEngine;

//coin 
public class ScorePickup : MonoBehaviour
{
    [SerializeField] private int points = 50;
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        int multiplier = 1;
        // PlayerPowerUps powerUps = other.GetComponent<PlayerPowerUps>();
        // if (powerUps != null)
        // {
        //     multiplier = powerUps.ScoreMultiplier;
        // }

        GameManager.Instance.AddScore(points * multiplier);

        // PlayClipAtPoint keeps playing even though we destroy the coin next
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        Destroy(gameObject);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

/// Manages the player's health and handles the game over state.
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float restartDelay = 2f;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;

    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int damage = 1)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth -= damage;
        if (CurrentHealth < 0) CurrentHealth = 0;

        if (CurrentHealth == 0)
        {
            Invoke(nameof(GameOver), restartDelay);
        }
    }

    private void GameOver()
    {
        GameManager.Instance.RestartLevel();
    }
}