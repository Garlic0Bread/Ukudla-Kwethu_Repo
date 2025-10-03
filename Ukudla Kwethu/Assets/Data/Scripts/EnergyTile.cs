using System.Collections;
using UnityEngine;

public class EnergyTile : MonoBehaviour
{
    public int energyAmount = 1;
    private bool ready = true;

    public int HarvestEnergy()
    {
        if (!ready) return 0;
        ready = false;
        StartCoroutine(Recharge());
        // Visual effect here
        return energyAmount;
    }

    IEnumerator Recharge()
    {
        yield return new WaitForSeconds(5f); // tile recharge time
        ready = true;
    }
}

