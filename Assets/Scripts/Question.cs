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

    public Dialogue[] dialogue;
    
    [Header("Prerequisite explored questions")]
    public List<Question> exploredIDs;
}

