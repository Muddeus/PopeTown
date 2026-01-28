using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
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
    void Start()
    {
        //location = Location.Entrance;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UIManager.Instance.SkipTextAnimation();
            print("MOUSE DOWN");
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