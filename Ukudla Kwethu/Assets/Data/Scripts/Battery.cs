using UnityEngine;

public class Battery : MonoBehaviour
{
    public int energyCarried = 0;
    private new Collider collider;
    private MeshRenderer meshRenderer;

    [SerializeField] private PlacementSystem placementSystem;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
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

    public bool TryRemoveEnergy(int energy)
    {
        if (energyCarried >= energy)
        {
            energyCarried -= energy;
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnergyTile"))
        {
            print("yes");
            EnergyTile tile = other.GetComponent<EnergyTile>();
            energyCarried += tile.HarvestEnergy();
        }

        /*if (other.CompareTag("Building"))
        {
            Building building = other.GetComponent<Building>();
            building.ReceiveEnergy(energyCarried);
            building.batteryOnPlatform = true;
            building.timeOnPlatform = 0f;
        }*/
    }
}
