using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColpoAlleSpalle : Ability {

	// Use this for initialization
	void Start ()
    {
        this.realName = "Backstab (Passive)";
        this.abilityDescription = "Hitting an enemy from the rear tile, the Assassin’s attacks will deal "+ GetComponent<PlayerController>().physicAttack / 100f * 150f + ", but he deals only "+ (int)(GetComponent<PlayerController>().physicAttack / 100f * 80f) + " physAtt from any other direction";

        playerUI = GetComponent<PlayerController>().playerUI;
        buttonPlayerUI = playerUI.GetComponentsInChildren<Button>()[0];
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
