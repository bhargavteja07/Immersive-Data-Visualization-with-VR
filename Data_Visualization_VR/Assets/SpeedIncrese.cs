using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using System.IO;


public class SpeedIncrese : MonoBehaviour {
    public SystemList sl;
    
    // Use this for initialization
    void Start () {}

    // Update is called once per frame
    void Update() {}

    private void OnMouseDown()
    {
        int total_systems = sl.Systems.Count;
        for (int i = 0; i < 4; i++)
        {
            Vector3 v = new Vector3(0f, (i * -30f), 0f);
            int m;
            Collider[] colliders;
            if ((colliders = Physics.OverlapSphere(v, 0f)).Length > 1)
            {
                foreach (var collider in colliders)
                {
                    var go1 = collider.gameObject;
//                    Debug.Log(go1);
                    for (int k=0;k<total_systems;k++)
                    {
                        if ((sl.Systems[k].sunName+"compare") == go1.name.Substring(0, go1.name.Length - 1))
                        {
                            int planet_count = sl.Systems[k].Planets.Count;
                            //Debug.Log(go1.name);
                            //Debug.Log(planet_count);
                            for (int j = 0; j < planet_count; j++)
                            {
                                string planet = string.Concat(sl.Systems[k].Planets[j].planetName, "compare");
                                m = Convert.ToInt32(go1.name.Substring(go1.name.Length - 1))+1;
                                string find = string.Concat(planet,m.ToString() ) + "Center";
                                GameObject g = GameObject.Find(find);
                                //Debug.Log(g.name);
                                g.GetComponent<rotate>().rotateSpeed = 2 * g.GetComponent<rotate>().rotateSpeed;
                            }
                        }
                        else if(go1.name=="Our Sun0")
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                m = 1;
                                string planet = sl.Systems[0].Planets[j].planetName;
                                string find = string.Concat(planet, m.ToString()) + "Center";
                                Debug.Log(find);
                                GameObject g = GameObject.Find(find);
                                //Debug.Log(g.name);
                                g.GetComponent<rotate>().rotateSpeed = 2 * g.GetComponent<rotate>().rotateSpeed;
                            }
                        }
                    }
                }
            }
        }
        set_speed();
    }

    public void set_speed()
    {
//        Debug.Log("yoo");
        string jsonString_values = File.ReadAllText("Assets/Resources/InputValues.json");
        jsonDct val = JsonUtility.FromJson<jsonDct>(jsonString_values);
        val.changedvalues.rotation_speed = (2 * float.Parse(val.changedvalues.rotation_speed)).ToString();
//        Debug.Log(val.changedvalues.rotation_speed);
        JsonData jm;
        jm = JsonMapper.ToJson(val);
        string st = jm.ToString();
        File.WriteAllText("Assets/Resources/InputValues.json", st);
    }
}