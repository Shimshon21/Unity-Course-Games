using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


// Gives movemnt and firing commands to units.
public class UnitCommandGiver : MonoBehaviour
{
    [SerializeField] private UnitSelectionHandler unitSelectionHandler = null;
    [SerializeField] LayerMask layerMask = new LayerMask();

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        GameOverHandler.ClientOnGameOver += ClientHandleGameOver;
    }

    private void OnDestroy()
    {
        GameOverHandler.ClientOnGameOver -= ClientHandleGameOver;
    }


    private void Update()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame) { return; }

        // Get the position mouse is clicked on by using ray.
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) { return; }

        if (hit.collider.TryGetComponent<Targetable>(out Targetable targetable))
        {

            Debug.Log("Target was given is" + targetable);
            if (targetable.hasAuthority)
            {
                TryToMove(hit.point);
                return;
            }

            TryTarget(targetable);
            return;
        }


        TryToMove(hit.point);
    }

    // Move the unit into the selected ray position.
    private void TryToMove(Vector3 point)
    {

        foreach (Unit unit in unitSelectionHandler.SelectedUnits)
        {
            unit.GetUnitMovement().CmdMove(point);
        }

    }


    // Set new target for all units selected.
    private void TryTarget(Targetable target)
    {
        Debug.Log("given target" + target);

        foreach(Unit unit in unitSelectionHandler.SelectedUnits)
        {
            unit.GetTargeter().CmdSetTarget(target.gameObject);
        }
    }


    // Disable the 'Update' function
    private void ClientHandleGameOver(string winnerName)
    {
        enabled = false;
    }
}
