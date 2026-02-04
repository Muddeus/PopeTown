using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class Item : ScriptableObject
{
    private Character character;
    public string titleText;
    public string text;    
    public bool newItem;

    
    public string mayorResponseText;
    public List<string> mayorConversation;
    
    //public string sexWorkerText;
    public string sexWorkerResponseText;
    public List<string> sexWorkerConversation;
    
    //public string homelessText;
    public string homelessResponseText;
    public List<string> homelessConversation;

    //public string artistText;
    public string artistResponseText;
    public List<string> artistConversation;
    
    //public string punkText;
    public string punkResponseText;
    public List<string> punkConversation;

    //public string handywomanText;
    public string handywomanResponseText;
    public List<string> handywomanConversation;


    public bool unlocked;
    //public bool newItem;
    //public Location location; 
    
    
    [Header("Prerequisite explored questions")]
    public List<Question> exploredIDs;

    public string devNote;
}
