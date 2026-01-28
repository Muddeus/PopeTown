using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextObj", menuName = "TextObj")]
public class TextObj : ScriptableObject
{
    public Character character;
    public Location location;
    public string text;
    
    


    public bool unlocked;
    

    [Header("Prerequisite explored questions")]
    public List<ScriptableObject> exploredIDs;
}
