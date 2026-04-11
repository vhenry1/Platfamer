using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI shopText;
    public GameObject Panel;
    private Rigidbody2D rb;
    private bool isPlayerInShop = false;
    public int coins = 0;
    public int fertilizer = 0;
    public int seeds = 0;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        updateText();
        DeactivateShop();
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
            if (playerController != null)
            {
                if (coins < playerController.score) // Only update coins if PlayerController's score is higher
                {
                    coins = playerController.score - fertilizer * 5 - seeds * 2 - playerController.plantsGrown*5 - playerController.seedsPlanted*2; // Deduct costs of fertilizer and seeds from score to calculate coins
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