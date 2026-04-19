using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TextMeshProUGUI finalScoreText;

    void Start()
    {
        int finalScore = GameManager.Instance.Score; // ← use Instance.Score
        finalScoreText.text = "Final Score: " + finalScore;
    }

    public void OnSubmitScore()
{
    string playerName = playerNameInput.text;

    if (string.IsNullOrEmpty(playerName))
        playerName = "Anonymous";

    int finalScore = GameManager.Instance?.Score ?? 0;
    float completionTime = Time.timeSinceLevelLoad;

    if (DatabaseManager.Instance == null)
    {
        Debug.LogError("DatabaseManager.Instance is null — is it in the scene?");
        return;
    }

    DatabaseManager.Instance.SaveHighScore(playerName, finalScore, completionTime);
    SceneManager.LoadScene("HighScores");
}
}