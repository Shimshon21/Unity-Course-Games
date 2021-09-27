using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderButton : MonoBehaviour
{

    [SerializeField] Defender defenderPrefab;

    bool selected = false;

    private void OnMouseDown()
    {

        foreach(var button in FindObjectsOfType<DefenderButton>())
        {
            button.GetComponent<SpriteRenderer>().color = Color.gray;
        }

        FindObjectOfType<DefenderSpawner>().SetDefender(defenderPrefab);

        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

}
