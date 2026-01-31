using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextObj", menuName = "TextObj")]
public class TextObj : ScriptableObject
{
    public int ID;
    public Character character;
    public Location location;
    public string[] text;
    

    public bool unlocked;
    
    public Item itemReceived;
    public int itemUnlockAt;
    public Item itemReceived2;
    public int itemUnlockAt2;
    public Item itemReceived3;
    public int itemUnlockAt3;

    [Header("Prerequisite explored questions")]
    public List<ScriptableObject> exploredIDs;
}
