using TMPro;
using UnityEngine;

public class StarDisplay : MonoBehaviour
{
    [SerializeField] int stars = 100;
    [SerializeField] TextMeshProUGUI starsText;


    // Start is called before the first frame update
    void Start()
    {
        starsText = GetComponent<TextMeshProUGUI>();
        UpdateDisplay();
    }


    private void UpdateDisplay()
    {
        starsText.text = stars.ToString();
    }

    public bool IsEnoughStars(int amount)
    {
        return stars >= amount;
    }


    public void AddStars(int income)
    {
        stars += income;

        UpdateDisplay();
    }


    public void SpendStars(int amount)
    {
        if(IsEnoughStars(amount))
        {
            stars -= amount;
            UpdateDisplay();
        }
    }


    public int getCurrentStars()
    {
        return stars;
    }


}
