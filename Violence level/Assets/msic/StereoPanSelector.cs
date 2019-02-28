using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This will pan the audio based on where it's coming from.
// Used for shots, explosions, etc.
public class StereoPanSelector : MonoBehaviour {

    // The maximum size in the X axis. The gamefield middle is 0.
    public float boundaryX;

	void Start ()
    {
        // Sets the pan based on the position of the object in the X axis.
        GetComponent<AudioSource>().panStereo = transform.position.x / boundaryX;
	}
}
