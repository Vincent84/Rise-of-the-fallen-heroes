using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

    public void OnRestart()
    {
        foreach (GameObject player in TileManager.playerInstanceClone)
        {
            DontDestroyOnLoad(player);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnMenu()
    {
        GameObject[] GameObjects = (FindObjectsOfType<GameObject>() as GameObject[]);

        for (int i = 0; i < GameObjects.Length; i++)
        {
            Destroy(GameObjects[i]);
        }
        TileManager.playerInstance = null;
        SceneManager.LoadScene("Menu Gioco");
    }
}
