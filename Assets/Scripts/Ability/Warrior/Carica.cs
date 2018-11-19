using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Carica : Ability {

    // Use this for initialization
    void Start () 
	{
        this.abilityType = AbilityType.MOVIMENTO;
        this.realName = "Charge";
		this.abilityName = "Carica";
        this.damage = GetComponent<PlayerController>().physicAttack / 100f * 70f;
		this.cure = 0;
		this.tileRange = 8;
		this.cooldown = 4;
        countCooldown = this.cooldown;
        this.abilityDescription = "Charge an enemy dealing " + this.damage + " and stunning him for 1 turn";
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
