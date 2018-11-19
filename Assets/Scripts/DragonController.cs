using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour {

    public GameObject flames;
    public Transform dragonHeadPosition;
    public static bool instanceFlame;
	
	// Update is called once per frame
	void Update () {
		if(instanceFlame)
        {
            Instantiate(flames, dragonHeadPosition);
            instanceFlame = false;
        }
	}
}
