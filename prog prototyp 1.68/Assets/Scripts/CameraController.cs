using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; //spelarobjektet

    public float smoothDampTime = 0.05f; //styrka på damp vid närmande av target
    public float cameraAccelerationMultiplier;
    private float smoothDampTimeHolder;
    private Vector3 smoothDampVelocity = Vector3.zero;
    public bool active;

    public bool cameraLockActivated;
    private bool camLock = false;

    public float transitionTarget;

    public float boundsOffset = 0.5f; //längden på bounds utanför kameran (y-led)

    public PlayerControllerWithDashAndSprint player;
    public CheckPointManager cpManager;

    [HideInInspector]
    public float width, height;
    [HideInInspector]
    public float boundary;

    private void Awake()
    {
        active = true;
    }

    private void Start()
    {
        //ORTOGRAPHIC CAMERA
        //height = Camera.main.orthographicSize * 2;

        //PERSPECTIVE CAMERA
        height = 2.0f * Mathf.Abs(player.transform.position.z - transform.position.z) * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        width = height * Camera.main.aspect;

        smoothDampTimeHolder = smoothDampTime;
        UpdateBounds();
    }

    private void Update()
    {
        if (active)
        {
            StopIfFalling();
            SmoothMovement();
        }      
        UpdateBounds();
    }

    private void SmoothMovement()
    {
        if (target)
        {
            if (!camLock)
            {
                float targetY = target.position.y;
                float y = Mathf.SmoothDamp(transform.position.y, targetY, ref smoothDampVelocity.y, smoothDampTime);
                //float transitionY = Mathf.SmoothDamp(transform.position.y, transitionTarget, ref smoothDampVelocity.y, smoothDampTime);

                if (target.transform.position.y > transitionTarget)
                {
                    transform.position = new Vector3(0, y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(0, y, transform.position.z);

					if(target.transform.position.y <= transitionTarget && transform.position.y <= transitionTarget)
                    {
                        transform.position = new Vector3(0, transitionTarget, transform.position.z);
                    }
                }

                // distance between camera and player for camera acceleration to activate
                if (transform.position.y >= player.transform.position.y + height / 4)
                {
                    smoothDampTime -= smoothDampTime * Time.deltaTime * cameraAccelerationMultiplier;

                    if (smoothDampTime <= 0)
                    {
                        smoothDampTime = 0;
                    }
                }
                else
                {
                    smoothDampTime = smoothDampTimeHolder;
                }
            }
            if (camLock)
            {
                transform.position = new Vector3(0, transform.position.y, transform.position.z);
            }
        }
    }

    private void StopIfFalling()
    {
        if (cameraLockActivated)
        {
            cpManager.boundsActive = true;
            if (target.position.y < transform.position.y)
            {
                camLock = true;
            }
            if (target.position.y >= transform.position.y)
            {
                camLock = false;
            }
        }

        else
        {
            cpManager.boundsActive = false;
        }
    }

    void UpdateBounds()
    {
        boundary = transform.position.y - height / 2 - boundsOffset; //uppdaterar boundary under spelaren (-y-led)
    }
}