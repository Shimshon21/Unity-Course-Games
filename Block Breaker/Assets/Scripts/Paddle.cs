using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{

    // Config params
    [SerializeField] float screenWidthInUnits = 16f;
    [SerializeField] float minX = 1f;
    [SerializeField] float maxX = 15f;


    // Cached component reference
    Ball ball;
    GamesSession gamesSession;


    // Start is called before the first frame update
    void Start()
    {
        ball = FindObjectOfType<Ball>();

        gamesSession = FindObjectOfType<GamesSession>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePaddleByMousePos();
    }


    void MovePaddleByMousePos()
    {

        Vector2 paddlePos = new Vector2(Input.mousePosition.x, transform.position.y);
        paddlePos.x = Mathf.Clamp(GetXPos(), minX, maxX);
        transform.position = paddlePos;
    }


    /*Return position of X axis the paddle should be on.
    * The position returned according: 
    * Position ball(if auto play enabled)
    * Mouse position.
    */
    private float GetXPos()
    {
        if(gamesSession.IsAutoPlayEnabled())
        {
            return ball.transform.position.x;
        }

        return Input.mousePosition.x / Screen.width * screenWidthInUnits;
    }
}
