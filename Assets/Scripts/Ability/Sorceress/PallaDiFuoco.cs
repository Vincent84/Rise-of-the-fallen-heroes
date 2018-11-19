using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PallaDiFuoco : Ability {

	// Use this for initialization
	void Start () 
	{
        this.abilityType = AbilityType.SINGOLO;
        this.abilityName = "PallaDiFuoco";
        this.realName = "Incinerate";
        this.damage = GetComponent<PlayerController>().magicAttack / 100f * 60f;
		this.cure = 0;
		this.tileRange = 8;
		this.cooldown = 1;
        countCooldown = this.cooldown;
        this.abilityDescription = "The Mage hits a single enemy with a burst of fire, dealing "+this.damage;

        playerUI = GetComponent<PlayerController>().playerUI;
        buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[0];
        buttonPlayerUI.onClick.AddListener (delegate {
			AttivaAbilita (SelectType.CROCE);
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

}
