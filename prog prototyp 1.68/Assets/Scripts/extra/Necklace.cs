using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Necklace : MonoBehaviour {


    Rigidbody2D rb;
    Rigidbody2D PlayerRb;
    LineRenderer lineRend;

	// Use this for initialization
	void Start () {
        //GetComponent<DistanceJoint2D>().r;
        lineRend = GetComponent<LineRenderer>();
        test();
	}
	
	// Update is called once per frame
	void Update () {
        test();
        test2();
	}


    void test()
    {
        Vector2 pos = transform.localPosition; // Vector from 0,0 to necklace
        pos.Normalize();
        var test = Vector2.Dot(Vector2.right, pos);
        var dot = Vector2.Dot(Vector2.down, pos);
        var k = Mathf.Acos(dot);
        Debug.Log(Mathf.Sign(dot) + "DOOOOOOOOT");

        Debug.Log(k + "kkkk");

        transform.rotation = Quaternion.Euler(0,0, (Mathf.Sign(test) < 0) ? ((Mathf.PI*2) - k) * Mathf.Rad2Deg : k * Mathf.Rad2Deg );
        //transform.rotation.eulerAngles = Vector3.up;
        //transform.localRotation.ToAngleAxis(out k , out Vector3.forward);
        //transform.rotation.eulerAngles = new Vector3(0, 0, k);

    }

    void test2()
    {


        lineRend.SetPosition(0, transform.parent.position);
        lineRend.SetPosition(2, transform.parent.position);

        lineRend.SetPosition(1, transform.position);

    }
}
