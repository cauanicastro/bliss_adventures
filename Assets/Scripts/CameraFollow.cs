using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform followTransform;
    public BoxCollider2D mapBounds;
    
    [HideInInspector]
    public float xMin, xMax, yMin, yMax;

    private float camY, camX;
    private float camOrthsize;
    
    private Camera mainCam;
    
    private Vector3 smoothPos;
    public float smoothSpeed = 0.725f;

    private GameManager gm;

    private void Start()
    {
        UpdateCameraBounds();
        mainCam = GetComponent<Camera>();
        camOrthsize = mainCam.orthographicSize;
        StartCamera();
    }
    void FixedUpdate()
    {
        camY = Mathf.Clamp(followTransform.position.y, yMin + camOrthsize, yMax - camOrthsize);
        camX = Mathf.Clamp(followTransform.position.x, xMin + camOrthsize, xMax - camOrthsize);
        smoothPos = Vector3.Lerp(this.transform.position, new Vector3(camX, camY, this.transform.position.z), smoothSpeed);
        this.transform.position = smoothPos;
    }

    public void UpdateCameraBounds()
    {
        xMin = mapBounds.bounds.min.x;
        xMax = mapBounds.bounds.max.x;
        yMin = mapBounds.bounds.min.y;
        yMax = mapBounds.bounds.max.y;
    }

    void StartCamera()
    {
        gm = GameManager.GetInstance();
        gm.mainCamera = mainCam;
        gm.cameraHandler = this;
    }
}