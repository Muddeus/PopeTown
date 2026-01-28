using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Text", menuName = "Text")]
public class Text : ScriptableObject
{
    private Character character;
    public string text;
    
    


    public bool unlocked;
    

    [Header("Prerequisite explored questions")]
    public List<Question> exploredIDs;
}
