using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public enum FollowTarget
    {
        Player, Vehicle
    }

    private static CameraMovement instance;
    public static CameraMovement Instance
    {
        get { return instance; }
    }

    private FollowTarget followTarget;
    private GameObject targetGameObject;
    private SpriteRenderer targetSpriteRenderer;
    private Vector2 targetCenterOffset;

    [SerializeField] private GameObject player;

    public GameObject vehicle;
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
        SetFollowTarget(FollowTarget.Player, player);
    }
    private void LateUpdate()
    {
        //transform.position = targetSpriteRenderer.bounds.center.ToVector2().ToVector3(transform.position.z);
        transform.position = targetGameObject.transform.position.ToVector2().ToVector3(transform.position.z) + targetCenterOffset.ToVector3(0);
    }
    public void SetFollowTarget(FollowTarget newFollowTarget, GameObject newTargetGameObject)
    {
        followTarget = newFollowTarget;
        targetGameObject = newTargetGameObject;
        targetSpriteRenderer = targetGameObject.GetComponent<SpriteRenderer>();
        targetCenterOffset = targetSpriteRenderer.bounds.center - targetGameObject.transform.position;
    }
}
