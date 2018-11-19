using System.Collections;
using UnityEngine;

public class TotemController : MonoBehaviour {

    public GameObject cutscene;
    public string followScene;

    public static string nextScene;
    private Animator anim;
    private string cutsceneName;

	// Use this for initialization
	void Awake () {
        cutsceneName = gameObject.name.Split('_')[1];
        anim = cutscene.GetComponent<Animator>();
        nextScene = followScene;
        GetComponent<Animator>().SetBool("showFlash", true);
	}
	
	// Update is called once per frame
	public void OnCutscene () {
        cutscene.SetActive(true);
        anim.SetBool(cutsceneName, true);
        anim.SetBool("showCutscene", true);
        StartCoroutine(ShowCutscene());
	}

    IEnumerator ShowCutscene()
    {
        GetComponent<Animator>().SetBool("showFlash", false);
        yield return new WaitForSeconds(11.5f);
        anim.SetBool("showCutscene", false);
        anim.SetBool(cutsceneName, false);

        if(cutsceneName != "Final")
        {
            foreach (GameObject player in TileManager.playerInstance)
            {
                DontDestroyOnLoad(player);
            }
        }

        GameManager.FinishLevel();
    }
}
