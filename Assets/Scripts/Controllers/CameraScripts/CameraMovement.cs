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

    private bool canMove = true;
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
    private void OnEnable()
    {
        EventMessenger.StartListening("FreezeCamera", FreezeCamera);
        EventMessenger.StartListening("UnfreezeCamera", UnfreezeCamera);
        PrimitiveMessenger.AddObject("cameraFrozenPosition", Vector2.zero);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("FreezeCamera", FreezeCamera);
        EventMessenger.StopListening("UnfreezeCamera", UnfreezeCamera);
        PrimitiveMessenger.RemoveObject("cameraFrozenPosition");
    }
    void Start()
    {
    }
    private void LateUpdate()
    {
        if (canMove)
        {
            transform.position = targetGameObject.transform.position.ToVector2().ToVector3(transform.position.z) + 
                targetCenterOffset.ToVector3(0);
        }
        else
        {
            transform.position = ((Vector2)PrimitiveMessenger.GetObject("cameraFrozenPosition")).ToVector3(transform.position.z);
        }
    }
    public static void SetFollowTarget(GameObject newTargetGameObject)
    {
        instance.targetGameObject = newTargetGameObject;
        instance.targetSpriteRenderer = instance.targetGameObject.GetComponent<SpriteRenderer>();
        instance.targetCenterOffset = instance.targetSpriteRenderer.bounds.center - instance.targetGameObject.transform.position;
    }
    private void FreezeCamera()
    {
        canMove = false;
    }
    private void UnfreezeCamera()
    {
        canMove = true;
    }
}
