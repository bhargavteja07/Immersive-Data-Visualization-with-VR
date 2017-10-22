using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class FilterController : MonoBehaviour {

	// Use this for initialization

	public SystemList sl;
	public Planets p;
	public filterDict fd;
	public string filterName;


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnMouseDown()
	{
		if (filterName == "planetNumber") {
			if (fd.ChangedFilters.moreThanTwoPlanets == "1")
				fd.ChangedFilters.moreThanTwoPlanets = "0";
			else
				fd.ChangedFilters.moreThanTwoPlanets = "1";
		}
		if (filterName == "sunLikeStars") {
			if (fd.ChangedFilters.sunLikeStars == "1")
				fd.ChangedFilters.sunLikeStars = "0";
			else
				fd.ChangedFilters.sunLikeStars = "1";
		}
		if (filterName == "earthSized") {
			if (fd.ChangedFilters.earthSizePlanets == "1")
				fd.ChangedFilters.earthSizePlanets = "0";
			else
				fd.ChangedFilters.earthSizePlanets = "1";
		}
		if (filterName == "closestEarth") {
			if (fd.ChangedFilters.nearestToSun == "1")
				fd.ChangedFilters.nearestToSun = "0";
			else
				fd.ChangedFilters.nearestToSun = "1";
		}
		if (filterName == "habitableZone") {
			if (fd.ChangedFilters.habitableSystems == "1")
				fd.ChangedFilters.habitableSystems = "0";
			else
				fd.ChangedFilters.habitableSystems = "1";
		}
		p.filterBasedOnFlag (fd);
		setNewFilterValues ();
	}

	public void setNewFilterValues()
	{		
		JsonData jm;
		jm = JsonMapper.ToJson(fd);
		string st = jm.ToString();
		File.WriteAllText("Assets/Resources/SystemFilters.json", st);
	}
}
