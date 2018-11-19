using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {

    private GameManager.States prevState;

    // Update is called once per frame
    public void OnPause () {

        if (gameObject.activeSelf)
        {
            Time.timeScale = 1f;
            GameManager.currentState = prevState;
            gameObject.SetActive(false);
        }
        else
        {
            prevState = GameManager.currentState;
            GameManager.currentState = GameManager.States.PAUSED;
            Time.timeScale = 0f;
            gameObject.SetActive(true);
        }

	}

    public void OnExit()
    {
        Time.timeScale = 1f;
        TileManager.playerInstance.Clear();
        SceneManager.LoadScene("Menu Gioco");
    }

}
