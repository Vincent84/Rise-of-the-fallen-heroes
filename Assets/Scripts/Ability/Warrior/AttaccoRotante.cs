using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AttaccoRotante : Ability {

	// Use this for initialization
	void Start () 
	{
		this.abilityName = "AttaccoRotante";
        this.realName = "Stomp";
        
        activedAbility = this.abilityName;
        this.damage = GetComponent<PlayerController>().physicAttack / 100f * 50f;
		this.cure = (5f + GetComponent<PlayerController>().magicAttack / 100f * 20f) * 4;
		this.tileRange = 2;
		this.cooldown = 4;
		countCooldown = this.cooldown;
        this.abilityDescription = "The Warrior stomps the ground around him, dealing "+this.damage+" and stunning enemies hit for 1 turn";
        playerUI = GetComponent<PlayerController>().playerUI;
        buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[1];
        buttonPlayerUI.onClick.AddListener(delegate
        {
			GameManager.currentState = GameManager.States.ABILITY;
			activedAbility = this.abilityName;
            AttivaAbilita(SelectType.QUADRATO);
			StartCoroutine (SelectPlayers (0.5f));
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

	IEnumerator SelectPlayers(float delay)
	{
		yield return new WaitForSeconds(delay);
		UsaAbilita ();
	}

	public override void UsaAbilita()
	{
		if (TileManager.CheckEnemy ()) {
			List<GameObject> enemyTargets = new List<GameObject> ();

			foreach (GameObject tileEnemy in TileManager.tilesSelectable) {
				if (tileEnemy.GetComponent<Tile> ().isEnemy) {
					foreach (GameObject enemy in TileManager.enemyInstance) {
                        EnemyController controller = enemy.GetComponent<EnemyController>();
                        if (controller.EnemyTile.transform.position == tileEnemy.transform.position) {
							enemyTargets.Add (enemy);
                            controller.stunned = true;
                            controller.AddEnemyStunned();
						}
					}
				}

			}

            AddAbilityToCooldownList(this);

            GetComponent<PlayerController> ().PhysicAttack (enemyTargets, "stomp", (int)this.damage);

            activedAbility = "";
			StartCoroutine (TileManager.WaitMoves (this.gameObject, GameManager.States.END_MOVE));
		}


	}
}
