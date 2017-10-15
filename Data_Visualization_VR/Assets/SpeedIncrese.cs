using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using System.IO;


public class SpeedIncrese : MonoBehaviour {
    public SystemList sl;
    
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        int total_systems = sl.Systems.Count;
        string sol; ;
        for (int i = 0; i < total_systems; i++)
        {
            int k = 0;
            sol = sl.Systems[i].sunName + "compare";
            int planet_count = sl.Systems[i].Planets.Count;
            string planet; ;
            Vector3 v = new Vector3(0f, 0f, 0f);
            Collider[] colliders;
            if ((colliders = Physics.OverlapSphere(v, 100000f /* Radius */)).Length > 1)
            {
                foreach (var collider in colliders)
                {
                    var go1 = collider.gameObject;
//                    Debug.Log(go1.name);
                    if (go1.GetComponent<rotate>() != null)
                    {
                        
                        string name = go1.name.Remove(go1.name.Length - 1);
                        char last = go1.name[go1.name.Length - 1];
                        int m = Convert.ToInt32(last);
                        m = m - 47;
                        if (sol == name)
                        {
                            for (int j=0;j<planet_count;j++)
                            {
                                planet = string.Concat(sl.Systems[i].Planets[j].planetName,"compare");
                                string find = string.Concat(planet, m.ToString()) + "Center";
                                GameObject g = GameObject.Find(find);
                                Debug.Log(g.name);
                                g.GetComponent<rotate>().rotateSpeed = 2 * g.GetComponent<rotate>().rotateSpeed;
//                                set_speed();
                            }
                        }
                    }
                }
            }
        }
    }
    void set_speed()
    {
        string jsonString_values = File.ReadAllText("Assets/Resources/InputValues.json");
        jsonDct val = JsonUtility.FromJson<jsonDct>(jsonString_values);
//        val.cv.rotation_speed = (2 * float.Parse(val.cv.rotation_speed)).ToString();
    }
}