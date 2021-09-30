using TMPro;
using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{
    [SerializeField] float baseLives = 3;
    [SerializeField] int damage = 1;

    float lives = 3;

    TextMeshProUGUI livesText;

    private void Start()
    {
        lives = baseLives - PlayerPrefController.GetDifficulty();

        livesText = GetComponent<TextMeshProUGUI>();


        UpdateDisplayHealth();
    }


    public void UpdateDisplayHealth()
    {
        livesText.text = lives.ToString();
    }


    public void DecreaseHealth(int amount)
    {
        lives -= damage;
        UpdateDisplayHealth();

        if (lives<= 0)
        {
            FindObjectOfType<LevelController>().HandleLoseCondition();

            return;
        }
  
    }


    public bool IsPlayerLost()
    {
        return lives <= 0;
    }
}
