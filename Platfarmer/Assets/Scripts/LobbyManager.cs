using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public void MoveFromLobby()
    {
        SceneManager.LoadScene("Level1");
        gameObject.SetActive(false);
        UnityEngine.Debug.Log("Move from Lobby button clicked, loading Level1");
    }

}