using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    public float fadeDuration;
    public float transitionDuration;

    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        StartCoroutine(PlayTransition());
    }
    private IEnumerator PlayTransition()
    {
        image.color = new Color(0, 0, 0, 0);
        float lerpCounter = 0;
        while (image.color.a < 1)
        {
            image.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, lerpCounter / fadeDuration));
            lerpCounter += Time.deltaTime;
            yield return null;
        }
        GameManager.Instance.doContinueTransition = true;
        yield return new WaitForSeconds(transitionDuration);
        while (image.color.a > 0)
        {
            image.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, lerpCounter / fadeDuration));
            lerpCounter -= Time.deltaTime;
            yield return null;
        }
        GameManager.Instance.EndTransition();
        yield break;
    }
}
