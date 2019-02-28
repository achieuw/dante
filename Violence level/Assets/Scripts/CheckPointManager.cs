using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointManager : MonoBehaviour
{
    public bool boundsActive;
    public float playerSpawnOffset = 0.3f;
    public float hellfireSpawnOffsetY;
    [HideInInspector]
    public Vector2 lastCheckpointPosition;
	public float deltaCheckpointPosition;
    [Space (10)]

    public CheckPoint checkpoint;
    public CameraController cam;
    public Hellfire hellfire;

    private void Start()
    {
        checkpoint = GameObject.FindGameObjectWithTag("Checkpoint").GetComponent<CheckPoint>();
    }
    void Update ()
    {
        OutOfBounds();
    }

    void OutOfBounds()
    {
        if (boundsActive)
        {
        if (transform.position.y < cam.boundary)
            {
                RespawnAtLastCheckPointPosition();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hellfire"))
        {
            RespawnAtLastCheckPointPosition();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            RespawnAtLastCheckPointPosition();
        }
    }

    void RespawnAtLastCheckPointPosition() 
    {
        if (lastCheckpointPosition.y < 0)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, 0, cam.transform.position.z);
        }
        else
        {
            cam.transform.position = new Vector3(cam.transform.position.x, lastCheckpointPosition.y, cam.transform.position.z);
        }

        transform.position = new Vector2(lastCheckpointPosition.x, lastCheckpointPosition.y + playerSpawnOffset);
		hellfire.transform.position = new Vector2(0, lastCheckpointPosition.y - cam.height / 2 - hellfireSpawnOffsetY);
    }
}
