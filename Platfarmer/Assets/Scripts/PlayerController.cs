using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Collider2D touchingPlantCollider = null;
    private Collider2D touchingSproutCollider = null;
    private ShopManager shopManager;
  // prefab reference — never call SetActive on this
    public GameObject PauseMenu;
    public float moveSpeed = 5f;
    [SerializeField] public float jumpForce = 12f;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;
    private bool touchingPlant = false;
    private bool touchingSprout = false;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool farmGrounded = false;
    public int health = 10;
    public int score = 0;
    private int plantLocationX = 0;
    public int seedsPlanted = 0;
    public int plantsGrown = 0;

    bool gamePaused = false;

    public static PlayerController Instance;

    private GameObject sproutPrefab;
    private GameObject plantPrefab;

private void Awake()
{
    if (Instance != null)
    {
        Destroy(gameObject);
        return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);

    // Load prefabs from Resources — safe across scene loads
    sproutPrefab = Resources.Load<GameObject>("Sprout");
    plantPrefab  = Resources.Load<GameObject>("Plant");

    if (sproutPrefab == null) Debug.LogError("Sprout prefab not found in Resources folder!");
    if (plantPrefab  == null) Debug.LogError("Plant prefab not found in Resources folder!");
}
    void Start()
    {
        // FIX: removed sprout.SetActive(true) and Plant.SetActive(true)
        // Never call SetActive on a prefab reference — it modifies the asset, not a scene instance

        if (PauseMenu != null)
            PauseMenu.SetActive(false);

        rb = GetComponent<Rigidbody2D>();
        rb.position = new Vector2(0, 0);

        shopManager = FindObjectOfType<ShopManager>();

        if (healthText == null || scoreText == null)
        {
            healthText = GameObject.Find("HealthText")?.GetComponent<TextMeshProUGUI>();
            scoreText  = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        }

        UpdateUI();
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        AudioManager.Instance?.PlaySoundEffect(AudioManager.Instance.jumpSound);
    }

    void Update()
    {
        if (!gamePaused)
        {
            float moveInput = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        if (Input.GetButtonDown("Jump") && (isGrounded || farmGrounded) && !gamePaused)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce / 2);

        if (Input.GetButtonDown("Fire1") && farmGrounded && !gamePaused)
            PlantSeeds();

        if (Input.GetButtonDown("Fire2") && touchingSprout && touchingSproutCollider != null && !gamePaused)
            GrowPlant(touchingSproutCollider);

        if (Input.GetButtonDown("Fire3") && touchingPlant && touchingPlantCollider != null && !gamePaused)
            Harvest(touchingPlantCollider);

        if (Input.GetButtonDown("Fire4"))
            PauseGame();
        if (score >= 150 && SceneManager.GetActiveScene().name == "Level1")
            Level2();
        if (score >= 300 && SceneManager.GetActiveScene().name == "Level2")
            Win();
        }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Player landed on the ground");
        }
        if (collision.gameObject.CompareTag("Farm"))
        {
            farmGrounded = true;
            Debug.Log("Player landed on the farm");
        }
        if (collision.gameObject.CompareTag("sprout"))
        {
            touchingSprout = true;
            touchingSproutCollider = collision.collider;
            Debug.Log("Player is touching the sprout");
        }
        if (collision.gameObject.CompareTag("plant"))
        {
            touchingPlant = true;
            touchingPlantCollider = collision.collider;
            Debug.Log("Player is touching the plant");
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health -= 1;
            Debug.Log("Player hit by enemy");
            rb.position = new Vector2(0, 0);
            UpdateUI();

            if (health <= 0)
                GameOver();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Debug.Log("Player left the ground");
        }
        if (collision.gameObject.CompareTag("Farm"))
        {
            farmGrounded = false;
            Debug.Log("Player left the farm");
        }
        if (collision.gameObject.CompareTag("plant"))
        {
            touchingPlant = false;
            touchingPlantCollider = null;
            Debug.Log("Player stopped touching the plant");
        }
        if (collision.gameObject.CompareTag("sprout"))
        {
            touchingSprout = false;
            touchingSproutCollider = null;
            Debug.Log("Player stopped touching the sprout");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            GameManager.Instance?.AddScore(10);
            score += 10;
            Destroy(other.gameObject);
            UpdateUI();
        }
    }

   void PlantSeeds()
{
    if (shopManager != null && shopManager.seeds > 0 && farmGrounded)
    {
        seedsPlanted++;
        
        float playerBottom = GetComponent<Collider2D>().bounds.min.y;
        Vector3 spawnPos = new Vector3(transform.position.x + 1, playerBottom, 0f);
        
        Instantiate(sproutPrefab, spawnPos, Quaternion.identity);
        shopManager.seeds--;
        shopManager.updateText();
    }
    else
    {
        Debug.Log("No seeds to plant!");
    }
}

void GrowPlant(Collider2D other)
{
    if (shopManager != null && shopManager.fertilizer > 0
        && other.CompareTag("sprout") && other.gameObject != null && farmGrounded)
    {
        
        Vector3 spawnPos = other.transform.position;
        Destroy(other.gameObject);
        plantsGrown++;
        Instantiate(plantPrefab, spawnPos, Quaternion.identity); // ← plantPrefab not Plant
        shopManager.fertilizer--;
        shopManager.updateText();
    }
}

    void Harvest(Collider2D other)
    {
        if (other.CompareTag("plant"))
        {
            Destroy(other.gameObject);
            score += 10;
            GameManager.Instance?.AddScore(10);
            Debug.Log("Player harvested the plant");
            UpdateUI();
        }
    }



    void UpdateUI()
    {
        if (healthText != null) healthText.text = "Lives: " + health;
        if (scoreText != null)  scoreText.text  = "Score: " + (GameManager.Instance?.Score ?? score);
    }

    public void PauseGame()
    {
        gamePaused = !gamePaused;
        if (PauseMenu != null)
            PauseMenu.SetActive(gamePaused);
    }

    void GameOver()
    {
        PlayerPrefs.SetInt("FinalScore", score);
        SceneManager.LoadScene("GameOver");
    }

    public void Level2()
    {
        rb.position = new Vector2(0, 0);
        PlayerPrefs.SetInt("FinalScore", score);
        SceneManager.LoadScene("Level2");
    }

    void Win()
    {
        PlayerPrefs.SetInt("FinalScore", score);
        SceneManager.LoadScene("WinScreen");
    }
}