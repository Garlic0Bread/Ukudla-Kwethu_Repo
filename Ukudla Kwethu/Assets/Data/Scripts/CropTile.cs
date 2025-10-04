using System.Collections;
using UnityEngine;

public class CropTile : MonoBehaviour
{
    [SerializeField] private GameObject seedlingObject;
    [SerializeField] private GameObject matureObject;
    [SerializeField] private CropData cropData; // which crop this tile grows

    private bool ready = false;
    private int foodAmount = 1;
    private float timeToMature = 1;
    private Coroutine growthRoutine;

    private void Start()
    {
        foodAmount = cropData.baseYield;
        timeToMature = cropData.timeToMature;

        growthRoutine = StartCoroutine(GrowCycle());
    }

    public (CropData, int) HarvestFood()
    {
        if (!ready) return (null, 0);

        seedlingObject.SetActive(true);
        matureObject.SetActive(false);
        ready = false;

        // Restart growth safely
        if (growthRoutine != null) StopCoroutine(growthRoutine);
        growthRoutine = StartCoroutine(GrowCycle());

        // return both type + amount
        return (cropData, foodAmount);
    }

    private IEnumerator GrowCycle()
    {
        seedlingObject.SetActive(true);
        matureObject.SetActive(false);

        yield return new WaitForSeconds(timeToMature);

        seedlingObject.SetActive(false);
        matureObject.SetActive(true);
        ready = true;
    }
}