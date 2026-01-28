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

    public TextObj[] AllTextArray;
    public List<TextObj> currentTextList;
    
    public List<TextObj> entranceTextList;
    public List<TextObj> townSquareTextList;
    public List<TextObj> mayorsOfficeTextList;
    public List<TextObj> docksTextList;
    public List<TextObj> suburbsTextList;
    public List<TextObj> artStudioTextList;
    public List<TextObj> shackTextList;
    public List<TextObj> parkTextList;

    void Start()
    {
        textProgress = 0;
        AllTextArray = Resources.LoadAll<TextObj>("");
        foreach (TextObj obj in AllTextArray)
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
        // When game loads, start here
        if (GameManager.Ins.location == Location.Entrance)
        {
            currentTextList = entranceTextList;
            mainText = currentTextList[0].text[0];
            print(mainText);
            //mainTextDisplay.text = mainText;
            AnimateText();
        }
    }

    void Update()
    {
        if (textAnimating )//&& textTimer > textSpeed)
        {
            textPosition = (int)(textTimer / textSpeed);
            textPosition = Math.Clamp(textPosition, 0, textLength);
            
                mainTextDisplay.text = mainText.Substring(0, textPosition);
                //textTimer = 0;
            
            if(textPosition >= textLength) textAnimating = false;
            

            textTimer += Time.deltaTime;
        }
        print(textAnimating);
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

    
    
    public void NextText()
    {
        if (textAnimating) // if text still coming out..
        {
            textTimer = 9999f;
        }
        else // if moving to next text/screen
        {
            textProgress++;
            if (textProgress <= currentTextList[GetCurrentLocationProgress()].text.Length)
            {
                mainText = currentTextList[GetCurrentLocationProgress()].text[textProgress];
            }
            else
            {
                SetCurrentLocationProgress(GetCurrentLocationProgress()+1);
                textProgress = 0;
            }

            if (GetCurrentLocationProgress() >= currentTextList.Count)
            {
                // Return to questions screen if no more text available
                print("RETURN TO QUESTION SCREEN");
            }
            
            

            AnimateText();
        }
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
