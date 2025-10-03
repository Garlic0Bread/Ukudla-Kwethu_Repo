using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private LayerMask placementLayermask;

    public event Action OnCliked, OnExit;

    private Vector3 lastPosition;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnCliked?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }

    public bool IsPointer_OverUI()//if hovering over UI we're not gonna click jack shit
        => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelected_MapPosition()
    {
        Vector3 mousePos = Input.mousePosition; 
        mousePos.z = sceneCamera.nearClipPlane;//make sure objects not seen by cam are not selected

        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }
}
