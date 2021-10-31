using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using System;


// Managee the units selection in the world.
public class UnitSelectionHandler : MonoBehaviour
{

    [SerializeField] private RectTransform unitSelectionArea = null;

    // Allowing us to determine which layers the ray casting should pay attention.
    [SerializeField] private LayerMask layerMask = new LayerMask();

    private Vector2 mouseStartPosition;

    private RTSPlayer player;

    private Camera mainCamera;

    // Selected unit list.
    public HashSet<Unit> SelectedUnits { get; } = new HashSet<Unit>();


    private void Start()
    {
        mainCamera = Camera.main;

        Unit.AuthortyOnUnitDeSpawned += AuthorityHandleUnitDespawned;

        GameOverHandler.ClientOnGameOver += ClientHandleGameOver;

    }

    private void OnDestroy()
    {
        Unit.AuthortyOnUnitDeSpawned -= AuthorityHandleUnitDespawned;

        GameOverHandler.ClientOnGameOver -= ClientHandleGameOver;
    }


    private void Update()
    {
        if(player == null)
            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartSelectionArea();
        }
        else if(Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
        else if(Mouse.current.leftButton.isPressed)
        {
            UpdateSelectionArea();
        }
    }

    // Set the start of the selection area on left click
    private void StartSelectionArea()
    {

        if (!Keyboard.current.shiftKey.isPressed)
        {
            // Realease prevoius unit selected.
            foreach (Unit selectedUnit in SelectedUnits)
            {
                selectedUnit.Deselect();
            }

            SelectedUnits.Clear();
        }

        unitSelectionArea.gameObject.SetActive(true);

        mouseStartPosition = Mouse.current.position.ReadValue();
    }


    // Set the size of the selection area according the mouse position.
    private void UpdateSelectionArea()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float areaWidth = mousePosition.x - mouseStartPosition.x;

        float areaHeight = mousePosition.y - mouseStartPosition.y;

        // Set rectangle size according the distance from start to current position.
        unitSelectionArea.sizeDelta = new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));

        // Set the anchor in the center of the rectangle.
        unitSelectionArea.anchoredPosition = mouseStartPosition +
            new Vector2(areaWidth / 2, areaHeight / 2);


    }

    private void ClearSelectionArea()
    {

        // Choose single unit.
        if (unitSelectionArea.sizeDelta.magnitude == 0)
        {
            SelectSingleUnit();
        }
        else
        {
            // Choose multiple units.
            SelectMultipleUnits();
        }

    }

    // Select single unit which directly clciked on.
    private void SelectSingleUnit()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;

        // Check if the object we hit, is a Unit.
        // If does,save the unit we hit.
        if (!hit.collider.TryGetComponent<Unit>(out Unit unit)) return;

        if (!unit.hasAuthority) return;

        SelectedUnits.Add(unit);

        foreach (Unit selectedUnit in SelectedUnits)
        {
            selectedUnit.Select();
        }
    }


    // Select multiple units inside the rectangle selected areaa.
    private void SelectMultipleUnits()
    {

        Vector2 min = unitSelectionArea.anchoredPosition - (unitSelectionArea.sizeDelta / 2);
        Vector2 max = unitSelectionArea.anchoredPosition + (unitSelectionArea.sizeDelta / 2);

        foreach (Unit unit in player.GetMyUnits())
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(unit.transform.position);

            if (screenPosition.x > min.x &&
                screenPosition.x < max.x &&
                screenPosition.y > min.y &&
                screenPosition.y < max.y)
            {
                SelectedUnits.Add(unit);
                unit.Select();
            }
        }

        unitSelectionArea.gameObject.SetActive(false);
    }

    private void AuthorityHandleUnitDespawned(Unit unit)
    {
        SelectedUnits.Remove(unit);
    }

    private void ClientHandleGameOver(string winnerName)
    {

        // MonoBehaviour member from stopping calls for the update function. 
        enabled = false;
    }
}


