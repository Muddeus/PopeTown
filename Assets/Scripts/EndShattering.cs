using UnityEngine;

public class EndShattering : MonoBehaviour
{
    public void EndShatter()
    {
        GameManager.Ins.shattering = false;
        UIManager.Ins.NextText();
        print("End of shatter trigger");
    }
}
