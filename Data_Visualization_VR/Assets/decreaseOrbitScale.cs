using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class decreaseOrbitScale : MonoBehaviour {

	// Use this for initialization
	public SystemList sl;

	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void setOrbitXScale(float changedOrbitSize)
	{
		string jsonString_values = File.ReadAllText("Assets/Resources/InputValues.json");
		jsonDct val = JsonUtility.FromJson<jsonDct>(jsonString_values);
		val.changedvalues.orbitXScale = changedOrbitSize.ToString();
		JsonData jm;
		jm = JsonMapper.ToJson(val);
		string st = jm.ToString();
		File.WriteAllText("Assets/Resources/InputValues.json", st);
	}

	public string getOrbitXScale()
	{
		string jsonString_values = File.ReadAllText("Assets/Resources/InputValues.json");
		jsonDct val = JsonUtility.FromJson<jsonDct>(jsonString_values);
		return 	val.changedvalues.orbitXScale;
	}


	private void OnMouseDown()
	{

		int sysCount = sl.Systems.Count;
		string orbitScale = getOrbitXScale ();
		float curOrbitScale = float.Parse (orbitScale);
		float rescaledOrbitScale = curOrbitScale / 1.1F;

		int i,j;




		/*SIDE PLANETS ORBIT SCALE   ---START---- */

		int sideStarNum = 0;

		GameObject hab = GameObject.Find ("Hab");


		for (i = 0; i < sl.Systems.Count; i++) {			
			int sidePCount = sl.Systems [i].Planets.Count;

			for (j = 0; j < sidePCount; j++) {
				string sidePName = "Side" + sl.Systems [i].Planets [j].planetName + sideStarNum.ToString ();
				GameObject sidePlanet = GameObject.Find (sidePName);
				if (sidePlanet != null) {
					float sidePSize = sidePlanet.GetComponent<planetMeta> ().planetSize;
					float sidePDist = sidePlanet.GetComponent<planetMeta> ().planetDistance;
					sidePlanet.transform.localPosition = new Vector3(-0.5F * 30.0F + sidePDist * rescaledOrbitScale, 0, 0);


				}
				GameObject sunMeta = GameObject.Find ("Side " + sl.Systems [i].sunName + " Star");
				float innerHab = sunMeta.GetComponent<planetMeta> ().sunInnerHab;
				float outerHab = sunMeta.GetComponent<planetMeta> ().sunOuterHab;

				GameObject habZone;
				habZone = GameObject.Find("Hab"+sl.Systems[i].sunName);
				habZone.transform.localPosition = new Vector3((-0.5F * 30.0F) + ((innerHab + outerHab) * 0.5F * rescaledOrbitScale), 0, 0);
				habZone.transform.localScale = new Vector3((outerHab - innerHab) * rescaledOrbitScale, 40.0F * .1F, 2.0F * .1F);
			}
			sideStarNum++;
		}

		/*SIDE PLANETS ORBIT SCALE   ---END---- */

		//For the top most system Solar
		string sunName = sl.Systems [0].sunName;
		GameObject sunMetaObj = GameObject.Find (sunName);
		if (sunMetaObj != null) {
			sunName = sunName + "0";
			GameObject orgSun =  GameObject.Find(sunName);
			if (orgSun != null) {
				float sunInHab = sunMetaObj.GetComponent<planetMeta> ().sunInnerHab;
				float sunOutHab = sunMetaObj.GetComponent<planetMeta> ().sunOuterHab;
				GameObject habIn = GameObject.Find (sunName + "Habitable Inner Ring");
				habIn.GetComponent<Circle> ().xradius = sunInHab  * rescaledOrbitScale;
				habIn.GetComponent<Circle> ().yradius = sunInHab  * rescaledOrbitScale;
				GameObject habOut = GameObject.Find (sunName + "Habitable Outer Ring");
				habOut.GetComponent<Circle> ().xradius = sunOutHab  * rescaledOrbitScale;
				habOut.GetComponent<Circle> ().yradius = sunOutHab  * rescaledOrbitScale;
				habIn.GetComponent<Circle> ().init ();
				habOut.GetComponent<Circle> ().init ();
			}
		}

		int planetCount = sl.Systems [0].Planets.Count;
		string planetName;
		GameObject planetMetaObj;

		for (j = 0; j < planetCount; j++) {
			planetName = sl.Systems [0].Planets [j].planetName + "1";
			planetMetaObj = GameObject.Find (planetName);
			if (planetMetaObj != null) {
				GameObject planet = GameObject.Find (planetName);
				GameObject planetOrbit = GameObject.Find (planetName + " orbit");


				float planetDist = planetMetaObj.GetComponent<planetMeta> ().planetDistance;
				float planetSize = planetMetaObj.GetComponent<planetMeta> ().planetSize;


				planetOrbit.GetComponent<Circle> ().xradius = planetDist * rescaledOrbitScale;
				planetOrbit.GetComponent<Circle> ().yradius = planetDist * rescaledOrbitScale;
				planetOrbit.GetComponent<Circle> ().init ();

				planet.transform.localPosition = new Vector3 (0, 0, planetDist * rescaledOrbitScale);
			}
		}

		//For the top most system Solar -----END


		for (i = 0; i < sysCount; i++) {
			planetCount = sl.Systems [i].Planets.Count;
			sunName = sl.Systems [i].sunName + "compare";
			sunMetaObj = GameObject.Find (sunName);
			int sunSuffix;
			int planetSuffix;
			if (sunMetaObj != null) {
				sunSuffix = sunMetaObj.GetComponent<planetMeta> ().sunSuffix;
				planetSuffix = sunSuffix + 1;
				sunName = sunName + sunMetaObj.GetComponent<planetMeta> ().sunSuffix.ToString ();

				GameObject orgSun =  GameObject.Find(sunName);
				if (orgSun != null) {
					float sunInHab = sunMetaObj.GetComponent<planetMeta> ().sunInnerHab;
					float sunOutHab = sunMetaObj.GetComponent<planetMeta> ().sunOuterHab;
					GameObject habIn = GameObject.Find (sunName + "Habitable Inner Ring");
					habIn.GetComponent<Circle> ().xradius = sunInHab  * rescaledOrbitScale;
					habIn.GetComponent<Circle> ().yradius = sunInHab  * rescaledOrbitScale;
					GameObject habOut = GameObject.Find (sunName + "Habitable Outer Ring");
					habOut.GetComponent<Circle> ().xradius = sunOutHab  * rescaledOrbitScale;
					habOut.GetComponent<Circle> ().yradius = sunOutHab  * rescaledOrbitScale;
					habIn.GetComponent<Circle> ().init ();
					habOut.GetComponent<Circle> ().init ();
				}
				for (j = 0; j < planetCount; j++) {
					planetName = sl.Systems [i].Planets [j].planetName + "compare" + planetSuffix.ToString();
					planetMetaObj = GameObject.Find (planetName);
					if (planetMetaObj != null) {
						GameObject planet = GameObject.Find (planetName);
						GameObject planetOrbit = GameObject.Find (planetName + " orbit");


						float planetDist = planetMetaObj.GetComponent<planetMeta> ().planetDistance;
						float planetSize = planetMetaObj.GetComponent<planetMeta> ().planetSize;


						planetOrbit.GetComponent<Circle> ().xradius = planetDist  * rescaledOrbitScale;
						planetOrbit.GetComponent<Circle> ().yradius = planetDist  * rescaledOrbitScale;
						planetOrbit.GetComponent<Circle> ().init ();

						planet.transform.localPosition = new Vector3 (0, 0, planetDist * rescaledOrbitScale);
					}
				}				
			}


		}

		setOrbitXScale (rescaledOrbitScale);
	}	

}
