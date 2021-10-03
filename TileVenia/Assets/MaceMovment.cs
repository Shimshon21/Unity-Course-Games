using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceMovment : MonoBehaviour
{
    [SerializeField] float moveSpeedX;
    [SerializeField] float moveSpeedY;
    int sign = 1;
    [SerializeField] float time = 0;
    [SerializeField] float timeChange;

    float maxYPos;



    // Start is called before the first frame update
    void Start()
    {
        maxYPos = GetComponentInParent<Transform>().position.y;



    }

    // Update is called once per frame
    void Update()
    {
        if (time >= timeChange)
        {
            //GetComponent<Rigidbody2D>().velocity = ;
            transform.Translate(new Vector2(0.01f, 0));

            //time = 0;
        }

        if(transform.position.y == maxYPos)
        {
            sign *= -1;
        }

        time += Time.deltaTime;
    }
}
