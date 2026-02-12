using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemLogic : MonoBehaviour
{
    public Item item;
    public bool newItem;
    private TMP_Text buttonText;
    private bool initialized = false;
    private Image image;
    void Start()
    {
        buttonText = GetComponentInChildren<TMP_Text>();
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (!initialized)
        {
            if (item != null)
            {
                initialized = true;
                
                newItem = item.newItem;
            }
        }
        else // After initialisation
        {
            //buttonText.text = item.titleText + (newItem?" (!)":"");
            buttonText.text = item.titleText + (item.titleText.Length>30?(newItem?"(<color=notiCol>!</color>)":""):(newItem?" (<color=notiCol>!</color>)":"")); // WAS item.titleText.Length>13?
            string notifyColor = "#" + ColorUtility.ToHtmlStringRGB(GameManager.Ins.notifyColor);
            buttonText.text = buttonText.text.Replace("notiCol", notifyColor);
        }
        
    }

    public void OnClick()
    {
        print("item click: " + item);
        if(!UIManager.Ins.GetPresentingMode())item.newItem = false;
        UIManager.Ins.SelectItem(item);
    }
}
