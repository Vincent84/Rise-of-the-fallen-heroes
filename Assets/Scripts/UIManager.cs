using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UIManager : MonoBehaviour {

    public TextMeshProUGUI popupDamage;
    public GameObject changeTurnText;
    public TextMeshProUGUI fightImage;
    public List<GameObject> playersTurns;
    public GameObject enemyHealth;
    public GameObject turnUIBar;
    public GameObject turnUI;
    public GameObject healthTurn;
    public GameObject healthTurnText;

    public Sprite borderSelected;
    public Sprite borderEmpty;

    private List<GameObject> healthEnemies;
    private List<GameObject> turnsBarUI;


    private void Update()
    {
        if(TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player")
        {
            int totalHealth = TurnManager.currentObjectTurn.GetComponent<ObjectController>().totalHealth;
            int currentHealth = TurnManager.currentObjectTurn.GetComponent<ObjectController>().currentHealth;
            float barHealth = (float)currentHealth / totalHealth;

            healthTurn.transform.GetChild(0).transform.localScale = new Vector3(Mathf.Clamp(barHealth, 0f, 1f), healthTurn.transform.localScale.y, healthTurn.transform.localScale.z);
            healthTurnText.GetComponent<TextMeshProUGUI>().SetText(currentHealth.ToString() + "/" + totalHealth.ToString() + " HP");
        }
    }

    public void ShowPopupDamage(int damage, Transform location)
    {
        TextMeshProUGUI popup = Instantiate(popupDamage);
        popup.transform.SetParent(transform, false);
        popup.transform.position = location.position + Vector3.up;
        popup.text = Mathf.Abs(damage).ToString();
        StartCoroutine(DestroyText(popup, 1));
    }

    /**
     * 
     * Metodo che fa apparire la UI del personaggio selezionato
     * 
     **/
    public void ShowPlayerUI(GameObject player)
    {
        player.GetComponent<PlayerController>().playerUI.SetActive(true);
        player.GetComponent<PlayerController>().iconUI.SetActive(true);
        healthTurn.gameObject.SetActive(true);
        healthTurnText.gameObject.SetActive(true);
    }

    /**
     * 
     * Metodo che nasconde la UI del personaggio selezionato
     * 
     **/
    public void HidePlayerUI(GameObject player)
    {
        player.GetComponent<PlayerController>().playerUI.SetActive(false);
        player.GetComponent<PlayerController>().iconUI.SetActive(false);
        healthTurn.gameObject.SetActive(false);
        healthTurnText.gameObject.SetActive(false);
    }

    public void SetChangeTurnText(string text)
    {
        GameObject turnText = Instantiate(changeTurnText);
        turnText.transform.SetParent(transform, false);
        turnText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(text);
        StartCoroutine(DestroyObject(turnText, 1f));
    }

    public void ShowImageFight()
    {
        TextMeshProUGUI txt = Instantiate(fightImage);
        txt.transform.SetParent(transform, false);
        StartCoroutine(DestroyText(txt, 2.5f));
    }

    public void SetPlayerHealthBar(GameObject player)
    {
        int totalHealth = player.GetComponent<ObjectController>().totalHealth;
        int currentHealth = player.GetComponent<ObjectController>().currentHealth;
        float barHealth = (float) currentHealth / totalHealth;
        int playerNumber = player.GetComponent<PlayerController>().originalPlayerNumber;

        GameObject healthBar = playersTurns[playerNumber].transform.GetChild(1).GetChild(1).gameObject;
        healthBar.transform.localScale = new Vector3(Mathf.Clamp(barHealth, 0f, 1f), healthBar.transform.localScale.y, healthBar.transform.localScale.z);

    }

    //public void SetTextHealth()
    //{
    //    for(int i=0; i<TileManager.playerInstance.Count;i++)
    //    {
    //        if (TileManager.playerInstance[i])
    //        {
    //            healthTexts[i].SetText(TileManager.playerInstance[i].GetComponent<ObjectController>().currentHealth.ToString() + "/" + TileManager.playerInstance[i].GetComponent<ObjectController>().totalHealth.ToString() + " HP");
    //        }
    //    }
    //}

    public void CreateEnemyUI(List<GameObject> enemyInstance)
    {
        healthEnemies = new List<GameObject>();
        for (int i=0;i<enemyInstance.Count;i++)
        {
            GameObject enemyUIInstance = Instantiate(enemyHealth);
            enemyUIInstance.transform.SetParent(transform, false);

            enemyUIInstance.GetComponent<EnemyHealth>().transform.position = enemyInstance[i].transform.position;
            enemyUIInstance.GetComponent<EnemyHealth>().target = enemyInstance[i].transform;

            for (int p = 0; p < enemyUIInstance.transform.childCount; p++)
            {
                if (enemyUIInstance.transform.GetChild(p).name == "HealthText")
                {
                    enemyUIInstance.transform.GetChild(p).GetComponent<TextMeshProUGUI>().SetText(enemyInstance[i].GetComponent<ObjectController>().currentHealth.ToString() + "/" + enemyInstance[i].GetComponent<ObjectController>().totalHealth.ToString() + " HP");
                }
            }

            healthEnemies.Add(enemyUIInstance);
        }
    }

    public void DestroyEnemyUI(int enemyPosition)
    {
        Destroy(healthEnemies[enemyPosition]);
        healthEnemies[enemyPosition] = null;
    }

    public void ClearEnemyList()
    {
        healthEnemies.Clear();
    }

    public void SetEnemyHealth(int enemyNumber, GameObject target)
    {
        for (int p = 0; p < healthEnemies[enemyNumber].transform.childCount; p++)
        {
            if (healthEnemies[enemyNumber] && healthEnemies[enemyNumber].transform.GetChild(p).name == "HealthText")
            {
                healthEnemies[enemyNumber].transform.GetChild(p).GetComponent<TextMeshProUGUI>().SetText(target.GetComponent<ObjectController>().currentHealth.ToString() + "/" + target.GetComponent<ObjectController>().totalHealth.ToString() + " HP");
                break;
            }
        }
    }

    public void DisablePlayerHUD(int playerNumber)
    {
        playersTurns[playerNumber].transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 0.1f);
        playersTurns[playerNumber].transform.GetChild(1).gameObject.SetActive(false);
        //playersTurns.RemoveAt(playerNumber);
    }

    public void SetPlayerTurnColor(GameObject player)
    {
        int playerNumber = player.GetComponent<PlayerController>().playerNumber;
        Color tileColor = player.GetComponent<PlayerController>().colorTile;
        playersTurns[playerNumber].GetComponent<Image>().color = new Color(tileColor.r, tileColor.g, tileColor.b, 0.5f);
    }

    public void ResetColor()
    {
        foreach(GameObject ui in playersTurns)
        {
            if(ui)
            {
                ui.GetComponent<Image>().color = new Color(255, 255, 255, 0.8f);
            }
        }
    }

    public void SetTurnBarUI(List<GameObject> turns, int currentTurn)
    {
        turnUIBar.SetActive(true);
        if (turns.Count > turnsBarUI.Count)
        {
            foreach(GameObject turnUI in turnsBarUI)
            {
                Destroy(turnUI);
            }
            SetTurnList(turns);
        }
        turnsBarUI[0].GetComponent<Image>().sprite = borderSelected;
        turnsBarUI[0].GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f);
        turnsBarUI[0].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = turns[currentTurn].GetComponent<ObjectController>().iconObject;

        int barPosition = 1;

        do
        {
            currentTurn++;
            if (currentTurn >= turns.Count)
            {
                currentTurn = 0;
            }
            turnsBarUI[barPosition].GetComponent<Image>().sprite = borderEmpty;
            turnsBarUI[barPosition].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = turns[currentTurn].GetComponent<ObjectController>().iconObject;
            barPosition++;
        } while (barPosition < turns.Count);

        if (turnsBarUI.Count > turns.Count)
        {
            Destroy(turnsBarUI.Last());
            turnsBarUI.Remove(turnsBarUI.Last());
            turnUIBar.GetComponent<RectTransform>().sizeDelta = new Vector2(150 * turns.Count, turnUIBar.GetComponent<RectTransform>().sizeDelta.y);
        }
    }

    public void DisableTurnUI()
    {
        turnUIBar.SetActive(false);
        foreach(GameObject turnUI in turnsBarUI)
        {
            Destroy(turnUI);
        }
        turnsBarUI.Clear();
    }

    public void SetTurnList(List<GameObject> turns)
    {
        turnsBarUI = new List<GameObject>();
        turnUIBar.GetComponent<RectTransform>().sizeDelta = new Vector2(150 * turns.Count, turnUIBar.GetComponent<RectTransform>().sizeDelta.y);
        foreach (GameObject turn in turns)
        {
            GameObject turnUIInstance = Instantiate(turnUI);
            turnUIInstance.transform.SetParent(turnUIBar.transform.GetChild(0).transform, false);
            turnsBarUI.Add(turnUIInstance);
        }
    }

    IEnumerator SetPlayersImage()
    {
        while(TileManager.playerInstance == null || TileManager.playerInstance.Count == 0)
        {
            yield return null;
        }

        for(int i=0; i<TileManager.playerInstance.Count;i++)
        {
            playersTurns[i].transform.GetChild(0).GetComponent<Image>().sprite = TileManager.playerInstance[i].GetComponent<ObjectController>().iconObject;
            int totalHealth = TileManager.playerInstance[i].GetComponent<ObjectController>().totalHealth;
            int currentHealth = TileManager.playerInstance[i].GetComponent<ObjectController>().currentHealth;
            healthTurnText.GetComponent<TextMeshProUGUI>().SetText(currentHealth.ToString() + "/" + totalHealth.ToString() + " HP");
        }
    }

    IEnumerator DestroyText(TextMeshProUGUI obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj.gameObject);
    }

    IEnumerator DestroyObject(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj.gameObject);
    }
}
