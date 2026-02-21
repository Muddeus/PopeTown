using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DropdownTextSpeed : MonoBehaviour
{

    public void Start()
    {
        GetComponent<TMP_Dropdown>().value = 1;
    }
    public void OnSelect(int index)
    {
        GameManager g = GameManager.Ins;
        switch (index)
        {
            case 0: // FAST
                g.textSpeedMult = g.textSpeedFast;
                break;
            case 1: // MEDIUM
                g.textSpeedMult = g.textSpeedMedium;
                break;
            case 2: // SLOW
                g.textSpeedMult = g.textSpeedSlow;
                break;
        }
    }
}
