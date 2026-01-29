using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    [SerializeField] private AudioClip notesAudioClip;

    public void ToggleMenu(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
        SFXManager.instance.PlaySound(notesAudioClip, transform, 1f);
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
