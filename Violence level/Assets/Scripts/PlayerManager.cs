using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public bool invulnerable;
    public bool ignorePlatformCollision;
	
    public void ToggleInvulnerable()
    {
        invulnerable = !invulnerable;
    }

    public bool IsInvulnerable()
    {
        if (!invulnerable)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hellfire"))
        {
            //GAME OVER
            //ReloadScene();
        }
    }
}
