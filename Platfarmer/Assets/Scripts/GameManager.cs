using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class GameData
{
    public string playerName;
    public int currentLevel;
    public int totalScore;
    public int health;          // Fix #2: persist health
    public float playTime;
    public bool[] unlockedLevels;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action<int> onScoreChanged;
    public event Action<int> onHealthChanged;
    public event Action onGameOver;

    // Fix #1: single source of truth — read directly from gameData
    public int Score => gameData?.totalScore ?? 0;
    public int Health => gameData?.health ?? 100;

    private GameData gameData;
    private string savePath;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "gamesave.json");
        LoadGame();
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved!");
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            gameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game loaded!");
        }
        else
        {
            gameData = new GameData
            {
                playerName = "Player",
                currentLevel = 1,
                totalScore = 0,
                health = 100,               // Fix #2: initialise health in save data
                playTime = 0f,
                unlockedLevels = new bool[10]
            };
            gameData.unlockedLevels[0] = true;
            Debug.Log("No save file found. Starting new game.");
        }
    }

    public void AddScore(int amount)
    {
        gameData.totalScore += amount;      // Fix #1: one field, not two
        onScoreChanged?.Invoke(gameData.totalScore);
    }

    public void TakeDamage(int damage)
    {
        gameData.health = Mathf.Max(0, gameData.health - damage); // Fix #3: clean clamp
        AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.damageSound);
        onHealthChanged?.Invoke(gameData.health);

        if (gameData.health == 0)
            TriggerGameOver();
    }

    public void ResetGame()
    {
        gameData.totalScore = 0;
        gameData.health = 100;

        onScoreChanged?.Invoke(gameData.totalScore);
        onHealthChanged?.Invoke(gameData.health);
    }

    public void TriggerGameOver()
    {
        onGameOver?.Invoke(); // Fix #5
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}