using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    Rigidbody2D rb;
    [HideInInspector]
    public Vector2 startPos;

    public float health;
    float startHealth;
    public bool invulnerable;
    public float invulnerableTime;
    public float blinkSpeed;
    float blinkTimer;
    float blinkTotalTime;
    bool startBlinking;

    public Text healthText;

    SpriteRenderer sprite;
    public FlyingEnemy flyingEnemy;
    public Hellfire hellfire;
    FireDudeEnemy fireDude;
    SeelingEnemy seeling;
    CheckPointManager cpManager;
    ParticleSystem bloodSplash;

    private void Awake()
    {
        cpManager = GameObject.FindGameObjectWithTag("Player").GetComponent<CheckPointManager>();
        seeling = GameObject.FindGameObjectWithTag("SeelingEnemy").GetComponent<SeelingEnemy>();
        fireDude = GameObject.FindGameObjectWithTag("FireDude").GetComponent<FireDudeEnemy>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        bloodSplash = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        startPos = transform.position;
        startHealth = health;
        SetHealthText();
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        SetHealthText();
        OnDeath();

        if (startBlinking)
        {
            BlinkSprite();
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FlyingEnemy"))
        {
            if (!invulnerable)
            {
                Vector2 moveDirection = transform.position - collision.transform.position;
                Vector2 force = new Vector2(moveDirection.normalized.x * flyingEnemy.knockbackForce * 100, moveDirection.normalized.y * flyingEnemy.knockbackForce * 20);
                rb.AddForce(force);
                health -= flyingEnemy.collisionDamage;
                startBlinking = true;
                StartCoroutine(SetInvulnerable());               
            }         
        }
        if (collision.gameObject.CompareTag("Hellfire"))
        {
            if (!invulnerable && hellfire.active)
            {
                health -= startHealth * hellfire.damagePercentage / 100;
                startBlinking = true;
                StartCoroutine(SetInvulnerable());
            }
        }

        if (collision.gameObject.CompareTag("SeelingGoo"))
        {
            StartCoroutine(SetBloody());
            bloodSplash.transform.position = collision.transform.position;
            bloodSplash.Play();

            if (!invulnerable)
            {              
                health -= seeling.gooDamage;
                startBlinking = true;               
                StartCoroutine(SetInvulnerable());
            }
        }

        if (collision.gameObject.CompareTag("SeelingEnemy"))
        {
            if (!invulnerable)
            {               
                health -= seeling.collisionDamage;
                startBlinking = true;
                StartCoroutine(SetInvulnerable());
            }
        }

        if (collision.gameObject.CompareTag("FireDude"))
        {
            if (!invulnerable)
            {
                Vector2 moveDirection = transform.position - collision.transform.position;
                Vector2 force = new Vector2(moveDirection.normalized.x * fireDude.knockbackForce * 100, moveDirection.normalized.y * fireDude.knockbackForce * 20);
                rb.AddForce(force);
                health -= fireDude.collisionDamage;
                startBlinking = true;
                StartCoroutine(SetInvulnerable());
            }
        }

        if (collision.gameObject.CompareTag("FireDudeShot"))
        {
            if (!invulnerable)
            {
                health -= fireDude.shotDamage;
                startBlinking = true;
                StartCoroutine(SetInvulnerable());
            }
        }

        if (collision.gameObject.CompareTag("FireDudeFlame"))
        {
            if (!invulnerable)
            {
                Vector2 moveDirection = transform.position - collision.transform.position;
                Vector2 force = new Vector2(moveDirection.normalized.x * fireDude.knockbackForce * 100, moveDirection.normalized.y * fireDude.knockbackForce * 20);
                rb.AddForce(force);
                health -= fireDude.flameDamage;
                startBlinking = true;
                StartCoroutine(SetInvulnerable());
            }
        }
    }

    void OnDeath()
    {
        if(health <= 0)
        {
            cpManager.RespawnAtLastCheckPointPosition();
            invulnerable = false;
            startBlinking = false;
            sprite.color = Color.white;
            health = 100;
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void SetHealthText()
    {
        healthText.text = "Health: " + health.ToString();
    }

    void BlinkSprite()
    {
        blinkTotalTime += Time.deltaTime;

        if(blinkTotalTime >= invulnerableTime)
        {
            startBlinking = false;
            sprite.enabled = true;
            blinkTotalTime = 0;
            return;
        }

        blinkTimer += Time.deltaTime;

        if (blinkTimer >= blinkSpeed)
        {
            blinkTimer = 0.0f;

            if (sprite.enabled == true)
            {
                sprite.enabled = false;  //make changes
            }
            else
            {
                sprite.enabled = true;   //make changes
            }
        }
    }

    IEnumerator SetBloody()
    {
        sprite.color = new Color(sprite.color.r, 0, 0, sprite.color.a);

        while (sprite.color.b < 1.0f)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g + (Time.deltaTime * 0.1f), sprite.color.b + (Time.deltaTime * 0.1f), sprite.color.a);
            yield return null;
        }
    }
    IEnumerator SetInvulnerable()
    {
        invulnerable = true;      
        yield return new WaitForSeconds(invulnerableTime);
        invulnerable = false;
    }
}
