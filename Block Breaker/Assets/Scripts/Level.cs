using UnityEngine;

public class Level : MonoBehaviour
{
    //Serlize for the debugging.
    [SerializeField]int breakableBlocks;

    // Cached reference
    SceneLoader loader;


    private void Start()
    {
        loader = FindObjectOfType<SceneLoader>();
    }



    public void CountBlock()
    {
        breakableBlocks++;
    }


    public void DecBreakableBlocks()
    {

        breakableBlocks--;

        // Load next scene when no blocks left.
        if (breakableBlocks <= 0)
        {
            loader.LoadNextScene();
        }

    }

}
