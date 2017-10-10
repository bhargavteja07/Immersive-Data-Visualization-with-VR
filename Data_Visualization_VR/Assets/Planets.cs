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

    float revolutionSpeed = 0.2F;

    float panelXScale = 2.0F;
    float orbitXScale = 2.0F;
    static int k = 0;
    public static string jsonString = File.ReadAllText("Assets/Resources/Planetary_system_information.json");
    public SystemList sl = JsonUtility.FromJson<SystemList>(jsonString);
    public GameObject allCenter;

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
            string textureName = planets[planetCounter, 3];
            string planetName = planets[planetCounter, 4];
            string planetMass = planets[planetCounter, 5];
            int earthRadius = 6371;
            if (planetSize == 0)
            {
                float mass = float.Parse(planetMass);
                if ((0 < mass) && (mass <= 2))
                {
                    Debug.Log("im in <2");
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
                Debug.Log(planetName + " --  " + planetSize);
            }
            planetSize = planetSize * 2.0F / 10000.0F;

            newPlanetCenter = new GameObject(planetName + "Center");
            newPlanetCenter.AddComponent<rotate>();

            newPlanet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newPlanet.name = planetName;
            newPlanet.transform.position = new Vector3(0, 0, planetDistance * orbitXScale);
            newPlanet.transform.localScale = new Vector3(planetSize, planetSize, planetSize);
            newPlanet.transform.parent = newPlanetCenter.transform;

            newPlanetCenter.GetComponent<rotate>().rotateSpeed = planetSpeed;

            planetMaterial = new Material(Shader.Find("Standard"));
            newPlanet.GetComponent<MeshRenderer>().material = planetMaterial;
            planetMaterial.mainTexture = Resources.Load(textureName) as Texture;

            drawOrbit(planetName + " orbit", planetDistance * orbitXScale, Color.white, orbitWidth, theseOrbits);

            sunRelated = thesePlanets;
            newPlanetCenter.transform.parent = sunRelated.transform;
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
            string planetName = planets[planetCounter, 4];
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
        float innerHab = float.Parse(star[4]) * 9.5F;
        float outerHab = float.Parse(star[4]) * 14F;


        newSun = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        newSun.AddComponent<rotate>();
        newSun.name = star[1];
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


        drawOrbit("Habitable Inner Ring", innerHab * orbitXScale, Color.green, habWidth, theseOrbits);
        drawOrbit("Habitable Outer Ring", outerHab * orbitXScale, Color.green, habWidth, theseOrbits);
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


//        if (k == 0)
//        {
            dealWithStar(starInfo, SunStuff, AllOrbits);
            dealWithPlanets(planetInfo, Planets1, AllOrbits);
            k++;
//        }
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

        dealWithStar(starInfo, SunStuff, AllOrbits);
        dealWithPlanets(planetInfo, Planets1, AllOrbits);
        k++;
       
        // need to do this last
        SolarCenter.transform.position = offset;
    }

    void Start()
    {
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
