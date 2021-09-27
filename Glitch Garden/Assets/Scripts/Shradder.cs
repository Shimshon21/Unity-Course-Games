using UnityEngine;

public class Shradder : MonoBehaviour
{

    [SerializeField] int enemiesHitPoints = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Attacker>())
        {
            FindObjectOfType<PlayerHealthDisplay>().DecreaseHealth(enemiesHitPoints);
        }

        Destroy(collision.gameObject);
    }

}
