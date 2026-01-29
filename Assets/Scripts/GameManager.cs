using System;
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
    void Start()
    {
        //location = Location.Entrance;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UIManager.Ins.NextText();
            
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

public struct Dialogue
{
    public string text;
    public Character character;
    public bool characterSpeaking;
}