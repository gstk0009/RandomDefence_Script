using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSceneInitializer : MonoBehaviour
{
    private void Start()
    {
        SoundManager audioManager = SoundManager.Instance;
        if (audioManager != null)
        {
            audioManager.ReinitializeSliders();
        }
    }
}
