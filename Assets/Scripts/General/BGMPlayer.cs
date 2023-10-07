using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    string key;
    void Start()
    {
        key = gameObject.scene.name + " Theme";
        switch (Random.Range(0, 2))
        {
            case 0:
                {
                    switch (Random.Range(0, 2))
                    {
                        case 0:
                            {
                                StartCoroutine(DelayBGM(1.5f));
                                break;
                            }
                        case 1:
                            {
                                StartCoroutine(DelayBGM(Random.Range(5f, 20f)));
                                break;
                            }
                    }
                    break;
                }
        }
    }
    private IEnumerator DelayBGM(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        while (!GameManager.isInWorld)
        {
            yield return null;
        }
        AudioManager.GetSound(key).volume = 1;
        AudioManager.PlaySound(key);
        yield break;
    }
}
