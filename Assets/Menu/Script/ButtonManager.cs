using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

    private GameObject value;
    private GameObject skillPointsValue;
    private int defaultValue;
    private int intValue;
    private int maxSkillPoints;

    private void Update()
    {
        if(value)
        {
            intValue = int.Parse(value.GetComponent<TextMeshProUGUI>().text);
            maxSkillPoints = int.Parse(skillPointsValue.GetComponent<TextMeshProUGUI>().text);

            if (gameObject.name.Contains("Min"))
            {
                if (defaultValue != intValue)
                {
                    GetComponent<Button>().interactable = true;
                }
                else if (defaultValue == intValue)
                {
                    GetComponent<Button>().interactable = false;
                }

            }

            if (gameObject.name.Contains("Plus"))
            {
                if(maxSkillPoints <= 0)
                {
                    GetComponent<Button>().interactable = false;
                }
                else
                {
                    GetComponent<Button>().interactable = true;
                }
            }
        }
        
    }

    public void OnAddValue()
    {
        maxSkillPoints--;
        intValue++;
        value.GetComponent<TextMeshProUGUI>().text = intValue.ToString();
        skillPointsValue.GetComponent<TextMeshProUGUI>().text = maxSkillPoints.ToString();
    }

    public void OnSubValue()
    {
        maxSkillPoints++;
        intValue--;
        value.GetComponent<TextMeshProUGUI>().text = intValue.ToString();
        skillPointsValue.GetComponent<TextMeshProUGUI>().text = maxSkillPoints.ToString();
    }

    public GameObject Value
    {
        get
        {
            return value;
        }

        set
        {
            intValue = int.Parse(value.GetComponent<TextMeshProUGUI>().text);
            defaultValue = intValue;
            this.value = value;
        }
    }

    public GameObject SkillPointsValue
    {
        get
        {
            return skillPointsValue;
        }

        set
        {
            maxSkillPoints = int.Parse(value.GetComponent<TextMeshProUGUI>().text);
            skillPointsValue = value;
        }
    }
}
