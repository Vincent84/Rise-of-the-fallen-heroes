using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Elementale : Ability {

	private int currentMind;
	private int currentConstitution;

	// Use this for initialization
	void Start ()
	{
		abilityName = "Elementale";
        this.realName = "Elemental Power";
        currentMind = GetComponent<PlayerController> ().mind; 
		currentConstitution = GetComponent<PlayerController> ().constitution; 
		this.damage = 0;
		this.cure = 0;
		this.tileRange = GetComponent<PlayerController> ().moves;
		this.cooldown = 4;
		countCooldown = this.cooldown;
		this.turnDuration = 3;
		countTurnDuration = this.turnDuration;
        this.abilityDescription = "The Mage unleashes her inner fire, gaining 5 Intelligence and 2 Constitution for 3 turns";

        playerUI = GetComponent<PlayerController>().playerUI;
		buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[1];
		buttonPlayerUI.onClick.AddListener(delegate {
			TileManager.ResetGrid ();
			StartCoroutine (Wait(0.5f));
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
        GameObject transformMage = Instantiate(GetComponent<PlayerController>().transformMage, GetComponent<PlayerController>().transform);
        StartCoroutine(TransformMage(transformMage));

        GetComponent<PlayerController> ().constitution += 2; 
		GetComponent<PlayerController> ().mind += 5;
		GetComponent<PlayerController> ().CalculateStatistics ();
        GetComponent<PlayerController>().currentHealth = GetComponent<PlayerController>().totalHealth;
        this.tileRange = GetComponent<PlayerController>().moves;

        AddAbilityToTurnDurationList(this);

        activedAbility = "";
		StartCoroutine(TileManager.WaitMoves(this.gameObject, GameManager.States.END_MOVE));

	}

	IEnumerator Wait(float delay)
	{
		GameManager.currentState = GameManager.States.ABILITY;
		yield return new WaitForSeconds(delay);

		TileManager.tilesSelectable.Add(GetComponent<PlayerController>().PlayerTile);
		GetComponent<PlayerController>().PlayerTile.GetComponent<SpriteRenderer>().color = Color.green;

		UsaAbilita ();
	}

    IEnumerator TransformMage(GameObject transformMage)
    {
        GetComponent<PlayerController>().gameObject.transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<AudioSource>().clip = GetComponent<PlayerController>().clipAudio[2];
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(2f);
        Destroy(transformMage);
        GetComponent<PlayerController>().gameObject.transform.GetChild(1).gameObject.SetActive(true);
        GetComponent<PlayerController>().StartFightAnimation();
    }

	public override void ResettaValori()
	{
		AddAbilityToCooldownList (this);
		countTurnDuration = turnDuration;
		//turnDurationList.Remove(this);
		PlayerController controller = GetComponent <PlayerController> ();
		controller.mind = currentMind;
		controller.constitution = currentConstitution;
		controller.CalculateStatistics ();
        this.damage = GetComponent<PlayerController>().magicAttack / 100f * 60f;
        this.tileRange = GetComponent<PlayerController>().moves;
        GetComponent<PlayerController>().gameObject.transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<PlayerController>().gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }


}
