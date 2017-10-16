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
		float rescaledOrbitScale = curOrbitScale/1.1F;


		string sunName = sl.Systems [0].sunName;
		GameObject sunMetaObj = GameObject.Find (sunName);
		if (sunMetaObj != null) {
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
		}



		for (int i = 0; i < sysCount; i++) {
			int planetCount = sl.Systems [i].Planets.Count;
			sunName = sl.Systems [i].sunName + "compare";
			sunMetaObj = GameObject.Find (sunName );
			if (sunMetaObj != null) {
				sunName = sunName +sunMetaObj.GetComponent<planetMeta> ().sunSuffix.ToString ();
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
			for (int j = 0; j < planetCount; j++) {
				string planetName = sl.Systems [i].Planets [j].planetName;
				GameObject planetMetaObj = GameObject.Find (planetName);
				if (planetMetaObj != null) {
					planetName = planetName + planetMetaObj.GetComponent<planetMeta> ().planetSuffixNumber.ToString ();
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
		setOrbitXScale (rescaledOrbitScale);
	}	

}

