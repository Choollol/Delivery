using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PillarManager : MonoBehaviour
{
    [SerializeField] private List<Pillar> pillars;

    private bool doEnd = false;
    private void OnEnable()
    {
        EventMessenger.StartListening("RotatePillars", StartRotating);
        EventMessenger.StartListening("RotatePillarsEnd", EndRotation);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("RotatePillars", StartRotating);
        EventMessenger.StopListening("RotatePillarsEnd", EndRotation);
    }
    private void StartRotating()
    {
        EventMessenger.TriggerEvent("SetPlayerCanActFalse");
        EventMessenger.TriggerEvent("FadeBGM");
        StartCoroutine(Rotation());
    }
    private void EndRotation()
    {
        doEnd = true;
    }
    private IEnumerator Rotation()
    {
        GameManager.canPause = false;
        AudioManager.PlaySound("Altar Used Sound");
        yield return new WaitForSeconds(4);
        foreach (Pillar pillar in pillars)
        {
            pillar.doRotate = true;
        }
        AudioManager.PlaySound("Pillar Hum");
        while (!doEnd)
        {
            yield return null;
        }
        EventMessenger.TriggerEvent("SetPlayerCanActTrue");
        StartCoroutine(AudioManager.FadeAudio("Pillar Hum", 0.5f, 0));
        GameManager.canPause = true;
        yield break;
    }
}
