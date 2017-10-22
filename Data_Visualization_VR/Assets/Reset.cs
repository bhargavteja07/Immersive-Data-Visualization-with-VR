using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LitJson;
using System.IO;


public class Reset : MonoBehaviour {
    public int k=0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnMouseDown()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
/*        string jsonString_values = File.ReadAllText("Assets/Resources/InputValues.json");
        jsonDct val= JsonUtility.FromJson<jsonDct>(jsonString_values);
        val = JsonUtility.FromJson<jsonDct>(jsonString_values);
        val.changedvalues.orbitXScale = val.orginalvalues.orbitXScale;
        val.changedvalues.planetScaleFactor = val.orginalvalues.planetScaleFactor;
        val.changedvalues.rotation_speed = val.orginalvalues.rotation_speed;
        JsonData jm;
        jm = JsonMapper.ToJson(val);
        string st = jm.ToString();
        File.WriteAllText("Assets/Resources/InputValues.json", st);
        SceneManager.LoadScene("solar ststem test for fall 17 class");*/
    }

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        /*        string jsonString_values = File.ReadAllText("Assets/Resources/InputValues.json");
                jsonDct val= JsonUtility.FromJson<jsonDct>(jsonString_values);
                val = JsonUtility.FromJson<jsonDct>(jsonString_values);
                val.changedvalues.orbitXScale = val.orginalvalues.orbitXScale;
                val.changedvalues.planetScaleFactor = val.orginalvalues.planetScaleFactor;
                val.changedvalues.rotation_speed = val.orginalvalues.rotation_speed;
                JsonData jm;
                jm = JsonMapper.ToJson(val);
                string st = jm.ToString();
                File.WriteAllText("Assets/Resources/InputValues.json", st);
                SceneManager.LoadScene("solar ststem test for fall 17 class");*/
    }
}
