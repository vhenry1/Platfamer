using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private ShopManager shopManager;
    public GameObject Plant;
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;
    
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool farmGrounded = false;
    private int health = 100;
    public int score = 0;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        UpdateUI();
        shopManager = FindObjectOfType<ShopManager>();
    }
    private static PlayerController Instance;
    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject); // Prevent duplicate players
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes
    }
    void Update()
    {
        // Horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        
        // Jumping
        if (Input.GetButtonDown("Jump") && (isGrounded || farmGrounded))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce/2);
        }

        // Planting seeds
        if (Input.GetButtonDown("Fire1") && (farmGrounded))
        {
            PlantSeeds();
        }   
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            UnityEngine.Debug.Log("Player landed on the ground");
        }
        if (collision.gameObject.CompareTag("Farm"))
        {
            farmGrounded = true;
            UnityEngine.Debug.Log("Player landed on the farm");
        }
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health -= 10;
            UnityEngine.Debug.Log("Player hit by enemy");
            rb.position = new Vector2(90, 0);
            UpdateUI();
            
            if (health <= 0)
            {
                GameOver();
            }
        }
    }
    void PlantSeeds()
    {
        if (shopManager != null && shopManager.seeds > 0)
        {
            Instantiate(Plant, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
            shopManager.seeds--;
            UnityEngine.Debug.Log("Player planted seeds");
            shopManager.updateText();
            if (shopManager.seeds == 0)
            {
                UnityEngine.Debug.Log("No more seeds to plant!");
            }
        }
        else
        {
            UnityEngine.Debug.Log("No seeds to plant!");
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            UnityEngine.Debug.Log("Player left the ground");
        }
        if (collision.gameObject.CompareTag("Farm"))
        {
            farmGrounded = false;
            UnityEngine.Debug.Log("Player left the farm");
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            score += 10;
            Destroy(other.gameObject);
            UpdateUI();
            if (score == 80)
            {
                Win();
            }
        }
    }
    
    void UpdateUI()
    {
        healthText.text = "Health: " + health;
        scoreText.text = "Score: " + score;
    }
    
    void GameOver()
    {
        PlayerPrefs.SetInt("FinalScore", score);
        SceneManager.LoadScene("GameOver");
    }
    void Win()
    {
        PlayerPrefs.SetInt("FinalScore", score);
        SceneManager.LoadScene(3);
    }
}