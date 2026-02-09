using UnityEngine;

public class EddieLogic : MonoBehaviour
{
    public GameObject portrait;
    public GameObject button;

    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(portrait.activeInHierarchy != button.activeInHierarchy)button.SetActive(portrait.activeInHierarchy);
        if (portrait.gameObject.activeSelf != GameManager.Ins.punkRevealed)
        {
            print(GameManager.Ins.punkRevealed);
            anim.SetTrigger(("revealed"));
            //UIManager.Ins.nameBoxText.text = "???";
            //portrait.SetActive(GameManager.Ins.punkRevealed);
        }
    }
}
