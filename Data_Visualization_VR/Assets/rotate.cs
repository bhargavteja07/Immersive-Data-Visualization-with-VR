using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour {

	public float rotateSpeed;

	// Use this for initialization
	void Start () {
//		Debug.Log ("Started rotate.cs");
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log ("Uopdated rotate.cs");

		transform.RotateAround (Vector3.up, rotateSpeed * Time.deltaTime);

	}
}
