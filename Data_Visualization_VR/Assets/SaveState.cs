using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;


public class SaveState : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnMouseDown()
    {
        Debug.Log("saving...");
        string jsonString_values = File.ReadAllText("Assets/Resources/InputValues.json");
        jsonDct val = JsonUtility.FromJson<jsonDct>(jsonString_values);
        val = JsonUtility.FromJson<jsonDct>(jsonString_values);
        val.orginalvalues.orbitXScale = val.changedvalues.orbitXScale;
        val.orginalvalues.planetScaleFactor = val.changedvalues.planetScaleFactor;
        val.orginalvalues.rotation_speed = val.changedvalues.rotation_speed;
        JsonData jm;
        jm = JsonMapper.ToJson(val);
        string st = jm.ToString();
        File.WriteAllText("Assets/Resources/InputValues.json", st);
    }
}
