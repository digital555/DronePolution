using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DroneFlight : MonoBehaviour {

    public GameObject[,] cubesXZ;

    public GameObject drone;
    Rigidbody m_Rigidbody;

    float x_Ax, y_Ax, fire, x_Start, y_Start, xMin, xMax, zMin, zMax, dX, dZ, previousTime = 0, currIntensity, currTime;

    int hits = 0;
    int segX, segZ, lastBoxX, lastBoxZ;

    Dictionary<string, float[]> magnitudes = new Dictionary<string, float[]>();

     

    // Use this for initialization
    void Start () {

        cubesXZ = new GameObject[1,1];
  

        xMax = drone.transform.position.x;
        xMin = drone.transform.position.x;
        zMax = drone.transform.position.z;
        zMin = drone.transform.position.z;


        m_Rigidbody = drone.GetComponent<Rigidbody>();

        x_Start = drone.transform.position.z;
        y_Start = drone.transform.position.y;

        Debug.Log(x_Start + "; " + y_Start);




    }
	
	// Update is called once per frame
	void Update () {

        x_Ax = Input.GetAxis("Horizontal");
        y_Ax = Input.GetAxis("Vertical");
        fire = Input.GetAxis("Fire1");

        m_Rigidbody.transform.position = new Vector3(drone.transform.position.x, drone.transform.position.y + y_Ax, drone.transform.position.z);

        m_Rigidbody.transform.position += transform.right * fire;

        m_Rigidbody.transform.RotateAroundLocal(new Vector3(0, 1, 0), x_Ax * 0.03f);

        //Debug.Log(m_Rigidbody.GetRelativePointVelocity(new Vector3(0,0,0)));

        WidthHeight(drone.transform.position.x, drone.transform.position.z);
	}


    void OnParticleCollision(GameObject test)
    {
        hits++;
        //Debug.Log("Got hit "/* + hits*/);
        //Debug.Log(previousTime);
        //SaveToExcel(("" + hits));
        //WriteString();

        if(Time.fixedTime - previousTime > 0.01f)
            DyeCubes((int)(drone.transform.position.x - xMin)/10, (int)(drone.transform.position.z - zMin)/10, 1, Time.fixedTime - previousTime);

        previousTime = Time.fixedTime;
    }

    /*void SaveToExcel(string textTo)
    {
        using(StreamWriter writetext = new StreamWriter("write.txt"))
        {
            writetext.WriteLine(textTo);
            writetext.WriteLine(textTo + "1");
        }
    }*/

    static void WriteString()
    {
        string path = "write.xlsx";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Test \t test \t test");
        writer.Close();

        //Re-import the file to update the reference in the editor

        //Print the text from the file
    }

    public void WidthHeight(float x, float z)
    {
        if(x > xMax)
        {
            xMax = x;
        }

        if(x < xMin)
        {
            xMin = x;
        }

        if(z > zMax)
        {
            zMax = z;
        }

        if(z < zMin)
        {
            zMin = z;
        }

        dX = xMax - xMin;
        dZ = zMax - zMin;

        segX = (int)(dX / 10) + 1 ;
        segZ = (int)(dZ / 10) + 1 ;

        Cubes(segX, segZ);
    }

    void Cubes(int dimX, int dimZ)
    {
        if (cubesXZ.GetLength(0) < dimX || cubesXZ.GetLength(1) < dimZ)
        {
            foreach (GameObject gos in cubesXZ)
            {
                Destroy(gos);
            }
            cubesXZ = new GameObject[dimX, dimZ];
            for(int i = 0; i < cubesXZ.GetLength(0); i++)
            {                
                for(int j = 0; j < cubesXZ.GetLength(1); j++)
                {
                    cubesXZ[i, j] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cubesXZ[i, j].transform.position = new Vector3(i, 0, j);
                    Debug.Log(i + " ; " + j);
                    DyeCubes(i, j, 0, 0);
                }
            }
        }
    }

    void DyeCubes(int x, int z, float intensity, float time)
    {
        if(!magnitudes.ContainsKey(x + ";" + z))
        {
            magnitudes.Add((x + ";" + z), new float[2]);
            magnitudes[(x + ";" + z)][0] += intensity;
            magnitudes[(x + ";" + z)][1] += time;
        } else
        {
            magnitudes[(x + ";" + z)][0] += intensity;
            magnitudes[(x + ";" + z)][1] += time;           
        }

        currIntensity = magnitudes[(x + ";" + z)][0];
        currTime = magnitudes[(x + ";" + z)][1];


        if(!(currIntensity > 0 || currTime > 0));
            cubesXZ[x, z].GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 0f, 0f, 0f, (currIntensity / (currTime * 100)), (currIntensity / (currTime * 100)));

        Debug.Log(x + " ; " + z + " " + (currIntensity / (currTime * 100)));
    }
}
