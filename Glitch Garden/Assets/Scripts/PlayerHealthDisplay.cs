using TMPro;
using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{
    [SerializeField] int currentHealth = 100;

    private void Start()
    {
        UpdateDisplayHealth();
    }


    public void UpdateDisplayHealth()
    {
        GetComponent<TextMeshProUGUI>().text = currentHealth.ToString();
    }


    public void DecreaseHealth(int amount)
    {
        currentHealth -= amount;

        UpdateDisplayHealth();
    }


    public bool IsPlayerLost()
    {
        return currentHealth <= 0;
    }
}
