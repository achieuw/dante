using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowers : MonoBehaviour
{
    public float freezeTimer = 2f;
    public float invulnerableTimer = 3f;

    private bool abilityQ;

    public Hellfire hellfire;

	void Update ()
    {
        PlayerInput();

        if (abilityQ)
        {   
            StartCoroutine(FreezeHellfire());
        }
	}

    void PlayerInput()
    {
        bool input_Q = Input.GetKeyDown(KeyCode.Q);

        abilityQ = input_Q; 
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
        yield return new WaitForSeconds(invulnerableTimer);
    }
}
