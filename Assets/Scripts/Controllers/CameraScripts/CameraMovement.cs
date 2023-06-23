using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private static CameraMovement instance;
    public static CameraMovement Instance
    {
        get { return instance; }
    }

    private GameObject targetGameObject;
    private SpriteRenderer targetSpriteRenderer;
    private Vector2 targetCenterOffset;

    [SerializeField] private GameObject player;
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
    }
    private void LateUpdate()
    {
        transform.position = targetGameObject.transform.position.ToVector2().ToVector3(transform.position.z) + targetCenterOffset.ToVector3(0);
    }
    public static void SetFollowTarget(GameObject newTargetGameObject)
    {
        instance.targetGameObject = newTargetGameObject;
        instance.targetSpriteRenderer = instance.targetGameObject.GetComponent<SpriteRenderer>();
        instance.targetCenterOffset = instance.targetSpriteRenderer.bounds.center - instance.targetGameObject.transform.position;
    }
}
