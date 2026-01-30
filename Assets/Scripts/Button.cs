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
}
