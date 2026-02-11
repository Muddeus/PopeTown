using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public Sprite notif, noNotif;
    public GameObject townSquare, mayorsOffice, shack, park, docks, eastDistrict, artStudio;
    void OnEnable()
    {
        townSquare.SetActive(GameManager.Ins.townSquareUnlocked);
        mayorsOffice.SetActive(GameManager.Ins.mayorsOfficeUnlocked);
        shack.SetActive(GameManager.Ins.shackUnlocked);
        park.SetActive(GameManager.Ins.parkUnlocked);
        docks.SetActive(GameManager.Ins.docksUnlocked);
        eastDistrict.SetActive(GameManager.Ins.suburbsUnlocked);
        artStudio.SetActive(GameManager.Ins.artStudioUnlocked);

        bool newTS = false, newMO = false, newShack = false, newPark = false, newDocks = false, newED = false, newAS = false;
        foreach (Question q in UIManager.Ins.allQuestionArray)
        {
            if (q.newQuestion && q.unlocked)
            {
                switch (q.location)
                {
                    case Location.Park:
                        newPark = true;
                        break;
                    case Location.Docks:
                        newDocks = true;
                        break;
                    case Location.ArtStudio:
                        newAS = true;
                        break;
                    case Location.Shack:
                        newShack = true;
                        break;
                    case Location.TownSquare:
                        newTS = true;
                        break;
                    case Location.MayorsOffice:
                        newMO = true;
                        break;
                    case Location.Suburbs:
                        newED = true;
                        break;
                }
                townSquare.GetComponent<Image>().sprite = newTS? notif : noNotif;
                mayorsOffice.GetComponent<Image>().sprite = newMO? notif : noNotif;
                shack.GetComponent<Image>().sprite = newShack? notif : noNotif;
                park.GetComponent<Image>().sprite = newPark? notif : noNotif;
                docks.GetComponent<Image>().sprite = newDocks? notif : noNotif;
                eastDistrict.GetComponent<Image>().sprite = newED? notif : noNotif;
                artStudio.GetComponent<Image>().sprite = newAS? notif : noNotif;
            }
        }
    }
}
