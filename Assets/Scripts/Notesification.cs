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
        string notifyColor = "#" + ColorUtility.ToHtmlStringRGB(GameManager.Ins.notifyColor);
        TMP_Text notifText = alert.GetComponent<TMP_Text>();
        notifText.text = notifText.text.Replace("notiCol", notifyColor);
        alert.SetActive(false);
    }

    public void UpdateNotification()
    {
        alert.SetActive(notified);
        if (notified)
        {
            anim.Play("Note Get");
        }
    }

    public void Notify()
    {
        notified = true;
        anim.Play("Note Get");
    }
    public void UpdateNotification(bool notify)
    {
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