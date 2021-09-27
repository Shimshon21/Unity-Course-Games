using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderSpawner : MonoBehaviour
{
    Defender defender;


    private void OnMouseDown()
    {
        AttemptToPlaceDefenderAt(GetSquareClicked());
    }


    public void SetDefender(Defender selectedDefender)
    {
        defender = selectedDefender;
    }


    private Vector2 GetSquareClicked()
    {
        Vector2 clickPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(clickPos);
        Vector2 gridPos = SnapToGrid(worldPos);

        return gridPos;
    }


    private Vector2 SnapToGrid(Vector2 rawWorldPos)
    {
        float newX = Mathf.RoundToInt(rawWorldPos.x);
        float newY = Mathf.RoundToInt(rawWorldPos.y);

        return new Vector2(newX, newY);
    }


    private void AttemptToPlaceDefenderAt(Vector2 gridPos)
    {
        var starDisplay = FindObjectOfType<StarDisplay>();

        int defenderCost = defender.GetStarCost();

        if(starDisplay.IsEnoughStars(defenderCost) && IsNotOccupiedArea(gridPos))
        {
            SpawnDefender(gridPos);

            starDisplay.SpendStars(defenderCost);
        }

    }

    private bool IsNotOccupiedArea(Vector2 gridPos)
    {
        var defenders = FindObjectsOfType<Defender>();

        foreach(var defender in defenders)
        {
            if(defender.transform.position.x == gridPos.x &&
                defender.transform.position.y == gridPos.y)
            {
                Debug.Log("Found");
                return false;
            }
        }

        return true;
    }

    private void SpawnDefender(Vector2 spawnPostion)
    {
        if (defender)
        {
            Defender newDefender = Instantiate(defender, spawnPostion, Quaternion.identity) as Defender;
        }
    }
}
