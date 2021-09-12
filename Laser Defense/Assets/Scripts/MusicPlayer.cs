using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private void Awake()
    {
        int musicPlayersCreated = FindObjectsOfType<MusicPlayer>().Length;
      
        if (musicPlayersCreated > 1)
        {
            gameObject.SetActive(false);

            Destroy(gameObject);
        }else
        {
            DontDestroyOnLoad(gameObject);
        }
    
    }
}
