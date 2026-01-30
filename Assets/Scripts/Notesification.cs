using UnityEngine;

public class Notesification : MonoBehaviour
{
    public bool notified;
    public GameObject alert;
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
            // play animation
        }
    }
    public void UpdateNotification(bool notify)
    {
        notified = notify;
        UpdateNotification();
    }
    
}
