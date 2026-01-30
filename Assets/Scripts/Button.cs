using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{

    public void ToggleMenu(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void PlaySound(int sound)
    {
        switch ((Sound)sound)
        {
            case Sound.None:
                break;
            case Sound.Notes:
                SFXManager.instance.PlaySound(SFXManager.instance.notes1, transform, 1f);
                break;
        }
    }
}


