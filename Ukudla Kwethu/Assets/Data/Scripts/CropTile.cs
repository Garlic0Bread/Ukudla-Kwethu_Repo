using System.Collections;
using UnityEngine;

public class CropTile : MonoBehaviour
{
    public int energyAmount = 1;
    private bool ready = true;

    public int HarvestFood()
    {
        if (!ready) return 0;
        ready = false;
        StartCoroutine(Recharge());
        return energyAmount;
    }

    IEnumerator Recharge()
    {
        yield return new WaitForSeconds(5f); 
        ready = true;
    }
}

