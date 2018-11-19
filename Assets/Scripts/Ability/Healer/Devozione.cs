using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Devozione : Ability {

	// Use this for initialization
	void Start () 
	{
        abilityName = "Devozione";
        this.realName = "Devotion";
        this.damage = GetComponent<PlayerController>().magicAttack;
		this.cure = 0;
		this.tileRange = 1;
		this.cooldown = 5;
        countCooldown = this.cooldown;
        this.abilityDescription = "The Cleric selects a fallen ally and resurrects him with "+this.damage+" hps";

        playerUI = GetComponent<PlayerController>().playerUI;
        buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[1];
        buttonPlayerUI.onClick.AddListener(delegate {
			TileManager.ResetGrid ();
			StartCoroutine (SelectPlayers(1f));
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
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		if (hit.collider != null && hit.collider.tag == "Player" ||
		    (hit.collider != null && hit.collider.tag == "Tile")) {
	
			GameObject playerTarget = null;
			foreach (GameObject player in TileManager.playerDead)
			{
				if (player.GetComponent<PlayerController>().PlayerTile.transform.position == hit.collider.transform.position)
				{
					playerTarget = player;
					break;
				}
			}

			if (playerTarget)
			{
				
				playerTarget.GetComponent<PlayerController> ().ResurrectPlayer ();
                playerTarget.GetComponentInChildren<Animator>().SetTrigger("isFighting");

                TileManager.playerDead.Remove (playerTarget);
				TurnManager.turns.Add (playerTarget);

                foreach (GameObject player in TileManager.playerInstance)
                {
                    player.GetComponent<PlayerController>().playerNumber = player.GetComponent<PlayerController>().originalPlayerNumber;
                }
                TileManager.playerInstance.Sort(delegate (GameObject a, GameObject b) {
                    return (a.GetComponent<PlayerController>().playerNumber).CompareTo(b.GetComponent<PlayerController>().playerNumber);
                });

                GetComponent<PlayerController>().Cure(playerTarget, "resurrect", (int)damage);

				AddAbilityToCooldownList(this);

                activedAbility = "";
				StartCoroutine(TileManager.WaitMoves(this.gameObject, GameManager.States.END_MOVE));
			}


		}
	}

	IEnumerator SelectPlayers(float delay)
	{
		GameManager.currentState = GameManager.States.ABILITY;
		yield return new WaitForSeconds(delay);

		foreach (GameObject player in TileManager.playerDead)
		{
			TileManager.tilesSelectable.Add(player.GetComponent<PlayerController>().PlayerTile);
			player.GetComponent<PlayerController>().PlayerTile.GetComponent<SpriteRenderer>().color = Color.green;
		}
	}

}
