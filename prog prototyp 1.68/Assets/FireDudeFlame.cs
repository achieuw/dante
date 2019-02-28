using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDudeFlame : MonoBehaviour
{
    public float duration;
    [HideInInspector]
    public float timer;

    private void Update()
    {
        timer = timer + Time.deltaTime;
        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}
