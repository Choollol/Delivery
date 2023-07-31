using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSwitcher : MonoBehaviour
{
    [SerializeField] private GameManager.Area newArea;
    [SerializeField] private SpriteAnimator.Direction direction;

    private bool canSwitch = true;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canSwitch)
        {
            if (collision.CompareTag("Player") || (collision.CompareTag("Vehicle") && GameManager.isPlayerInVehicle))
            {
                if (collision.GetComponent<SpriteAnimator>().direction == direction)
                {
                    GameManager.Instance.SwitchArea(newArea);
                    //gameObject.SetActive(false);
                    canSwitch = false;
                }
            }
            else if (collision.transform.parent.CompareTag("Player"))
            {
                if (collision.transform.parent.GetComponent<SpriteAnimator>().direction == direction)
                {
                    GameManager.Instance.SwitchArea(newArea);
                    //gameObject.SetActive(false);
                    canSwitch = false;
                }
            }
        }
    }
}
