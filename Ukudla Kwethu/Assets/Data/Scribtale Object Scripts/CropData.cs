using UnityEngine;

[CreateAssetMenu(menuName = "Database/Crop Data")]
public class CropData : ScriptableObject
{
    public string cropID;       // unique string, e.g. "sorghum", "amaranth"
    public Sprite cropIcon;
    public int baseYield = 1;
    public float timeToMature = 1;
}

