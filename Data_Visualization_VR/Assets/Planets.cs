/// Sample Code for CS 491 Virtual And Augmented Reality Course - Fall 2017
/// written by Andy Johnson
/// 
/// makes use of various textures from the celestia motherlode - http://www.celestiamotherlode.net/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System;

public class Planets : MonoBehaviour
{
    float panelHeight = 0.1F;
    float panelWidth = 30.0F;
    float panelDepth = 0.1F;

    float orbitWidth = 0.01F;
    float habWidth = 0.03F;

    static float revolutionSpeed;
    static int sunk = 0;

    float panelXScale = 2.0F;
    float orbitXScale = 2.0F;
    float planetScaleFactor = 1.0F;

    float innerHab;
    float outerHab;

    static int k = 0;
    public static string jsonString = File.ReadAllText("Assets/Resources/Planetary_system_information.json");
    public SystemList sl; //= JsonUtility.FromJson<SystemList>(jsonString);

    public static string jsonString_values = File.ReadAllText("Assets/Resources/InputValues.json");
    public jsonDct val;// = JsonUtility.FromJson<jsonDct>(jsonString_values);
    
	public static string jsonString_Filters = File.ReadAllText("Assets/Resources/SystemFilters.json");
	public filterDict fd;

    public GameObject allCenter;
    public GameObject SolarCenter;
    public GameObject AllOrbits;
    public GameObject SunStuff;
    public GameObject Planets1;

    public static bool menuDisplay = false;

    public GameObject Save;
    public GameObject SaveText;

    public GameObject Reset;
    public GameObject ResetText;

    public GameObject planetSize;
    public GameObject planetSizeText;
    public GameObject planetSizePlus;
    public GameObject planetSizeMinus;

    public GameObject orbitSize;
    public GameObject orbitSizeText;
    public GameObject orbitSizePlus;
    public GameObject orbitSizeMinus;

    public GameObject speed;
    public GameObject speedText;
    public GameObject speedPlus;
    public GameObject speedMinus;

    public GameObject planetsNumber;
    public GameObject habitableZones;
    public GameObject earthSized;
    public GameObject closestEarth;
    public GameObject sunLikeStars;

    public GameObject planetsNumberText;
    public GameObject habitableZonesText;
    public GameObject earthSizedText;
    public GameObject closestEarthText;
    public GameObject sunLikeStarsText;

    public static int starNumber = 0;
    public static int sideStarNumber = 0;

    //------------------------------------------------------------------------------------//

    public void drawOrbit(string orbitName, float orbitRadius, Color orbitColor, float myWidth, GameObject myOrbits)
    {

        GameObject newOrbit;
        GameObject orbits;

        newOrbit = new GameObject(orbitName);
        newOrbit.AddComponent<Circle>();
        newOrbit.AddComponent<LineRenderer>();

        newOrbit.GetComponent<Circle>().xradius = orbitRadius;
        newOrbit.GetComponent<Circle>().yradius = orbitRadius;

        var line = newOrbit.GetComponent<LineRenderer>();
        line.startWidth = myWidth;
        line.endWidth = myWidth;
        line.useWorldSpace = false;

        newOrbit.GetComponent<LineRenderer>().material.color = orbitColor;

        orbits = myOrbits;
        newOrbit.transform.parent = orbits.transform;


    }


	public string getTexture(float planetSize)
	{
		string textureName = "";

		if(planetSize<3000)
			textureName = "mercury";
		else if(planetSize >=3000 && planetSize<=5000)
			textureName = "mars";
		else if(planetSize >=5000 && planetSize<=6100)
			textureName = "venus";
		else if(planetSize >=6100 && planetSize<=6800)
			textureName = "earthmap";
		else if(planetSize >=6800 && planetSize<=25000)
			textureName = "neptune";
		else if(planetSize >=25000 && planetSize<=30000)
			textureName = "uranus";
		else if(planetSize >=30000 && planetSize<=60000)
			textureName = "saturn";
		else if(planetSize >=60000)
			textureName = "jupiter";
		
		return textureName;
	}

    //------------------------------------------------------------------------------------//

    public void dealWithPlanets(string[,] planets, GameObject thesePlanets, GameObject theseOrbits)
    {
        GameObject newPlanetCenter;
        GameObject newPlanet;
        GameObject sunRelated;
        Material planetMaterial;
        int planetCounter;
        for (planetCounter = 0; planetCounter < planets.GetLength(0); planetCounter++)
        {

            float planetDistance = float.Parse(planets[planetCounter, 0]) / 149600000.0F * 10.0F;
            float planetSize = float.Parse(planets[planetCounter, 1]);
            float planetSpeed = -1.0F / float.Parse(planets[planetCounter, 2]) * revolutionSpeed;
            //Debug.Log("planetSpeed");
            //Debug.Log(revolutionSpeed);
            string textureName = planets[planetCounter, 3];
            string planetName = planets[planetCounter, 4] + (starNumber.ToString());
            string planetMass = planets[planetCounter, 5];
            int earthRadius = 6371;
            if (planetSize == 0)
            {
                float mass = float.Parse(planetMass);
                if ((0 < mass) && (mass <= 2))
                {
//                    Debug.Log("im in <2");
                    planetSize = ((5F / 8F) * mass) * earthRadius;
                }
                else if ((2 < mass) && (mass <= 5))
                {
                    planetSize = ((mass + 3) / 4) * earthRadius;
                }
                else if ((5 < mass) && (mass <= 10))
                {
                    planetSize = ((mass + 5) / 5) * earthRadius;
                }
                else if ((10 < mass) && (mass <= 30))
                {
                    planetSize = (((3 * mass) + 30) / 20) * earthRadius;
                }
                else if ((30 < mass) && (mass <= 300))
                {
                    planetSize = ((mass + 150) / 3) * earthRadius;
                }
                else
                {
                    planetSize = mass * earthRadius / 20;
                }
//                Debug.Log(planetName + " --  " + planetSize);
            }
            

			textureName = getTexture (planetSize);



            planetSize = planetSize * 2.0F / 10000.0F;

            newPlanetCenter = new GameObject();
            newPlanetCenter.name = planetName + "Center";
            newPlanetCenter.AddComponent<rotate>();

            newPlanet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newPlanet.name = planetName;
            newPlanet.transform.position = new Vector3(0, 0, planetDistance * orbitXScale);
            newPlanet.transform.localScale = new Vector3(planetSize * planetScaleFactor, planetSize * planetScaleFactor, planetSize * planetScaleFactor);
            newPlanet.transform.parent = newPlanetCenter.transform;

            //GameObject planetMetaObject = new GameObject(planets[planetCounter, 4]);
            newPlanet.AddComponent<planetMeta>();
            newPlanet.GetComponent<planetMeta>().planetSuffixNumber = starNumber;
            newPlanet.GetComponent<planetMeta>().planetSize = planetSize;
            newPlanet.GetComponent<planetMeta>().planetDistance = planetDistance;

            newPlanetCenter.GetComponent<rotate>().rotateSpeed = planetSpeed;
            
            planetMaterial = new Material(Shader.Find("Standard"));
            newPlanet.GetComponent<MeshRenderer>().material = planetMaterial;
            planetMaterial.mainTexture = Resources.Load(textureName) as Texture;

            drawOrbit(planetName + " orbit", planetDistance * orbitXScale, Color.white, orbitWidth, theseOrbits);

            sunRelated = thesePlanets;
            newPlanetCenter.transform.parent = sunRelated.transform;

            planetSize = planetSize * 10000.0F / 2.0F;
            if (planetSize / earthRadius <= 1.5)
            {
                //Debug.Log(innerHab + "   " + outerHab+"   "+planetName +"   "+planetDistance);
                if ((innerHab < planetDistance) && (outerHab > planetDistance))
                {
                    AudioSource water = newPlanet.AddComponent<AudioSource>();
                    water.clip = Resources.Load("water") as AudioClip;
                    water.loop = true;
                    water.maxDistance = 3;
                    water.spatialBlend = 1;
                    water.Play();
                }
            }
        }
    }

    //------------------------------------------------------------------------------------//

    public void sideDealWithPlanets(string[,] planets, GameObject thisSide, GameObject theseOrbits)
    {
        GameObject newPlanet,sideplanetText;
        GameObject sunRelated;
        Material planetMaterial;
        int planetCounter;
        for (planetCounter = 0; planetCounter < planets.GetLength(0); planetCounter++)
        {
            float planetDistance = float.Parse(planets[planetCounter, 0]) / 149600000.0F * 10.0F;
            float planetSize = float.Parse(planets[planetCounter, 1]);
            string textureName = planets[planetCounter, 3];
            string planetName = "Side" + planets[planetCounter, 4] + sideStarNumber.ToString();
            string planetMass = planets[planetCounter, 5];
            int earthRadius = 6371;
            if (planetSize == 0)
            {
                float mass = float.Parse(planetMass);
                if (mass <= 2)
                {
                    planetSize = ((5F / 8F) * mass) * earthRadius;
                }
                else if ((2 < mass) && (mass <= 5))
                {
                    planetSize = ((mass + 3) / 4) * earthRadius;
                }
                else if ((5 < mass) && (mass <= 10))
                {
                    planetSize = ((mass + 5) / 5) * earthRadius;
                }
                else if ((10 < mass) && (mass <= 30))
                {
                    planetSize = (((3 * mass) + 30) / 20) * earthRadius;
                }
                else if ((30 < mass) && (mass <= 300))
                {
                    planetSize = ((mass + 150) / 3) * earthRadius;
                }
                else
                {
                    planetSize = mass * earthRadius / 20;
                }
                //                Debug.Log(planetSize);
            }


			textureName = getTexture (planetSize);


            planetSize = planetSize * 2.0F / 10000.0F;
            // limit the planets to the width of the side view
			if ((panelXScale * planetDistance) < panelWidth) {
				newPlanet = GameObject.CreatePrimitive (PrimitiveType.Sphere);
				newPlanet.name = planetName;
				newPlanet.transform.position = new Vector3 (-0.5F * panelWidth + planetDistance * panelXScale, 0, 0);
				newPlanet.transform.localScale = new Vector3 (planetSize * planetScaleFactor, planetSize * planetScaleFactor, 5.0F * panelDepth * planetScaleFactor);
				newPlanet.AddComponent<planetMeta> ();
				newPlanet.GetComponent<planetMeta> ().planetSize = planetSize;
				newPlanet.GetComponent<planetMeta> ().planetDistance = planetDistance;

				newPlanet.GetComponent<Collider> ().isTrigger = true;

				planetMaterial = new Material (Shader.Find ("Standard"));
				newPlanet.GetComponent<MeshRenderer> ().material = planetMaterial;
				planetMaterial.mainTexture = Resources.Load (textureName) as Texture;

				sunRelated = thisSide;
				newPlanet.transform.parent = sunRelated.transform;

				sideplanetText = new GameObject ();
				sideplanetText.name = "Side planet Name";

				sideplanetText.transform.localScale = new Vector3 (0.05F, 0.05F, 0.05F);
				var planetTextMesh = sideplanetText.AddComponent<TextMesh> ();
				planetTextMesh.text = planets [planetCounter, 4];
				planetTextMesh.fontSize = 100;
				sideplanetText.transform.parent = thisSide.transform;
				sideplanetText.GetComponent<Renderer> ().enabled = false;

				newPlanet.AddComponent<DisplayNames> ();
				newPlanet.GetComponent<DisplayNames> ().text = sideplanetText;
				sideplanetText.transform.position = newPlanet.transform.position;

			} else {
				GameObject arrow;
				arrow = GameObject.CreatePrimitive(PrimitiveType.Cube);
				arrow.transform.parent = thisSide.transform;
				arrow.transform.position = new Vector3 (16.5F, 0, 0);
				arrow.transform.localScale = new Vector3 (3, .2F, .2F);
				Material dec = new Material(Resources.Load("arrow") as Material);
				arrow.GetComponent<MeshRenderer> ().material = dec;
				arrow.transform.Rotate (0, 0, 180);
			}
        }
        sideStarNumber++;
    }

    //------------------------------------------------------------------------------------//

    public void sideDealWithStar(string[] star, GameObject thisSide, GameObject theseOrbits)
    {
        GameObject newSidePanel;
        GameObject newSideSun;
        GameObject sideSunText;
        GameObject habZone;
        GameObject Swap;
        GameObject sunCompare;
        sunCompare = new GameObject();
        sunCompare.name = star[1];
        Material sideSunMaterial, habMaterial;

        newSidePanel = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newSidePanel.name = "Side " + star[1] + " Panel";
        newSidePanel.transform.position = new Vector3(0, 0, 0);
        newSidePanel.transform.localScale = new Vector3(panelWidth, panelHeight, panelDepth);
        newSidePanel.transform.parent = thisSide.transform;

        newSideSun = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newSideSun.name = "Side " + star[1] + " Star";
        newSideSun.transform.position = new Vector3(-0.5F * panelWidth - 0.5F, 0, 0);
        newSideSun.transform.localScale = new Vector3(1.0F, panelHeight * 40.0F, 2.0F * panelDepth);
        newSideSun.transform.parent = thisSide.transform;

        DrawStar(star, sunCompare);

        sideSunMaterial = new Material(Shader.Find("Unlit/Texture"));
        newSideSun.GetComponent<MeshRenderer>().material = sideSunMaterial;
        sideSunMaterial.mainTexture = Resources.Load(star[2]) as Texture;


        sideSunText = new GameObject();
        sideSunText.name = "Side Star Name";
        sideSunText.transform.position = new Vector3(-0.8F * panelWidth, 22.0F * panelHeight, 0);
        sideSunText.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
        var sunTextMesh = sideSunText.AddComponent<TextMesh>();
        sunTextMesh.text = star[1] +"\n"+ star[5] + "\n" + star[6];
        sunTextMesh.fontSize = 100;
        sideSunText.transform.parent = thisSide.transform;
        sideSunText.GetComponent<Renderer>().enabled = false;

        newSideSun.AddComponent<DisplayNames>();
        newSideSun.GetComponent<DisplayNames>().text = sideSunText;

        newSideSun.AddComponent<Display3D>();
        newSideSun.GetComponent<Display3D>().star_name = star[1];
        newSideSun.GetComponent<Display3D>().sl = sl;
        newSideSun.GetComponent<Display3D>().go = allCenter;
        
        newSideSun.GetComponent<Collider>().isTrigger = true;

        float innerHab = float.Parse(star[4]) * 9.5F;
        float outerHab = float.Parse(star[4]) * 14F;

        newSideSun.AddComponent<planetMeta>();
        newSideSun.GetComponent<planetMeta>().sunInnerHab = innerHab;
        newSideSun.GetComponent<planetMeta>().sunOuterHab = outerHab;

        // need to take panelXScale into account for the hab zone

        habZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
        habZone.name = "Hab" + star[1];
        habZone.transform.position = new Vector3((-0.5F * panelWidth) + ((innerHab + outerHab) * 0.5F * panelXScale), 0, 0);
        habZone.transform.localScale = new Vector3((outerHab - innerHab) * panelXScale, 40.0F * panelHeight, 2.0F * panelDepth);
        habZone.transform.parent = thisSide.transform;

        habMaterial = new Material(Shader.Find("Standard"));
        habZone.GetComponent<MeshRenderer>().material = habMaterial;
        habMaterial.mainTexture = Resources.Load("habitable") as Texture;

        

    }

    //------------------------------------------------------------------------------------//

    public void dealWithStar(string[] star, GameObject thisStar, GameObject theseOrbits)
    {

        GameObject newSun, upperSun;
        Material sunMaterial;

        GameObject sunRelated;
        GameObject sunSupport;
        GameObject sunText;

        float sunScale = float.Parse(star[0]) / 100000F;
        float centerSunSize = 0.25F;
        // set the habitable zone based on the star's luminosity
        innerHab = float.Parse(star[4]) * 9.5F;
        outerHab = float.Parse(star[4]) * 14F;


        GameObject sunMeta = new GameObject(star[1]);
        sunMeta.AddComponent<planetMeta>();
        sunMeta.GetComponent<planetMeta>().sunSuffix = starNumber;
        sunMeta.GetComponent<planetMeta>().sunInnerHab = innerHab;
        sunMeta.GetComponent<planetMeta>().sunOuterHab = outerHab;

        newSun = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        newSun.AddComponent<rotate>();
        newSun.name = star[1] + (starNumber.ToString());
        starNumber++;
        newSun.transform.position = new Vector3(0, 0, 0);
        newSun.transform.localScale = new Vector3(centerSunSize, centerSunSize, centerSunSize);

        sunRelated = thisStar;

        newSun.GetComponent<rotate>().rotateSpeed = -0.25F;

        sunMaterial = new Material(Shader.Find("Unlit/Texture"));
        newSun.GetComponent<MeshRenderer>().material = sunMaterial;
        sunMaterial.mainTexture = Resources.Load(star[2]) as Texture;

        newSun.transform.parent = sunRelated.transform;


        // copy the sun and make a bigger version above

        upperSun = Instantiate(newSun);
        upperSun.name = star[1] + " upper";
        upperSun.transform.localScale = new Vector3(sunScale, sunScale, sunScale);
        upperSun.transform.position = new Vector3(0, 10, 0);

        upperSun.transform.parent = sunRelated.transform;

        // draw the support between them
        sunSupport = GameObject.CreatePrimitive(PrimitiveType.Cube);
        sunSupport.transform.localScale = new Vector3(0.1F, 10.0F, 0.1F);
        sunSupport.transform.position = new Vector3(0, 5, 0);
        sunSupport.name = "Sun Support"+"  "+newSun.name;

        sunSupport.transform.parent = sunRelated.transform;


        sunText = new GameObject();
        sunText.name = newSun.name+" text";
        sunText.transform.position = new Vector3(0, 5, 0);
        sunText.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
        var sunTextMesh = sunText.AddComponent<TextMesh>();
        sunTextMesh.text = star[1];
        sunTextMesh.fontSize = 200;

        sunText.transform.parent = sunRelated.transform;

        drawOrbit(newSun.name + "Habitable Inner Ring", innerHab * orbitXScale, Color.green, habWidth, theseOrbits);
        drawOrbit(newSun.name + "Habitable Outer Ring", outerHab * orbitXScale, Color.green, habWidth, theseOrbits);
    }

    //------------------------------------------------------------------------------------//

    public void dealWithSystem(string[] starInfo, string[,] planetInfo, Vector3 offset, GameObject allThings)
    {
        GameObject SolarCenter;
        GameObject AllOrbits;
        GameObject SunStuff;
        GameObject Planets1;
        GameObject Swap;
        GameObject deleteStar;
        GameObject redoStar;


        SolarCenter = new GameObject();
        AllOrbits = new GameObject();
        SunStuff = new GameObject();
        Planets1 = new GameObject();

        SolarCenter.name = "SolarCenter" + " " + starInfo[1]+starNumber.ToString();
        AllOrbits.name = "All Orbits" + " " + starInfo[1];
        SunStuff.name = "Sun Stuff" + " " + starInfo[1];
        Planets1.name = "Planets" + " " + starInfo[1];

        SolarCenter.transform.parent = allThings.transform;
        AllOrbits.transform.parent = SolarCenter.transform;
        SunStuff.transform.parent = SolarCenter.transform;
        Planets1.transform.parent = SolarCenter.transform;

        
        if (k == 0)
        {
            dealWithStar(starInfo, SunStuff, AllOrbits);
            dealWithPlanets(planetInfo, Planets1, AllOrbits);
            k++;
        }
        // need to do this last
        SolarCenter.transform.position = offset;


        // add in second 'flat' representation
        GameObject SolarSide;
        SolarSide = new GameObject();
        SolarSide.name = "Side View of" + starInfo[1];
        SolarSide.AddComponent<BoxCollider>();

        Swap = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Vector3 v = new Vector3(-17f, 1.5f, 0f);
        Swap.transform.position = SolarSide.transform.position + v;
        Swap.transform.localScale = new Vector3(1f, 1f, 0.1f);
        Swap.name = SolarSide.name + "swap";
        Swap.transform.parent = SolarSide.transform;
        Swap.AddComponent<Swapping>();
        Swap.GetComponent<Swapping>().swapname = SolarSide.name;

        deleteStar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Vector3 v1 = new Vector3(-17f, -1.5f, 0f);
        deleteStar.transform.position= SolarSide.transform.position + v1;
        deleteStar.transform.localScale= new Vector3(1f, 1f, 0.1f);
        deleteStar.name = SolarSide.name + "deletestar";
        deleteStar.transform.parent = SolarSide.transform;
        deleteStar.AddComponent<DeleteStar>();
        deleteStar.GetComponent<DeleteStar>().starname = SolarSide.name;
        Material dec = new Material(Resources.Load("New Material") as Material);
        deleteStar.GetComponent<MeshRenderer>().material = dec;

        sideDealWithStar(starInfo, SolarSide, AllOrbits);
        sideDealWithPlanets(planetInfo, SolarSide, AllOrbits);

        SolarSide.transform.position = new Vector3(0, 8, 10.0F);
        SolarSide.transform.position += (offset * 0.15F);

    }

    //------------------------------------------------------------------------------------//
	public jsonDct getCurrentConfig()
	{
		jsonString_values = File.ReadAllText("Assets/Resources/InputValues.json");
		val = JsonUtility.FromJson<jsonDct>(jsonString_values);
		return val;
		
	}

    public void dealWithSystem_once(string[] starInfo, string[,] planetInfo, Vector3 offset, GameObject allThings)
    {
		getCurrentConfig ();
		orbitXScale = float.Parse(val.changedvalues.orbitXScale);
		planetScaleFactor = float.Parse(val.changedvalues.planetScaleFactor);

        SolarCenter = new GameObject();
        AllOrbits = new GameObject();
        SunStuff = new GameObject();
        Planets1 = new GameObject();

        SolarCenter.name = "SolarCenter" + " " + starInfo[1] + starNumber.ToString();
        AllOrbits.name = "All Orbits" + " " + starInfo[1];
        SunStuff.name = "Sun Stuff" + " " + starInfo[1];
        Planets1.name = "Planets" + " " + starInfo[1];

        SolarCenter.transform.parent = allThings.transform;
        AllOrbits.transform.parent = SolarCenter.transform;
        SunStuff.transform.parent = SolarCenter.transform;
        Planets1.transform.parent = SolarCenter.transform;

        dealWithStar(starInfo, SunStuff, AllOrbits);
        dealWithPlanets(planetInfo, Planets1, AllOrbits);
        k++;
       
        // need to do this last
        SolarCenter.transform.position = offset;
    }

    void DrawStar(string[] star, GameObject gstar)
    {
        GameObject newSun;
        Material sunMaterial;
        GameObject sunText;        

        float sunScale = float.Parse(star[0]) / 100000F;
        float centerSunSize = 1F;

        float lightYears = float.Parse(star[5])/10000F;

        newSun = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        newSun.AddComponent<rotate>();
        newSun.name = star[1];
        newSun.transform.position = new Vector3(-50f,-50f*lightYears,0f);
        newSun.transform.localScale = new Vector3(centerSunSize, centerSunSize, centerSunSize);

        newSun.GetComponent<rotate>().rotateSpeed = -0.25F;

        sunMaterial = new Material(Shader.Find("Unlit/Texture"));
        newSun.GetComponent<MeshRenderer>().material = sunMaterial;
        sunMaterial.mainTexture = Resources.Load(star[2]) as Texture;

        newSun.transform.parent = gstar.transform;
        
        sunText = new GameObject();
        sunText.name = newSun.name + " text";
        sunText.transform.position = newSun.transform.position;
        sunText.transform.localScale = new Vector3(0.01F, 0.01F, 0.01F);
        var sunTextMesh = sunText.AddComponent<TextMesh>();
        sunTextMesh.text = star[1];
        sunTextMesh.fontSize = 200;

        sunText.transform.parent = gstar.transform;
    }

    void menu()
    {
        Debug.Log("in menu");
        if (menuDisplay)
        {
            Save.GetComponent<MeshRenderer>().enabled = false;
            SaveText.GetComponent<MeshRenderer>().enabled = false;
            Reset.GetComponent<MeshRenderer>().enabled = false;
            ResetText.GetComponent<MeshRenderer>().enabled = false;

            planetSize.GetComponent<MeshRenderer>().enabled = false;
            planetSizeText.GetComponent<MeshRenderer>().enabled = false;
            planetSizePlus.GetComponent<MeshRenderer>().enabled = false;
            planetSizeMinus.GetComponent<MeshRenderer>().enabled = false;

            orbitSize.GetComponent<MeshRenderer>().enabled = false;
            orbitSizeText.GetComponent<MeshRenderer>().enabled = false;
            orbitSizePlus.GetComponent<MeshRenderer>().enabled = false;
            orbitSizeMinus.GetComponent<MeshRenderer>().enabled = false;

            speed.GetComponent<MeshRenderer>().enabled = false;
            speedText.GetComponent<MeshRenderer>().enabled = false;
            speedPlus.GetComponent<MeshRenderer>().enabled = false;
            speedMinus.GetComponent<MeshRenderer>().enabled = false;

            planetsNumber.GetComponent<MeshRenderer>().enabled = false;
            planetsNumberText.GetComponent<MeshRenderer>().enabled = false;
            habitableZones.GetComponent<MeshRenderer>().enabled = false;
            habitableZonesText.GetComponent<MeshRenderer>().enabled = false;
            sunLikeStars.GetComponent<MeshRenderer>().enabled = false;
            sunLikeStarsText.GetComponent<MeshRenderer>().enabled = false;
            closestEarth.GetComponent<MeshRenderer>().enabled = false;
            closestEarthText.GetComponent<MeshRenderer>().enabled = false;
            earthSized.GetComponent<MeshRenderer>().enabled = false;
            earthSizedText.GetComponent<MeshRenderer>().enabled = false;

            menuDisplay = false;
        }
        else
        {
            Save.GetComponent<MeshRenderer>().enabled = true;
            SaveText.GetComponent<MeshRenderer>().enabled = true;
            Reset.GetComponent<MeshRenderer>().enabled = true;
            ResetText.GetComponent<MeshRenderer>().enabled = true;

            planetSize.GetComponent<MeshRenderer>().enabled = true;
            planetSizeText.GetComponent<MeshRenderer>().enabled = true;
            planetSizePlus.GetComponent<MeshRenderer>().enabled = true;
            planetSizeMinus.GetComponent<MeshRenderer>().enabled = true;

            orbitSize.GetComponent<MeshRenderer>().enabled = true;
            orbitSizeText.GetComponent<MeshRenderer>().enabled = true;
            orbitSizePlus.GetComponent<MeshRenderer>().enabled = true;
            orbitSizeMinus.GetComponent<MeshRenderer>().enabled = true;

            speed.GetComponent<MeshRenderer>().enabled = true;
            speedText.GetComponent<MeshRenderer>().enabled = true;
            speedPlus.GetComponent<MeshRenderer>().enabled = true;
            speedMinus.GetComponent<MeshRenderer>().enabled = true;

            planetsNumber.GetComponent<MeshRenderer>().enabled = true;
            planetsNumberText.GetComponent<MeshRenderer>().enabled = true;
            habitableZones.GetComponent<MeshRenderer>().enabled = true;
            habitableZonesText.GetComponent<MeshRenderer>().enabled = true;
            sunLikeStars.GetComponent<MeshRenderer>().enabled = true;
            sunLikeStarsText.GetComponent<MeshRenderer>().enabled = true;
            closestEarth.GetComponent<MeshRenderer>().enabled = true;
            closestEarthText.GetComponent<MeshRenderer>().enabled = true;
            earthSized.GetComponent<MeshRenderer>().enabled = true;
            earthSizedText.GetComponent<MeshRenderer>().enabled = true;
            menuDisplay = true;
        }
    }

    void createButtons(GameObject g1,GameObject g2,string s,float offset)
    {
        Vector3 v = new Vector3(0.5f, 0f+offset, 1f);

        g1.transform.position = GameObject.FindGameObjectWithTag("Working Camera").transform.position + v;
        g1.transform.localScale = new Vector3(.5f, .1f, .001f);
        g1.transform.parent = GameObject.FindGameObjectWithTag("Working Camera").transform;
        
        
        g2.transform.position = new Vector3(g1.transform.position.x - 0.18f, g1.transform.position.y + 0.045f, g1.transform.position.z);
        g2.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
        g2.transform.parent = g1.transform;
        var ResetMesh = g2.AddComponent<TextMesh>();
        ResetMesh.text = s;
        ResetMesh.fontSize = 10;
        ResetMesh.color = Color.red;
        g1.GetComponent<MeshRenderer>().enabled = false;
        g2.GetComponent<MeshRenderer>().enabled = false;
    }

    void createSmallButtons(GameObject g1, GameObject g2, GameObject g3)
    {
        Vector3 plus=new Vector3(.5f,0f,0f);
        Vector3 minus = new Vector3(-.5f, 0f, 0f);
        Vector3 size_decrese = new Vector3(0f,1f,0f);
        Material inc = new Material(Resources.Load("Increse") as Material);
        Material dec = new Material(Resources.Load("New Material") as Material);

        g2.transform.parent = g1.transform;
        g2.transform.position = g1.transform.position + plus;
        g2.transform.localScale = g1.transform.localScale + size_decrese;
        g2.GetComponent<MeshRenderer>().material = inc;
        g2.GetComponent<MeshRenderer>().enabled = false;


        g3.transform.parent = g1.transform;
        g3.transform.position = g1.transform.position + minus;
        g3.transform.localScale = g1.transform.localScale + size_decrese;
        g3.GetComponent<MeshRenderer>().material = dec;
        g3.GetComponent<MeshRenderer>().enabled = false;
    }

    void createMenu()
    {
        Save = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Save.AddComponent<SaveState>();
        SaveText = new GameObject();
        createButtons(Save, SaveText, "SAVE", 0);

        Reset = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Reset.AddComponent<Reset>();
        ResetText = new GameObject();
        createButtons(Reset,ResetText,"RESET",0.13f);
      
        planetSize = GameObject.CreatePrimitive(PrimitiveType.Cube);
        planetSizeText = new GameObject();
        planetSizePlus = GameObject.CreatePrimitive(PrimitiveType.Cube);
        planetSizeMinus = GameObject.CreatePrimitive(PrimitiveType.Cube);
        createButtons(planetSize, planetSizeText, "PLANET",0.26f);
        createSmallButtons(planetSize,planetSizePlus,planetSizeMinus);
        planetSizePlus.AddComponent<increasePlanetScale>();
		planetSizePlus.GetComponent<increasePlanetScale>().p = this;
        planetSizePlus.GetComponent<increasePlanetScale>().sl = sl;
        planetSizeMinus.AddComponent<decreasePlanetScale>();
        planetSizeMinus.GetComponent<decreasePlanetScale>().sl = sl;

        orbitSize = GameObject.CreatePrimitive(PrimitiveType.Cube);
        orbitSizeText = new GameObject();
        orbitSizePlus = GameObject.CreatePrimitive(PrimitiveType.Cube);
        orbitSizeMinus = GameObject.CreatePrimitive(PrimitiveType.Cube);
        createButtons(orbitSize, orbitSizeText, "ORBIT", 0.39f);
        createSmallButtons(orbitSize,orbitSizePlus,orbitSizeMinus);
        orbitSizePlus.AddComponent<increaseOrbitScale>();
        orbitSizePlus.GetComponent<increaseOrbitScale>().sl = sl;
        orbitSizeMinus.AddComponent<decreaseOrbitScale>();
        orbitSizeMinus.GetComponent<decreaseOrbitScale>().sl = sl;

        speed = GameObject.CreatePrimitive(PrimitiveType.Cube);
        speedText = new GameObject();
        speedPlus = GameObject.CreatePrimitive(PrimitiveType.Cube);
        speedMinus = GameObject.CreatePrimitive(PrimitiveType.Cube);
        speedPlus.AddComponent<SpeedIncrese>();
        speedPlus.GetComponent<SpeedIncrese>().sl = sl;
        speedMinus.AddComponent<SpeedDecrese>();
        speedMinus.GetComponent<SpeedDecrese>().sl = sl;
        createButtons(speed, speedText, "SPEED", 0.52f);
        createSmallButtons(speed, speedPlus, speedMinus);
    }

    void createButtons2(GameObject g1, GameObject g2, string s, float offset)
    {
        Vector3 v = new Vector3(-1.1f + offset, -0.5f, 1f);

        g1.transform.position = GameObject.FindGameObjectWithTag("Working Camera").transform.position + v;
        g1.transform.localScale = new Vector3(.5f, .1f, .001f);
        g1.transform.parent = GameObject.FindGameObjectWithTag("Working Camera").transform;

        g2.transform.position = new Vector3(g1.transform.position.x - 0.18f, g1.transform.position.y + 0.045f, g1.transform.position.z);
        g2.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
        g2.transform.parent = g1.transform;
        var ResetMesh = g2.AddComponent<TextMesh>();
        ResetMesh.text = s;
        ResetMesh.fontSize = 10;
        ResetMesh.color = Color.red;
        g1.GetComponent<MeshRenderer>().enabled = false;
        g2.GetComponent<MeshRenderer>().enabled = false;
    }

    void createMenu2()
    {
		jsonString_Filters = File.ReadAllText("Assets/Resources/SystemFilters.json");
		fd = JsonUtility.FromJson<filterDict>(jsonString_Filters);


        Debug.Log("CREATEMENU2");
        planetsNumber = GameObject.CreatePrimitive(PrimitiveType.Cube);
        planetsNumber.name = "planetsNumber";
        planetsNumberText = new GameObject();
        createButtons2(planetsNumber,planetsNumberText,"Planets",0f);

		planetsNumber.AddComponent<FilterController> ();
		planetsNumber.GetComponent<FilterController> ().p = this;
		planetsNumber.GetComponent<FilterController> ().fd = fd;
		planetsNumber.GetComponent<FilterController> ().filterName = "planetNumber";

        habitableZones = GameObject.CreatePrimitive(PrimitiveType.Cube);
        habitableZones.name = "habitableZones";
        habitableZonesText = new GameObject();
        createButtons2(habitableZones,habitableZonesText,"Habitable",.55f);

		habitableZones.AddComponent<FilterController> ();
		habitableZones.GetComponent<FilterController> ().p = this;
		habitableZones.GetComponent<FilterController> ().fd = fd;
		habitableZones.GetComponent<FilterController> ().filterName = "habitableZone";


        earthSized = GameObject.CreatePrimitive(PrimitiveType.Cube);
        earthSized.name = "earthsized";
        earthSizedText = new GameObject();
        createButtons2(earthSized,earthSizedText,"earthSize",1.1f);


		earthSized.AddComponent<FilterController> ();
		earthSized.GetComponent<FilterController> ().p = this;
		earthSized.GetComponent<FilterController> ().fd = fd;
		earthSized.GetComponent<FilterController> ().filterName = "earthSized";


        closestEarth = GameObject.CreatePrimitive(PrimitiveType.Cube);
        closestEarth.name = "closest to earth";
        closestEarthText = new GameObject();
        createButtons2(closestEarth,closestEarthText,"Nearest",1.65f);

		closestEarth.AddComponent<FilterController> ();
		closestEarth.GetComponent<FilterController> ().p = this;
		closestEarth.GetComponent<FilterController> ().fd = fd;
		closestEarth.GetComponent<FilterController> ().filterName = "closestEarth";


        sunLikeStars = GameObject.CreatePrimitive(PrimitiveType.Cube);
        sunLikeStars.name = "sun like stars";
        sunLikeStarsText = new GameObject();
        createButtons2(sunLikeStars,sunLikeStarsText,"sun Like",2.2f);

		sunLikeStars.AddComponent<FilterController> ();
		sunLikeStars.GetComponent<FilterController> ().p = this;
		sunLikeStars.GetComponent<FilterController> ().fd = fd;
		sunLikeStars.GetComponent<FilterController> ().filterName = "sunLikeStars";


    }

	public void filterBasedOnFlag(filterDict fd)
	{
		DeleteSideView ();
		List<systems> newSys = new List<systems>();
		SystemList sl1 = sl;
		List<systems> systems = sl1.Systems;
		bool shouldAdd = true;
		foreach (systems system in systems) {
			shouldAdd = true;
			if (fd.ChangedFilters.moreThanTwoPlanets == "1") {			
				if (system.Planets.Count > 2)
					shouldAdd = true;
				else {
					shouldAdd = false;
					continue;
				}
				}
			if (fd.ChangedFilters.habitableSystems == "1") {			
				if (system.sunHabitat != "0") 
					shouldAdd = true;
				else {
					shouldAdd = false;
					continue;
				}
			}
			if (fd.ChangedFilters.earthSizePlanets == "1") {			
				foreach (var planet in system.Planets) {
					if (float.Parse(planet.planetSize) < 6800 && float.Parse(planet.planetSize) > 5900) {
						shouldAdd = true;
						break;
					}
					else
						shouldAdd = false;
				}
				if (!shouldAdd)
					continue;
			}
			if (fd.ChangedFilters.sunLikeStars == "1") {			
				if (float.Parse(system.sunScale)>0.85 && float.Parse(system.sunScale)<1.1) 
					shouldAdd = true;
				else {
					shouldAdd = false;
					continue;
				}
			}
			if (fd.ChangedFilters.nearestToSun == "1") {			
				if (float.Parse(system.lightYears) < 500) 
					shouldAdd = true;
				else {
					shouldAdd = false;
					continue;
				}
			}

			if (shouldAdd)
				newSys.Add (system);

		}


		//sl = new SystemList ();
		//sl.Systems = newSys;

		//sl1.Systems = newSys;
		SystemList newSl = new SystemList();
		newSl.Systems = newSys;

		planetSizeMinus.GetComponent<decreasePlanetScale> ().sl = newSl;
		planetSizePlus.GetComponent<increasePlanetScale> ().sl = newSl;
		orbitSizePlus.GetComponent<increaseOrbitScale> ().sl = newSl;
		orbitSizeMinus.GetComponent<decreaseOrbitScale> ().sl = newSl;
		speedPlus.GetComponent<SpeedIncrese> ().sl = newSl;
		speedMinus.GetComponent<SpeedDecrese> ().sl = newSl;


		sideStarNumber = 0;
		starNumber = 0;

		allCenter = new GameObject();
		int sunScaleRelative = 695500;
		long austronamicalUnit = 149597870;
		float year = 365;
		int earth_mass = 1;
		int earth_radius = 6371;
		allCenter.name = "all systems";
		var systemOffset = new Vector3(0, 0, 0);
		var oneOffset = new Vector3(0, -30, 0);
		int total_systems = newSys.Count;
		string[] sol = new string[7];
		k = 1;
		for (int i = 0; i < total_systems; i++)
		{
			int k = 0;
			sol[k++] = (float.Parse(newSys[i].sunScale) * sunScaleRelative).ToString();
			sol [k++] = newSys [i].sunName;
			sol[k++] = newSys[i].sunTexture;
			sol[k++] = newSys[i].sunVar;
			sol[k++] = newSys[i].sunHabitat;
			sol[k++] = newSys[i].lightYears;
			sol[k++] = newSys[i].discoveryMethod;
			int planet_count = newSys[i].Planets.Count;
			string[,] planets = new string[planet_count, 6];
			for (int j = 0; j < planet_count; j++)
			{
				k = 0;
				planets[j, k++] = (float.Parse(newSys[i].Planets[j].planetDistance) * austronamicalUnit).ToString();
				planets[j, k++] = newSys[i].Planets[j].planetSize;
				planets[j, k++] = (float.Parse(newSys[i].Planets[j].planetSpeed) / year).ToString();
				planets[j, k++] = newSys[i].Planets[j].textureName;
				planets[j, k++] = newSys[i].Planets[j].planetName;
				planets[j, k++] = newSys[i].Planets[j].planetMass;
			}
			dealWithSystem(sol, planets, systemOffset, allCenter);
			systemOffset += oneOffset;
		}
		allCenter.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
	}


    void SetInitialValues()
    {
		val = JsonUtility.FromJson<jsonDct>(jsonString_values);
        val.changedvalues.orbitXScale = val.orginalvalues.orbitXScale;
        val.changedvalues.planetScaleFactor = val.orginalvalues.planetScaleFactor;
        val.changedvalues.rotation_speed = val.orginalvalues.rotation_speed;
        JsonData jm;
        jm = JsonMapper.ToJson(val);
        string st = jm.ToString();
        File.WriteAllText("Assets/Resources/InputValues.json", st);

		//For Filters
		fd = JsonUtility.FromJson<filterDict>(jsonString_Filters);
		fd.ChangedFilters.moreThanTwoPlanets = fd.OrginalFilters.moreThanTwoPlanets;
		fd.ChangedFilters.earthSizePlanets = fd.OrginalFilters.earthSizePlanets;
		fd.ChangedFilters.sunLikeStars = fd.OrginalFilters.sunLikeStars;
		fd.ChangedFilters.nearestToSun = fd.OrginalFilters.nearestToSun;
		fd.ChangedFilters.habitableSystems = fd.OrginalFilters.habitableSystems;
		jm = JsonMapper.ToJson(fd);
		st = jm.ToString();
		File.WriteAllText("Assets/Resources/SystemFilters.json", st);
    }

    void DeleteSideView()
    {
        for (int index = 0; index <= 600; index++)
        {
            Vector3 v = new Vector3(0f, 8f-index*4.5f, 10f);
            Collider[] colliders;
            if ((colliders = Physics.OverlapSphere(v, 1f)).Length > 1)
            {
                foreach (var collider in colliders)
                {
                    var go = collider.gameObject;
                    if ((go.name.Length>12)&&(go.name.Substring(0,12)=="Side View of"))
                    {
                        Destroy(go);
                    }
                }
            }
        }
    }
    void Start()
    {
        sl= JsonUtility.FromJson<SystemList>(jsonString);
        SetInitialValues();
        revolutionSpeed = float.Parse(val.orginalvalues.rotation_speed);
        allCenter = new GameObject();
        int sunScaleRelative = 695500;
        long austronamicalUnit = 149597870;
        float year = 365;
        int earth_mass = 1;
        int earth_radius = 6371;
        allCenter.name = "all systems";
        var systemOffset = new Vector3(0, 0, 0);
        var oneOffset = new Vector3(0, -30, 0);
        int total_systems = sl.Systems.Count;
        string[] sol = new string[7];
        for (int i = 0; i < total_systems; i++)
        {
            int z = 0;
            sol[z++] = (float.Parse(sl.Systems[i].sunScale) * sunScaleRelative).ToString();
            sol[z++] = sl.Systems[i].sunName;
            sol[z++] = sl.Systems[i].sunTexture;
            sol[z++] = sl.Systems[i].sunVar;
            sol[z++] = sl.Systems[i].sunHabitat;
            sol[z++] = sl.Systems[i].lightYears;
            sol[z++] = sl.Systems[i].discoveryMethod;
            int planet_count = sl.Systems[i].Planets.Count;
            string[,] planets = new string[planet_count, 6];
            for (int j = 0; j < planet_count; j++)
            {
                z = 0;
                planets[j, z++] = (float.Parse(sl.Systems[i].Planets[j].planetDistance) * austronamicalUnit).ToString();
                planets[j, z++] = sl.Systems[i].Planets[j].planetSize;
                planets[j, z++] = (float.Parse(sl.Systems[i].Planets[j].planetSpeed) / year).ToString();
                planets[j, z++] = sl.Systems[i].Planets[j].textureName;
                planets[j, z++] = sl.Systems[i].Planets[j].planetName;
                planets[j, z++] = sl.Systems[i].Planets[j].planetMass;
            }
            dealWithSystem(sol, planets, systemOffset, allCenter);
            systemOffset += oneOffset;
        }
        allCenter.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
        createMenu();
        createMenu2();
        k = Reset.GetComponent<Reset>().k;
    }

    void Update()
    {
        bool isKeyPressed = Input.GetKeyDown(KeyCode.Space);
        if (isKeyPressed)
        {
            menu();
            //DeleteSideView();
        }
        //if (SteamVR_Controller.Input(SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost)).GetHairTriggerDown())
        //  menu();
    }

}

[System.Serializable]
public class Planats
{
    public string planetDistance;
    public string planetSize;
    public string planetSpeed;
    public string textureName;
    public string planetName;
    public string planetMass;
}

[System.Serializable]
public class systems
{
    public string sunScale;
    public string sunName;
    public string sunTexture;
    public string sunVar;
    public string sunHabitat;
    public string lightYears;
    public string discoveryMethod;
    public List<Planats> Planets;
}


[System.Serializable]
public class SystemList
{
    public List<systems> Systems;
}


[System.Serializable]
public class OrginalValues
{
    public string rotation_speed;
    public string orbitXScale;
    public string planetScaleFactor;
}

[System.Serializable]
public class ChangedValues
{
    public string rotation_speed;
    public string orbitXScale;
    public string planetScaleFactor;
}

[System.Serializable]
public class jsonDct
{
    public OrginalValues orginalvalues;
    public ChangedValues changedvalues;
}

[System.Serializable]
public class OrginalFilters
{
	public string moreThanTwoPlanets;
	public string habitableSystems;
	public string earthSizePlanets;
	public string nearestToSun;
	public string sunLikeStars;
}

[System.Serializable]
public class ChangedFilters
{
	public string moreThanTwoPlanets;
	public string habitableSystems;
	public string earthSizePlanets;
	public string nearestToSun;
	public string sunLikeStars;
}

[System.Serializable]
public class filterDict
{
	public OrginalFilters OrginalFilters;
	public ChangedFilters ChangedFilters;
}