using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] enemies;

	void Start ()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }
}
