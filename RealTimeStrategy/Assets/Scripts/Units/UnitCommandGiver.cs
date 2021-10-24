using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommandGiver : MonoBehaviour
{
    [SerializeField] private UnitSelectionHandler unitSelectionHandler = null;
    [SerializeField] LayerMask layerMask = new LayerMask();

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }


    private void Update()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) { return; }

        TryToMove(hit.point);
    }

    // Move the unit into the selected ray position.
    private void TryToMove(Vector3 point)
    {

        foreach(Unit unit in unitSelectionHandler.SelectedUnits)
        {
            unit.GetUnitMovement().CmdMove(point);
        }
        
    }
}