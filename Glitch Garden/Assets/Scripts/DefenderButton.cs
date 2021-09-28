using UnityEngine.UI;
using UnityEngine;

public class DefenderButton : MonoBehaviour
{

    [SerializeField] Defender defenderPrefab;



    bool selected = false;

    private void Start()
    {
        if(defenderPrefab)
            SetPriceText();

    }


    private void OnMouseDown()
    {

        foreach(var button in FindObjectsOfType<DefenderButton>())
        {
            button.GetComponent<SpriteRenderer>().color = Color.gray;
        }

        FindObjectOfType<DefenderSpawner>().SetDefender(defenderPrefab);

        gameObject.GetComponent<SpriteRenderer>().color = Color.white;

    }


    private void SetPriceText()
    {
        GetComponentInChildren<Text>().text = defenderPrefab.GetStarCost().ToString();
    }
}
