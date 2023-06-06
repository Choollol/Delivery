using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get { return instance; }
    }
    

    private static Dictionary<string, AudioSource> sounds = new Dictionary<string, AudioSource>();

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
        GameObject soundsHolder = GameObject.Find("Sounds Holder");
        for (int i = 0; i < soundsHolder.transform.GetChild(0).childCount; i++)
        {
            sounds.Add(soundsHolder.transform.GetChild(0).GetChild(i).name, soundsHolder.transform.GetChild(0).GetChild(i).GetComponent<AudioSource>());
        }
        for (int i = 0; i < soundsHolder.transform.GetChild(1).childCount; i++)
        {
            sounds.Add(soundsHolder.transform.GetChild(1).GetChild(i).name, soundsHolder.transform.GetChild(1).GetChild(i).GetComponent<AudioSource>());
        }
        
    }
    
    public static void PlaySound(string key)
    {
        sounds[key].Play();
    }
    public static void PlaySound(string key, float minPitch, float maxPitch)
    {
        sounds[key].pitch = Random.Range(minPitch, maxPitch);
        sounds[key].Play();
    }
    public static void StopSound(string key)
    {
        sounds[key].Stop();
    }
    public static AudioSource GetSound(string key)
    {
        return sounds[key];
    }

    /*public IEnumerator FadeAudio(string key, float duration, float targetVolume)
    {
        if (sounds[key].volume == targetVolume) { yield break; }
        float currentTime = 0;
        float start = sounds[key].volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            sounds[key].volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        if (sounds[key].volume == 0)
        {
            sounds[key].Stop();
        }
        yield break;
    }*/
}
