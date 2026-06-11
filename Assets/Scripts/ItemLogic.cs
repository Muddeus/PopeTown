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

    public Image avatarSprite;
    public Sprite guardSprite;
    public Sprite mayorSprite;
    public Sprite artistSprite;
    public Sprite punkSprite;
    public Sprite homelessSprite;
    public Sprite handywomanSprite;
    public Sprite sexWorkerSprite;
    void Start()
    {
        buttonText = GetComponentInChildren<TMP_Text>();



        if (item != null)
        {
            // Happens once on initialisation
            initialized = true;

            newItem = item.newItem;
            avatarSprite.gameObject.SetActive(true);
            switch (item.character)
            {
                case Character.None:
                    //avatarSprite.sprite = null;
                    avatarSprite.gameObject.SetActive(false);
                    break;
                case Character.Mayor:
                    avatarSprite.sprite = mayorSprite;
                    break;
                case Character.SexWorker:
                    avatarSprite.sprite = sexWorkerSprite;
                    break;
                case Character.Homeless:
                    avatarSprite.sprite = homelessSprite;
                    break;
                case Character.Artist:
                    avatarSprite.sprite = artistSprite;
                    break;
                case Character.Punk:
                    avatarSprite.sprite = punkSprite;
                    break;
                case Character.Handywoman:
                    avatarSprite.sprite = handywomanSprite;
                    break;
                case Character.Guard:
                    avatarSprite.sprite = guardSprite;
                    break;
                case Character.Twinskin:
                    avatarSprite.sprite = null;
                    break;
            }
        }
    }

    void Update()
    {
        if (!initialized)
        {
            if (item != null)
            {
                // Happens once on initialisation
                initialized = true;
                
                newItem = item.newItem;
                switch (item.character)
                {
                    case Character.None:
                        avatarSprite.sprite = null;
                        break;
                    case Character.Mayor:
                        avatarSprite.sprite = mayorSprite;
                        break;
                    case Character.SexWorker:
                        avatarSprite.sprite = sexWorkerSprite;
                        break;
                    case Character.Homeless:
                        avatarSprite.sprite = homelessSprite;
                        break;
                    case Character.Artist:
                        avatarSprite.sprite = artistSprite;
                        break;
                    case Character.Punk:
                        avatarSprite.sprite = punkSprite;
                        break;
                    case Character.Handywoman:
                        avatarSprite.sprite = handywomanSprite;
                        break;
                    case Character.Guard:
                        avatarSprite.sprite = guardSprite;
                        break;
                    case Character.Twinskin:
                        avatarSprite.sprite = null;
                        break;
                }
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
