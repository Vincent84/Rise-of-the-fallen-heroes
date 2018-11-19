using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class FineCrediti : MonoBehaviour {
    public float Time = 2.0f;
    public bool timer = true;
	public int sceneIndex = 1;

	// Use this for initialization
	void Start () {
        Invoke("cambiaScena", Time);
        //if (timer == true) { }
	}
	
	// Update is called once per frame
	void Update () {	
	}

    void cambiaScena() {
		SceneManager.LoadScene(sceneIndex);
    }
}
