using Unity.Netcode;
using UnityEngine;

public class NetworkManager : NetworkBehaviour
{
    public static NetworkManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }
}
