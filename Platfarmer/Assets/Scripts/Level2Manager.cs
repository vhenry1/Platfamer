using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Level2Manager : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;
    public GameObject panel;
    private GameObject player;
    public GameObject PauseMenu;
    public int startingHealth;
    public int score;
    public int startingScore;
    public int health;

    void Start()
    {

        Update();
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                health = playerController.health;
                score = playerController.score;
            }
            else
            {
                health = startingHealth;
                score = startingScore;
            }
        }
        else
        {
            health = startingHealth;
            score = startingScore;
        }

    }
    void Update()
    {
        UpdateUI();
        PauseMenu.SetActive(false);
    }
    void UpdateUI()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        health = PlayerController.Instance != null ? PlayerController.Instance.health : 0;
        score = PlayerController.Instance != null ? PlayerController.Instance.score : 0;    
        healthText.text = "Health: " + health;
        scoreText.text = "Score: " + score;
    }
    void PlantSeeds()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.seedsPlanted++;
            UpdateUI();
        }
    }
    
}