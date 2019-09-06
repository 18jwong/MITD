using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volSlider;

    private void Start()
    {
        float lastSetVolume = PlayerPrefs.GetFloat("volume");
        volSlider.value = lastSetVolume;
        audioMixer.SetFloat("volume", lastSetVolume);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("volume", volume);
    }
}
