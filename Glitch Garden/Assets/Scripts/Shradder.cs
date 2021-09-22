using UnityEngine;

public class Shradder : MonoBehaviour
{

    private void Start()
    {
        Debug.Log("Shradder start");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision" + collision.gameObject);
        Destroy(collision.gameObject);
    }

}
