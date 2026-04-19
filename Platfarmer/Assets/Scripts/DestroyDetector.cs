using UnityEngine;

public class DestroyDetector : MonoBehaviour
{
    void OnDestroy()
    {
        Debug.LogError("Sprout destroyed by: " + gameObject.name);
        UnityEngine.Debug.Break(); // pauses the editor the moment it's destroyed
    }
}