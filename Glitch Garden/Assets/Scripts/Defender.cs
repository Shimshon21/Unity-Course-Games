using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour
{
    [SerializeField] int starCost = 100;

    [SerializeField] GameObject starAddPrefab;
    [SerializeField] GameObject starSpawn;

    const float STAR_DELAY_TIME = 0.7f;


    // Function for adding stars defenders.
    public void AddStars(int amount)
    {
        FindObjectOfType<StarDisplay>().AddStars(amount);

        var star = Instantiate(starAddPrefab, starSpawn.transform.position, Quaternion.identity);

        Destroy(star, STAR_DELAY_TIME);
    }

    // Get the price of the current defender.
    public int GetStarCost()
    {
        return starCost;
    }
}
