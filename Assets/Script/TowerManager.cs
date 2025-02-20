using UnityEngine;
using TMPro; // For UI text updates

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance; // Singleton instance

    public int playerGold = 100; // Starting gold amount
    public TMP_Text goldText; // Assign the Gold UI Text in the Inspector

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateGoldUI();
    }

    public bool CanPlaceTower()
    {
        return playerGold >= 100; // First tower is free, others cost 100 gold
    }

    public bool SpendGold(int amount)
    {
        if (playerGold >= amount)
        {
            playerGold -= amount;
            UpdateGoldUI();
            return true; // Successfully spent gold
        }
        return false; // Not enough gold
    }


    public void EarnGold(int amount)
    {
        playerGold += amount;
        UpdateGoldUI();
    }

    void UpdateGoldUI()
    {
        if (goldText != null)
            goldText.text = "Gold: " + playerGold;
    }
}
