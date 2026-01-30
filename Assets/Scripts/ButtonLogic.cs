using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLogic : MonoBehaviour
{
    public Question question;
    private TMP_Text buttonText;
    private bool initialized = false;
    private Image image;
    public float fadedAmount;
    public Color fadedColor;
    void Start()
    {
        buttonText = GetComponentInChildren<TMP_Text>();
        image = GetComponent<Image>();
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
                fadedColor = Color.Lerp(image.color, Color.black, fadedAmount);
                if (!question.newQuestion) buttonText.color = fadedColor;
            }
        }
        else // After initialisation
        {
           
        }
        
    }

    public void OnClick()
    {
        UIManager.Ins.SelectQuestion(question);
        question.newQuestion = false;
        UIManager.Ins.UpdateNotes();
        print("clicked");
    }
}
