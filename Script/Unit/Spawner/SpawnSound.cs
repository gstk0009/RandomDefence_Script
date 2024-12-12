using System.Collections;
using UnityEngine;

public class SpawnSound : MonoBehaviour, ISoundPlayer
{
    public AudioSource audioSource;
    public AudioClip summonSoundClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySummonSound()
    {
        if (audioSource != null && summonSoundClip != null)
        {
            audioSource.PlayOneShot(summonSoundClip);
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is not assigned!");
        }
    }
}
