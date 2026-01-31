using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "TextObj", menuName = "TextObj")]
public class TextObj : ScriptableObject
{
    public int ID;
    public Character character;
    public Location location;
    public string[] text;
    

    public bool unlocked;
    
    public Item itemReceived;
    [FormerlySerializedAs("itemUnlockAt")] public int itemUnlockAfter;
    public Item itemReceived2;
    [FormerlySerializedAs("itemUnlockAt2")] public int itemUnlockAfter2;
    public Item itemReceived3;
    [FormerlySerializedAs("itemUnlockAt3")] public int itemUnlockAfter3;

    [Header("Prerequisite explored questions")]
    public List<ScriptableObject> exploredIDs;
}
