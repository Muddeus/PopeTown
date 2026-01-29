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
                buttonText.text = item.text;
            }
        }
        else // After initialisation
        {
           
        }
        
    }

    public void OnClick()
    {
        UIManager.Ins.SelectItem(item);
    }
}
