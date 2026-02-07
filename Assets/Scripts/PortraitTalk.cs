using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class PortraitTalk : MonoBehaviour
{
    private Animator animator;
    public Sprite idle;
    public Sprite talking;
    private bool wasTalking;
    [Header("How long the character takes while talking to change to their talking sprite.")]
    [Header("Adds a bit of personality.")]
    [Header("Don't go below 0.1f or it glitches a lil")]
    public float timeToTalk;

    private float timer;
    private float timer2;
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        timer = 0f;
        timer2 = 0f;
    }

    void Update()
    {
        bool isTalking = UIManager.Ins.GetTextAnimating();
        if(UIManager.Ins.mainText == "")isTalking = false; // because textAnimating is true during question list for some reason...
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;
        if (!isTalking) timer = 0f;
        if (isTalking) timer2 = 0f;
        if (isTalking == wasTalking) return;
        if (isTalking && timer < timeToTalk * Random.Range(1f,1.5f)) return;
        if(!isTalking && timer2 < timeToTalk * Random.Range(1f,1.5f)) return;
        animator.SetBool("isTalking", isTalking);//image.sprite = isTalking? talking : idle;
        wasTalking = isTalking;
    }
}
