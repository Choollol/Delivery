using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    private static VolumeManager instance;
    public static VolumeManager Instance
    {
        get { return instance; }
    }

    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;

    public static float sfxVolume { get; private set; }
    public static float bgmVolume { get; private set; }

    [SerializeField] private AudioMixer mixer;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        sfxSlider.value = sfxVolume = 0.5f;
        bgmSlider.value = bgmVolume = 0.5f;
        UpdateVolume();
    }
    public void UpdateVolume()
    {
        sfxVolume = sfxSlider.value;
        if (sfxVolume == 0)
        {
            sfxVolume = 0.001f;
        }
        bgmVolume = bgmSlider.value;
        if (bgmVolume == 0)
        {
            bgmVolume = 0.001f;
        }

        mixer.SetFloat("sfxVolume", Mathf.Log10(sfxVolume) * 20);
        mixer.SetFloat("bgmVolume", Mathf.Log10(bgmVolume) * 20);
    }
    public void UpdateSliders()
    {
        sfxSlider.value = sfxVolume;
        bgmSlider.value = bgmVolume;
    }
    public void SetVolumes(float newSFXVolume, float newBGMVolume)
    {
        sfxVolume = newSFXVolume;
        bgmVolume = newBGMVolume;
        UpdateSliders();
    }
}
