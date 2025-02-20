using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance; // Singleton instance
    public int maxHealth = 10;
    private int currentHealth;
    public TMP_Text healthText; // Assign in UI

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        // Trigger Camera Shake
        if (CameraShake.instance != null)
        {
            StartCoroutine(CameraShake.instance.Shake(0.2f, 0.2f)); // Adjust duration & intensity
        }

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }


    void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = "HP: " + currentHealth;
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        if (GameOverManager.instance != null)
        {
            GameOverManager.instance.ShowGameOverScreen();
        }
    }


}
