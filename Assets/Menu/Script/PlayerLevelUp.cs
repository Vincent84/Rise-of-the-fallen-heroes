using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLevelUp : MonoBehaviour {
    
    public int maxSkilPoint = 3;
    public GameObject mindValue;
    public GameObject constitutiionValue;
    public GameObject skillValue;
    public GameObject strengthValue;
    public GameObject skillPointsValue;
    public bool isComplete;

    private int mind;
    private int costitution;
    private int skill;
    private int strength;
    private GameObject player;


    // Use this for initialization
    void Start () {
        mind = Player.GetComponent<ObjectController>().mind;
        costitution = Player.GetComponent<ObjectController>().constitution;
        skill = Player.GetComponent<ObjectController>().skill;
        strength = Player.GetComponent<ObjectController>().strength;

        skillPointsValue.GetComponent<TextMeshProUGUI>().SetText(maxSkilPoint.ToString());
        transform.GetChild(0).GetComponent<Image>().sprite = Player.GetComponent<ObjectController>().iconObject;
        mindValue.GetComponent<TextMeshProUGUI>().SetText(mind.ToString());
        constitutiionValue.GetComponent<TextMeshProUGUI>().SetText(costitution.ToString());
        skillValue.GetComponent<TextMeshProUGUI>().SetText(skill.ToString());
        strengthValue.GetComponent<TextMeshProUGUI>().SetText(strength.ToString());

        int i = 0;
        bool min = false;
        bool add = false;

        foreach(Button button in GetComponentsInChildren<Button>())
        {
            if (button.name.Contains("Plus"))
            {
                button.gameObject.GetComponent<ButtonManager>().SkillPointsValue = skillPointsValue;
                button.onClick.AddListener(() => { button.gameObject.GetComponent<ButtonManager>().OnAddValue(); });
                add = true;
            }
            else
            {
                button.gameObject.GetComponent<ButtonManager>().SkillPointsValue = skillPointsValue;
                button.onClick.AddListener(() => { button.gameObject.GetComponent<ButtonManager>().OnSubValue(); });
                min = true;
            }

            switch (i)
            {
                case 0:
                    button.gameObject.GetComponent<ButtonManager>().Value = mindValue;
                    break;
                case 1:
                    button.gameObject.GetComponent<ButtonManager>().Value = constitutiionValue;
                    break;
                case 2:
                    button.gameObject.GetComponent<ButtonManager>().Value = skillValue;
                    break;
                case 3:
                    button.gameObject.GetComponent<ButtonManager>().Value = strengthValue;
                    break;
            }

            if (min && add)
            {
                min = false;
                add = false;
                i++;
            }
            
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(int.Parse(skillPointsValue.GetComponent<TextMeshProUGUI>().text) <= 0)
        {
            isComplete = true;
        }
        else
        {
            isComplete = false;
        }
	}

    public GameObject Player
    {
        get
        {
            return player;
        }

        set
        {
            player = value;
        }
    }
}
