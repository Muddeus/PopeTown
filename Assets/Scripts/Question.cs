using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Question")]
public class Question : ScriptableObject
{
    //public int ID;
    public string questionText;
    //public string responseText;
    public List<string> conversation;
    public bool unlocked;
    public bool newQuestion;
    // locations: e.g. 0-TownCenter, 1-blank
    //[Header("Prerequisite explored questions")]
    //public int location;
    public Location location; 
    // characters: e.g. 0-Goobli
    //public int character;
    public Character character;
    public Item itemReceived;
    public int itemUnlockAt;
    public Item itemReceived2;
    public int itemUnlockAt2;
    public Item itemReceived3;
    public int itemUnlockAt3;

    public Item itemPresent;
    public int itemPresentAt;
    public Item itemPresent2;
    public int itemPresentAt2;
    public Item itemPresent3;
    public int itemPresentAt3;

    public bool checkToUnlockLocation;
    public Location unlockLocation;
    
    public Dialogue[] dialogue;
    
    [Header("Prerequisite explored questions")]
    public List<Question> exploredIDs;
    
    [Header("Prerequisite owned items")]
    public List<Item> necessaryItems;
}

