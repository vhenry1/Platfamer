using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    public AudioManager audioManagerPrefab;
    void Start()
    {
        // Ensure AudioManager exists in the scene
        if (AudioManager.Instance == null)
        {
            AudioManager instance = Instantiate(audioManagerPrefab);
            DontDestroyOnLoad(instance.gameObject);
        }
        SceneManager.LoadScene("Menu");
    }
}