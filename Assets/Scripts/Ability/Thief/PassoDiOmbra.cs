using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PassoDiOmbra : Ability {

	// Use this for initialization
	void Start () 
	{
        this.abilityName = "PassoDiOmbra";
        this.realName = "Shadow Step";
        this.abilityDescription = "The Assassin can teleport 3 tiles away from his position. This ability is not considered a movement action";
        //this.abilityType = AbilityType.TELETRASPORTO;
        this.damage = 0;
		this.cure = 0;
		this.tileRange = 4;
		this.cooldown = 3;
        countCooldown = this.cooldown;
        playerUI = GetComponent<PlayerController>().playerUI;
        buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[1];
        buttonPlayerUI.onClick.AddListener(delegate {
            AttivaAbilita(SelectType.ROMBO);
            GetComponent<PlayerController>().SetTransparency();
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
		//GameObject playerTarget = null;
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		if (hit.collider != null && hit.collider.tag == "Tile" && hit.collider.GetComponent<Tile>().isSelected
			&& !hit.collider.GetComponent<Tile>().isEnemy && !hit.collider.GetComponent<Tile>().isPlayer) {
            GetComponent<PlayerController>().Disappear();
            //playerTarget.transform.position = hit.collider.gameObject.transform.position;
            GetComponent<AILerp>().enabled = false;
			this.gameObject.transform.position = hit.collider.gameObject.transform.position;

			GetComponent<PlayerController>().PlayerTile = hit.collider.gameObject;


			GetComponent<AILerp> ().target = hit.collider.gameObject.transform;
			GetComponent<AILerp> ().targetReached = true;
			GetComponent<AILerp> ().enabled = true;

            StartCoroutine(AppearTransparent(.5f));

            AddAbilityToCooldownList(this);

            activedAbility = "";
			StartCoroutine(TileManager.WaitMoves(this.gameObject, GameManager.States.END_MOVE));
		}
	}

    IEnumerator AppearTransparent(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<PlayerController>().SetTransparency();
        StartCoroutine(Appear(.5f));
    }

    IEnumerator Appear(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<PlayerController>().DeleteTransparency();
    }
}
