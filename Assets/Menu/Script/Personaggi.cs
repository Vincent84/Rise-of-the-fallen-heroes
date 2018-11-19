using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Personaggi : MonoBehaviour {

    public Button Conferma;

    public int party;

	// Use this for initialization
	void Start () {
        party = 0;
	}
	
	// Update is called once per frame
	public void Update () {
        if (party >= 4)
        {
            sblocco();
        }
        else {
            Conferma.interactable = false;
         
        }
    }

    public void sblocco() {
            Conferma.interactable = true;
    }

    public void reset()
    {
        party = 0;
    }


    public void ImIn() {
        party++;
    }
}
