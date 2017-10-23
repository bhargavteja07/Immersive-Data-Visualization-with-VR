using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;


public class SaveState : MonoBehaviour {

	// Use this for initialization
	public Planets p;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnMouseDown()
    {
        Debug.Log("saving...");

		JsonData jdata;
		jdata = JsonMapper.ToJson(p.sl);
		string SystemSt = jdata.ToString();
		File.WriteAllText("Assets/Resources/Planetary_system_information.json", SystemSt);


		p.fd.OrginalFilters.moreThanTwoPlanets = p.fd.ChangedFilters.moreThanTwoPlanets;
		p.fd.OrginalFilters.earthSizePlanets = p.fd.ChangedFilters.earthSizePlanets;
		p.fd.OrginalFilters.sunLikeStars = p.fd.ChangedFilters.sunLikeStars;
		p.fd.OrginalFilters.nearestToSun = p.fd.ChangedFilters.nearestToSun;
		p.fd.OrginalFilters.habitableSystems = p.fd.ChangedFilters.habitableSystems;
		jdata = JsonMapper.ToJson(p.fd);
		SystemSt = jdata.ToString();
		File.WriteAllText("Assets/Resources/SystemFilters.json", SystemSt);

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
