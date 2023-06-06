using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Camera mainCamera;
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        transform.SetPosition(mainCamera.transform.position);
    }
}
