using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public Animator anim;
    [Range(60, 120)] public int targetFrameRate = 60;
    public string gameSceneName = "ChrisUI";
    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }

    public void NewGame()
    {
        StartCoroutine(BeginTransition());
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator BeginTransition()
    {
        anim.Play("Begin");
        yield return new WaitForSeconds(6.7f);
        SceneManager.LoadScene(gameSceneName);
    }


}
