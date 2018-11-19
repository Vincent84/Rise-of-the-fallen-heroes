using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PowerUpManager : MonoBehaviour {

    public GameObject playersPowerUp;
    public Button confirm;
    private List<GameObject> playersPanel;

	// Use this for initialization
	void Awake () {
        playersPanel = new List<GameObject>();
        for(int i=0; i<TileManager.playerInstance.Count; i++)
        {
            TileManager.playerInstance[i].SetActive(false);
            playersPowerUp.transform.GetChild(i).gameObject.SetActive(true);
            playersPowerUp.transform.GetChild(i).GetComponent<PlayerLevelUp>().Player = TileManager.playerInstance[i];
            playersPanel.Add(playersPowerUp.transform.GetChild(i).gameObject);
        }

        confirm.onClick.AddListener(() => OnConfirm());
	}

    private void Update()
    {
        StartCoroutine(CheckAllComplete());
    }

    public void OnConfirm()
    {
        foreach (GameObject playerPanel in playersPanel)
        {
            playerPanel.GetComponent<PlayerLevelUp>().Player.GetComponent<ObjectController>().mind = int.Parse(playerPanel.GetComponent<PlayerLevelUp>().mindValue.GetComponent<TextMeshProUGUI>().text);
            playerPanel.GetComponent<PlayerLevelUp>().Player.GetComponent<ObjectController>().constitution = int.Parse(playerPanel.GetComponent<PlayerLevelUp>().constitutiionValue.GetComponent<TextMeshProUGUI>().text);
            playerPanel.GetComponent<PlayerLevelUp>().Player.GetComponent<ObjectController>().skill = int.Parse(playerPanel.GetComponent<PlayerLevelUp>().skillValue.GetComponent<TextMeshProUGUI>().text);
            playerPanel.GetComponent<PlayerLevelUp>().Player.GetComponent<ObjectController>().strength = int.Parse(playerPanel.GetComponent<PlayerLevelUp>().strengthValue.GetComponent<TextMeshProUGUI>().text);
        }
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(TotemController.nextScene);
    }

    IEnumerator CheckAllComplete()
    {
        int i = 0;
        foreach(GameObject playerPanel in playersPanel)
        {
            if(playerPanel.GetComponent<PlayerLevelUp>().isComplete)
            {
                i++;
            }
        }

        if(i != playersPanel.Count)
        {
            confirm.interactable = false;
            yield return null;
        }
        else
        {
            confirm.interactable = true;
        }
    }
}
