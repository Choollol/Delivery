using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSwitcher : MonoBehaviour
{
    [SerializeField] private GameManager.Area newArea;
    [SerializeField] private SpriteAnimator.Direction direction;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Vehicle"))
        {
            if (collision.GetComponent<SpriteAnimator>().direction == direction)
            {
                GameManager.Instance.SwitchArea(newArea);
                gameObject.SetActive(false);
            }
        }
    }
}
