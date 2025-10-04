using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Recipe Data")]
public class Recipe_Databas : ScriptableObject
{
    public string recipeID;
    public string recipeName;
    public List<string> requiredCropIDs;  // e.g. ["sorghum", "amaranth"]
    public Sprite recipeIcon;
    public string description;
}
