using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Battery : MonoBehaviour
{
    private Dictionary<string, int> carriedCrops = new Dictionary<string, int>();
    [SerializeField] private int maxCapacity = 20;
    [SerializeField] private int currentCapacity;

    private new Collider collider;
    private MeshRenderer meshRenderer;

    [SerializeField] private PlacementSystem placementSystem;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        currentCapacity = GetTotalCarried();
    }

    public int GetTotalCarried()
    {
        int sum = 0;
        foreach (var kvp in carriedCrops) sum += kvp.Value;
        return sum;
    }

    public bool TryAddCrop(CropData crop, int amount = 1)
    {
        if (GetTotalCarried() + amount > maxCapacity) return false;

        if (!carriedCrops.ContainsKey(crop.cropID))
            carriedCrops[crop.cropID] = 0;

        carriedCrops[crop.cropID] += amount;
        return true;
    }

    public CropData TryRemoveAnyCrop()
    {
        foreach (var kvp in carriedCrops)
        {
            if (kvp.Value > 0)
            {
                carriedCrops[kvp.Key]--;

                if(carriedCrops[kvp.Key] <= 0)
                {
                    if (carriedCrops[kvp.Key] <= 0)
                    {
                        carriedCrops.Remove(kvp.Key);
                    }

                    CropData crop = Crop_Database.Instance.GetCropByID(kvp.Key);
                    return crop;
                }

            }
        }
        return null; // nothing carried
    }
    public CropData TryPeekAnyCrop()
    {//looks at the first available crop and returns its data without removing it from the basket.
        // Use carriedCrops.FirstOrDefault() to find the first entry with a positive count
        var firstAvailable = carriedCrops.FirstOrDefault(kvp => kvp.Value > 0);

        if (firstAvailable.Key != null)
        {
            //get CropData using the key (cropID)
            return Crop_Database.Instance.GetCropByID(firstAvailable.Key);
        }

        return null; // Basket is empty
    }

    public Dictionary<string, int> GetContents()
    {
        return carriedCrops;
    }

    public void Enable_Battery()
    {
        placementSystem.StopPlacement();
        meshRenderer.enabled = true;
        collider.enabled = true;
    }
    public void Disable_Battery()
    {
        meshRenderer.enabled = false;
        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropTile"))
        {
            CropTile tile = other.GetComponent<CropTile>();
            var (crop, amount) = tile.HarvestFood();

            if (crop != null)
            {
                TryAddCrop(crop, amount);
            }
        }
    }
}
