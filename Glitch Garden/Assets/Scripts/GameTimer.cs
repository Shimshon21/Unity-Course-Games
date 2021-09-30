using UnityEngine;
using UnityEngine.UI;


public class GameTimer : MonoBehaviour
{
    [Tooltip("Our level timer in Seconds")]

    [SerializeField] float levelTime = 10;

    private void Update()
    {
        GetComponent<Slider>().value = Time.timeSinceLevelLoad / levelTime;


        bool isTimeEnded = (Time.timeSinceLevelLoad >= levelTime);

        if (isTimeEnded)
        {
            FindObjectOfType<LevelController>().LevelTimerEnded();
        }
    
    }

}
