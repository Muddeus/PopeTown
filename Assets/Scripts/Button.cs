using UnityEngine;

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
}
