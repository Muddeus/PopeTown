using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ColorUtility = UnityEngine.ColorUtility;

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
    [SerializeField] private bool presentingMode;
    [SerializeField] private bool presentable;
    private bool talking;
    public GameObject questionsBox;
    public GameObject dialogueBox;
    public GameObject contentBox; // Where the question buttons go
    public GameObject examineObj;
    public GameObject leaveObj;
    public GameObject buttonPrefab;
    public GameObject notePrefab;
    public GameObject emptyNotesText;
    public Question currentQuestion;

    public List<Item> ownedItemList;
    private int lastOwnedItemListCount;
    
    public TMP_Text nameBoxText;
    public Image nameBox;
    public Transform notesBox;
    [FormerlySerializedAs("notesBox")] public Transform notesBoxContent;
    public Image handsBox;
    public Texture2D hands;

    private int portraitIndex;
    private Image portraitImage;
    public GameObject currentPortrait;
    public GameObject portraitPanel;
    public GameObject currentBG;
    public GameObject guardPortrait; // no entranceBG
    public GameObject mayorPortrait;
    public GameObject sexWorkerPortrait;
    public GameObject homelessPortrait;
    public GameObject artistPortrait;
    public GameObject punkPortrait;
    public GameObject handywomanPortrait;
    public GameObject townSquareBG;
    public GameObject mayorsOfficeBG;
    public GameObject docksBG;
    public GameObject suburbsBG;
    public GameObject artStudioBG;
    public GameObject shackBG;
    public GameObject parkBG;

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
    
    //Animation stuff
    private Animator anim;
    public Animator handAnim;

    public Sprite mayorIdle;
    public Sprite mayorTalk;
    private Character currentCharacter;
    private Location currentLocation;

    void Start()
    {
        currentCharacter = Character.None;
        currentLocation = Location.Entrance;
        anim = GetComponent<Animator>();
        presentable = false;
        talking = true;
        textProgress = 0;
        questionMode = false;
        questionsBox.SetActive(false);
        nameBoxText.text = "";
        nameBox.enabled = false;
        leaveObj.SetActive(false);
        examineObj.SetActive(false);
        portraitIndex = 0;
        ClearNotes();
        ownedItemList = GameManager.Ins.startingItems;
        emptyNotesText.SetActive(false); //disables EMPTY text
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
            List<Item> necessaryItems = new List<Item>();
            if(obj.itemPresent)necessaryItems.Add(obj.itemPresent);
            if(obj.itemPresent2)necessaryItems.Add(obj.itemPresent2);
            if(obj.itemPresent3)necessaryItems.Add(obj.itemPresent3);
            if (obj.exploredIDs.Count == 0 && necessaryItems.Count == 0) // FIX THIS
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

    public bool GetTextAnimating()
    {
        return textAnimating;
    }
    private float handsTimer = 0;
    private float textSpeedMult;

    public Scrollbar scrollbar;
    void Update()
    {
        //if (nameBoxText.text == "") nameBox.enabled = false;
        nameBox.enabled = nameBoxText.text != "";
        // hands animation
        handsBox.enabled = questionMode && !GameManager.Ins.shattering;
        if (questionMode)
        {
            // animate character
            /*if (portraitPanel.transform.childCount > 0)
            {
                Image talkAnim = GameObject.FindWithTag("CharacterPortrait").GetComponent<Image>();//portraitPanel.transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>();
                if (talkAnim != null)
                {
                    switch (GameManager.Ins.character)
                    {
                        case Character.None:
                            break;
                        case Character.Mayor:
                            talkAnim.sprite = textAnimating?mayorTalk:mayorIdle;
                            break;
                        case Character.Artist:
                            break;
                        case Character.Guard:
                            break;
                        case Character.Handywoman:
                            break;
                        case Character.Homeless:
                            break;
                        case Character.Punk:
                            break;
                        case Character.Twinskin:
                            break;
                        case Character.SexWorker:
                            break;
                    }
                }
            }*/
            
            // show hands
            if (textAnimating)
            {
                
            }
        }
        if (textAnimating && (!questionMode || currentQuestion !=null))//&& textTimer > textSpeed) COME BACK HERE FOR TALKING AND SOUND
        {
            // text animation
            textSpeedMult = textLength < 20 ? 0.3f : 1f;
            if (textLength < 10) textSpeedMult = 0.1f;
            if(GameManager.Ins.character == Character.None) PlayTextSound();
            if(GameManager.Ins.character != Character.None) PlayTextSound();
            textPosition = (int)(textTimer / textSpeed * textSpeedMult);
            textPosition = Math.Clamp(textPosition, 0, textLength);
            scrollbar.value = 0;
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
            else if (mainText == "[MASK SHATTERS]")
            {
                mainTextDisplay.text = "";
                ShatterMask();
                GameManager.Ins.shattering = true;
            }
            else
            {
                FormatTextDisplay();
                dialogueBox.transform.SetAsLastSibling();
                //mainTextDisplay.text = "<line-height=11.18>" + mainTextDisplay.text + "</line-height>";
                //preferred height 11.18
                //mainTextDisplay.layout
            }

            if (textPosition >= textLength)
            {
                if (textAnimating) StartCoroutine(SetScrollbarPos(0.01f));
                textAnimating = false;
            }
            

            textTimer += Time.deltaTime;
        }

        if (questionMode && !questionsBox.activeInHierarchy) // Runs once each time question mode is activated
        {
            RefreshQuestions();
        }

        if (questionMode)
        {
            
        }
        //print("textProgress: " + textProgress);
    }
    IEnumerator SetScrollbarPos(float time)
    {
        yield return new WaitForSeconds(time);

        scrollbar.value = 0;
    }

    public void SetTalking(bool t)
    {
        talking = t;
    }

    private void FormatTextDisplay()
    {
        // Check for bad characters to replace
        mainText = mainText.Replace('’', '\'');
        mainText = mainText.Replace('‘', '’');
        string notifyColor = "#" + ColorUtility.ToHtmlStringRGB(GameManager.Ins.notifyColor);
        mainText = mainText.Replace("notiCol", notifyColor);
        
        // Only show as much of the string as as been revealed so far in the animation.
        mainTextDisplay.text = mainText.Substring(0, textPosition);
        // Remove \ from end
        if(mainTextDisplay.text.EndsWith('\\'))mainTextDisplay.text = mainTextDisplay.text.Substring(0, mainTextDisplay.text.Length - 1);
        int mTextDisPos = 0;
        int clearedText = 0;
        // Dealing with incomplete/broken blocks (<, <b, </i, etc...)
        foreach (char c in mainTextDisplay.text)
        {
            if (c == '<')
            {
                if (mTextDisPos < mainTextDisplay.text.Length) // If not at end of text
                {
                    int pairedBrackets = 1; // < increases count, > decreases count. if 0 at end then all are pairs.
                    int lastOpenBracket = 0;
                    foreach (char c2 in mainTextDisplay.text.Substring(mTextDisPos + 1)) // check characters after the <
                    {
                        if (c2 == '>') pairedBrackets--;
                        if (c2 == '<') pairedBrackets++;
                    }

                    if (pairedBrackets != 0) //if uneven
                    {
                        // Remove text from last bracket segment onwards
                        int badPastIndex = mainTextDisplay.text.LastIndexOf('<');
                        mainTextDisplay.text = mainTextDisplay.text.Substring(0, badPastIndex);
                    }
                }
            }
            mTextDisPos++;
        }

        /*// dealing with complete blocks (<x>, </x>, etc...) but still bad formatting
        string[] formatTypes = new string[] { "i", "b" };
        foreach (string formatType in formatTypes)
        {
            // if the number of <x> doesn't match the number of </x> then (formatting is bad)...
            if (Regex.Matches(mainTextDisplay.text, "<" + formatType + ">") != Regex.Matches(mainTextDisplay.text, "</" + formatType + ">"))
            {
                int badPastIndex = mainTextDisplay.text.LastIndexOf("</" + formatType + ">");
                print("badPastIndex: " + badPastIndex);
                if(badPastIndex>1)mainTextDisplay.text = mainTextDisplay.text.Substring(0, badPastIndex);
            }
        }*/
        
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
        talking = true;
        //evidenceCount = 0; // resets for next evidence. could be cause of bug..
        questionsBox.transform.SetAsLastSibling();
        presentable = true;
        currentQuestion = null;
        questionsBox.SetActive(true);
        leaveObj.SetActive(GameManager.Ins.townSquareUnlocked); // once town hall is unlocked, permanently unlock the leave button
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
                GameManager.Ins.character = obj.character; // was disabled: might be able to bring back later if needed for eddie??
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

    private bool presentPassed = false;
    private bool playAnimNext = false;
    public void NextText() // @@@@@[[[[[[[[[[[@@@@@]]]]]]]]]]]]@@@@@
    {
        if (textAnimating) // if text still coming out..
        {
            //if(GameManager.Ins.location != Location.Entrance) SetPortrait(); // don't want it to affect start of game
            //print("Still animating text..");
            textTimer = 9999f;
        }
        else // if moving to next text/screen
        {
            /*if (playAnimNext)
            {
                anim.Play("Hand Writing");
                notesification.Notify();
                playAnimNext = false;
            }
            if (lastOwnedItemListCount != ownedItemList.Count) // play the animation
            {
                playAnimNext = true;
            }
            lastOwnedItemListCount = ownedItemList.Count;*/
            //PresentEvidenceCheck(emptyItem);
            if(talking)print("1");
            bool expectedItem = false;
            if (currentQuestion == null || presentPassed)
            {
            
            }
            else
            {
                if (currentQuestion.itemPresent != null && textProgress-1 == currentQuestion.itemPresentAt)
                {
                    expectedItem = true;
                }
                else if (currentQuestion.itemPresent2 != null && textProgress-1 == currentQuestion.itemPresentAt2)
                {
                    expectedItem = true;
                }
                else if (currentQuestion.itemPresent3 != null && textProgress-1 == currentQuestion.itemPresentAt3)
                {
                    expectedItem = true;
                }
            }

            if (expectedItem)
            {
                RefreshQuestions();
                if(talking)print("2");
            }
            presentPassed = false;
            // Update the log here, text has been read
            if(!GameManager.Ins.shattering)SetPortrait();
            UpdateNotes();
            emptyNotesText.SetActive(notesBoxContent.childCount == 0); // Get rid of EMPTY text if not empty
            
            textProgress++;
            if(talking)print("3");
            if (questionMode)
            {
                if (currentQuestion != null) // If there is a current question
                {
                    ////////// SPECIAL CASES FIRST
                    
                    
                    if (currentQuestion.name == "ExamineDocks1" && textProgress == 6)
                    {
                        nameBoxText.text = "???";
                    }
                    if (currentQuestion.name == "ExamineDocks1" && textProgress == 7)
                    {
                        print("EXAMINE DOCKS REVEAL EDDIE");
                        GameManager.Ins.punkRevealed = true;
                    }

                    if (GameManager.Ins.punkRevealed && !GameManager.Ins.punkNameRevealed &&
                        GameManager.Ins.location == Location.Docks)
                    {
                        nameBoxText.text = "???";
                    }

                    if (currentQuestion.name == "Docks0" && textProgress == 10) GameManager.Ins.punkNameRevealed = true;
                    ////////// END SPECIAL CASES
                    
                    notesBox.gameObject.SetActive(false);
                    notesification.UpdateUnderline(notesBox.gameObject);
                    leaveObj.SetActive(false);
                    examineObj.SetActive(false);
                    // set portrait
                    GameManager.Ins.character = currentQuestion.character;
                    //SetPortrait();
                    //PresentEvidenceCheck();
                    // Check if item to unlock (multiple items ahead)
                    if(talking)print("4");
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
                    
                    if ((textProgress - 1) < currentQuestion.conversation.Count) // WAIT THIS LOOKS WRONG WHY DOES IT WORK??
                    {
                        mainText = currentQuestion.conversation[textProgress-1]; // start from 1 in question mode
                    }
                    else // question over, update changes and return to questions
                    {
                        //CHECK QUESTION ACTUALLY OVER BEFORE MARKING IT AS READ
                        if (textProgress >= currentQuestion.conversation.Count)
                        {
                            //print("current question count: " + currentQuestion.conversation.Count + "\ntextProgress: " + textProgress);
                            //print("Marking question as read!" + currentQuestion.name);
                            currentQuestion.newQuestion = false;
                            if (currentQuestion.checkToUnlockLocation)
                            {
                                switch (currentQuestion.unlockLocation)
                                {
                                    case Location.Entrance:
                                        break;
                                    case Location.TownSquare:
                                        GameManager.Ins.townSquareUnlocked = true;
                                        break;
                                    case Location.MayorsOffice:
                                        GameManager.Ins.mayorsOfficeUnlocked = true;
                                        break;
                                    case Location.Docks: // All locations unlocked after Mayor's Office chat
                                        GameManager.Ins.docksUnlocked = true;
                                        GameManager.Ins.suburbsUnlocked = true;
                                        GameManager.Ins.artStudioUnlocked = true;
                                        GameManager.Ins.shackUnlocked = true;
                                        GameManager.Ins.parkUnlocked = true;
                                        break;
                                    case Location.Suburbs:
                                        GameManager.Ins.suburbsUnlocked = true;
                                        break;
                                    case Location.ArtStudio:
                                        GameManager.Ins.artStudioUnlocked = true;
                                        break;
                                    case Location.Shack:
                                        GameManager.Ins.shackUnlocked = true;
                                        break;
                                    case Location.Park:
                                        GameManager.Ins.parkUnlocked = true;
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                        }
                        if(talking)print("5");
                        // Check prerequisites
                        foreach (Question q in allQuestionArray)
                        {
                            if (true)//q.exploredIDs != null && q.necessaryItems != null)
                            {
                                bool unlocked = true;
                                // we will check questions first...
                                foreach (Question r in q.exploredIDs)
                                {
                                    // If any prerequisites are still unexplored, keep that question locked,
                                    if(r.newQuestion == true) unlocked = false; 
                                    // otherwise, continue
                                }
                                //...then check items
                                List<Item> necessaryItems = new List<Item>();
                                if(q.itemPresent)necessaryItems.Add(q.itemPresent);
                                if(q.itemPresent2)necessaryItems.Add(q.itemPresent2);
                                if(q.itemPresent3)necessaryItems.Add(q.itemPresent3);
                                //print("necessary item count for '" + q.name + "': " + necessaryItems.Count);
                                foreach (Item n in necessaryItems) // each necessary item
                                {
                                    bool unlocked2 = false;
                                    foreach (Item o in ownedItemList) // is checked if it's owned
                                    {
                                        if (o == n) unlocked2 = true;
                                    }
                                    if (unlocked2 == false) unlocked = false; // if any are missing then keep locked, otherwise continue
                                }
                                // only unlocks if it managed to stay unlocked through every test 
                                q.unlocked = unlocked;
                            }
                        }
                        currentQuestion = null;
                        textProgress = -1; // to ensure text starts from actual start as it is technically triggered by a NextText
                        RefreshQuestions();
                        if(talking)print("6");
                    }
                }
                else // If there is no current question
                {
                    textProgress = 0;
                    if (GetCurrentLocationProgress() < currentTextList.Count) // if there is still unread text, continue text mode
                    {
                        questionMode = false;
                    }
                    else
                    {
                        RefreshQuestions();
                        SetPortrait();
                        //this doesnt trigger when returning to entrance, return, bug
                    }
                    if(talking)print("7");
                    //if(GameManager.Ins.townSquareUnlocked)leaveObj.SetActive(true);
                }

                if (GetCurrentLocationProgress() >= currentTextList.Count)
                {
                    // Return to questions screen if no more text available
                    //print("RETURN TO QUESTION SCREEN");
                    questionMode = true;
                }
                else
                {
                    questionMode = false;
                    textProgress = -1;
                }
                if(talking)print("8");
            }
            else // NOT in Question Mode (text mode)
            {
                presentable = false;
                talking = false;
                leaveObj.SetActive(false);
                examineObj.SetActive(false);
                if(GetCurrentLocationProgress() < 0) SetCurrentLocationProgress(0);
                AddItemFromText();
                // Implement locking text here (for prerequisites etc
                
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
                    //GameManager.Ins.character = Character.None; why was this here??
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

    //private int evidencesRequired;
    //private int evidenceCount = 0; // make sure this is reset each question
    public Item emptyItem;
    public void PresentEvidenceCheck(Item presentedItem)
    {
        //print("Evidence count: " + evidenceCount);
        // check if the current message has an associated piece of evidence
        //
        // check if the presented evidence matches the associated one

        // find total evidence required
        bool match = false;
        /*int addUp = 0;
        if (currentQuestion.itemPresent != null) addUp++;
        if (currentQuestion.itemPresent2 != null) addUp++;
        if (currentQuestion.itemPresent3 != null) addUp++;
        evidencesRequired = addUp;
            */
        if (currentQuestion == null)
        {
            
        }
        else
        {
            if (currentQuestion.itemPresent == presentedItem && textProgress-1 == currentQuestion.itemPresentAt)
            {
                match = true;
            }
            else if (currentQuestion.itemPresent2 == presentedItem && textProgress-1 == currentQuestion.itemPresentAt2)
            {
                match = true;
            }
            else if (currentQuestion.itemPresent3 == presentedItem && textProgress-1 == currentQuestion.itemPresentAt3)
            {
                match = true;
            }
        }

        bool nothingPresented = presentedItem == emptyItem;

        if (match) // EVIDENCE PRESENTED CORRECTLY
        {
            // continue on with the question convo
            print("CORRECT EVIDENCE!");
            presentPassed = true;
            NextText();
        } 
        else if (nothingPresented)
        {
            print("Nothing presented");
            //NextText();
        }
        else // INCORRECT EVIDENCE
        {
            if (presentedItem == null)
            {
                //RefreshQuestions();
                return;
            }
            // give character response and then refresh questions
            Question wrongEvidenceResponse = ScriptableObject.CreateInstance<Question>();
            wrongEvidenceResponse.conversation = new List<string>();
            wrongEvidenceResponse.character = GameManager.Ins.character;
            wrongEvidenceResponse.location = GameManager.Ins.location;
            wrongEvidenceResponse.conversation.Add(GameManager.Ins.CharacterWrongEvidenceResponse(GameManager.Ins.character));
            SelectQuestion(wrongEvidenceResponse);
            //notesification.UpdateNotification();
            //UpdateNotes();
        }
        
        
        // CAN I DELETE TS? THIS WAS JUST A COPY PASTE FOR REFERENCE HERE
        /*if (currentQuestion.itemReceived != null)
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
        }*/
    }
    public void AddItemFromText()
    {
        // Check if item to unlock (multiple items ahead)
        if (currentTextList[GetCurrentLocationProgress()].itemReceived != null)
        {
            if (textProgress >= currentTextList[GetCurrentLocationProgress()].itemUnlockAfter)
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
                    //notesification.UpdateNotification();
                }
            }
        }
        if (currentTextList[GetCurrentLocationProgress()].itemReceived2 != null)
        {
            if (textProgress >= currentTextList[GetCurrentLocationProgress()].itemUnlockAfter2)
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
                    //notesification.UpdateNotification();
                }
            }
        }
        if (currentTextList[GetCurrentLocationProgress()].itemReceived3 != null)
        {
            if (textProgress >= currentTextList[GetCurrentLocationProgress()].itemUnlockAfter3)
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
                    //notesification.UpdateNotification();
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
            // if not question mode then text sound

            if (!questionMode) // text mode
            {
                sfx.PlayRandomSound(sfx.text,transform,0.1f);
            }
            else
            {
                switch (GameManager.Ins.character)
                {
                    case Character.Guard:
                        sfx.PlayRandomSound(sfx.periwinkleSpeaks,transform,0.1f);
                        break;
                    case Character.Handywoman:
                        sfx.PlayRandomSound(sfx.handywomanSpeaks,transform,0.1f);
                        break;
                    case Character.Punk:
                        sfx.PlayRandomSound(sfx.punkSpeaks,transform,0.1f);
                        break;
                    case Character.Artist:
                        sfx.PlayRandomSound(sfx.artistSpeaks,transform,0.1f);
                        break;
                    case Character.Homeless:
                        sfx.PlayRandomSound(sfx.homelessSpeaks,transform,0.1f);
                        break;
                    case Character.SexWorker:
                        sfx.PlayRandomSound(sfx.sexWorkerSpeaks,transform,0.1f);
                        break;
                    case Character.Mayor:
                        sfx.PlayRandomSound(sfx.mayorSpeaks,transform,0.1f);
                        break;
                    case Character.None:
                        break;
            
                    // ADD TWINSKIN "Now we’re really falling apart, aren’t we?"
                }
            }
                //sfx.PlayRandomSound(sfx.periwinkleSpeaks,transform,0.1f);
        }

        soundTimer += Time.deltaTime;
    }

    public void SelectQuestion(Question question)
    {
        currentQuestion = question;
        textProgress = 0;
        ClearQuestions();
        textAnimating = false;
        questionMode = true;
        NextText();
    }

    public void SelectItem(Item item)
    {
        if (presentingMode) // Presenting evidence
        {
            //TODO
            PresentEvidenceCheck(item);
            handAnim.Play("Hand QUESTION");
        }
        else // Reading Note
        {
            ClearPortrait(); // So that it looks like you're reading your note and not someone else talking
            nameBoxText.text = "";
            Question itemQuestion = ScriptableObject.CreateInstance<Question>();
            itemQuestion.conversation = new List<string>();
            itemQuestion.character = GameManager.Ins.character;
            itemQuestion.location = GameManager.Ins.location;
            itemQuestion.conversation.Add(item.text);
            SelectQuestion(itemQuestion);
            //notesification.UpdateNotification();
            UpdateNotes();
        }
        
    }

    public void FaceClick()
    {
        UpdateNotes();
        notesBox.gameObject.SetActive(true);
        notesification.UpdateUnderline(notesBox.gameObject);
        SetPresentingMode(true);
    }

    public void SetPresentingMode(bool set)
    {
        presentingMode = set;
    }

    public bool GetPresentingMode()
    {
        return presentingMode;
    }
    

    public void GoToLocation(Location location) // ONLY TO BE ACCESSED BY GAMEMANAGER SCRIPT OF SAME NAME
    {
        ClearPortrait();
        GameManager.Ins.location = location;
        GameManager.Ins.character = Character.None;
        questionMode = false;
        questionsBox.SetActive(false); // This could mess up something.
        leaveObj.SetActive(false);
        examineObj.SetActive(false);
        ClearQuestions();
        currentLocation = location; //currentLocation is for audio management only
        switch (location)
        {
            case Location.Entrance:
                currentTextList = entranceTextList;
                currentQuestionList = entranceQuestionList;
                currentPortrait = guardPortrait;
                //GameManager.Ins.character = Character.Guard; THIS DOESNT FIX IT
                break;
            case Location.TownSquare:
                currentTextList = townSquareTextList;
                currentQuestionList = townSquareQuestionList;
                currentPortrait = townSquareBG;
                MusicManager.ins.PlayTownSquare();
                break;
            case Location.MayorsOffice:
                currentTextList = mayorsOfficeTextList;
                currentQuestionList = mayorsOfficeQuestionList;
                currentPortrait = mayorPortrait;
                MusicManager.ins.PlayMayor();
                break;
            case Location.Docks:
                currentTextList = docksTextList;
                currentQuestionList = docksQuestionList;
                currentPortrait = docksBG;
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
        textProgress = 0;
        // Make it impossible to go out of bounds
        //int currentTextLength = currentTextList[GetCurrentLocationProgress()].text.Length;
        
        ClearPortrait();
        
        int currentTextLength = currentTextList.Count;
        if (GetCurrentLocationProgress() >= currentTextLength) // If at end of text already...
        {
            SetCurrentLocationProgress(currentTextLength);
            questionMode = true;
            RefreshQuestions();
            
            SetPortrait();
            return; // THIS MIGHT BE CAUSING BUGS??
        }
        mainText = currentTextList[GetCurrentLocationProgress()].text[0]; //if(currentTextList!=null )
        SetPortrait();
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
                SelectQuestion(GameManager.Ins.corpseRetrieved?GameManager.Ins.examineTownSquare2:GameManager.Ins.examineTownSquare);
                break;
            case Location.MayorsOffice:
                SelectQuestion(GameManager.Ins.examineMayorsOffice);
                break;
            case Location.Docks:
                SelectQuestion(GameManager.Ins.punkRevealed?GameManager.Ins.examineDocks2:GameManager.Ins.examineDocks);
                //GameManager.Ins.punkRevealed = true;
                break;
            case Location.Suburbs:
                SelectQuestion(GameManager.Ins.examineSuburbs);
                break;
            case Location.ArtStudio:
                SelectQuestion(GameManager.Ins.examineArtStudio);
                break;
            case Location.Shack:
                SelectQuestion(GameManager.Ins.examineShack);
                break;
            case Location.Park:
                SelectQuestion(GameManager.Ins.examinePark);
                break;
        }
    }

    public void ClearNotes()
    {
        for (int i = 0; i < notesBoxContent.childCount; i++)
        {
            Destroy(notesBoxContent.GetChild(i).gameObject);
        }
    }

    private int lastOwnedListCount = 0;
    public void UpdateNotes()
    {
        for (int i = 0; i < notesBoxContent.childCount; i++)
        {
            Destroy(notesBoxContent.GetChild(i).gameObject);
        }

        bool unreadNotes = false;
        foreach (Item i in ownedItemList)
        {
            GameObject note = Instantiate(notePrefab, notesBoxContent);
            ItemLogic itemLogic = note.GetComponent<ItemLogic>();
            itemLogic.item = i;
            if (i.newItem == true)
            {
                unreadNotes = true;
                //itemLogic.item.titleText = itemLogic.item.titleText + "(!)";
            }
        }
        // Updats notification only if it needs updating (result differs from expected)
        if (notesification.notified != unreadNotes)
        {
            notesification.UpdateNotification(unreadNotes);
            //print("last count: " + lastOwnedListCount);
            //print("current count: " + ownedItemList.Count);
            //if(unreadNotes || lastOwnedListCount != ownedItemList.Count)handAnim.Play("Hand Writing");
        }
        if (unreadNotes)
        {
            if (lastOwnedListCount != ownedItemList.Count)
            {
                handAnim.Play("Hand Writing");
                notesification.Notify();
            }
        }

        if (lastOwnedListCount != ownedItemList.Count)
        {
            //anim.Play("Hand Writing");
        }
        lastOwnedListCount = ownedItemList.Count;
        emptyNotesText.SetActive(notesBoxContent.childCount == 0);
    }

    public void SetMainTextDisplay(string text)
    {
        mainTextDisplay.text = text;
    }

    public Item[] tutorialItems;
    public void ClearTutorialItems()
    {
        if (tutorialItems == null) return;
        foreach (Item i in tutorialItems)
        {
            for (int x = ownedItemList.Count-1; x >= 0; x-- )
            {
                if (i == ownedItemList[x]) ownedItemList.RemoveAt(x);
            }
        }

        tutorialItems = null;
    }

    private Animator shatterAnim;
    public void ShatterMask()
    {
        //disable name
        shatterAnim = null;
        shatterAnim = portraitPanel.GetComponentInChildren<Animator>();
        shatterAnim.Play("Mask Shitter 2");
        handsBox.enabled = false;
        nameBoxText.text = "";
    }

    public void PostShatterRestore()
    {
        // restore name
    }

    public void ClearPortrait()
    {
        if (portraitPanel.transform.childCount > 0)
        {
            Destroy(portraitPanel.transform.GetChild(0).gameObject); // CAREFUL THIS IS DANGEROUS Immediate
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
                nameBoxText.text = "The Handyma'am";
                
                break;
            case Character.Punk:
                if(GameManager.Ins.punkNameRevealed)nameBoxText.text = "Eddie";
                break;
            case Character.Artist:
                nameBoxText.text = "Gemini";
                break;
            case Character.Homeless:
                nameBoxText.text = "Fishburn";
                break;
            case Character.SexWorker:
                nameBoxText.text = "Spider";
                break;
            case Character.Mayor:
                currentPortrait = mayorPortrait;
                nameBoxText.text = "Mayor Bunyon";
                break;
            case Character.None:
                nameBoxText.text = "";
                break;
            case Character.Twinskin:
                nameBoxText.text = "The Twinskin";
                //nameBoxText.text = "T̵̢͙̻̼̠̺̬̗̭̹̹̝̹͖͖̻̺̯̬͐͊ͪ̿ͧͦ̽ͪ̑̋ͫͯ̓͘͜͜͡h̹̯̻͍̟̥ͯͫͨͨ́͒ͬ̃͑͗͒ͅͅe̶̢͈̹͓̖̖̗̮͙͔̦͎̠̲̠̜͆ͩ͗̊ͦ̏̎̍̂̂̑ͦ̔̍͑͂̂̈̚̕͞ͅ T͚̦̂ͪẃ̸̢̛̦̮̬̻̹͍͍̣̹̮̝͙̀ͬ̍ͪ̂ͫ̊͐ͥͯ̊́͋̀̌̒͐͌̽̿̈́ͦ͘͜͡͞i̘̭͛͛̅ńͥş͇͙͕̱̎͑ͨ̔̓̌̔̚͞͡ͅk̵̨̨͓͎̝͕̎__̧͚͉̤̅̎̏̂̾̒͘̚͞ͅi̤̺̞͍̯̞͈͕̊́̔ͨ͐͐͗͘͠n̡̗̝̣̬̦̠̞̼͎̤ͭ͗ͤ͘͡";
                break;
        }
        switch (GameManager.Ins.location)
        {
            case Location.Entrance:
                //currentPortrait = guardPortrait;
                //nameBoxText.text = "Periwinkle";
                break;
            case Location.TownSquare:
                //currentPortrait = null;
                currentPortrait = townSquareBG;
                nameBoxText.text = "Town Square";
                break;
            case Location.MayorsOffice:
                //nameBoxText.text = "";
                break;
            case Location.Docks:
                currentPortrait = docksBG;
                //nameBoxText.text = "";
                if (GameManager.Ins.punkRevealed && !GameManager.Ins.punkNameRevealed) nameBoxText.text = "???";
                break;
            case Location.Suburbs:
                currentPortrait = sexWorkerPortrait;
                nameBoxText.text = "SPIDER";
                break;
            case Location.ArtStudio:
                currentPortrait = artistPortrait;
                nameBoxText.text = "GEMINI";
                break;
            case Location.Shack:
                currentPortrait = handywomanPortrait;
                nameBoxText.text = "HANDYMA'AM";
                break;
            case Location.Park:
                currentPortrait = homelessPortrait;
                nameBoxText.text = "FISHBURN";
                break;
        }

        //nameBox.enabled = !(nameBoxText.text == ""); // Hides box when no text
        //if(portraitPanel.transform.childCount > 0) Destroy(portraitPanel.transform.GetChild(0).gameObject); // DOES THIS CAUSE ERROR?
        
        if (currentPortrait == null)
        {
            //print("null portrait. child count: " + portraitPanel.transform.childCount);
            if (portraitPanel.transform.childCount > 0)
            {
                Destroy(portraitPanel.transform.GetChild(0).gameObject); // CAREFUL THIS IS DANGEROUS Immediate

            }
        }
        else if(portraitPanel.transform.childCount == 0)
        {
            GameObject obj = Instantiate(currentPortrait, portraitPanel.transform);
            print("Portrait instantiated position: " + obj.transform.position);
            obj.GetComponent<RectTransform>().localPosition = new Vector3(0, obj.GetComponent<yOffset>().offset, 0f);
            
            //obj.GetComponent<RectTransform>().position = new Vector3(obj.transform.position.x, obj.transform.position.y + obj.GetComponent<yOffset>().offset, 0f);
            //obj.GetComponent<RectTransform>().position
            //print("Set Portrait..." + currentPortrait.name);
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
/*

414
398
Set Portraits commented out at these lines to allow for name to be cleared while reading items

*/