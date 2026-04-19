using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    


    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;


    [Header("Audio Clips")]
    public AudioClip coinSound;
    public AudioClip jumpSound;
    public AudioClip damageSound;
    public AudioClip winSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}