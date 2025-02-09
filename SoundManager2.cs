using UnityEngine;

public class SoundManager2 : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip clearRowSE;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ClearRowSound()
    {
        audioSource.PlayOneShot(clearRowSE);
    }


}
