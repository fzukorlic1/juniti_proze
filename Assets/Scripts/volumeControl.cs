using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class volumeControl : MonoBehaviour
{
    public AudioMixer mixer;

    public void setVolume(float value) {
        mixer.SetFloat("soundVol", Mathf.Log10 (value) * 20);
    }
}
