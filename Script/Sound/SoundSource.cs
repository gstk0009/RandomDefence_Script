using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

internal class SoundSource : MonoBehaviour
{
    private AudioSource audioSource;

    public void Play(AudioClip clip, float soundEffectVolume, float soundEffectPitchVariance, AudioMixerGroup audioMixerGroup)
    {
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        CancelInvoke();
        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.clip = clip;
        audioSource.volume = soundEffectVolume;
        audioSource.pitch = 1f + Random.Range(-soundEffectPitchVariance, soundEffectPitchVariance);
        audioSource.Play();

        Invoke("Disable", clip.length + 2);
    }

    public void Disable()
    {
        audioSource.Stop();
        gameObject.SetActive(false);
    }
}
