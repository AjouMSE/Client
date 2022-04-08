using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowRotationScript : MonoBehaviour {
    public bool rotateOn = true;
    public float rotateSpeed = 3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (rotateOn) {
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        }
    }
}
