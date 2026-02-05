using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Ins
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(GameManager).ToString());
                    _instance = singleton.AddComponent<GameManager>();
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

    public Location location; // Leave this set to Entrance in the inspector as default starting location
    public Character character;
    public int entranceTextProgress;
    public int townSquareTextProgress;
    public int mayorsOfficeTextProgress;
    public int docksTextProgress;
    public int suburbsTextProgress;
    public int artStudioTextProgress;
    public int shackTextProgress;
    public int parkTextProgress;

    public bool townSquareUnlocked;
    public bool mayorsOfficeUnlocked;
    public bool docksUnlocked;
    public bool suburbsUnlocked;
    public bool artStudioUnlocked;
    public bool shackUnlocked;
    public bool parkUnlocked;

    public List<Item> startingItems;
    
    public Question examineEntrance;
    public Question examineTownSquare;
    public Question examineTownSquare2;
    public Question examineMayorsOffice;
    public Question examineDocks;
    public Question examineDocks2;
    public Question examineSuburbs;
    public Question examineArtStudio;
    public Question examineShack;
    public Question examinePark;

    public bool punkRevealed;
    public bool corpseRetrieved;
    
    void Start()
    {
        //location = Location.Entrance;
    }

    public void GoToEntrance()
    {
        UIManager.Ins.GoToLocation(Location.Entrance);
    }

    public void GoToTownSquare()
    {
        UIManager.Ins.GoToLocation(Location.TownSquare);
    }

    public void GoToMayorsOffice()
    {
        UIManager.Ins.GoToLocation(Location.MayorsOffice);
    }

    public void GoToDocks()
    {
        UIManager.Ins.GoToLocation(Location.Docks);
    }

    public void GoToSuburbs()
    {
        UIManager.Ins.GoToLocation(Location.Suburbs);
    }

    public void GoToArtStudio()
    {
        UIManager.Ins.GoToLocation(Location.ArtStudio);
    }

    public void GoToShack()
    {
        UIManager.Ins.GoToLocation(Location.Shack);
    }

    public void GoToPark()
    {
        UIManager.Ins.GoToLocation(Location.Park);
    }

    public string CharacterWrongEvidenceResponse(Character character)
    {
        switch (character)
        {
            case Character.Guard:
                return "Uh... what’s that now?";
            case Character.Handywoman:
                return "..?";
            case Character.Punk:
                return "Huh? What? What are you even saying right now?";
            case Character.Artist:
                return "I... I don’t follow.";
            case Character.Homeless:
                // he could have 3-4 that he picks from at random
                return "Ooo, sharp. But not as sharp as Fishburn. Do that one over.";
            case Character.SexWorker:
                return "Sorry to disappoint hun, but I don’t have anything to hide.";
            case Character.Mayor:
                return "I think you’re losing the plot woman.";
            case Character.None:
                return "You proclaim to no one.";
            
            // ADD TWINSKIN "Now we’re really falling apart, aren’t we?"
        }

        return "huh..?";
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos=
                Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(
                mouseWorldPos,
                Vector2.zero,
                0f,
                LayerMask.GetMask("QuestionsBox")
            );

            if (hit.collider !=null)
            {
                UIManager.Ins.NextText();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UIManager.Ins.NextText();
            }
        }
    }


}


public enum Character
{
    None,
    Mayor,
    SexWorker,
    Homeless,
    Artist,
    Punk,
    Handywoman,
    Guard,
    Twinskin,
}

public enum Location
{
    Entrance,
    TownSquare,
    MayorsOffice,
    Docks,
    Suburbs,
    ArtStudio,
    Shack,
    Park
}

[System.Serializable]
public struct Dialogue
{
    public string text;
    public Character character;
    public bool characterSpeaking;
}