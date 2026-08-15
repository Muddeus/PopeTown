using UnityEngine;

public class FaceButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        UIManager.Ins.FaceClick();
    }

    public void OnMouseEnter()
    {
        UIManager.Ins.ShowFaceBox();
    }

    public void OnMouseExit()
    {
        UIManager.Ins.HideFaceBox();
    }
}
