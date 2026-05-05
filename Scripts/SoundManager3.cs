using UnityEngine;

public class SoundManager3 : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip bomSE;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void BomSound()
    {
        audioSource.PlayOneShot(bomSE); 
    }

}