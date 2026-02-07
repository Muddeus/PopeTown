using UnityEngine;
using UnityEngine.UI;

public class PortraitTalk : MonoBehaviour
{
    private Animator animator;
    public Sprite idle;
    public Sprite talking;
    private bool wasTalking;
    
    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    void Update()
    {
        bool isTalking = UIManager.Ins.GetTextAnimating();
        if(UIManager.Ins.mainText == "")isTalking = false; // because textAnimating is true during question list for some reason...
        if (isTalking == wasTalking) return;
        animator.SetBool("isTalking", isTalking);//image.sprite = isTalking? talking : idle;
        wasTalking = isTalking;
    }
}
