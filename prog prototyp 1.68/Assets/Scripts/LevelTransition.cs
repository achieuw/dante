﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTransition : MonoBehaviour
{
    PlayerControllerWithDashAndSprint player;
    CameraController cam;
    EnemyHandler enemies;
    public Hellfire hellfire;

    public bool transitionCam = false;

    public float cameraSpeed = 2f;
    public float triggerBounceForce = 8f;

    [HideInInspector] public float targetY; 
   
	[Header("Text")]
    public Text transitionText;
    public float textTimer = 2f;
    public float textFadeSpeed = 1f;

	[Header("Music")]
	//public AudioSource transitionMusic;
    [Header("Background")]
    public SpriteRenderer background;

    BoxCollider2D col;
    

    private void Awake()
    {       
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerWithDashAndSprint>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        enemies = GameObject.FindGameObjectWithTag("Enemies").GetComponent<EnemyHandler>();
        col = GetComponent<BoxCollider2D>();      
    }
    void Start()
    {
        background.transform.position = new Vector2(transform.position.x, transform.position.y + background.bounds.size.y / 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {       
        if (collision.gameObject.CompareTag("Player"))
        {			
            DeleteCollider();
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, triggerBounceForce); //small bounce to help player get up to platform  
            if (hellfire)
            {
                hellfire.executed = false;
                hellfire.active = false;
            }            
            hellfire.playerStartPosition = player.transform.position.y;
			player.move = false;
			player.movement2 = 0;
            SwitchCamera();
            StartCoroutine(TransitionText());
            DestroyPreviousEnemies();
            //play transition music
            //play dialog
            //allow shopping         
            //start hellfire
        }
    }

    private void Update()
    {
        CameraMovement();             
    }
    void CameraMovement()
    {              
        Vector3 position = cam.transform.position;
        position.y += cameraSpeed * Time.deltaTime;

        if (cam.transform.position.y < targetY && transitionCam)
        {
            cam.transform.position = position;

            if (cam.transform.position.y >= targetY)
            {
                cam.active = true;
                transitionCam = false;
				player.move = true;
            }
        }      
    }

    void DestroyPreviousEnemies()
    {
        foreach(GameObject enemy in enemies.enemies)
        {
            if(enemy.transform.position.y < player.transform.position.y)
            {
                Destroy(enemy);
            }
        }
    }
    void SwitchCamera()
    {
        targetY = transform.position.y + cam.height / 2;
        cam.active = false;
        transitionCam = true;      
        cam.transitionTarget = targetY;
    }
    void DeleteCollider()
    {
        Destroy(col);
    }
    IEnumerator TransitionText()
    {
        transitionText.gameObject.SetActive(true);

        transitionText.color = new Color(transitionText.color.r, transitionText.color.g, transitionText.color.b, 0);

        while (transitionText.color.a < 1.0f)
        {
            transitionText.color = new Color(transitionText.color.r, transitionText.color.g, transitionText.color.b, transitionText.color.a + (Time.deltaTime * textFadeSpeed));
            yield return null;
        }
        yield return new WaitForSeconds(textTimer);

        while(transitionText.color.a > 0)
        {
            transitionText.color = new Color(transitionText.color.r, transitionText.color.g, transitionText.color.b, transitionText.color.a - (Time.deltaTime * textFadeSpeed));
            yield return null;
        }     
    }
}
