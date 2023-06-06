using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAudioController : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void PlayStartAudio()
    {
        StartCoroutine("HandlePlayStartAudio");
    }
    private IEnumerator HandlePlayStartAudio()
    {
        AudioManager.PlaySound("Vehicle Ignition");
        AudioSource vehicleIgnition = AudioManager.GetSound("Vehicle Ignition");
        while (vehicleIgnition.isPlaying)
        {
            yield return null;
        }
        AudioManager.PlaySound("Vehicle Engine");
        yield break;
    }
    public void PlayExitAudio()
    {
        StopCoroutine("HandlePlayStartAudio");
        AudioManager.StopSound("Vehicle Engine");
        AudioManager.StopSound("Vehicle Ignition");
        AudioManager.PlaySound("Vehicle Exit");
    }
}
