using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set; }
    
    public AudioSource musicSource;
    public GameObject PauseMenu;

    void Awake()
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.transform.position = new Vector2(0, 0);
        }
    }
    void Start()
    {
        PauseMenu?.SetActive(false);

    }
 
    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
        UnityEngine.Debug.Log("Quit Game");
    }
    public void VolumeUp()
    {
        musicSource.volume += 0.1f;
        UnityEngine.Debug.Log("Volume Up: " + musicSource.volume);
    }
    public void VolumeDown()
    {
        musicSource.volume -= 0.1f;
        UnityEngine.Debug.Log("Volume Down: " + musicSource.volume);
    }
}