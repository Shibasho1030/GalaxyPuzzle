using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip rotateSE;
    public AudioClip bottomSE;
   
    public AudioClip gameOverSE;
    public AudioClip uiSE;
    public AudioClip gameStartSE;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void RotateSound()
    {
        audioSource.PlayOneShot(rotateSE);
    }

    public void BottomSound()
    {
        audioSource.PlayOneShot(bottomSE);
    }
    
    public void GameOverSound()
    {
        audioSource.PlayOneShot(gameOverSE);
    }

    public void UiSound()
    {
        audioSource.PlayOneShot(uiSE);
    }

    public void GameStartSound()
    {
        audioSource.PlayOneShot(gameStartSE);
    }

    
}
