using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractionManager : MonoBehaviour
{
    private static InteractionManager instance;
    public static InteractionManager Instance
    {
        get { return instance; }
    }
    private bool hasInteracted;
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
    public void Interact(Vector2 position)
    {
        if (!GameManager.isGameActive || hasInteracted)
        {
            return;
        }
        var interactables = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>();
        foreach (IInteractable interactable in interactables)
        {
            if (Vector2.Distance((interactable as MonoBehaviour).transform.position, position) <= interactable.interactRange)
            {
                interactable.OnInteract();
                hasInteracted = true;
                StartCoroutine(SetHasInteractedFalse());
                return;
            }
        }
    }
    private IEnumerator SetHasInteractedFalse()
    {
        yield return new WaitForEndOfFrame();
        hasInteracted = false;
        yield break;
    }
    /*public void Interact(BoxCollider2D boxCollider)
    {
        var interactables = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>();
        foreach (IInteractable interactable in interactables)
        {
            if ((interactable as MonoBehaviour).GetComponent<BoxCollider2D>().IsTouching(boxCollider))
            {
                interactable.OnInteract();
            }
        }
    }*/
}
