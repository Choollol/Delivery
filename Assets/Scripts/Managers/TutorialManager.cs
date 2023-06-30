using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPartsHolder;
    private List<GameObject> tutorialParts = new List<GameObject>() ;

    [SerializeField] private string onFinishEventName;

    [SerializeField] private GameObject tutorialUI;

    private int index = 0;
    private bool doContinue;

    private void OnEnable()
    {
        EventMessenger.StartListening("SkipTutorial", SkipTutorial);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("SkipTutorial", SkipTutorial);
    }
    private void Start()
    {
        for (int i = 0; i < tutorialPartsHolder.transform.childCount; i++)
        {
            tutorialParts.Add(tutorialPartsHolder.transform.GetChild(i).gameObject);
            tutorialParts[i].SetActive(false);
        }

        tutorialParts[0].SetActive(true);
        StartCoroutine("PlayTutorial");
    }
    private void Update()
    {
        if (Input.anyKeyDown && !Input.GetButtonDown("Cancel"))
        {
            doContinue = true;
        }
    }
    private void SkipTutorial()
    {
        EventMessenger.TriggerEvent(onFinishEventName);
        index = tutorialParts.Count;
        onFinishEventName = "";
        tutorialUI.SetActive(false);
        StopCoroutine("PlayTutorial");
    }
    private IEnumerator PlayTutorial()
    {
        doContinue = false;
        while (!doContinue)
        {
            yield return null;
        }
        tutorialParts[index].SetActive(false);
        index++;

        if (index < tutorialParts.Count)
        {
            tutorialParts[index].SetActive(true);
            StartCoroutine("PlayTutorial");
        }
        else if (onFinishEventName != "")
        {
            tutorialUI.SetActive(false);
            EventMessenger.TriggerEvent(onFinishEventName);
        }
        yield break;
    }
}
