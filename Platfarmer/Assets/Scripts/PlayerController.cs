using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private ShopManager shopManager;
    public GameObject sprout;
    public GameObject Plant;
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;
    private bool touchingPlant = false;
    private bool touchingSprout = false;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool farmGrounded = false;
    private int plantHeight = 0;
    private int health = 100;
    public int score = 0;
    private int plantLocationX = 0;
    
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
        // Use a valid input for growing plant, e.g., another button like "Fire2" (must be set in Input Manager)
        if (Input.GetButtonDown("Fire2") && (touchingSprout))
        {
            growPlant();
        }
        if (Input.GetButtonDown("Fire2") && (touchingPlant))
        {
            makePlantTaller();
        }
        if (Input.GetButtonDown("Vertical") && (touchingPlant))
        {
            climbPlant();
        }
        if (Input.GetButtonDown("Fire3") && (touchingPlant))
        {
            Harvest();
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
        if (collision.gameObject.CompareTag("sprout"))
        {
            touchingSprout = true;
            UnityEngine.Debug.Log("Player is touching the sprout");
        }
        if (collision.gameObject.CompareTag("plant"))
        {
            touchingPlant = true;
            UnityEngine.Debug.Log("Player is touching the plant");
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
    void Harvest()
    {
        if (touchingPlant)
        {
            score += 20; // Increase score for harvesting
            plantHeight = 0; // Reset plant height after harvesting
            Destroy(GameObject.FindWithTag("plant")); // Remove the plant from the scene
            UnityEngine.Debug.Log("Player harvested the plant");
            UpdateUI();
        }
    }
    void climbPlant()
    {
        if (touchingPlant || touchingSprout)
        {
            if (Input.GetAxis("Vertical") > 0) // Check if player is pressing up
            {
                rb.position += new Vector2(0, plantHeight); // Move player up by the plant's height
                UnityEngine.Debug.Log("Player climbed the plant");
            }
            else if (Input.GetAxis("Vertical") < 0) // Check if player is pressing down
            {
                rb.position -= new Vector2(0, plantHeight); // Move player down by the plant's height
                UnityEngine.Debug.Log("Player climbed down the plant");
            }

        }
    }
    void PlantSeeds()
    { 
        if (shopManager != null && shopManager.seeds > 0)
        {
            plantLocationX = (int)transform.position.x; // Get player's current X position to plant at that location
            plantHeight = 0; // Reset plant height when planting new seeds
            Instantiate(sprout, transform.position + new Vector3(0, plantHeight, 0), Quaternion.identity);
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
    void growPlant()
    {
        if (touchingSprout && farmGrounded)
        {
            UnityEngine.Debug.Log("Touching the sprout");
            if (shopManager != null && shopManager.fertilizer > 0)
            {
                
                plantHeight += 1; // Update plant height based on its scale
                Instantiate(Plant, transform.position + new Vector3(1, plantHeight, 0), Quaternion.identity);
                shopManager.fertilizer--;
                UnityEngine.Debug.Log("Player fertilized the plant");
                shopManager.updateText();
                if (shopManager.fertilizer == 0)
                {
                    UnityEngine.Debug.Log("No more fertilizer to use!");
                }
            }
            else
            {
                UnityEngine.Debug.Log("No fertilizer to use!");
            }
        }
    }
    void makePlantTaller()
    {
        if (touchingPlant && farmGrounded)
        {
            if (shopManager != null && shopManager.fertilizer > 0)
            {

            plantHeight += 1; // Update plant height based on its scale
            Instantiate(Plant, transform.position + new Vector3(1, plantHeight, 0), Quaternion.identity);
            shopManager.fertilizer--;
            UnityEngine.Debug.Log("Player fertilized the plant");
            shopManager.updateText();
            if (shopManager.fertilizer == 0)
            {
                UnityEngine.Debug.Log("No more fertilizer to use!");
            }
        }
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
        if (collision.gameObject.CompareTag("plant"))
        {
            touchingPlant = false;
            UnityEngine.Debug.Log("Player stopped touching the plant");
        }
        if (collision.gameObject.CompareTag("sprout"))
        {
            touchingSprout = false;
            UnityEngine.Debug.Log("Player stopped touching the sprout");
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
