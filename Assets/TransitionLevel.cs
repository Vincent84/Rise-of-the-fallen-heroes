using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionLevel : MonoBehaviour {

    private void OnEnable()
    {
        GameManager.currentState = GameManager.States.WAIT;
        StartCoroutine(ShowMap());
    }

    IEnumerator ShowTransition(float delay)
    {
        GetComponentInChildren<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
        GameManager.currentState = GameManager.States.EXPLORATION;
    }

    IEnumerator ShowMap()
    {
        GetComponentInChildren<Animator>().SetTrigger(SceneManager.GetActiveScene().name);
        StartCoroutine(ShowTransition(5));
        yield return null;
    }
}
