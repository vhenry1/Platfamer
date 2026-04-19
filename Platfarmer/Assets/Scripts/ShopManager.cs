using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.ComponentModel;


public class ShopManager : MonoBehaviour
{
    private static ShopManager instance;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI shopText;
    public GameObject Panel;
    private Rigidbody2D rb;
    private bool isPlayerInShop = false;
    public int coins = 0;
    public int fertilizer = 0;
    public int seeds = 0;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        updateText();
        DeactivateShop();
        
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            OnLoadLevel1();
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            OnLoadLevel2();
        }
    }
    void OnLoadLevel1()
    {

        rb.position = new Vector2(84, -5.5f);
        
    }
    void OnLoadLevel2()
    {
        
        rb.position = new Vector2(39, 2);
        
    }
    
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInShop = true;
            UnityEngine.Debug.Log("Player entered shop");
            Panel.SetActive(true);
            // No longer reset coins from PlayerController score
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            Level2Manager level2Manager = FindObjectOfType<Level2Manager>();
            if (SceneManager.GetActiveScene().name != "Level2" || level2Manager == null || coins >= level2Manager.score) // Only update coins if not in Level2 and Level2Manager's score is higher
            {
                coins = playerController.score - fertilizer * 5 - seeds * 2 - playerController.plantsGrown * 5 - playerController.seedsPlanted * 2; // Keep existing coins if in Level2 or Level2Manager's score is not higher
            }
            else
            {
                coins = level2Manager.score - fertilizer * 5 - seeds * 2 - playerController.plantsGrown * 5 - playerController.seedsPlanted * 2; // Deduct costs of fertilizer and seeds from score to calculate coins
            }
            
            if (playerController != null)
            {
                if (coins < playerController.score) // Only update coins if PlayerController's score is higher
                {
                  
                }
                else
                {
                    coins = coins; // Keep existing coins if PlayerController's score is not higher
                }
            }
            else
            {
                coins = 0;
            }
            updateText();
        }
    
    }
    
     void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInShop = false;
                Panel.SetActive(false);
            UnityEngine.Debug.Log("Player left the shop");
        }
    }
    void DeactivateShop()
    {
        Panel.SetActive(false);
    }
    
    public void BuyFertilizer()
    {
        if (coins >= 5)
        {
            coins -= 5;
            fertilizer += 1;
            UnityEngine.Debug.Log("Bought Fertilizer");
            updateText();
        }
        else
        {
            UnityEngine.Debug.Log("Not enough coins for Fertilizer");
        }
    }
    public void BuySeeds()
    {
        if (coins >= 2)
        {
            coins -= 2;
            seeds += 1;
            UnityEngine.Debug.Log("Bought Seeds");
            updateText();
        }
        else
        {
            UnityEngine.Debug.Log("Not enough coins for Seeds");
        }
    }
    public void updateText()
    {

        shopText.text = "Fertilizer:" + fertilizer;
        shopText.text += "\nSeeds:" + seeds;
        coinsText.text = "Coins: " + coins;
    }
    
}