using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteStar : MonoBehaviour {
    public string starname;
    public Planets p;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnMouseDown()
    {
        Debug.Log(starname);
        p.filterBasedOnSunName(starname);
    }
    private void OnTriggerEnter(Collider other)
    {
        
        //    p.filterBasedOnSunName(starname);
        
    }
}
