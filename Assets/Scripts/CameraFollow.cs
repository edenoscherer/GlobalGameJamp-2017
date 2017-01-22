using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    /**
     * Objeto a ser seguido pela camera
     */
    public GameObject targetObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        float targetObjectPositionX = targetObject.transform.position.x;

        Vector3 newCameraPosition = this.transform.position;
        newCameraPosition.x = targetObjectPositionX;
        this.transform.position = newCameraPosition;
    }
}
