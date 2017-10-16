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

		for (int i = 0; i < sysCount; i++) {
			int planetCount = sl.Systems [i].Planets.Count;
			for (int j = 0; j < planetCount; j++) {
				string planetName = sl.Systems [i].Planets [j].planetName;
				GameObject planetMetaObj = GameObject.Find (planetName);
				if (planetMetaObj != null) {
					planetName = planetName + planetMetaObj.GetComponent<planetMeta> ().planetSuffixNumber.ToString ();
					GameObject planet = GameObject.Find (planetName);

					float planetSize = planetMetaObj.GetComponent<planetMeta> ().planetSize;

					planet.transform.localScale = new Vector3 (planetSize*rescaledPlanetScale,planetSize*rescaledPlanetScale,planetSize*rescaledPlanetScale);
				}
			}				
		}
		setPlanetScaleFactor (rescaledPlanetScale);
	}	

}
	

