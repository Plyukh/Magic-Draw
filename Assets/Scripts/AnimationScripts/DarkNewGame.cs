using UnityEngine;

public class DarkNewGame : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip[] clips;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(int index)
    {
        audioSource.clip = clips[index];
        audioSource.Play();
    }
}
