using System.Collections.Generic;
using UnityEngine;

public class Crop_Database : MonoBehaviour
{
    public static Crop_Database Instance;

    [SerializeField] private List<CropData> crops = new List<CropData>();
    private Dictionary<string, CropData> lookup = new Dictionary<string, CropData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        foreach (var crop in crops)
        {
            if (!lookup.ContainsKey(crop.cropID))
                lookup.Add(crop.cropID, crop);
        }
    }

    public CropData GetCropByID(string id)
    {
        lookup.TryGetValue(id, out var crop);
        return crop;
    }
}
