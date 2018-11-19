using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {

    

    public int strength; // Attributo forza
    public int constitution; // Attributo costituzione
    public int skill; // Attributo destrezza
    public int mind; // Attributo intelligenza
    public string ObjectName;
    public int combatMoves;
    public Color colorTile;
    public Sprite iconObject;

    //Stats of single enemy
    public int totalHealth;
    public int currentHealth;
    public int physicAttack;
    public int magicAttack;
    public int moves;
    protected int critic;
    protected int evasion;
    protected Animator anim;

    protected GameObject UI;
    protected GameObject SFXManager;
    public List<AudioClip> clipAudio;

    private void Awake()
    {
        CalculateStatistics();
        UI = GameObject.Find("UI");
        SFXManager = GameObject.Find("AudioManager_Sfx");
        currentHealth = totalHealth;
    }

    public void CalculateStatistics()
    {
        totalHealth = 2 * strength + 6 * constitution + 2 * mind;
        magicAttack = 5 * mind;
        physicAttack = 3 * strength + constitution;
        moves = Mathf.CeilToInt(skill / 3f);
        critic = skill;
        evasion = skill;
    }

    protected Animator GetAnimator()
    {
        anim = GetComponentInChildren<Animator>();
        if(anim != null)
        {
            return anim;
        }
        else
        {
            return null;
        }
    }

    protected void OnHit(GameObject target, int damage)
    {
        UI.GetComponent<UIManager>().ShowPopupDamage(damage, target.transform);
        target.GetComponent<ObjectController>().currentHealth = target.GetComponent<ObjectController>().currentHealth - damage;
        
        if (target.tag == "Enemy")
        {
            UI.GetComponent<UIManager>().SetEnemyHealth(target.GetComponent<EnemyController>().position, target);
        }
        else if(target.tag == "Player")
        {
            UI.GetComponent<UIManager>().SetPlayerHealthBar(target);
        }
    }

    protected void OnCure(GameObject target, int cure)
    {
        ObjectController controller = target.GetComponent<ObjectController>();

        if (controller.currentHealth + cure > controller.totalHealth)
        {
            cure = controller.totalHealth - controller.currentHealth;
        }
        UI.GetComponent<UIManager>().ShowPopupDamage(cure, target.transform);
        target.GetComponent<ObjectController>().currentHealth = target.GetComponent<ObjectController>().currentHealth + cure;
        UI.GetComponent<UIManager>().SetPlayerHealthBar(target);
        
    }

    protected bool IsDead(ObjectController target)
    {
        if(target.currentHealth <= 0)
        {
            if (target.tag == "Enemy")
            {
                UI.GetComponent<UIManager>().DestroyEnemyUI(target.GetComponent<EnemyController>().position);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public int CompareTo(ObjectController compare)
    {
        // A null value means that this object is greater.
        if (compare == null)
            return 1;

        else
            return this.skill.CompareTo(compare.skill);
    }

}
