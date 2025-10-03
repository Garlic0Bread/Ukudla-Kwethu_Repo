using UnityEngine;

public class Battery : MonoBehaviour
{
    public int foodCarried = 0;
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

    public bool TryRemoveEnergy(int food)
    {
        if (foodCarried >= food)
        {
            foodCarried -= food;
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnergyTile"))
        {
            print("yes");
            CropTile tile = other.GetComponent<CropTile>();
            foodCarried += tile.HarvestFood();
        }

        /*if (other.CompareTag("Building"))
        {
            Building building = other.GetComponent<Building>();
            building.ReceiveEnergy(foodCarried);
            building.basketOnPlatform = true;
            building.timeOnPlatform = 0f;
        }*/
    }
}
