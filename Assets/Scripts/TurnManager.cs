using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    public static GameObject currentObjectTurn;
    public static List<GameObject> turns;
    public static bool refreshTurn;

    private GameObject UI;
    private GameObject bottomUI;
    private bool refreshTurnUI;
    public static int currentTurn = 0;

    public enum TurnStates
    {
        INIT,
        EXECUTE,
        EXECUTED,
        WAIT,
        FINISH
    }

    public static TurnStates currentTurnState;

    private void Awake()
    {
        UI = GameObject.Find("UI");
        bottomUI = GameObject.Find("BottomUI");
    }

    private void Update()
    {
        if(refreshTurnUI)
        {
            UI.GetComponent<UIManager>().SetTurnBarUI(turns, currentTurn);
            refreshTurnUI = false;
        }
        
    }

    public void CreateEnemyUI(List<GameObject> enemyInstance)
    {
        UI.GetComponent<UIManager>().CreateEnemyUI(enemyInstance);
    }

    //Calculate turns
    public void CalculateTurns(List<GameObject> players, List<GameObject> enemies)
    {
        currentTurnState = TurnStates.INIT;
        turns = new List<GameObject>();

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                turns.Add(enemy);
            }
        }
        foreach (GameObject player in players)
        {
            if(player != null)
            {
                turns.Add(player);
            }
            
        }

        turns.Sort(delegate (GameObject a, GameObject b) {
            return (b.GetComponent<ObjectController>().skill).CompareTo(a.GetComponent<ObjectController>().skill);
        });

       UI.GetComponent<UIManager>().SetTurnList(turns);
    }

    public GameObject GetNextTurn()
    {
        refreshTurnUI = true;
        
        if(currentTurn >= turns.Count)
        {
            currentTurn = 0;
            Ability.CheckTurnDurationList();
            Ability.CheckCooldownList();
            EnemyController.CheckEnemyStunned();
        }

        //UI.GetComponent<UIManager>().ResetColor();
        if (currentObjectTurn && currentObjectTurn.tag == "Player")
        {
            UI.GetComponent<UIManager>().HidePlayerUI(currentObjectTurn);
        }
        currentObjectTurn = turns[currentTurn];

        if(currentObjectTurn.tag == "Enemy" && currentObjectTurn.GetComponent<EnemyController>().stunned)
        {
            currentTurn++;
            GetNextTurn();
        }

        if(currentObjectTurn && currentObjectTurn.name == "Dragon")
        {
            EnemyController.ResetBossGrid();
        }
        //currentTurn++;
        PlayerController.canMove = true;
        UI.GetComponent<UIManager>().SetChangeTurnText(currentObjectTurn.GetComponent<ObjectController>().ObjectName + " Turn");
        if(currentObjectTurn.tag == "Player")
        {
           // UI.GetComponent<UIManager>().SetPlayerTurnColor(currentObjectTurn);
            UI.GetComponent<UIManager>().ShowPlayerUI(currentObjectTurn);
        }

        currentTurnState = TurnStates.EXECUTE;
        //StartCoroutine(Wait(2f));
        return currentObjectTurn;
    }

    //IEnumerator Wait(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    currentTurnState = TurnStates.EXECUTE;
    //}


    public IEnumerator RecalculateTurn(List<GameObject> players, List<GameObject> enemies, GameManager.States nextState, TurnManager.TurnStates turnState)
    {
        GameManager.currentState = GameManager.States.WAIT;

        //yield return new WaitForEndOfFrame();

        while (!refreshTurn)
        {
            yield return null;
        }

        refreshTurn = false;

        if (IsAllTurnFinished())
        {
            currentTurn = 0;
        }

        if (AreEnemiesAlive(enemies))
        {
            List<GameObject> removeTurn = new List<GameObject>();
            for (int i = 0; i < turns.Count; i++)
            {
                if (turns[i] == null)
                {
                    removeTurn.Add(turns[i]);
                }
            }
            foreach (GameObject remove in removeTurn)
            {
                turns.Remove(remove);
            }

            currentTurn = turns.IndexOf(currentObjectTurn);

            //if (GameManager.pointAction == 0 && removeTurn.Count > 0)
            //{
            //    currentTurn--;
            //    if (currentTurn < 0)
            //    {
            //        currentTurn = 0;
            //    }
            //}

            GameManager.currentState = nextState;
            currentTurnState = turnState;
        }
        else
        {
            currentTurn = 0;
            currentTurnState = TurnStates.FINISH;
            GameManager.currentState = GameManager.States.END_TURN;
            currentObjectTurn = null;
            turns.Clear();

            yield return new WaitForSeconds(1.5f);
        }

    }

    public void ResetTurnColor()
    {
        //UI.GetComponent<UIManager>().ResetColor();
    }


    public bool IsAllTurnFinished()
    {
        if (currentTurn >= turns.Count){
            
            return true;
        }
        else
        {
            return false;
        }
            
    }

    public bool AreEnemiesAlive(List<GameObject> enemies)
    {
        bool found = false;
        if (enemies.Count > 0)
        {
            foreach(GameObject enemy in enemies)
            {
                if(enemy != null)
                {
                    found = true;
                }
                else
                {
                    continue;
                }
            }
            
        }

        if(!found)
        {
            UI.GetComponent<UIManager>().DisableTurnUI();
            UI.GetComponent<UIManager>().ClearEnemyList();
        }
        return found;
    }

    public void ShowBattleImage()
    {
        UI.GetComponent<UIManager>().ShowImageFight();
    }
}
