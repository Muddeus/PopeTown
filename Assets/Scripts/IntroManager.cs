using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [Range(60,120)]public int targetFrameRate = 60;
    public string gameSceneName = "ChrisUI";
    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }

    public void LoadNewGame()
    {
        Application.LoadLevel(gameSceneName);
    }
}
