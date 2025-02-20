using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    private int currentWave = 1;
    public TMP_Text waveText;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateWaveUI();
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }

    public void NextWave()
    {
        currentWave++;
        UpdateWaveUI();
    }

    void UpdateWaveUI()
    {
        if (waveText != null)
            waveText.text = "Wave: " + currentWave;
    }
}
