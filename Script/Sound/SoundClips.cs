using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundClips : MonoBehaviour
{
    public AudioClip[] clips;

    void Start()
    {
        SoundManager.Instance.RegisterSoundEffect("ElfShooting", clips[0]);
        SoundManager.Instance.RegisterSoundEffect("OrcShooting", clips[1]);
        SoundManager.Instance.RegisterSoundEffect("HumanShooting", clips[2]);
        SoundManager.Instance.RegisterSoundEffect("PickSound", clips[3]);
    }
}
