using TMPro;
using UnityEngine;

public class ButtonLogic : MonoBehaviour
{
    public Question question;
    private TMP_Text buttonText;
    private bool initialized = false;
    void Start()
    {
        buttonText = GetComponentInChildren<TMP_Text>();
        //buttonText.text = question.questionText;
    }

    void Update()
    {
        if (!initialized)
        {
            if (question != null)
            {
                initialized = true;
                buttonText.text = question.questionText;
            }
        }
    }

    public void OnClick()
    {
        UIManager.Ins.SelectQuestion(question);
    }
}
