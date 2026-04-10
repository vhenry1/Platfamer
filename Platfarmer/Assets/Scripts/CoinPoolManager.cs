using UnityEngine;
using System.Collections.Generic;

public class CoinPoolManager : MonoBehaviour
{
    public static CoinPoolManager Instance { get; private set; }
    
    [SerializeField] private ObjectPool coinPool;
    [SerializeField] private Transform[] spawnPoints;
    private List<GameObject> activeCoins = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ResetAllCoins();
    }

    public void ResetAllCoins()
    {
        // Return all currently active coins to pool
        foreach (GameObject coin in activeCoins)
        {
            coinPool.ReturnObject(coin);
        }
        activeCoins.Clear();

        // Re-spawn at original points
        foreach (Transform point in spawnPoints)
        {
            GameObject coin = coinPool.GetObject();
            coin.transform.position = point.position;
            activeCoins.Add(coin);
        }
    }

    public void ReturnCoin(GameObject coin)
    {
        coinPool.ReturnObject(coin);
        activeCoins.Remove(coin);
    }
}