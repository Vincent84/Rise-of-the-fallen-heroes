using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelMusic : MonoBehaviour {

	[Header("Muische scene")]

	[Space]

	[Tooltip ("Queste musiche vanno alternate nelle varie scene di gioco")]
	public AudioClip menu;
	public AudioClip forest;
	public AudioClip desert;
	public AudioClip iceLand;
	public AudioClip swamp;
	public AudioClip castel;
	public AudioClip enemyFight;
	public AudioClip nemesi;
	public AudioClip bossFight;
	public AudioClip credits;
	public AudioClip levelUp;
	public AudioClip gameOver;

	static LevelMusic instance=null;
	public AudioSource main;
    public AudioSource second;
    private AudioClip currentMusic;
    private bool isPlay = true;

	void Awake ()
	{
		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {

			instance = this;
			GameObject.DontDestroyOnLoad (gameObject);
			SceneManager.sceneLoaded += OnSceneLoaded;
		}
    }
	   

	// Use this for initialization
	void Start ()
	{
		if (instance != null && instance != this) {
			Destroy (gameObject);
			print ("AudioManager extra distrutto");
		} else {
			instance = this; 
			GameObject.DontDestroyOnLoad (gameObject);
			main.clip = menu;
			main.Play ();
		}
    }
	// Aggiungere il nome o l'index della scena
	void OnSceneLoaded (Scene scene, LoadSceneMode loadscenemode){
        //music.Stop ();
        if (scene.buildIndex == 1) {
            main.clip = menu;
        }
        if (scene.buildIndex == 2) {
            main.clip = credits;
        }
        if (scene.buildIndex == 3) {
            main.clip = forest;
        }
	    if (scene.buildIndex == 4) {
            main.clip = desert;
	    }
	    if (scene.buildIndex == 5) {
            main.clip = iceLand;
	    }
	    if (scene.buildIndex == 6) {
            main.clip = swamp;
	    }
	    if (scene.buildIndex == 7) {
            main.clip = castel;
	    }
	    if (scene.buildIndex == 8) {
            main.clip = levelUp;
            main.loop = false;
        }

        if (scene.buildIndex != 8)
        {
            main.loop = true;
        }

        second.Stop();
        StartCoroutine(FadeIn(main, 1f));
    }

    private void Update()
    {   
        if (GameManager.currentState == GameManager.States.ENGAGE_ENEMY)
        {
            main.Pause();
            if(PlayerController.enemyEngaged == "EnemyGroup")
            {
                StartCoroutine(FadeOutIn(second, 1f, enemyFight));
            }
            else if(PlayerController.enemyEngaged == "Nemesy")
            {
                StartCoroutine(FadeOutIn(second, 1f, nemesi));
            }
            else if(PlayerController.enemyEngaged == "Boss")
            {
                StartCoroutine(FadeOutIn(second, 1f, bossFight));
            }
            isPlay = false;
        }
        else if (GameManager.currentState == GameManager.States.GAME_OVER)
        {
            main.Pause();
            isPlay = false;
            StartCoroutine(FadeOutIn(second, 1f, gameOver));
        }
        else if (GameManager.currentState == GameManager.States.EXPLORATION && !isPlay)
        {
            second.Pause();
            StartCoroutine(FadeIn(main, 1f));
            isPlay = true;
        }
    }

    public IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float startVolume = 0.2f;

        audioSource.volume = 0;
        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        while (audioSource && audioSource.volume < 1.0f)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = 1f;
        if(audioSource.clip != levelUp)
        {
            audioSource.loop = true;
        }
    }

    public IEnumerator FadeOutIn(AudioSource audioSource, float FadeTime, AudioClip newClip)
    {
        float startVolume = 1f;

        while (audioSource && audioSource.volume > 0f)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = 0f;
        second.clip = newClip;

        StartCoroutine(FadeIn(second, 2f));
    }

}


