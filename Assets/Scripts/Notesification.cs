using TMPro;
using UnityEngine;

public class Notesification : MonoBehaviour
{
    public bool notified;
    public GameObject alert;
    public Animator anim;
    public TMP_Text text;
    public GameObject notesBox;
    void Start()
    {
        alert.SetActive(false);
    }

    void Update()
    {
        
    }

    public void UpdateNotification()
    {
        alert.SetActive(notified);
        if (notified)
        {
            anim.Play("Note Get");
        }
    }
    public void UpdateNotification(bool notify)
    {
        print("NOTIFY: " + notify);
        notified = notify;
        UpdateNotification();
    }

    public void UpdateUnderline(GameObject gameObject)
    {
        if (gameObject.gameObject.activeInHierarchy)
        {
            text.fontStyle = FontStyles.Underline;
        }
        else
        {
            text.fontStyle = FontStyles.Normal;
        }
    }
    
}
