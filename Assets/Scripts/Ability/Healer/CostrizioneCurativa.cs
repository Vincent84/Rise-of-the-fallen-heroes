using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CostrizioneCurativa : Ability {

    // Use this for initialization
    void Start () 
	{
        this.abilityName = "CostrizioneCurativa";
        this.realName = "Healing Costriction";
        this.damage = GetComponent<PlayerController>().magicAttack / 100f * 50f;
		this.cure = GetComponent<PlayerController>().magicAttack / 100f * 50f;
		this.tileRange = 3;
		this.cooldown = 2;
        countCooldown = this.cooldown;
        this.abilityDescription = " The Cleric steals "+this.damage+" hp to an enemy and heals an ally target or himself for the same amount";

        playerUI = GetComponent<PlayerController>().playerUI;
        buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[0];
        buttonPlayerUI.onClick.AddListener (delegate {
			AttivaAbilita (SelectType.ROMBO);
            activedAbility = this.abilityName;
        });
        EventTrigger trigger = buttonPlayerUI.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data); });
        trigger.triggers.Add(exit);

    }

    public override void UsaAbilita()
    {
        if (TileManager.CheckEnemy() || TileManager.CheckBoss())
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
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
            GetComponent<PlayerController>().PhysicAttack(enemyTarget, "attack", (int)this.damage);
            TileManager.ResetGrid();
            StartCoroutine(SelectPlayers(0.5f));
        }
		else if(isRunning)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "Player" ||
                    (hit.collider != null && hit.collider.tag == "Tile" && hit.collider.GetComponent<Tile>().isPlayer))
            {
                GameObject playerTarget = null;
                foreach (GameObject player in TileManager.playerInstance)
                {
                    if (player.GetComponent<PlayerController>().PlayerTile.transform.position == hit.collider.transform.position)
                    {
                        playerTarget = player;
                        break;
                    }
                }
                GetComponent<PlayerController>().Cure(playerTarget, "attack", (int)this.damage);

                AddAbilityToCooldownList(this);

				isRunning = false;

                activedAbility = "";
                StartCoroutine(TileManager.WaitMoves(this.gameObject, GameManager.States.END_MOVE));

            }
        }
                
            
    }

    IEnumerator SelectPlayers(float delay)
    {

		isRunning = true;
        yield return new WaitForSeconds(delay);

		foreach (GameObject player in TileManager.playerInstance)
		{
			TileManager.tilesSelectable.Add(player.GetComponent<PlayerController>().PlayerTile);
			player.GetComponent<PlayerController>().PlayerTile.GetComponent<SpriteRenderer>().color = Color.green;
		}
    }

}
