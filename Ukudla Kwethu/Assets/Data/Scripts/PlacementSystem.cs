using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    private Renderer previewRender;
    private GridData floorData, tilesData;
    private int selectedObject_Index = -1;
    private List<GameObject> placedGameobjects = new();

    [SerializeField] private Grid grid;
    [SerializeField] private Object_Database database;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject gridVisualisation;
    [SerializeField] GameObject mouseIndicator, cellIndicator;

    private void Start()
    {
        StopPlacement();
        floorData = new();
        tilesData = new();

        previewRender = cellIndicator.GetComponentInChildren<Renderer>();
    }
    private void Update()
    {
        if (selectedObject_Index < 0)
            return;
        Vector3 mousePos = inputManager.GetSelected_MapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);

        bool placementValidity = CheckPlacementValidity(gridPos, selectedObject_Index);
        previewRender.material.color = placementValidity ? Color.cyan : Color.gray;

        mouseIndicator.transform.position = mousePos;
        cellIndicator.transform.position = grid.CellToWorld(gridPos);
    }

    public void StopPlacement()
    {
        selectedObject_Index = -1;
        gridVisualisation.SetActive(false);
        cellIndicator.SetActive(false);

        inputManager.OnCliked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }
    private void PlaceStructure()
    {
        if (inputManager.IsPointer_OverUI()) return;

        Vector3 mousePos = inputManager.GetSelected_MapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);

        bool placementValidity = CheckPlacementValidity(gridPos, selectedObject_Index);
        if (!placementValidity) return;

        GameObject newObject = Instantiate(database.objectsData[selectedObject_Index].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPos);
        placedGameobjects.Add(newObject);

        GridData selectedData = database.objectsData[selectedObject_Index].ID == 0 ? floorData : tilesData;

        selectedData.AddObjectAt(gridPos, database.objectsData[selectedObject_Index].Size,
            database.objectsData[selectedObject_Index].ID,
            placedGameobjects.Count - 1);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : tilesData;

        return selectedData.CanPlaceObject(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    public void StartPlacement(int ID)
    {
        selectedObject_Index = database.objectsData.FindIndex(data => data.ID == ID);

        if (selectedObject_Index < 0)
        {
            Debug.LogError($"no id found{ID}");
            return;
        }

        gridVisualisation.SetActive(true);
        cellIndicator.SetActive(true);

        inputManager.OnCliked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }
}
