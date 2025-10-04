using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Battery basket;
    [SerializeField] private GameObject cropUIPrefab;
    [SerializeField] private Transform uiContainer;

    [Header("Config")]
    [SerializeField] private int maxUnitsPerCropType = 10;
    [SerializeField] private int maxUniqueCrops = 3;
    [SerializeField] private float leechInterval = 1.5f;
    [SerializeField] private float timeBeforeStartLeeching = 1f;
    [SerializeField] private int unitsPerLeech = 1;
    [SerializeField] private float cropUseInterval = 1f;

    [Header("Events")]
    public UnityEvent onPlatformFull;

    private Dictionary<string, int> storedCrops = new();
    private Dictionary<string, Image> cropUIElements = new();

    private float nextLeechTime = 0f;
    private float timeOnPlatform = 0f;
    private float nextConsumeTime = 0f;
    private bool basketOnPlatform = false;

    private void Update()
    {
        HandleLeeching();
        HandleConsumption();
    }

    private void HandleLeeching()
    {
        if (!basketOnPlatform) return;

        if (timeOnPlatform < timeBeforeStartLeeching)
        {
            timeOnPlatform += Time.deltaTime;
            return; // Don't leech until the delay is over
        }

        if (Time.time < nextLeechTime) return;

        CropData crop = basket.TryRemoveAnyCrop(); // take ANY available crop
        if (crop != null)
        {
            AddCrop(crop, unitsPerLeech);
            nextLeechTime = Time.time + leechInterval;
        }
    }
    private void HandleConsumption()
    {
        if (Time.time < nextConsumeTime) return;
        if (storedCrops.Count == 0) return;

        // pick one crop type and consume it
        string cropIDToConsume = null;
        foreach (string cropID in storedCrops.Keys.ToList())
        {
            if (storedCrops[cropID] > 0)
            {
                cropIDToConsume = cropID;
                break;
            }
        }

        if (cropIDToConsume != null)
        {
            storedCrops[cropIDToConsume]--;
            UpdateCropUI(cropIDToConsume, storedCrops[cropIDToConsume]); // Update UI after consumption

            if (storedCrops[cropIDToConsume] <= 0)
            {
                storedCrops.Remove(cropIDToConsume);
                DestroyCropUI(cropIDToConsume); // Destroy UI element when count hits zero
            }
        }

        nextConsumeTime = Time.time + cropUseInterval;
    }

    private void AddCrop(CropData crop, int amount)
    {
        if (!storedCrops.ContainsKey(crop.cropID))//if is new/unique crop
        {
            if (storedCrops.Count >= maxUniqueCrops)
            {
                //building full of unique crops
                return;
            }

            //instantiate UI for the new unique crop type
            if (cropUIPrefab != null && uiContainer != null)
            {
                GameObject uiInstance = Instantiate(cropUIPrefab, uiContainer);
                Image cropImage = uiInstance.GetComponent<Image>();

                if (cropImage != null)
                {
                    print("image here");
                    cropImage.sprite = crop.cropIcon;
                    cropUIElements.Add(crop.cropID, cropImage);
                }
                else
                {
                    Debug.LogError("Crop UI Prefab is missing an Image component!");
                }
            }

            storedCrops.Add(crop.cropID, 0); // Initialize count to 0
        }

        //check for max units per crop type before adding
        int currentAmount = storedCrops[crop.cropID];
        int newAmount = Mathf.Min(currentAmount + amount, maxUnitsPerCropType);

        if (newAmount > currentAmount)
        {
            storedCrops[crop.cropID] = newAmount;

            //update the specific crop's UI
            UpdateCropUI(crop.cropID, newAmount);

            // recipe unlock check
            Reciper_Manager.Instance.TryUnlockRecipe(storedCrops);
        }
        else
        {
            Debug.Log("Storage for " + crop.cropID + " is full.");
        }
    }
    private void UpdateCropUI(string cropID, int currentAmount)
    {
        if (cropUIElements.TryGetValue(cropID, out Image cropImage))
        {
            float fillRatio = (float)currentAmount / maxUnitsPerCropType;
            cropImage.fillAmount = Mathf.Clamp01(fillRatio);
        }
    }
    private void DestroyCropUI(string cropID)
    {
        if (cropUIElements.TryGetValue(cropID, out Image cropImage))
        {
            Destroy(cropImage.gameObject);
            cropUIElements.Remove(cropID);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Basket"))
        {
            basketOnPlatform = true;
            timeOnPlatform = 0f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Basket"))
        {
            basketOnPlatform = false;
            timeOnPlatform = 0f;
        }
    }
}



