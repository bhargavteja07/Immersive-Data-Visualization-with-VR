using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayNames : MonoBehaviour {
    public GameObject text;
    bool disp = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnMouseDown()
    {
        if (disp)
        {
            text.GetComponent<Renderer>().enabled = false;
            disp = false;
        }
        else
        {
            text.GetComponent<Renderer>().enabled = true;
            disp = true;
        }
    }
}
