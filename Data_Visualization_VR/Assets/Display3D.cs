using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display3D : MonoBehaviour {
    public string star_name;
    public SystemList sl;
    public GameObject go;

    Planets p;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
    private void OnMouseDown()
    {
        p = new Planets();
        int sunScaleRelative = 695500;
        long austronamicalUnit = 149597870;
        float year = 365;
        int earth_mass = 1;
        int earth_radius = 6371;
        var oneOffset = new Vector3(0, -30, 0);
        int total_systems = sl.Systems.Count;
        string[] sol = new string[7];
        for (int i = 0; i < total_systems; i++)
        {
            int k = 0;
            sol[k++] = (float.Parse(sl.Systems[i].sunScale) * sunScaleRelative).ToString();
            sol[k++] = sl.Systems[i].sunName;
            sol[k++] = sl.Systems[i].sunTexture;
            sol[k++] = sl.Systems[i].sunVar;
            sol[k++] = sl.Systems[i].sunHabitat;
            sol[k++] = sl.Systems[i].lightYears;
            sol[k++] = sl.Systems[i].discoveryMethod;
            int planet_count = sl.Systems[i].Planets.Count;
            string[,] planets = new string[planet_count, 6];
            for (int j = 0; j < planet_count; j++)
            {
                k = 0;
                planets[j, k++] = (float.Parse(sl.Systems[i].Planets[j].planetDistance) * austronamicalUnit).ToString();
                planets[j, k++] = sl.Systems[i].Planets[j].planetSize;
                planets[j, k++] = (float.Parse(sl.Systems[i].Planets[j].planetSpeed) / year).ToString();
                planets[j, k++] = sl.Systems[i].Planets[j].textureName;
                planets[j, k++] = sl.Systems[i].Planets[j].planetName;
                planets[j, k++] = sl.Systems[i].Planets[j].planetMass;
            }
//            Debug.Log("yo1");
            if (sol[1]==star_name)
            {
//                Debug.Log("yo2");
                p.dealWithSystem_once(sol, planets, oneOffset, go);
//                Debug.Log(go.name);
                break;
            }
        }
    }
}