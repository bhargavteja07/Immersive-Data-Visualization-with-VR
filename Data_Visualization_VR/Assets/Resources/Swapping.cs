using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swapping : MonoBehaviour {

    public string swapname;
    static string[] names=new string[2];
    static int k;

	// Use this for initialization
	void Start () {
        k = 0;
	}
	
	// Update is called once per frame
	void Update () {
	}
    private void OnMouseDown()
    {
        names[k++] = swapname;
        checkSwap();
    }
    void checkSwap()
    {
        if(k==2)
        {
            GameObject gb1 = GameObject.Find(names[0]);
            GameObject gb2 = GameObject.Find(names[1]);
            Vector3 temp = gb1.transform.position;
            gb1.transform.position = gb2.transform.position;
            gb2.transform.position = temp;
            k = 0;
        }
    }
}
