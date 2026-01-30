using UnityEngine;

public class Notesification : MonoBehaviour
{
    public bool notified;
    public GameObject alert;
    public Animator anim;
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
    
}
