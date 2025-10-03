using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionsToOccupy = CalculatePosistions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionsToOccupy, ID, placedObjectIndex);

        foreach(var position in positionsToOccupy)
        {
            if(placedObjects.ContainsKey(position))
            {
                throw new Exception($"Dicationary already contains{position}");
            }

            placedObjects[position] = data;
        }
    }

    private List<Vector3Int> CalculatePosistions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(new Vector3Int(gridPosition.x + x, gridPosition.y, gridPosition.z + y));
            }
        }
        return returnVal;
    }

    public bool CanPlaceObject(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePosistions(gridPosition, objectSize);
        foreach(var position in positionToOccupy)
        {
            if (placedObjects.ContainsKey(position))
                return false;
        }
        return true;
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int PlacedObjectIndex { get; private set; }
    public int ID { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        PlacedObjectIndex = placedObjectIndex;
        ID = iD;
    }
}
