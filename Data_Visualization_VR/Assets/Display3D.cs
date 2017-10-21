using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display3D : MonoBehaviour {
    public string star_name;
    public SystemList sl;
    public GameObject go;
    public Vector3 oneOffset;
    Planets p;
    public static int count=0;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	}
    void deleteStar(GameObject go)
    {
        Debug.Log(go.name);
        int total_systems = sl.Systems.Count;
        string[] sol = new string[7];
        for (int i = 0; i < total_systems; i++)
        {
            if (go.name.Substring(0,go.name.Length-1) == sl.Systems[i].sunName + "compare")
            {
                GameObject solarcenter = GameObject.Find("SolarCenter " + go.name);
                Destroy(solarcenter);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        count = count % 3;
        count++;
        p = new Planets();
        int sunScaleRelative = 695500;
        long austronamicalUnit = 149597870;
        float year = 365;
        int earth_mass = 1;
        int earth_radius = 6371;
        var systemOffset = new Vector3(0, 0f, 0);
        oneOffset = new Vector3(0, (-30f * count), 0);
        int total_systems = sl.Systems.Count;
        string[] sol = new string[7];
        for (int i = 0; i < total_systems; i++)
        {
            int k = 0;
            sol[k++] = (float.Parse(sl.Systems[i].sunScale) * sunScaleRelative).ToString();
            sol[k++] = sl.Systems[i].sunName + "compare";
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
                planets[j, k++] = sl.Systems[i].Planets[j].planetName + "compare";
                planets[j, k++] = sl.Systems[i].Planets[j].planetMass;
            }
            Debug.Log(star_name);
            if (sol[1] == (star_name + "compare"))
            {
                //                Debug.Log("yo2");
                Collider[] colliders;
                if ((colliders = Physics.OverlapSphere(oneOffset, 0f /* Radius */)).Length > 1) //Presuming the object you are testing also has a collider 0 otherwise
                {
                    foreach (var collider in colliders)
                    {

                        var go1 = collider.gameObject; //This is the game object you collided with
                        //Debug.Log(go1);
                        deleteStar(go1);
                    }
                }
                p.dealWithSystem_once(sol, planets, systemOffset + oneOffset, go);
                //                Debug.Log(go.name);
                break;
            }
        }
    }

    private void OnMouseDown()
    {
        count = count % 3;
        count++;
        p = new Planets();
        int sunScaleRelative = 695500;
        long austronamicalUnit = 149597870;
        float year = 365;
        int earth_mass = 1;
        int earth_radius = 6371;
        oneOffset = new Vector3(0, (-30f*count), 0);
        int total_systems = sl.Systems.Count;
        string[] sol = new string[7];
        for (int i = 0; i < total_systems; i++)
        {
            int k = 0;
            sol[k++] = (float.Parse(sl.Systems[i].sunScale) * sunScaleRelative).ToString();
            sol[k++] = sl.Systems[i].sunName+"compare";
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
                planets[j, k++] = sl.Systems[i].Planets[j].planetName + "compare";
                planets[j, k++] = sl.Systems[i].Planets[j].planetMass;
            }

            if (sol[1]==(star_name + "compare"))
            {
                Collider[] colliders;                
                if ((colliders = Physics.OverlapSphere(oneOffset, 0f /* Radius */)).Length > 1) //Presuming the object you are testing also has a collider 0 otherwise
                {
                    foreach (var collider in colliders)
                    {    
                        var go1 = collider.gameObject; //This is the game object you collided with
                        Debug.Log(go1);
                        deleteStar(go1);
                    }
                }
                p.dealWithSystem_once(sol, planets,oneOffset, go);
//                Debug.Log(go.name);
                break;
            }
        }
    }
}


