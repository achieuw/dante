using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPowers : MonoBehaviour
{
    public float freezeTimer = 2f;
    public float freezeCooldown;
    float freezeTimeStamp = 0;
    float freezeCooldownRemaining;
    public float invulnerableTimer = 3f;
    bool invulnerable;

    private bool abilityQ;

    public Hellfire hellfire;
    public Text freezeCooldownText;
    public Outline FreezeCooldownOutline;

    private void Start()
    {
        SetCooldownText();
    }

    void Update ()
    {
        freezeCooldownRemaining = freezeTimeStamp - Time.time;
        PlayerInput();
        SetCooldownText();

        if (abilityQ)
        {          
            if (freezeTimeStamp <= Time.time)
            {
                StartCoroutine(FreezeHellfire());
                freezeTimeStamp = Time.time + freezeCooldown;
            }           
        }
	}

    void PlayerInput()
    {
        bool input_Q = Input.GetKeyDown(KeyCode.Q);

        abilityQ = input_Q; 
    }

    void SetCooldownText()
    {
        if(freezeCooldownRemaining > -.1)
        {
            freezeCooldownText.text = "Freeze: " + Mathf.Ceil(freezeCooldownRemaining).ToString("F0");
            FreezeCooldownOutline.enabled = false;
        }
        else
        {
            freezeCooldownText.text = "Freeze: " + Mathf.Ceil(-freezeCooldownRemaining).ToString(" READY!");
            FreezeCooldownOutline.enabled = true;
        }           
    }

    IEnumerator FreezeHellfire()
    {  //sets the non trigger collider to active to allow player to walk on hellfire
        hellfire.ToggleCollider();
        hellfire.active = false;
        yield return new WaitForSeconds(freezeTimer);
        hellfire.ToggleCollider();
        hellfire.active = true;
    }

    IEnumerator SetInvulnerable()
    {
        invulnerable = true;
        //call invuln animation
        yield return new WaitForSeconds(invulnerableTimer);
        //stop anim
        invulnerable = false;
    }
}
