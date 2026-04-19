using UnityEngine;
using UnityEngine.SceneManagement;


public class ShowHighScores : MonoBehaviour
{
   public void Scores()
    {
        SceneManager.LoadScene("HighScores");
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}