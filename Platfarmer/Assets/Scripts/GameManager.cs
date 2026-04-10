using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action<int> onScoreChanged;
    public event Action<int> onHealthChanged;
    public event Action onGameOver;

    private int score = 0;
    private int health = 100;

    public int Health => health;
    public int Score => score;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void AddScore(int points)
    {
        score += points;
        onScoreChanged?.Invoke(score);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
            health = 0;

        onHealthChanged?.Invoke(health);
    }

    public void ResetGame()
    {
        score = 0;
        health = 100;

        onScoreChanged?.Invoke(score);
        onHealthChanged?.Invoke(health);
    }

    public void TriggerGameOver()
    {
        onGameOver?.Invoke();
    }
    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Coin"))
    {
        // Instead of Destroy(other.gameObject);
        CoinPoolManager.Instance.ReturnCoin(other.gameObject);
        
        // Update score via GameManager
        GameManager.Instance.AddScore(1); 
    }
}

// Inside your Jump method
public void Jump()
{
    // ... existing jump logic ...
    AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.jumpSound);
}
public int GetScore()
{
    return score;
}
}