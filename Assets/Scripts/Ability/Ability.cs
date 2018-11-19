using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Ability : MonoBehaviour
{

	public static bool isRunning = false;

    public string abilityName;
    public string realName;
    public string abilityDescription;
    public float damage;
    public float cure;
    public int tileRange;
    public int turnDuration;
	public int countTurnDuration;
    public int cooldown;
    public int countCooldown;
    public static string activedAbility;
    public AbilityType abilityType;
    public static List<Ability> cooldownList; 
	public static List<Ability> turnDurationList; 
    public Sprite image;

    private GameObject UI;
	protected GameObject playerUI;
    protected Button buttonPlayerUI;

    public enum SelectType
    {
        QUADRATO,
        ROMBO,
        CROCE,
        COMPAGNI
    }

    public enum AbilityType
    {
        SINGOLO,
        MOVIMENTO
    }

    // Use this for initialization
    void Awake()
    {
        UI = GameObject.Find("UI");
        abilityName = "";
        abilityDescription = "";
        damage = 0;
        cure = 0;
        tileRange = 0;
        turnDuration = 0;
		cooldown = 0;
        countCooldown = 0;
		countTurnDuration = 0;
        activedAbility = "";
        cooldownList = new List<Ability>();
		turnDurationList = new List<Ability>();

    }

	void Update()
	{
		if (GameManager.currentState == GameManager.States.ABILITY && Input.GetMouseButtonDown(1) && !CostrizioneCurativa.isRunning)
		{
			TileManager.ResetGrid();
			GameManager.currentState = GameManager.States.SELECT;
			TurnManager.currentTurnState = TurnManager.TurnStates.EXECUTE;
		}
	}

    protected Vector2[] CalcolaSelezioneQuadrata()
    {
        Vector2[] points = new Vector2[] {
            new Vector2(
                    TileManager.quadInitialPoint[0].x*tileRange,
                    TileManager.quadInitialPoint[0].y*tileRange),
                new Vector2(
                    TileManager.quadInitialPoint[1].x*tileRange,
                    TileManager.quadInitialPoint[1].y*tileRange),
                new Vector2(
                    TileManager.quadInitialPoint[2].x*tileRange,
                    TileManager.quadInitialPoint[2].y*tileRange),
                new Vector2(
                    TileManager.quadInitialPoint[3].x*tileRange,
                    TileManager.quadInitialPoint[3].y*tileRange)
        };

        return points;

    }

    protected Vector2[] CalcolaSelezioneRomboidale()
    {
        Vector2[] points = new Vector2[] {
            new Vector2(
                    TileManager.polygonInitialPoint[0].x*tileRange,
                    TileManager.polygonInitialPoint[0].y*tileRange),
                new Vector2(
                    TileManager.polygonInitialPoint[1].x*tileRange,
                    TileManager.polygonInitialPoint[1].y*tileRange),
                new Vector2(
                    TileManager.polygonInitialPoint[2].x*tileRange,
                    TileManager.polygonInitialPoint[2].y*tileRange),
                new Vector2(
                    TileManager.polygonInitialPoint[3].x*tileRange,
                    TileManager.polygonInitialPoint[3].y*tileRange)
        };

        return points;

    }

    protected Vector2[] CalcolaSelezioneACroce()
    {
        Vector2[] points = new Vector2[] {
            new Vector2(
                    TileManager.quadInitialPoint[0].x*tileRange,
                    TileManager.quadInitialPoint[0].y),
                new Vector2(
                    TileManager.quadInitialPoint[1].x*tileRange,
                    TileManager.quadInitialPoint[1].y),
                new Vector2(
                    TileManager.quadInitialPoint[2].x*tileRange,
                    TileManager.quadInitialPoint[2].y),
                new Vector2(
                    TileManager.quadInitialPoint[3].x*tileRange,
                    TileManager.quadInitialPoint[3].y),
                new Vector2(
                    TileManager.quadInitialPoint[0].x,
                    TileManager.quadInitialPoint[0].y*tileRange),
                new Vector2(
                    TileManager.quadInitialPoint[1].x,
                    TileManager.quadInitialPoint[1].y*tileRange),
                new Vector2(
                    TileManager.quadInitialPoint[2].x,
                    TileManager.quadInitialPoint[2].y*tileRange),
                new Vector2(
                    TileManager.quadInitialPoint[3].x,
                    TileManager.quadInitialPoint[3].y*tileRange)
        };

        return points;

    }

    public void AttivaAbilita(SelectType currentType)
    {
        Vector2[] newPoints = null;
        switch (currentType)
        {
            case SelectType.QUADRATO:
                newPoints = CalcolaSelezioneQuadrata();
                break;
            case SelectType.ROMBO:
                newPoints = CalcolaSelezioneRomboidale();
                break;
            case SelectType.CROCE:
                newPoints = CalcolaSelezioneACroce();
                break;
        }
		TileManager.ResetGrid();
		TileManager.SetTrigger(this.GetComponent<PlayerController>().PlayerTile, newPoints);
		StartCoroutine(TileManager.WaitMovesAbility(this.gameObject));
        
    }

    public virtual void UsaAbilita()
	{
        if(TileManager.CheckEnemy() || TileManager.CheckBoss())
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            //if (hit.collider != null && hit.collider.tag == "Enemy" || (hit.collider != null && hit.collider.tag == "Tile" && hit.collider.GetComponent<Tile>().isSelected && hit.collider.GetComponent<Tile>().isEnemy))
            //{
                GameObject enemyTarget = null;
                if (hit.collider != null && hit.collider.tag == "Enemy" || (hit.collider != null && hit.collider.tag == "Tile"
                    && hit.collider.GetComponent<Tile>().isSelected && hit.collider.GetComponent<Tile>().isEnemy) && !TileManager.CheckBoss())
                    
                {
                    
                    foreach (GameObject enemy in TileManager.enemyInstance)
                    {
                        if (enemy.GetComponent<EnemyController>().EnemyTile.transform.position == hit.collider.transform.position)
                        {
                            enemyTarget = enemy;
                            break;
                        }
                    }
                }
                else if (hit.collider != null && hit.collider.tag == "Enemy" || (hit.collider != null && hit.collider.tag == "Tile"
                    && hit.collider.GetComponent<Tile>().isSelected && (hit.collider.GetComponent<Tile>().isEnemy || hit.collider.GetComponent<Tile>().isAttackable)) && TileManager.CheckBoss())
                {
                    foreach (GameObject enemy in TileManager.enemyInstance)
                    {
                        enemyTarget = enemy;
                    }
                        
                }

                if (abilityType == AbilityType.SINGOLO)
                {
                    GetComponent<PlayerController>().PhysicAttack(enemyTarget, "attack", (int)this.damage);

                    Ability ability = GetComponent(Type.GetType(Ability.activedAbility)) as Ability;
                    AddAbilityToCooldownList(ability);

                activedAbility = "";
                    StartCoroutine(TileManager.WaitMoves(this.gameObject, GameManager.States.END_MOVE));
                }
                else if (abilityType == AbilityType.MOVIMENTO)
                {

                    Vector2 playerPosition = this.gameObject.transform.position;
                    Vector2 enemyPosition = enemyTarget.transform.position;

                    GameObject tileToSelect = null;

                    if (playerPosition.x != enemyPosition.x)
                    {
                        if (playerPosition.x < enemyPosition.x)
                        {
                            tileToSelect = enemyTarget.GetComponent<EnemyController>().GetTileNearEnemy(Vector2.left);
                        }
                        else
                        {
                            tileToSelect = enemyTarget.GetComponent<EnemyController>().GetTileNearEnemy(Vector2.right);
                        }
                    }
                    else if (playerPosition.y != enemyPosition.y)
                    {
                        if (playerPosition.y < enemyPosition.y)
                        {
                            tileToSelect = enemyTarget.GetComponent<EnemyController>().GetTileNearEnemy(Vector2.down);
                        }
                        else
                        {
                            tileToSelect = enemyTarget.GetComponent<EnemyController>().GetTileNearEnemy(Vector2.up);
                        }
                    }

                    if(tileToSelect && !tileToSelect.GetComponent<Tile>().isPlayer && !tileToSelect.GetComponent<Tile>().isEnemy)
                    {
                        GetComponent<AILerp>().target = tileToSelect.transform;
                        GetComponent<PlayerController>().PlayerTile = tileToSelect;

                        EnemyController enemyController = enemyTarget.GetComponent<EnemyController>();
                        enemyController.stunned = true;
                        enemyController.AddEnemyStunned();

                        GetComponent<PlayerController>().PhysicAttack(enemyTarget, "charge", (int)this.damage);

                        Ability ability = GetComponent(Type.GetType(Ability.activedAbility)) as Ability;
                        AddAbilityToCooldownList(ability);

                        activedAbility = "";
                        StartCoroutine(TileManager.WaitMoves(this.gameObject, GameManager.States.END_MOVE));
                    }
                   
                }
            //}
        }

    }

	protected void AddAbilityToCooldownList(Ability ability)
    {
        cooldownList.Add(ability);
        ability.buttonPlayerUI.interactable = false;
        ability.buttonPlayerUI.transform.GetChild(0).gameObject.SetActive(true);
        ability.buttonPlayerUI.GetComponentInChildren<Text>().text = ability.countCooldown.ToString();
    }

    public static void CheckCooldownList()
    {

        if(cooldownList.Count > 0)
        {
            for (int i = 0; i<cooldownList.Count; i++)
            {
                cooldownList[i].buttonPlayerUI.GetComponentInChildren<Text>().text = cooldownList[i].countCooldown.ToString();
                cooldownList[i].countCooldown--;

                if (cooldownList[i].countCooldown < 0)
                {
                    cooldownList[i].buttonPlayerUI.transform.GetChild(0).gameObject.SetActive(false);
                    cooldownList[i].buttonPlayerUI.interactable = true;
                    cooldownList[i].countCooldown = cooldownList[i].cooldown;
                    cooldownList.Remove(cooldownList[i]);
                }
            }
        }

    }

	protected void AddAbilityToTurnDurationList(Ability ability)
	{
		ability.buttonPlayerUI.interactable = false;
		turnDurationList.Add(ability);
		ability.buttonPlayerUI.interactable = false;
	}

	public static void CheckTurnDurationList()
	{
        Ability[] removed = new Ability[turnDurationList.Count];
		if(turnDurationList.Count > 0)
		{
			for (int i = 0; i<turnDurationList.Count; i++)
			{
				turnDurationList[i].countTurnDuration--;

				if (turnDurationList[i].countTurnDuration < 0)
				{
					turnDurationList[i].ResettaValori();
                    removed[i] = turnDurationList[i];
                }
			}

            foreach(Ability ability in removed)
            {
                turnDurationList.Remove(ability);
            }
		}

	}


	public virtual void ResettaValori()
	{
		
	}

	public static void ResetCooldown()
	{
        if(cooldownList != null && cooldownList.Count > 0)
        {
            for (int i = 0; i < cooldownList.Count; i++)
            {
                cooldownList[i].buttonPlayerUI.transform.GetChild(0).gameObject.SetActive(false);
                cooldownList[i].buttonPlayerUI.interactable = true;
                cooldownList[i].countCooldown = cooldownList[i].cooldown;

            }
            cooldownList.Clear();
        }
		
	}

	public static void ResetTurnDuration()
	{
        if(turnDurationList != null && turnDurationList.Count > 0)
        {
            for (int i = 0; i < turnDurationList.Count; i++)
            {
                turnDurationList[i].ResettaValori();
                turnDurationList[i].buttonPlayerUI.interactable = true;
                turnDurationList[i].countTurnDuration = turnDurationList[i].turnDuration;
            }
            turnDurationList.Clear();
        }
		
	}

    public void OnPointerEnterDelegate(PointerEventData data)
    {
        GameObject panel = UI.transform.GetChild(4).gameObject;
        panel.SetActive(true);
        panel.transform.GetChild(0).GetComponent<Image>().sprite = image;
        panel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = realName;
        panel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = abilityDescription;
    }

    public void OnPointerExitDelegate(PointerEventData data)
    {
        UI.transform.GetChild(4).gameObject.SetActive(false);
    }
}
