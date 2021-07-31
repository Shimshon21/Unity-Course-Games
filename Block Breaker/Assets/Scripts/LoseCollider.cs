using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoseCollider : MonoBehaviour
{
    string GAME_OVER = "Game Over";
    // Collision detection with 'Collider2D' object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        loadGameOverScene();
    }

    // Load the game over scene.
    private void loadGameOverScene()
    {
        SceneManager.LoadScene(GAME_OVER);
    }


}
