using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levelmanager : MonoBehaviour {

	public bool isNeeded = true;
	public float Timer;
	public int sceneIndex;

	
	public void LoadLevel (string name){
		SceneManager.LoadScene(name);
	}

	public void LoadByIndex (int sceneIndex){
		SceneManager.LoadScene (sceneIndex);
	}

	public void LoadAfterTimer(){
		if (isNeeded == true) {
			Invoke ("LoadScene", Timer);
		}
	}

	public void LoadScene(){
		SceneManager.LoadScene(sceneIndex);
	}

	public void Quit(){
//#if UNITY_EDITOR
	//UnityEditor.EditorApplication.isPlaying = false;
//#else
	//Application.Quit();
//#endif 
		Application.Quit() ; 
	}

}
