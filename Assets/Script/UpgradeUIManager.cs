using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIManager : MonoBehaviour
{
    public static UpgradeUIManager instance;

    public GameObject upgradePanel;
    public Button fireRateButton, rangeButton, bulletSpeedButton, damageButton, multiTargetButton, closeButton;
    private Tower selectedTower;

    void Awake()
    {
        instance = this;
        upgradePanel.SetActive(false); // Ensure it's hidden at start
    }

    public void ShowUpgradeUI(Tower tower)
    {
        selectedTower = tower;
        upgradePanel.SetActive(true);
        UpdateButtonState(); //Disable buttons if tower is max level
    }

    public void HideUpgradeUI()
    {
        upgradePanel.SetActive(false);
    }

    public void UpgradeFireRate()
    {
        if (selectedTower != null)
        {
            selectedTower.UpgradeFireRate();
            UpdateButtonState(); //Recheck button state after upgrade
        }
    }

    public void UpgradeRange()
    {
        if (selectedTower != null)
        {
            selectedTower.UpgradeRange();
            UpdateButtonState();
        }
    }

    public void UpgradeBulletSpeed()
    {
        if (selectedTower != null)
        {
            selectedTower.UpgradeBulletSpeed();
            UpdateButtonState();
        }
    }

    public void UpgradeDamage()
    {
        if (selectedTower != null)
        {
            selectedTower.UpgradeDamage();
            UpdateButtonState();
        }
    }

    public void UpgradeMultiTarget()
    {
        if (selectedTower != null)
        {
            selectedTower.UpgradeMultiTarget();
            UpdateButtonState();
        }
    }

    //Disable upgrade buttons if the tower is max level
    void UpdateButtonState()
    {
        bool isMax = selectedTower != null && selectedTower.IsMaxLevel();
        fireRateButton.interactable = !isMax;
        rangeButton.interactable = !isMax;
        bulletSpeedButton.interactable = !isMax;
        damageButton.interactable = !isMax;
        multiTargetButton.interactable = !isMax;
    }

    //Close Button Function
    public void CloseUpgradeUI()
    {
        upgradePanel.SetActive(false);
    }
}
