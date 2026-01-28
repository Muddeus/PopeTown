using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Ins
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(UIManager).ToString());
                    _instance = singleton.AddComponent<UIManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public string mainText;
    public TMP_Text mainTextDisplay;
    private int textProgress;

    public bool questionMode;
    public GameObject questionsBox;
    public GameObject contentBox; // Where the question buttons go
    public GameObject buttonPrefab;
    public Question currentQuestion;

    public TextObj[] allTextArray;
    public List<TextObj> currentTextList;
    
    public List<TextObj> entranceTextList;
    public List<TextObj> townSquareTextList;
    public List<TextObj> mayorsOfficeTextList;
    public List<TextObj> docksTextList;
    public List<TextObj> suburbsTextList;
    public List<TextObj> artStudioTextList;
    public List<TextObj> shackTextList;
    public List<TextObj> parkTextList;

    public Question[] allQuestionArray;
    public List<Question> currentQuestionList;
    
    public List<Question> entranceQuestionList;
    public List<Question> townSquareQuestionList;
    public List<Question> mayorsOfficeQuestionList;
    public List<Question> docksQuestionList;
    public List<Question> suburbsQuestionList;
    public List<Question> artStudioQuestionList;
    public List<Question> shackQuestionList;
    public List<Question> parkQuestionList;

    void Start()
    {
        textProgress = 0;
        questionMode = false;
        questionsBox.SetActive(false);
        allTextArray = Resources.LoadAll<TextObj>("");
        foreach (TextObj obj in allTextArray)
        {
            switch (obj.location)
            {
                case Location.Entrance:
                    entranceTextList.Add(obj);
                    break;
                case Location.TownSquare:
                    townSquareTextList.Add(obj);
                    break;
                case Location.MayorsOffice:
                    mayorsOfficeTextList.Add(obj);
                    break;
                case Location.Docks:
                    docksTextList.Add(obj);
                    break;
                case Location.Suburbs:
                    suburbsTextList.Add(obj);
                    break;
                case Location.ArtStudio:
                    artStudioTextList.Add(obj);
                    break;
                case Location.Shack:
                    shackTextList.Add(obj);
                    break;
                case Location.Park:
                    parkTextList.Add(obj);
                    break;
            }
        }
        
        allQuestionArray = Resources.LoadAll<Question>("");
        foreach (Question obj in allQuestionArray)
        {
            switch (obj.location)
            {
                case Location.Entrance:
                    entranceQuestionList.Add(obj);
                    break;
                case Location.TownSquare:
                    townSquareQuestionList.Add(obj);
                    break;
                case Location.MayorsOffice:
                    mayorsOfficeQuestionList.Add(obj);
                    break;
                case Location.Docks:
                    docksQuestionList.Add(obj);
                    break;
                case Location.Suburbs:
                    suburbsQuestionList.Add(obj);
                    break;
                case Location.ArtStudio:
                    artStudioQuestionList.Add(obj);
                    break;
                case Location.Shack:
                    shackQuestionList.Add(obj);
                    break;
                case Location.Park:
                    parkQuestionList.Add(obj);
                    break;
            }
        }
        // When game loads, start here
        if (GameManager.Ins.location == Location.Entrance)
        {
            currentTextList = entranceTextList;
            currentQuestionList = entranceQuestionList;
            mainText = currentTextList[0].text[0];
            print(mainText);
            //mainTextDisplay.text = mainText;
            AnimateText();
        }
    }

    void Update()
    {
        if (textAnimating && (!questionMode || currentQuestion !=null))//&& textTimer > textSpeed)
        {
            textPosition = (int)(textTimer / textSpeed);
            textPosition = Math.Clamp(textPosition, 0, textLength);

            if (mainText.Length == 0)
            {
                mainTextDisplay.text = "";
            }
            else
            {
                mainTextDisplay.text = mainText.Substring(0, textPosition);
            }
            
            if(textPosition >= textLength) textAnimating = false;
            

            textTimer += Time.deltaTime;
        }

        if (questionMode && !questionsBox.activeInHierarchy) // Runs once each time question mode is activated
        {
            RefreshQuestions();
        }

        if (questionMode)
        {
            
        }
    }

    private float textTimer;
    private int textPosition;
    private int textLength;
    [Range(0.01f, 0.25f)]public float textSpeed;
    private bool textAnimating = false;
    private void AnimateText()
    {
        textAnimating = true;
        textPosition = 0;
        textTimer = 0;
        textLength = mainText.Length;
    }

    public void RefreshQuestions()
    {
        questionsBox.SetActive(true);
        mainText = "";
        mainTextDisplay.text = "";
        //foreach(Transform child in contentBox.transform) Destroy(child.gameObject);
        ClearQuestions();
        foreach (Question obj in currentQuestionList)
        {
            GameObject button = Instantiate(buttonPrefab, contentBox.transform);
            button.GetComponent<ButtonLogic>().question = obj;
        }
    }

    public void ClearQuestions()
    {
        foreach(Transform child in contentBox.transform) Destroy(child.gameObject);
    }
    
    public void NextText()
    {
        if (textAnimating) // if text still coming out..
        {
            textTimer = 9999f;
        }
        else // if moving to next text/screen
        {
            // Update the log here, text has been read
            textProgress++;

            if (questionMode)
            {
                if (currentQuestion != null) // If there is a current question
                {
                    print("there is a current question");
                    mainText = currentQuestion.conversation[textProgress-1]; // start from 1 in question mode
                }
                else // If there is no current question
                {
                    
                    textProgress = 0;
                    RefreshQuestions();
                }

                if (GetCurrentLocationProgress() >= currentTextList.Count)
                {
                    // Return to questions screen if no more text available
                    print("RETURN TO QUESTION SCREEN");
                    questionMode = true;
                }
            }
            else
            {
                if (textProgress < currentTextList[GetCurrentLocationProgress()].text.Length)
                {
                    mainText = currentTextList[GetCurrentLocationProgress()].text[textProgress];
                }
                else
                {
                    SetCurrentLocationProgress(GetCurrentLocationProgress() + 1);
                    textProgress = 0;
                }

                if (GetCurrentLocationProgress() >= currentTextList.Count)
                {
                    // Return to questions screen if no more text available
                    print("RETURN TO QUESTION SCREEN");
                    questionMode = true;
                }
            }
            
            
            

            AnimateText();
        }
    }

    public void SelectQuestion(Question question)
    {
        currentQuestion = question;
        textProgress = 0;
        ClearQuestions();
        NextText();
    }

    public int GetCurrentLocationProgress()
    {
        int prog = 0;
        switch (GameManager.Ins.location)
        {
            case Location.Entrance:
                prog = GameManager.Ins.entranceTextProgress;
                break;
            case Location.TownSquare:
                prog = GameManager.Ins.townSquareTextProgress;
                break;
            case Location.MayorsOffice:
                prog = GameManager.Ins.mayorsOfficeTextProgress;
                break;
            case Location.Docks:
                prog = GameManager.Ins.docksTextProgress;
                break;
            case Location.Suburbs:
                prog = GameManager.Ins.suburbsTextProgress;
                break;
            case Location.ArtStudio:
                prog = GameManager.Ins.artStudioTextProgress;
                break;
            case Location.Shack:
                prog = GameManager.Ins.shackTextProgress;
                break;
            case Location.Park:
                prog = GameManager.Ins.parkTextProgress;
                break;
        }

        return prog;
    }
    public void SetCurrentLocationProgress(int prog)
    {
        switch (GameManager.Ins.location)
        {
            case Location.Entrance:
                GameManager.Ins.entranceTextProgress = prog;
                break;
            case Location.TownSquare:
                GameManager.Ins.townSquareTextProgress = prog;
                break;
            case Location.MayorsOffice:
                GameManager.Ins.mayorsOfficeTextProgress = prog;
                break;
            case Location.Docks:
                GameManager.Ins.docksTextProgress = prog;
                break;
            case Location.Suburbs:
                GameManager.Ins.suburbsTextProgress = prog;
                break;
            case Location.ArtStudio:
                GameManager.Ins.artStudioTextProgress = prog;
                break;
            case Location.Shack:
                GameManager.Ins.shackTextProgress = prog;
                break;
            case Location.Park:
                GameManager.Ins.parkTextProgress = prog;
                break;
            default:
                print("ERROR: NO VALID LOCATION");
                break;
        }
    }
}
