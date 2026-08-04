using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public Animator anim;
    public Animator menuAnim;
    [Range(60, 120)] public int targetFrameRate = 60;
    public string gameSceneName = "ChrisUI";

    //Andy stuff
    public bool introOver = false;
    public AudioSource introMusic;
    public float introPress = 0f;
    public float introTimer = 0f;
    private bool musicFadeEnabled;

    public TMP_Text skipText;

    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }

    public void Update()
    {
        if ((Input.GetMouseButton(0)) && !introOver)
        {
            Debug.Log("Pressed");
            introPress += Time.deltaTime;
        }
        else
        {
            introPress = 0f;
        }

        if ((introPress >= 1f || introTimer >= 17.15f) && !introOver)
        {
            introOver = true;
            menuAnim.Play("Menu Rising", 0, 1.0f);
            anim.Play("Intro", 0, 1.0f);
        }

        if (introOver)
        {
            introPress = 0;
        }
        else
        {
            introTimer += Time.deltaTime;
        }

        skipText.alpha = introPress;

        if(musicFadeEnabled)
        {
            float newVolume = introMusic.volume - (0.1f * Time.deltaTime);
        }

    }

    public void NewGame()
    {
        StartCoroutine(NewTransition());
        introOver = true;
    }

    public void Continue()
    {
        StartCoroutine(ContinueTransition());
        introOver = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator NewTransition()
    {
        musicFadeEnabled = true;
        Debug.Log("New Game");
        anim.Play("Begin");
        yield return new WaitForSeconds(6.7f);
        SceneManager.LoadScene(gameSceneName);
    }

    IEnumerator ContinueTransition()
    {
        musicFadeEnabled = true;
        Debug.Log("Continue");
        anim.Play("Begin");
        yield return new WaitForSeconds(6.7f);
        //Put in something here to load saved data
        SceneManager.LoadScene(gameSceneName);
    }


}
