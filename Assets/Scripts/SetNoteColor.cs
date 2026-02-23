using UnityEngine;
using UnityEngine.UI;

public class SetNoteColor : MonoBehaviour
{
    void Start()
    {
        Image image = GetComponent<Image>();
        image.color = GameManager.Ins.notifyColor;
    }

}
