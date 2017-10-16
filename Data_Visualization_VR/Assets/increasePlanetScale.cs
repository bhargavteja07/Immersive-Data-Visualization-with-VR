using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;


public class increasePlanetScale : MonoBehaviour {

	public SystemList sl;

	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void setPlanetScaleFactor(float changedOrbitSize)
	{
		string jsonString_values = File.ReadAllText("Assets/Resources/InputValues.json");
		jsonDct val = JsonUtility.FromJson<jsonDct>(jsonString_values);
		val.changedvalues.planetScaleFactor = changedOrbitSize.ToString();
		JsonData jm;
		jm = JsonMapper.ToJson(val);
		string st = jm.ToString();
		File.WriteAllText("Assets/Resources/InputValues.json", st);
	}

	public string getPlanetScaleFactor()
	{
		string jsonString_values = File.ReadAllText("Assets/Resources/InputValues.json");
		jsonDct val = JsonUtility.FromJson<jsonDct>(jsonString_values);
		return 	val.changedvalues.planetScaleFactor;
	}


	private void OnMouseDown()
	{
		int sysCount = sl.Systems.Count;
		string planetScale = getPlanetScaleFactor ();
		float curPlanetScale = float.Parse (planetScale);
		float rescaledPlanetScale = curPlanetScale * 1.1F;
		int planetCount;
		string planetName;





		/*Side Planet Scale ----START*/


		int sideStarNum = 0,i,j;

		GameObject hab = GameObject.Find ("Hab");


		for (i = 0; i < sl.Systems.Count; i++) {			
			int sidePCount = sl.Systems [i].Planets.Count;

			for (j = 0; j < sidePCount; j++) {
				string sidePName = "Side" + sl.Systems [i].Planets [j].planetName + sideStarNum.ToString ();
				GameObject sidePlanet = GameObject.Find (sidePName);
				if (sidePlanet != null) {
					float sidePSize = sidePlanet.GetComponent<planetMeta> ().planetSize;
					sidePlanet.transform.localScale = new Vector3(sidePSize*rescaledPlanetScale, sidePSize*rescaledPlanetScale, 5.0F * 0.1F *rescaledPlanetScale);

				}
			}
			sideStarNum++;
		}



		/*Side Planet Scale ----END*/

		//BASE CASE FOR SOLAR SYSTEM
		GameObject sunMetaObj;
		sunMetaObj = GameObject.Find (sl.Systems[0].sunName);
		if (sunMetaObj != null) {
		    planetCount = sl.Systems [0].Planets.Count;
			for (j = 0; j < planetCount; j++) {
				planetName = sl.Systems [0].Planets [j].planetName + "1";

				GameObject planetMetaObj = GameObject.Find (planetName);
				if (planetMetaObj != null) {
					planetName = planetName;
					GameObject planet = GameObject.Find (planetName);

					float planetSize = planetMetaObj.GetComponent<planetMeta> ().planetSize;

					planet.transform.localScale = new Vector3 (planetSize * rescaledPlanetScale, planetSize * rescaledPlanetScale, planetSize * rescaledPlanetScale);
				}
			}
		}

		//BASE CASE FOR SOLAR SYSTEM ---ENDS


		for ( i = 0; i < sysCount; i++) {
		    sunMetaObj = GameObject.Find (sl.Systems[i].sunName + "compare");
			int sunSuffix;
			int planetSuffix;
			if (sunMetaObj != null) {
				sunSuffix = sunMetaObj.GetComponent<planetMeta> ().sunSuffix;
				planetSuffix = sunSuffix + 1;
				planetCount = sl.Systems [i].Planets.Count;
				for ( j = 0; j < planetCount; j++) {
					planetName = sl.Systems [i].Planets [j].planetName + "compare" + planetSuffix.ToString();
					GameObject planetMetaObj = GameObject.Find (planetName);
					if (planetMetaObj != null) {
						planetName = planetName;
						GameObject planet = GameObject.Find (planetName);

						float planetSize = planetMetaObj.GetComponent<planetMeta> ().planetSize;

						planet.transform.localScale = new Vector3 (planetSize*rescaledPlanetScale,planetSize*rescaledPlanetScale,planetSize*rescaledPlanetScale);
					}
				}			
			}				
		}
		setPlanetScaleFactor (rescaledPlanetScale);
	}	

}
	

