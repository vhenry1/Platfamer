using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasOverlay : MonoBehaviour
{
    public GameObject canvasOverlay;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1" || scene.name == "Level2")
        {
            canvasOverlay.SetActive(true);
        }
        else
        {
            canvasOverlay.SetActive(false);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}