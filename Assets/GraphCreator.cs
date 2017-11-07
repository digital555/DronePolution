using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphCreator : MonoBehaviour {




	// Use this for initialization
	void Start () {
       
        //cubeCreate();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void cubeCreate ()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }


}
