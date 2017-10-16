/// Sample Code for CS 491 Virtual And Augmented Reality Course - Fall 2017
/// written by Andy Johnson
/// 
/// makes use of various textures from the celestia motherlode - http://www.celestiamotherlode.net/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Planets : MonoBehaviour
{
    float panelHeight = 0.1F;
    float panelWidth = 30.0F;
    float panelDepth = 0.1F;

    float orbitWidth = 0.01F;
    float habWidth = 0.03F;

    static float revolutionSpeed;

    float panelXScale = 2.0F;
    float innerHab;
    float outerHab;

    static int k = 0;
    public static string jsonString = File.ReadAllText("Assets/Resources/Planetary_system_information.json");
    public SystemList sl = JsonUtility.FromJson<SystemList>(jsonString);


    public static string jsonString_values = File.ReadAllText("Assets/Resources/InputValues.json");
    public jsonDct val = JsonUtility.FromJson<jsonDct>(jsonString_values);

	float orbitXScale = 2.0F;
	float planetScaleFactor = 1.0F;

    
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
    public static int starNumber = 0;
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
            Debug.Log("planetSpeed");
            Debug.Log(revolutionSpeed);
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
            
            planetSize = planetSize * 2.0F / 10000.0F;

            newPlanetCenter = new GameObject();
            newPlanetCenter.name = planetName + "Center";
            newPlanetCenter.AddComponent<rotate>();

            newPlanet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newPlanet.name = planetName;
            newPlanet.transform.position = new Vector3(0, 0, planetDistance * orbitXScale);
			newPlanet.transform.localScale = new Vector3(planetSize * planetScaleFactor, planetSize*planetScaleFactor, planetSize*planetScaleFactor);
            newPlanet.transform.parent = newPlanetCenter.transform;


			//GameObject planetMetaObject = new GameObject(planets[planetCounter, 4]);
			newPlanet.AddComponent<planetMeta>();
			newPlanet.GetComponent<planetMeta> ().planetSuffixNumber = starNumber;
			newPlanet.GetComponent<planetMeta> ().planetSize = planetSize;
			newPlanet.GetComponent<planetMeta> ().planetDistance = planetDistance;


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
                Debug.Log(innerHab + "   " + outerHab+"   "+planetName +"   "+planetDistance);
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
            string planetName = "Side"+planets[planetCounter, 4];
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
            planetSize = planetSize * 2.0F / 10000.0F;
            // limit the planets to the width of the side view
            if ((panelXScale * planetDistance) < panelWidth)
            {

                newPlanet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                newPlanet.name = planetName;
                newPlanet.transform.position = new Vector3(-0.5F * panelWidth + planetDistance * panelXScale, 0, 0);
                newPlanet.transform.localScale = new Vector3(planetSize, planetSize, 5.0F * panelDepth);

                newPlanet.GetComponent<Collider>().isTrigger = true;

                planetMaterial = new Material(Shader.Find("Standard"));
                newPlanet.GetComponent<MeshRenderer>().material = planetMaterial;
                planetMaterial.mainTexture = Resources.Load(textureName) as Texture;

                sunRelated = thisSide;
                newPlanet.transform.parent = sunRelated.transform;

                sideplanetText = new GameObject();
                sideplanetText.name = "Side planet Name";

                sideplanetText.transform.localScale = new Vector3(0.05F, 0.05F, 0.05F);
                var planetTextMesh = sideplanetText.AddComponent<TextMesh>();
                planetTextMesh.text = planets[planetCounter, 4];
                planetTextMesh.fontSize = 100;
                sideplanetText.transform.parent = thisSide.transform;
                sideplanetText.GetComponent<Renderer>().enabled = false;

                newPlanet.AddComponent<DisplayNames>();
                newPlanet.GetComponent<DisplayNames>().text = sideplanetText;
                sideplanetText.transform.position = newPlanet.transform.position;

            }
        }
    }

    //------------------------------------------------------------------------------------//

    public void sideDealWithStar(string[] star, GameObject thisSide, GameObject theseOrbits)
    {
        GameObject newSidePanel;
        GameObject newSideSun;
        GameObject sideSunText;
        GameObject habZone;
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

        newSideSun.GetComponent<Rigidbody>();
        newSideSun.AddComponent<GrabbableObject>();

        newSideSun.GetComponent<Collider>().isTrigger = true;

        float innerHab = float.Parse(star[4]) * 9.5F;
        float outerHab = float.Parse(star[4]) * 14F;


        // need to take panelXScale into account for the hab zone

        habZone = GameObject.CreatePrimitive(PrimitiveType.Cube);
        habZone.name = "Hab";
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

		GameObject sunMeta = new GameObject (star [1]);

		sunMeta.AddComponent<planetMeta> ();
		sunMeta.GetComponent<planetMeta> ().sunSuffix = starNumber;
		sunMeta.GetComponent<planetMeta> ().sunInnerHab = innerHab;
		sunMeta.GetComponent<planetMeta> ().sunOuterHab = outerHab;


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
        sunSupport.name = "Sun Support";

        sunSupport.transform.parent = sunRelated.transform;


        sunText = new GameObject();
        sunText.name = "Star Name";
        sunText.transform.position = new Vector3(0, 5, 0);
        sunText.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
        var sunTextMesh = sunText.AddComponent<TextMesh>();
        sunTextMesh.text = star[1];
        sunTextMesh.fontSize = 200;

        sunText.transform.parent = sunRelated.transform;

		drawOrbit(newSun.name+"Habitable Inner Ring", innerHab * orbitXScale, Color.green, habWidth, theseOrbits);
		drawOrbit(newSun.name+"Habitable Outer Ring", outerHab * orbitXScale, Color.green, habWidth, theseOrbits);
    }

    //------------------------------------------------------------------------------------//

    public void dealWithSystem(string[] starInfo, string[,] planetInfo, Vector3 offset, GameObject allThings)
    {
        GameObject SolarCenter;
        GameObject AllOrbits;
        GameObject SunStuff;
        GameObject Planets1;

        SolarCenter = new GameObject();
        AllOrbits = new GameObject();
        SunStuff = new GameObject();
        Planets1 = new GameObject();

        SolarCenter.name = "SolarCenter" + " " + starInfo[1];
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

        sideDealWithStar(starInfo, SolarSide, AllOrbits);
        sideDealWithPlanets(planetInfo, SolarSide, AllOrbits);

        SolarSide.transform.position = new Vector3(0, 8, 10.0F);
        SolarSide.transform.position += (offset * 0.15F);

    }

    //------------------------------------------------------------------------------------//

    public void dealWithSystem_once(string[] starInfo, string[,] planetInfo, Vector3 offset, GameObject allThings)
    {
        SolarCenter = new GameObject();
        AllOrbits = new GameObject();
        SunStuff = new GameObject();
        Planets1 = new GameObject();

        SolarCenter.name = "SolarCenter" + " " + starInfo[1];
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
		planetSizePlus.GetComponent<increasePlanetScale> ().sl = sl;
		planetSizeMinus.AddComponent<decreasePlanetScale>();
		planetSizeMinus.GetComponent<decreasePlanetScale> ().sl = sl;


        orbitSize = GameObject.CreatePrimitive(PrimitiveType.Cube);
        orbitSizeText = new GameObject();
        orbitSizePlus = GameObject.CreatePrimitive(PrimitiveType.Cube);
        orbitSizeMinus = GameObject.CreatePrimitive(PrimitiveType.Cube);
        createButtons(orbitSize, orbitSizeText, "ORBIT", 0.39f);
        createSmallButtons(orbitSize,orbitSizePlus,orbitSizeMinus);
		orbitSizePlus.AddComponent<increaseOrbitScale>();
		orbitSizePlus.GetComponent<increaseOrbitScale> ().sl = sl;
		orbitSizeMinus.AddComponent<decreaseOrbitScale>();
		orbitSizeMinus.GetComponent<decreaseOrbitScale> ().sl = sl;

        speed = GameObject.CreatePrimitive(PrimitiveType.Cube);
        speedText = new GameObject();
        speedPlus = GameObject.CreatePrimitive(PrimitiveType.Cube);
        speedMinus = GameObject.CreatePrimitive(PrimitiveType.Cube);
        speedPlus.AddComponent<SpeedIncrese>();
        speedPlus.GetComponent<SpeedIncrese>().sl = sl;

        speedMinus.AddComponent<SpeedDecrese>();
        createButtons(speed, speedText, "SPEED", 0.52f);
        createSmallButtons(speed, speedPlus, speedMinus);
    }

    void Start()
    {

	//	orbitXScale = float.Parse(val.changedvalues.orbitXScale);
	//	planetScaleFactor = float.Parse(val.changedvalues.planetScaleFactor);
		
        createMenu();
        k = Reset.GetComponent<Reset>().k;
        revolutionSpeed = float.Parse(val.changedvalues.rotation_speed);
//        Debug.Log(val.changedvalues.rotation_speed);
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
            dealWithSystem(sol, planets, systemOffset, allCenter);
            systemOffset += oneOffset;
        }
        allCenter.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
    }
    // Update is called once per frame
    void Update()
    {
        revolutionSpeed = float.Parse(val.changedvalues.rotation_speed);
        bool isKeyPressed = Input.GetKeyDown(KeyCode.Space);
        if (isKeyPressed)
            menu();
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