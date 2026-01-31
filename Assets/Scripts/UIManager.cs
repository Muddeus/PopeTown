using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
    private Notesification notesification;

    public bool questionMode;
    public GameObject questionsBox;
    public GameObject contentBox; // Where the question buttons go
    public GameObject examineObj;
    public GameObject leaveObj;
    public GameObject buttonPrefab;
    public GameObject notePrefab;
    public GameObject emptyNotesText;
    public Question currentQuestion;

    public List<Item> ownedItemList;
    
    public TMP_Text nameBoxText;
    public Image nameBox;
    public Transform notesBox;
    public Image handsBox;
    public Texture2D hands;

    private int portraitIndex;
    private Image portraitImage;
    public GameObject currentPortrait;
    public GameObject portraitPanel;
    public GameObject guardPortrait;

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
        nameBoxText.text = "";
        nameBox.enabled = false;
        leaveObj.SetActive(false);
        examineObj.SetActive(false);
        portraitIndex = 0;
        ClearNotes();
        notesification = FindFirstObjectByType<Notesification>();
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
            // (re)Sets unlocked and newQuestion fields
            obj.newQuestion = true;
            if (obj.exploredIDs.Count == 0)
            {
                obj.unlocked = true;
            }
            else
            {
                obj.unlocked = false;
            }
            // Make lists of questions by location
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
            GameManager.Ins.character = Character.None;
            currentTextList = entranceTextList;
            currentQuestionList = entranceQuestionList;
            mainText = currentTextList[0].text[0];
            AnimateText();
        }
    }
    private float handsTimer = 0;
    private float textSpeedMult;
    void Update()
    {
        if (nameBoxText.text == "") nameBox.enabled = false;
        // hands animation
        handsBox.enabled = questionMode;
        if (questionMode)
        {
            // show hands
            if (textAnimating)
            {
                // animate hands
                
            }
        }
        if (textAnimating && (!questionMode || currentQuestion !=null))//&& textTimer > textSpeed)
        {
            // text animation
            textSpeedMult = textLength < 20 ? 0.3f : 1f;
            if (textLength < 10) textSpeedMult = 0.05f;
            if(!(GameManager.Ins.character == Character.None)) PlayTextSound();
            textPosition = (int)(textTimer / textSpeed * textSpeedMult);
            textPosition = Math.Clamp(textPosition, 0, textLength);
            
            // portrait animation
            if (currentPortrait != null)
            {
                portraitImage = currentPortrait.GetComponent<Image>();
                //portraitImage.sprite.
            }
            else //
            {
                
            }

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
        currentQuestion = null;
        questionsBox.SetActive(true);
        leaveObj.SetActive(GameManager.Ins.townSquareUnlocked);
        examineObj.SetActive(true);
        mainText = "";
        mainTextDisplay.text = "";
        //foreach(Transform child in contentBox.transform) Destroy(child.gameObject);
        ClearQuestions();
        List<Transform> oldQuestions = new List<Transform>();
        foreach (Question obj in currentQuestionList)
        {
            if (obj.unlocked) // Locked questions will not show up until unlocked and refreshed again
            {
                GameManager.Ins.character = obj.character;
                SetPortrait();
                GameObject button = Instantiate(buttonPrefab, contentBox.transform);
                //button.transform.SetAsFirstSibling();
                if (!obj.newQuestion)
                {
                    button.transform.SetAsLastSibling();
                    oldQuestions.Add(button.transform);
                }
                button.GetComponent<ButtonLogic>().question = obj;
            }
        }

        foreach (Transform t in oldQuestions)
        {
            t.SetAsLastSibling();
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
            //if(GameManager.Ins.location != Location.Entrance) SetPortrait(); // don't want it to affect start of game
            //print("Still animating text..");
            textTimer = 9999f;
        }
        else // if moving to next text/screen
        {
            // Update the log here, text has been read
            SetPortrait();
            UpdateNotes();
            emptyNotesText.SetActive(notesBox.childCount == 0); // Get rid of EMPTY text if not empty
            
            textProgress++;
            if (questionMode)
            {
                if (currentQuestion != null) // If there is a current question
                {
                    leaveObj.SetActive(false);
                    examineObj.SetActive(false);
                    // set portrait
                    GameManager.Ins.character = currentQuestion.character;
                    SetPortrait();
                    
                    // Check if item to unlock (multiple items ahead)
                    if (currentQuestion.itemReceived != null)
                    {
                        if (textProgress >= currentQuestion.itemUnlockAt)
                        {
                            // add itemReceived to ownedItemsList
                            bool dupe = false;
                            foreach (Item i in ownedItemList) // check if it's already there
                            {
                                if (i == currentQuestion.itemReceived) // dupe exists, don't add
                                {
                                    dupe = true;
                                    print("Already own item " + currentQuestion.itemReceived);
                                }
                            }

                            if (!dupe)
                            {
                                currentQuestion.itemReceived.newItem = true; // makes items marked true by default
                                ownedItemList.Add(currentQuestion.itemReceived);
                            }
                        }
                    }
                    if (currentQuestion.itemReceived2 != null)
                    {
                        if (textProgress >= currentQuestion.itemUnlockAt2)
                        {
                            // add itemReceived2 to ownedItemsList
                            bool dupe = false;
                            foreach (Item i in ownedItemList) // check if it's already there
                            {
                                if (i == currentQuestion.itemReceived2) // dupe exists, don't add
                                {
                                    dupe = true;
                                    print("Already own item " + currentQuestion.itemReceived2);
                                }
                            }

                            if (!dupe)
                            {
                                currentQuestion.itemReceived2.newItem = true; // makes items marked true by default
                                ownedItemList.Add(currentQuestion.itemReceived2);
                            }
                        }
                    }
                    if (currentQuestion.itemReceived3 != null)
                    {
                        if (textProgress >= currentQuestion.itemUnlockAt3)
                        {
                            // add itemReceived3 to ownedItemsList
                            bool dupe = false;
                            foreach (Item i in ownedItemList) // check if it's already there
                            {
                                if (i == currentQuestion.itemReceived3) // dupe exists, don't add
                                {
                                    dupe = true;
                                    print("Already own item " + currentQuestion.itemReceived3);
                                }
                            }

                            if (!dupe)
                            {
                                currentQuestion.itemReceived3.newItem = true; // makes items marked true by default
                                ownedItemList.Add(currentQuestion.itemReceived3);
                            }
                        }
                    }
                    
                    if ((textProgress - 1) < currentQuestion.conversation.Count)
                    {
                        mainText = currentQuestion.conversation[textProgress-1]; // start from 1 in question mode
                    }
                    else // question over, update changes and return to questions
                    {
                        currentQuestion.newQuestion = false;
                        // Check prerequisites
                        foreach (Question q in allQuestionArray)
                        {
                            if (q.exploredIDs != null)
                            {
                                bool unlocked = true;
                                foreach (Question r in q.exploredIDs)
                                {
                                    // If any prerequisites are still unexplored, keep that question locked,
                                    if(r.newQuestion == true) unlocked = false; 
                                    // otherwise, unlock
                                }
                                q.unlocked = unlocked;
                            }
                        }
                        currentQuestion = null;
                        RefreshQuestions();
                    }
                }
                else // If there is no current question
                {
                    
                    textProgress = 0;
                    RefreshQuestions();
                    //if(GameManager.Ins.townSquareUnlocked)leaveObj.SetActive(true);
                }

                if (GetCurrentLocationProgress() >= currentTextList.Count)
                {
                    // Return to questions screen if no more text available
                    //print("RETURN TO QUESTION SCREEN");
                    questionMode = true;
                }
            }
            else // NOT in Question Mode (text mode)
            {
                leaveObj.SetActive(false);
                examineObj.SetActive(false);
                
                AddItemFromText();
                
                // if we have more text segments to go..
                if (textProgress < currentTextList[GetCurrentLocationProgress()].text.Length)
                {
                    //print("current text length: " +  currentTextList[GetCurrentLocationProgress()].text.Length);
                    mainText = currentTextList[GetCurrentLocationProgress()].text[textProgress];
                    GameManager.Ins.character = currentTextList[GetCurrentLocationProgress()].character;
                    SetPortrait();
                }
                else // end of text chain..
                {
                    SetCurrentLocationProgress(GetCurrentLocationProgress() + 1);
                    textProgress = 0;
                    GameManager.Ins.character = Character.None;
                    SetPortrait();
                }
                //print("current loc progress: " + GetCurrentLocationProgress());
                
                if (GetCurrentLocationProgress() >= currentTextList.Count)
                {
                    // Return to questions screen if no more text available
                    //print("RETURN TO QUESTION SCREEN");
                    questionMode = true;
                    
                }
                if(!questionMode) mainText = currentTextList[GetCurrentLocationProgress()].text[textProgress];
            }
            
            
            

            AnimateText();
        }
    }

    public void AddItemFromText()
    {
        // Check if item to unlock (multiple items ahead)
                    if (currentTextList[GetCurrentLocationProgress()].itemReceived != null)
                    {
                        if (textProgress >= currentTextList[GetCurrentLocationProgress()].itemUnlockAt)
                        {
                            // add itemReceived to ownedItemsList
                            bool dupe = false;
                            foreach (Item i in ownedItemList) // check if it's already there
                            {
                                if (i == currentTextList[GetCurrentLocationProgress()].itemReceived) // dupe exists, don't add
                                {
                                    dupe = true;
                                }
                            }

                            if (!dupe)
                            {
                                currentTextList[GetCurrentLocationProgress()].itemReceived.newItem = true; // makes items marked true by default
                                ownedItemList.Add(currentTextList[GetCurrentLocationProgress()].itemReceived);
                            }
                        }
                    }
                    if (currentTextList[GetCurrentLocationProgress()].itemReceived2 != null)
                    {
                        if (textProgress >= currentTextList[GetCurrentLocationProgress()].itemUnlockAt2)
                        {
                            // add itemReceived2 to ownedItemsList
                            bool dupe = false;
                            foreach (Item i in ownedItemList) // check if it's already there
                            {
                                if (i == currentTextList[GetCurrentLocationProgress()].itemReceived2) // dupe exists, don't add
                                {
                                    dupe = true;
                                }
                            }

                            if (!dupe)
                            {
                                currentTextList[GetCurrentLocationProgress()].itemReceived2.newItem = true; // makes items marked true by default
                                ownedItemList.Add(currentTextList[GetCurrentLocationProgress()].itemReceived2);
                            }
                        }
                    }
                    if (currentTextList[GetCurrentLocationProgress()].itemReceived3 != null)
                    {
                        if (textProgress >= currentTextList[GetCurrentLocationProgress()].itemUnlockAt3)
                        {
                            // add itemReceived3 to ownedItemsList
                            bool dupe = false;
                            foreach (Item i in ownedItemList) // check if it's already there
                            {
                                if (i == currentTextList[GetCurrentLocationProgress()].itemReceived3) // dupe exists, don't add
                                {
                                    dupe = true;
                                }
                            }

                            if (!dupe)
                            {
                                currentTextList[GetCurrentLocationProgress()].itemReceived3.newItem = true; // makes items marked true by default
                                ownedItemList.Add(currentTextList[GetCurrentLocationProgress()].itemReceived3);
                            }
                        }
                    }
    }

    private float soundTimer = 0;
    public void PlayTextSound()
    {
        float textSoundMult = textSpeedMult == 1 ? 1f : 1.5f;
        if (soundTimer > textSpeed / (textSpeedMult * textSoundMult) * 4f)
        {
            soundTimer = 0;
            SFXManager sfx = SFXManager.instance;
            sfx.PlayRandomSound(sfx.periwinkleSpeaks,transform,0.1f);
        }

        soundTimer += Time.deltaTime;
    }

    public void SelectQuestion(Question question)
    {
        currentQuestion = question;
        textProgress = 0;
        ClearQuestions();
        textAnimating = false;
        NextText();
    }

    public void SelectItem(Item item)
    {
        Question itemQuestion = ScriptableObject.CreateInstance<Question>();
        itemQuestion.conversation = new List<string>();
        itemQuestion.character = GameManager.Ins.character;
        itemQuestion.location = GameManager.Ins.location;
        itemQuestion.conversation.Add(item.text);
        SelectQuestion(itemQuestion);
        //notesification.UpdateNotification();
        UpdateNotes();
    }
    

    public void GoToLocation(Location location) // ONLY TO BE ACCESSED BY GAMEMANAGER SCRIPT OF SAME NAME
    {
        GameManager.Ins.location = location;
        GameManager.Ins.character = Character.None;
        questionMode = false;
        questionsBox.SetActive(false); // This could mess up something.
        ClearQuestions();
        ClearPortrait();
        switch (location)
        {
            case Location.Entrance:
                currentTextList = entranceTextList;
                currentQuestionList = entranceQuestionList;
                break;
            case Location.TownSquare:
                currentTextList = townSquareTextList;
                currentQuestionList = townSquareQuestionList;
                break;
            case Location.MayorsOffice:
                currentTextList = mayorsOfficeTextList;
                currentQuestionList = mayorsOfficeQuestionList;
                break;
            case Location.Docks:
                currentTextList = docksTextList;
                currentQuestionList = docksQuestionList;
                break;
            case Location.Suburbs:
                currentTextList = suburbsTextList;
                currentQuestionList = suburbsQuestionList;
                break;
            case Location.ArtStudio:
                currentTextList = artStudioTextList;
                currentQuestionList = artStudioQuestionList;
                break;
            case Location.Shack:
                currentTextList = shackTextList;
                currentQuestionList = shackQuestionList;
                break;
            case Location.Park:
                currentTextList = parkTextList;
                currentQuestionList = parkQuestionList;
                break;
        }
        mainText = currentTextList[0].text[0];
        textProgress = 0;
        AnimateText();
    }

    public void Examine()
    {
        switch (GameManager.Ins.location)
        {
            case Location.Entrance:
                SelectQuestion(GameManager.Ins.examineEntrance);
                break;
            case Location.TownSquare:
                break;
            case Location.MayorsOffice:
                break;
            case Location.Docks:
                break;
            case Location.Suburbs:
                break;
            case Location.ArtStudio:
                break;
            case Location.Shack:
                break;
            case Location.Park:
                break;
        }
    }

    public void Leave()
    {
        
    }

    public void ClearNotes()
    {
        for (int i = 0; i < notesBox.childCount; i++)
        {
            Destroy(notesBox.GetChild(i).gameObject);
        }
    }
    public void UpdateNotes()
    {
        emptyNotesText.SetActive(notesBox.childCount == 0);
        for (int i = 0; i < notesBox.childCount; i++)
        {
            Destroy(notesBox.GetChild(i).gameObject);
        }

        bool unreadNotes = false;
        foreach (Item i in ownedItemList)
        {
            GameObject note = Instantiate(notePrefab, notesBox);
            ItemLogic itemLogic = note.GetComponent<ItemLogic>();
            itemLogic.item = i;
            if (i.newItem == true)
            {
                unreadNotes = true;
                //itemLogic.item.titleText = itemLogic.item.titleText + "(!)";
            }
        }
        //print(notesification);
        // Updats notification only if it needs updating (result differs from expected)
        if(notesification.notified != unreadNotes)notesification.UpdateNotification(unreadNotes);
        if (unreadNotes)
        {
            
        }

        
    }

    public void ClearPortrait()
    {
        if (portraitPanel.transform.childCount > 0)
        {
            Destroy(portraitPanel.transform.GetChild(0).gameObject); // CAREFUL THIS IS DANGEROUS

        }

        nameBoxText.text = "";
    }

    public void SetPortrait() // and name
    {
        nameBoxText.text = "";
        
        
        
        switch (GameManager.Ins.character)
        {
            case Character.Guard:
                currentPortrait = guardPortrait;
                nameBoxText.text = "Periwinkle";
                break;
            case Character.Handywoman:
                nameBoxText.text = "";
                break;
            case Character.Punk:
                nameBoxText.text = "";
                break;
            case Character.Artist:
                nameBoxText.text = "";
                break;
            case Character.Homeless:
                nameBoxText.text = "";
                break;
            case Character.SexWorker:
                nameBoxText.text = "";
                break;
            case Character.Mayor:
                nameBoxText.text = "";
                break;
            case Character.None:
                nameBoxText.text = "";
                break;
        }
        switch (GameManager.Ins.location)
        {
            case Location.Entrance:
                //currentPortrait = guardPortrait;
                //nameBoxText.text = "Periwinkle";
                break;
            case Location.TownSquare:
                currentPortrait = null;
                nameBoxText.text = "Town Square";
                break;
            case Location.MayorsOffice:
                nameBoxText.text = "";
                break;
            case Location.Docks:
                nameBoxText.text = "";
                break;
            case Location.Suburbs:
                nameBoxText.text = "";
                break;
            case Location.ArtStudio:
                nameBoxText.text = "";
                break;
            case Location.Shack:
                nameBoxText.text = "";
                break;
            case Location.Park:
                nameBoxText.text = "";
                break;
        }

        nameBox.enabled = !(nameBoxText.text == ""); // Hides box when no text
        //if(portraitPanel.transform.childCount > 0) Destroy(portraitPanel.transform.GetChild(0).gameObject); // DOES THIS CAUSE ERROR?
        
        if (currentPortrait == null)
        {
            if (portraitPanel.transform.childCount > 0)
            {
                Destroy(portraitPanel.transform.GetChild(0).gameObject); // CAREFUL THIS IS DANGEROUS

            }
        }
        else if(portraitPanel.transform.childCount == 0)
        {
            Instantiate(currentPortrait, portraitPanel.transform);
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
